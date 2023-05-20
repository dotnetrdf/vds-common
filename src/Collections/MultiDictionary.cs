/*
VDS.Common is licensed under the MIT License

Copyright (c) 2012-2015 Robert Vesse
Copyright (c) 2016-2018 dotNetRDF Project (http://dotnetrdf.org/)

Permission is hereby granted, free of charge, to any person obtaining a copy of this software
and associated documentation files (the "Software"), to deal in the Software without restriction,
including without limitation the rights to use, copy, modify, merge, publish, distribute,
sublicense, and/or sell copies of the Software, and to permit persons to whom the Software 
is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or
substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using VDS.Common.Trees;

namespace VDS.Common.Collections
{
    /// <summary>
    /// Possible modes to use for the binary search tree based buckets of the <see cref="MultiDictionary{TKey,TValue}"/>
    /// </summary>
    public enum MultiDictionaryMode
    {
        /// <summary>
        /// Use unbalanced trees, best when you expect minimal key collisions and are willing to trade faster insert performance for slower lookup performance
        /// </summary>
        Unbalanced,
        /// <summary>
        /// Use Scapegoat trees, good when there are a few key collisions and key comparisons are inexpensive.  Provides amortized O(log n) performance but ocassional operations may be O(n)
        /// </summary>
        Scapegoat,
        /// <summary>
        /// Use AVL trees, likely gives the best overall performance
        /// </summary>
        AVL   
    }

    /// <summary>
    /// An advanced dictionary implementation
    /// </summary>
    /// <typeparam name="TKey">Key Type</typeparam>
    /// <typeparam name="TValue">Value Type</typeparam>
    /// <remarks>
    /// <para>
    /// A multi-dictionary is essentially just a dictionary which deals more efficiently with key collisions, with a normal .Net dictionary colliding hash codes cause performance degradation which makes them less than ideal for certain use cases such as indexes.  A multi-dictionary can also handle null keys provided that the hash function in use can cope with these.
    /// </para>
    /// <para>
    /// With this implementation the keys are used to split the values into buckets and then each bucket uses a binary search tree to maintain full information about the keys and values.  This means that all keys and values are properly preserved and keys cannot interfer with each other in most cases.  In the case where keys have the same hash code and compare to be equal then they will interfere with each other but that is the correct behaviour.  The implementation is designed to be flexible in that it allows you to specify the hash function, the comparer used and the form of tree used for the buckets.
    /// </para>
    /// <para>
    /// This means the multi-dictionary gives slightly worse performance than a dictionary for well distributed data with minimal key collisions but provides order of magnitude better performance for data with lots of hash collisions.  This makes it ideal as a use for an indexing structure since you can index using partial properties of the keys (thus giving hash code collisions) while still preserving all the keys.
    /// </para>
    /// <h3>Null Key Handling</h3>
    /// <para>
    /// A MultiDictionary is capable of handling null keys when the key type is a nullable type provided that the hash function used supports them.  To determine this it will attempt to apply the hash function to <em>default(TKey)</em> in the constructor, if the hash function supports null keys or the key type is non-nullable this will succeed and it will allow null keys.  If a <see cref="NullReferenceException"/> is thrown it will ignore the error and use the default behaviour of forbidding null keys, any attempt to use a null key in this case will result in an <see cref="ArgumentNullException"/>
    /// </para>
    /// </remarks>
    public class MultiDictionary<TKey, TValue>
        : IDictionary<TKey, TValue>, IEnumerable<TValue>
    {
        /// <summary>
        /// Default Mode for Multi-Dictionaries
        /// </summary>
        public const MultiDictionaryMode DefaultMode = MultiDictionaryMode.AVL;

        private Dictionary<int, ITree<IBinaryTreeNode<TKey, TValue>, TKey, TValue>> _dict;
        private IComparer<TKey> _comparer = Comparer<TKey>.Default;
        private Func<TKey, int> _hashFunc = k => k.GetHashCode();
        private MultiDictionaryMode _mode;
        private readonly bool _allowNullKeys;

        /// <summary>
        /// Creates a new multi-dictionary
        /// </summary>
        public MultiDictionary()
            : this(null, false) { }

        /// <summary>
        /// Creates a new multi-dictionary
        /// </summary>
        /// <param name="mode">Mode to use for the buckets</param>
        public MultiDictionary(MultiDictionaryMode mode)
            : this(null, false, null, mode) { }

        /// <summary>
        /// Creates a new multi-dictionary
        /// </summary>
        /// <param name="comparer">Comparer used for keys within the binary search trees</param>
        public MultiDictionary(IComparer<TKey> comparer)
            : this(null, false, comparer) { }

        /// <summary>
        /// Creates a new multi-dictionary
        /// </summary>
        /// <param name="hashFunction">Hash Function to split the keys into the buckets</param>
        /// <param name="mode">Mode to use for the buckets</param>
        [Obsolete("You must use the three argument form of the constructor and explicitly state whether your hash function supports null keys", true)]
        public MultiDictionary(Func<TKey, int> hashFunction, MultiDictionaryMode mode)
            : this(hashFunction, false, null, mode) { }

        /// <summary>
        /// Creates a new multi-dictionary
        /// </summary>
        /// <param name="hashFunction">Hash Function to split the keys into the buckets</param>
        /// <param name="allowNullKeys">Whether to allow null keys</param>
        /// <param name="mode">Mode to use for the buckets</param>
        public MultiDictionary(Func<TKey, int> hashFunction, bool allowNullKeys, MultiDictionaryMode mode)
            : this(hashFunction, allowNullKeys, null, mode) { }

        /// <summary>
        /// Creates a new multi-dictionary
        /// </summary>
        /// <param name="comparer">Comparer used for keys within the binary search trees</param>
        /// <param name="mode">Mode to use for the buckets</param>
        public MultiDictionary(IComparer<TKey> comparer, MultiDictionaryMode mode)
            : this(null, false, comparer, mode) { }

        /// <summary>
        /// Creates a new multi-dictionary
        /// </summary>
        /// <param name="hashFunction">Hash Function to splut the keys into the buckets</param>
        /// <param name="allowNullKeys">Whether null keys are allowed</param>
        /// <param name="comparer">Comparer used for keys within the binary search trees</param>
        /// <param name="mode">Mode to use for the buckets</param>
        public MultiDictionary(Func<TKey, int> hashFunction, bool allowNullKeys = false, IComparer<TKey> comparer = null, MultiDictionaryMode mode = DefaultMode)
        {
            this._comparer = comparer ?? this._comparer;
            this._hashFunc = hashFunction ?? this._hashFunc;
            this._allowNullKeys = allowNullKeys;
            this._dict = new Dictionary<int, ITree<IBinaryTreeNode<TKey, TValue>, TKey, TValue>>();
            this._mode = mode;
        }

        /// <summary>
        /// Creates a new Tree to be used as a key/value bucket
        /// </summary>
        /// <returns>Tree to use as the bucket</returns>
        private ITree<IBinaryTreeNode<TKey, TValue>, TKey, TValue> CreateTree()
        {
            switch (this._mode)
            {
                case MultiDictionaryMode.AVL:
                    return new AVLTree<TKey, TValue>(this._comparer);
                case MultiDictionaryMode.Scapegoat:
                    return new ScapegoatTree<TKey, TValue>(this._comparer);
                case MultiDictionaryMode.Unbalanced:
                    return new UnbalancedBinaryTree<TKey, TValue>(this._comparer);
                default:
                    return new AVLTree<TKey, TValue>(this._comparer);
            }
        }

        #region IDictionary<TKey,TValue> Members

        /// <summary>
        /// Adds a key value pair to the dictionary
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        public void Add(TKey key, TValue value)
        {
            if (!this._allowNullKeys && key == null) throw new ArgumentNullException(nameof(key), "Key cannot be null");

            int hash = this._hashFunc(key);
            if (this._dict.TryGetValue(hash, out ITree<IBinaryTreeNode<TKey, TValue>, TKey, TValue> tree))
            {
                //Add into existing tree
                tree.Add(key, value);
            }
            else
            {
                //Add new tree
                tree = this.CreateTree();
                tree.Add(key, value);
                this._dict.Add(hash, tree);
            }
        }

        /// <summary>
        /// Gets whether the dictionary contains the given key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>True if the given key exists in the dictionary, false otherwise</returns>
        public bool ContainsKey(TKey key)
        {
            if (!this._allowNullKeys && key == null) throw new ArgumentNullException(nameof(key), "Key cannot be null");

            int hash = this._hashFunc(key);
            if (this._dict.TryGetValue(hash, out ITree<IBinaryTreeNode<TKey, TValue>, TKey, TValue> tree))
            {
                return tree.ContainsKey(key);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the keys of the dictionary
        /// </summary>
        public ICollection<TKey> Keys
        {
            get 
            {
                return new ImmutableView<TKey>(from hashKey in this._dict.Keys
                        from k in this._dict[hashKey].Keys
                        select k, "Modifying the Keys collection of a MultiDictionary directly is not supported");
            }
        }

        /// <summary>
        /// Removes a key value pair from the dictionary based on the key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>True if a key value pair was removed, false otherwise</returns>
        public bool Remove(TKey key)
        {
            if (!this._allowNullKeys && key == null) throw new ArgumentNullException(nameof(key), "Key cannot be null");

            int hash = this._hashFunc(key);
            if (this._dict.TryGetValue(hash, out ITree<IBinaryTreeNode<TKey, TValue>, TKey, TValue> tree))
            {
                bool removed = tree.Remove(key);
                if (removed && tree.Root == null)
                {
                  // Clear up empty trees
                  this._dict.Remove(hash);
                }
                return removed;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Tries to get the value associated with a key
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <returns>True if the key exists in the dictionary and a value can be returned, false otherwise</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (!this._allowNullKeys && key == null) throw new ArgumentNullException(nameof(key), "Key cannot be null");

            int hash = this._hashFunc(key);
            if (this._dict.TryGetValue(hash, out ITree<IBinaryTreeNode<TKey, TValue>, TKey, TValue> tree))
            {
                return tree.TryGetValue(key, out value);
            }
            else
            {
                value = default;
                return false;
            }
        }

        /// <summary>
        /// Tries to get the actual key instance stored for a given key
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="actualKey">Actual Key</param>
        /// <returns>True if the key exists in the dictionary and the instance was returned, false otherwise</returns>
        [Obsolete("TryGetKey() is included for historical reasons and will be removed in future versions")]
        public bool TryGetKey(TKey key, out TKey actualKey)
        {
            int hash = this._hashFunc(key);
            if (this._dict.TryGetValue(hash, out ITree<IBinaryTreeNode<TKey, TValue>, TKey, TValue> tree))
            {
                IBinaryTreeNode<TKey, TValue> node = tree.Find(key);
                if (node == null)
                {
                    actualKey = default;
                    return false;
                }
                else
                {
                    actualKey = node.Key;
                    return true;
                }
            }
            else
            {
                actualKey = default;
                return false;
            }
        }

        /// <summary>
        /// Gets the values in the dictionary
        /// </summary>
        public ICollection<TValue> Values
        {
            get
            {
                return new ImmutableView<TValue>(from hashKey in this._dict.Keys
                        from v in this._dict[hashKey].Values
                        select v, "Modiying the values collection of a MultiDictionary directly is not supported");
            }
        }

        /// <summary>
        /// Gets/Sets a Value in the dictionary
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>The value associated with the given key</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the given key does not exist in the dictionary</exception>
        public TValue this[TKey key]
        {
            get
            {
                if (!this._allowNullKeys && key == null) throw new ArgumentNullException(nameof(key), "Key cannot be null");

                if (this.TryGetValue(key, out TValue value))
                {
                    return value;
                }
                else
                {
                    throw new KeyNotFoundException(key.ToSafeString() + " not found");
                }
            }
            set
            {
                if (!this._allowNullKeys && key == null) throw new ArgumentNullException(nameof(key), "Key cannot be null");

                int hash = this._hashFunc(key);
                if (this._dict.TryGetValue(hash, out ITree<IBinaryTreeNode<TKey, TValue>, TKey, TValue> tree))
                {
                    //Move to appropriate node
                    IBinaryTreeNode<TKey, TValue> node = tree.MoveToNode(key, out bool _);
                    node.Value = value;
                }
                else
                {
                    //Add new tree
                    tree = this.CreateTree();
                    tree.Add(key, value);
                    this._dict.Add(hash, tree);
                }
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        /// <summary>
        /// Adds a key value pair to the dictionary
        /// </summary>
        /// <param name="item">Key value pair</param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        /// <summary>
        /// Clears the dictionary
        /// </summary>
        public void Clear()
        {
            this._dict.Clear();
        }

        /// <summary>
        /// Gets whether the dictionary contains a given key value pair
        /// </summary>
        /// <param name="item">Key value pair</param>
        /// <returns>True if the given key value pair exists in the dictionary</returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            if (!this._allowNullKeys && item.Key == null) throw new InvalidOperationException( "item.Key cannot be null" );
            if (this.TryGetValue(item.Key, out TValue value))
            {
                if (value != null) return value.Equals(item.Value);
                if (item.Value == null) return true; //Both null so equal
                return false; //One is null so not equal
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Copies the dictionary into an array
        /// </summary>
        /// <param name="array">Array</param>
        /// <param name="arrayIndex">Index to start copying at</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array),"Cannot copy to a null array");
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex),"Cannot start copying at index < 0");
            if (this.Count > array.Length - arrayIndex) throw new ArgumentException("Insufficient space in array");

            int i = arrayIndex;
            foreach (ITree<IBinaryTreeNode<TKey, TValue>, TKey, TValue> tree in this._dict.Values)
            {
                foreach (IBinaryTreeNode<TKey, TValue> node in tree.Nodes)
                {
                    array[i] = new KeyValuePair<TKey, TValue>(node.Key, node.Value);
                    i++;
                }
            }
        }

        /// <summary>
        /// Gets the number of values in the dictionary
        /// </summary>
        public int Count
        {
            get
            {
                return this._dict.Values.Sum(t => t.Nodes.Count()); 
            }
        }

        /// <summary>
        /// Returns false because this dictionary is read/write
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Removes a key value pair from the dictionary
        /// </summary>
        /// <param name="item">Key value pair</param>
        /// <returns>True if the key value pair was removed from the dictionary</returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (!this._allowNullKeys && item.Key == null) throw new InvalidOperationException("item.Key cannot be null");
            if (this.TryGetValue(item.Key, out TValue value))
            {
                if (value != null && value.Equals(item.Value)) return this.Remove(item.Key);
                if (item.Value == null) return this.Remove(item.Key);
                return false;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        /// <summary>
        /// Gets an enumerator for the key value pairs in the dictionary
        /// </summary>
        /// <returns>Enumerator over key value pairs</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return (from t in this._dict.Values
                    from n in t.Nodes
                    select new KeyValuePair<TKey, TValue>(n.Key, n.Value)).GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Gets the enumerator for the dictionary
        /// </summary>
        /// <returns>Enumerator</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region IEnumerable<TValue> Members

        /// <summary>
        /// Gets the enumeration of values in the dictionary
        /// </summary>
        /// <returns>Enumerator over values</returns>
        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
        {
            return (from hashKey in this._dict.Keys
                    from v in this._dict[hashKey].Values
                    select v).GetEnumerator();
        }

        #endregion
    }
}
