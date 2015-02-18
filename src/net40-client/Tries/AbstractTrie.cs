/*
VDS.Common is licensed under the MIT License

Copyright (c) 2012-2015 Robert Vesse

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

namespace VDS.Common.Tries
{
    /// <summary>
    /// An abstract implementation of a Trie
    /// </summary>
    /// <typeparam name="TKey">Type of keys</typeparam>
    /// <typeparam name="TKeyBit">Type of key bits</typeparam>
    /// <typeparam name="TValue">Type of values</typeparam>
    /// <remarks>
    /// <para>
    /// Keys are converted into a sequence of key bits using a user provided function
    /// </para>
    /// <para>
    /// The original code that inspired this was taken from <a href="http://code.google.com/p/typocalypse/source/browse/#hg/Trie">Typocolypse</a> but has been heavily rewritten to be much more abstract, generic and LINQ friendly
    /// </para>
    /// </remarks>
    public abstract class AbstractTrie<TKey, TKeyBit, TValue>
        : ITrie<TKey, TKeyBit, TValue>
        where TValue : class
    {
        /// <summary>
        /// Key Mapper function
        /// </summary>
        protected readonly Func<TKey, IEnumerable<TKeyBit>> _keyMapper;
        /// <summary>
        /// Root of the Trie
        /// </summary>
        protected readonly ITrieNode<TKeyBit, TValue> _root;
        
        /// <summary>
        /// Create an empty trie with an empty root node.
        /// </summary>
        public AbstractTrie(Func<TKey, IEnumerable<TKeyBit>> keyMapper)
        {
            if (keyMapper == null) throw new ArgumentNullException("keyMapper", "Key Mapper function cannot be null");
            this._keyMapper = keyMapper;
            this._root = this.CreateRoot(default(TKeyBit));
            this.KeyBitComparer = Comparer<TKeyBit>.Default;
        }

        /// <summary>
        /// Method which creates a new child node
        /// </summary>
        /// <param name="key">Key Bit</param>
        /// <returns></returns>
        protected abstract ITrieNode<TKeyBit, TValue> CreateRoot(TKeyBit key);

        /// <summary>
        /// Gets the Root Node of the Trie
        /// </summary>
        public ITrieNode<TKeyBit, TValue> Root
        {
            get
            {
                return this._root;
            }
        }

        /// <summary>
        /// Key comparer used for ordered operations like FindSuccessor
        /// </summary>
        public IComparer<TKeyBit> KeyBitComparer { get; set; }

        /// <summary>
        /// Adds a new key value pair, overwriting the existing value if the given key is already in use
        /// </summary>
        /// <param name="key">Key to search for value by</param>
        /// <param name="value">Value associated with key</param>
        public void Add(TKey key, TValue value)
        {
            ITrieNode<TKeyBit, TValue> node = _root;
            IEnumerable<TKeyBit> bs = this._keyMapper(key);
            foreach (TKeyBit b in bs)
            {
                node = node.MoveToChild(b);
            }
            node.Value = value;
        }

        /// <summary>
        /// Remove the value that a key leads to and any redundant nodes which result from this action
        /// </summary>
        /// <param name="key">Key of the value to remove</param>
        public void Remove(TKey key)
        {
            ITrieNode<TKeyBit, TValue> node = this._root;
            IEnumerable<TKeyBit> bs = this._keyMapper(key);
            foreach (TKeyBit b in bs)
            {
                //Bail out early if the key doesn't go anywhere
                if (!node.TryGetChild(b, out node)) return;
            }
            node.Value = null;

            //Remove all ancestor nodes which don't lead to a value.
            while (!node.IsRoot && !node.HasValue && node.Count == 0)
            {
                TKeyBit prevKey = node.KeyBit;
                node = node.Parent;
                node.RemoveChild(prevKey);
            }
        }


        /// <summary>
        /// Gets whether the Trie contains the given key value pair
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <returns>True if the key value pair exists, false otherwise</returns>
        public bool Contains(TKey key, TValue value)
        {
            ITrieNode<TKeyBit, TValue> node = this.Find(key);
            if (node == null) return false;
            if (node.HasValue && node.Value.Equals(value)) return true;
            if (!node.HasValue && value == null) return true;
            return false;
        }

        /// <summary>
        /// Gets whether the Trie contains a specific key and has a value associated with that key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>True if the Trie contains a specific key and has a value associated with it, false otherwise</returns>
        public bool ContainsKey(TKey key)
        {
            return this.ContainsKey(key, true);
        }

        /// <summary>
        /// Gets whether the Trie contains a specific key
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="requireValue">Whether the key is required to have a value associated with it in order to be considered as being contained</param>
        /// <returns>True if the Trie contains the given key and meets the value requirement</returns>
        public bool ContainsKey(TKey key, bool requireValue)
        {
            ITrieNode<TKeyBit, TValue> node = this.Find(key);
            if (node == null) return false;
            return (!requireValue || node.HasValue);
        }

        /// <summary>
        /// Finds and returns a Node for the given Key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Null if the Key does not map to a Node</returns>
        public ITrieNode<TKeyBit, TValue> Find(TKey key)
        {
            return this.Find(key, this._keyMapper);
        }

        /// <summary>
        /// Finds and returns a Node for the given Key using the given Key to Key Bit mapping function
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="keyMapper">Function to map keys to key bits</param>
        /// <returns>Null if the Key does not map to a Node</returns>
        /// <remarks>
        /// The ability to provide a custom mapping function allows you to do custom lookups into the Trie.  For example you might want to only match some portion of the key rather than the entire key
        /// </remarks>
        public ITrieNode<TKeyBit, TValue> Find(TKey key, Func<TKey, IEnumerable<TKeyBit>> keyMapper)
        {
            return this.Find(keyMapper(key));
        }

        /// <summary>
        /// Finds and returns a Node for the given sequence of Key Bits
        /// </summary>
        /// <param name="bs">Key Bits</param>
        /// <returns>Null if the Key does not map to a Node</returns>
        /// <remarks>
        /// The ability to provide a specific seqeunce of key bits may be useful for custom lookups where you don't necessarily have a value of the <strong>TKey</strong> type but do have values of the <strong>TKeyBit</strong> type
        /// </remarks>
        public ITrieNode<TKeyBit, TValue> Find(IEnumerable<TKeyBit> bs)
        {
            ITrieNode<TKeyBit, TValue> node = this._root;
            foreach (TKeyBit b in bs)
            {
                //Bail out early if key does not exist
                if (!node.TryGetChild(b, out node)) return null;
            }
            return node;
        }

        /// <summary>
        /// Finds and returns a Node that has the longest prefix match for the given sequence of Key Bits
        /// </summary>
        /// <param name="bs">Key Bits</param>
        /// <returns>Null if the Key does not map to a Node</returns>
        /// <remarks>
        /// The ability to provide a specific seqeunce of key bits may be useful for custom lookups where you don't necessarily have a value of the <strong>TKey</strong> type but do have values of the <strong>TKeyBit</strong> type
        /// </remarks>
        public ITrieNode<TKeyBit, TValue> FindPredecessor(TKey key)
        {
            return this.FindPredecessor(key, this._keyMapper);
        }

        /// <summary>
        /// Finds and returns a Node that has the longest prefix match for the given Key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Null if the Key does not map to a Node</returns>
        public ITrieNode<TKeyBit, TValue> FindPredecessor(TKey key, Func<TKey, IEnumerable<TKeyBit>> keyMapper)
        {
            return this.FindPredecessor(keyMapper(key));
        }

        /// <summary>
        /// Finds and returns a Node that has the longest prefix match for the given Key using the given Key to Key Bit mapping function
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="keyMapper">Function to map keys to key bits</param>
        /// <returns>Null if the Key does not map to a Node</returns>
        /// <remarks>
        /// The ability to provide a custom mapping function allows you to do custom lookups into the Trie.  For example you might want to only match some portion of the key rather than the entire key
        /// </remarks>
        public ITrieNode<TKeyBit, TValue> FindPredecessor(IEnumerable<TKeyBit> bs)
        {
            ITrieNode<TKeyBit, TValue> node = this._root;
            ITrieNode<TKeyBit, TValue> predecessor = null;

            foreach (TKeyBit b in bs)
            {
                //Bail out early if key does not exist
                if (!node.TryGetChild(b, out node))
                {
                    return predecessor;
                }

                if (node.HasValue)
                {
                    predecessor = node;
                }
            }

            return predecessor;
        }

        /// <summary>
        /// Finds and returns a Node that has the shortest prefix match greater than or equal to the given sequence of Key Bits
        /// </summary>
        /// <param name="bs">Key Bits</param>
        /// <returns>Null if the Key does not map to a Node</returns>
        /// <remarks>
        /// The ability to provide a specific seqeunce of key bits may be useful for custom lookups where you don't necessarily have a value of the <strong>TKey</strong> type but do have values of the <strong>TKeyBit</strong> type
        /// </remarks>
        public ITrieNode<TKeyBit, TValue> FindSuccessor(TKey key)
        {
            return this.FindSuccessor(key, this._keyMapper);
        }

        /// <summary>
        /// Finds and returns a Node that has the shortest prefix match greater than or equal to the given Key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Null if the Key does not map to a Node</returns>
        public ITrieNode<TKeyBit, TValue> FindSuccessor(TKey key, Func<TKey, IEnumerable<TKeyBit>> keyMapper)
        {
            return this.FindSuccessor(keyMapper(key));
        }

        /// <summary>
        /// Finds and returns a Node that has the shortest prefix match greater than or equal to the given Key using the given Key to Key Bit mapping function
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="keyMapper">Function to map keys to key bits</param>
        /// <returns>Null if the Key does not map to a Node</returns>
        /// <remarks>
        /// The ability to provide a custom mapping function allows you to do custom lookups into the Trie.  For example you might want to only match some portion of the key rather than the entire key
        /// </remarks>
        public ITrieNode<TKeyBit, TValue> FindSuccessor(IEnumerable<TKeyBit> bs)
        {
            ITrieNode<TKeyBit, TValue> node = this._root;

            foreach (TKeyBit b in bs)
            {
                //Bail out early if key does not exist
                if (!node.TryGetChild(b, out node))
                {
                    return null;
                }
            }
            
            Queue<ITrieNode<TKeyBit, TValue>> depthFirstQueue = new Queue<ITrieNode<TKeyBit, TValue>>();
            depthFirstQueue.Enqueue(node);

            while (depthFirstQueue.Count > 0)
            {
                node = depthFirstQueue.Dequeue();
                if (node.HasValue)
                {
                    return node;
                }

                foreach (var child in node.Children.OrderBy(n => n.KeyBit, this.KeyBitComparer))
                {
                    depthFirstQueue.Enqueue(child);
                }
            }

            return null;
        }

        /// <summary>
        /// Moves to the Node associated with the given Key creating new nodes if necessary
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Trie Node</returns>
        public ITrieNode<TKeyBit, TValue> MoveToNode(TKey key)
        {
            ITrieNode<TKeyBit, TValue> node = _root;
            IEnumerable<TKeyBit> bs = this._keyMapper(key);
            foreach (TKeyBit b in bs)
            {
                node = node.MoveToChild(b);
            }
            return node;
        }

        /// <summary>
        /// Gets/Sets the Value associated with a given Key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Value associated with the given Key, may be null if no value is associated</returns>
        /// <exception cref="KeyNotFoundException">Thrown if you try to get a value for a key that is not in the Trie</exception>
        public TValue this[TKey key]
        {
            get
            {
                ITrieNode<TKeyBit, TValue> node = this.Find(key);
                if (node == null) throw new KeyNotFoundException();
                return node.Value;
            }
            set
            {
                this.Add(key, value);
            }
        }

        /// <summary>
        /// Tries to get the Value associated with a given Key
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value, will be null if the key does not exist or has no value associated with it</param>
        /// <returns>True if the Key exists in the Trie and has a value associated with it, False if it does not</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            value = null;

            ITrieNode<TKeyBit, TValue> node = this._root;
            IEnumerable<TKeyBit> bs = this._keyMapper(key);
            foreach (TKeyBit b in bs)
            {
                //Bail out early if key does not exist
                if (!node.TryGetChild(b, out node))
                {
                    return false;
                }
            }
            if (node.HasValue)
            {
                value = node.Value;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the Count of all Nodes in the Trie
        /// </summary>
        public int Count
        {
            get
            {
                return this._root.CountAll;
            }
        }

        /// <summary>
        /// Gets all the Values in the Trie
        /// </summary>
        public IEnumerable<TValue> Values
        {
            get
            {
                return this._root.Values;
            }
        }

        /// <summary>
        /// Clears the Trie
        /// </summary>
        public void Clear()
        {
            this._root.Clear();
        }
    }
}