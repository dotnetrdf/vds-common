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
    /// An implementation of a dictionary where the sort order of keys is preserved using a binary tree behind the scenes.  This makes all operations on the dictionary O(log n)
    /// </summary>
    /// <typeparam name="TKey">Key Type</typeparam>
    /// <typeparam name="TValue">Value Type</typeparam>
    public class TreeSortedDictionary<TKey, TValue>
        : IDictionary<TKey, TValue>, IEnumerable<TValue>
    {
        private ITree<IBinaryTreeNode<TKey, TValue>, TKey, TValue> _tree;
        private int _count;

        /// <summary>
        /// Creates a new dictionary using the default comparer for the key type
        /// </summary>
        public TreeSortedDictionary()
            : this(Comparer<TKey>.Default) { }

        /// <summary>
        /// Creates a new dictionary using the given comparer for the keys
        /// </summary>
        /// <param name="comparer">Comparer</param>
        public TreeSortedDictionary(IComparer<TKey> comparer)
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer), "Comparer cannot be null");
            this._tree = new AVLTree<TKey, TValue>(comparer);
        }

        #region IDictionary<TKey,TValue> Members

        /// <summary>
        /// Adds a key value pair to the dictionary
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        public void Add(TKey key, TValue value)
        {
            if (this._tree.Add(key, value))
            {
                this._count++;
            }
        }

        /// <summary>
        /// Checks whether the dictionary contains the given key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>True if the dictionary contains the key, false otherwise</returns>
        public bool ContainsKey(TKey key)
        {
            return this._tree.ContainsKey(key);
        }

        /// <summary>
        /// Gets the collection of keys
        /// </summary>
        public ICollection<TKey> Keys
        {
            get 
            {
                return new ImmutableView<TKey>(this._tree.Keys, "Modifying the Keys collection of a TreeSortedDictionary directly is not supported");
            }
        }

        /// <summary>
        /// Removes a key from the dictionary
        /// </summary>
        /// <param name="key">Key to remove</param>
        /// <returns>True if a key was removed, false otherwise</returns>
        public bool Remove(TKey key)
        {
            if (this._tree.Remove(key))
            {
                this._count--;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Tries to get the value associated with the given key
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <returns>True if a value has been returned</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return this._tree.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets the collection of values in the dictionary
        /// </summary>
        public ICollection<TValue> Values
        {
            get
            {
                return new ImmutableView<TValue>(this._tree.Values); 
            }
        }

        /// <summary>
        /// Gets/Sets the value associated with a key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Value</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the given key does not exist in the dictionary</exception>
        public TValue this[TKey key]
        {
            get
            {
                return this._tree[key];
            }
            set
            {
                this._tree[key] = value;
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        /// <summary>
        /// Adds a key value pair to the dictionary
        /// </summary>
        /// <param name="item">Key Value pair</param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        /// <summary>
        /// Clears the dictionary
        /// </summary>
        public void Clear()
        {
            this._tree.Clear();
            this._count = 0;
        }

        /// <summary>
        /// Checks whether the dictionary contains the given key value pair
        /// </summary>
        /// <param name="item">Key Value pair</param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            TValue value;

            if (this.TryGetValue(item.Key, out value))
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
        /// Copies the contents of the dictionary to an array
        /// </summary>
        /// <param name="array">Array</param>
        /// <param name="arrayIndex">Index to start copying elements at</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array), "Cannot copy to a null array");
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Cannot start copying at index < 0");
            if (this.Count > array.Length - arrayIndex) throw new ArgumentException("Insufficient space in array");

            int i = arrayIndex;
            foreach (ITreeNode<TKey, TValue> node in this._tree.Nodes)
            {
                array[i] = new KeyValuePair<TKey, TValue>(node.Key, node.Value);
                i++;
            }
        }

        /// <summary>
        /// Gets the number of key value pairs in the dictionary
        /// </summary>
        public int Count
        {
            get 
            {
                return this._count;
            }
        }

        /// <summary>
        /// Gets whether the dictionary is read-only
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
        /// <param name="item">Key Value pair</param>
        /// <returns>True if a key value pair was removed, false otherwise</returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            TValue value;
            if (this.TryGetValue(item.Key, out value))
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
        /// Gets the enumerator of key value pairs
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return (from node in this._tree.Nodes
                    select new KeyValuePair<TKey, TValue>(node.Key, node.Value)).GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Gets the enumerator of values
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Gets the enumerator of values
        /// </summary>
        /// <returns></returns>
        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
        {
            return this._tree.Values.GetEnumerator();
        }
    }
}
