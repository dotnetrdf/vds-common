/*
VDS.Common is licensed under the MIT License

Copyright (c) 2009-2013 Robert Vesse

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
    /// Node of a Trie
    /// </summary>
    /// <typeparam name="TKeyBit">Key Bit Type</typeparam>
    /// <typeparam name="TValue">Value Type</typeparam>
    /// <remarks>
    /// <para>
    /// Original code taken from <a href="http://code.google.com/p/typocalypse/source/browse/#hg/Trie">Typocolypse</a> but has been heavily rewritten to be much more generic and LINQ friendly
    /// </para>
    /// </remarks>
    public class SparseTrieNode<TKeyBit, TValue> 
        : ITrieNode<TKeyBit, TValue>
        where TKeyBit : struct, IEquatable<TKeyBit>
        where TValue : class
    {
        private Nullable<TKeyBit> _singleton;
        private ITrieNode<TKeyBit, TValue> _singletonNode;
        private Dictionary<TKeyBit, ITrieNode<TKeyBit, TValue>> _children;

        /// <summary>
        /// Create an empty node with no children and null value
        /// </summary>
        /// <param name="parent">Parent node of this node</param>
        /// <param name="key">Key Bit</param>
        public SparseTrieNode(ITrieNode<TKeyBit, TValue> parent, TKeyBit key)
        {
            this.KeyBit = key;
            this.Value = null;
            this.Parent = parent;
            this._singleton = null;
        }

        /// <summary>
        /// Gets/Sets value stored by this node
        /// </summary>
        public TValue Value { get; set; }

        /// <summary>
        /// Gets the key bit that is associated with this node
        /// </summary>
        public TKeyBit KeyBit { get; private set; }

        /// <summary>
        /// Get the parent of this node
        /// </summary>
        public ITrieNode<TKeyBit, TValue> Parent { get; private set; }

        private bool MatchesSingleton(TKeyBit key)
        {
            return this._singleton.HasValue && this._singleton.Value.Equals(key);
        }

        /// <summary>
        /// Get a child of this node which is associated with a key bit
        /// </summary>
        /// <param name="key">Key associated with the child of interest</param>
        /// <returns>The child or null if no child is associated with the given key</returns>
        internal ITrieNode<TKeyBit, TValue> GetChild(TKeyBit key)
        {
            ITrieNode<TKeyBit, TValue> child;
            lock (this)
            {
                if (this._children != null)
                {
                    if (this._children.TryGetValue(key, out child)) return child;
                }
                else if (this.MatchesSingleton(key))
                {
                    return this._singletonNode;
                }
            }
            return null;
        }

        /// <summary>
        /// Tries to get a child of this node which is associated with a key bit
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="child">Child</param>
        /// <returns></returns>
        public bool TryGetChild(TKeyBit key, out ITrieNode<TKeyBit, TValue> child)
        {
            lock (this)
            {
                if (this._children != null)
                {
                    return this._children.TryGetValue(key, out child);
                }
                else if (this.MatchesSingleton(key))
                {
                    child = this._singletonNode;
                    return true;
                }
            }
            child = null;
            return false;
        }

        /// <summary>
        /// Check whether this Node is the Root Node
        /// </summary>
        public bool IsRoot
        {
            get
            {
                return this.Parent == null;
            }
        }

        /// <summary>
        /// Check whether or not this node has a value associated with it
        /// </summary>
        public bool HasValue
        {
            get
            {
                return this.Value != null;
            }
        }

        /// <summary>
        /// Gets the number of immediate child nodes this node has
        /// </summary>
        public int Count
        {
            get
            {
                lock (this)
                {
                    if (this._children != null)
                    {
                        return this._children.Count;
                    }
                    else if (this._singleton.HasValue)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the number of descendant nodes this node has
        /// </summary>
        public int CountAll
        {
            get
            {
                return this.Descendants.Count();
            }
        }

        /// <summary>
        /// Check whether or not this node has any children.
        /// </summary>
        public bool IsLeaf
        {
            get
            {
                lock (this)
                {
                    if (this._children != null)
                    {
                        return this._children.Count == 0;
                    }
                    else
                    {
                        return this._singleton.HasValue;
                    }
                }
            }
        }

        /// <summary>
        /// Check whether or not one of the children of this node uses the given key bit
        /// </summary>
        /// <param name="key">The key bit to check for</param>
        /// <returns>True if a child with given key exists, false otherwise</returns>
        internal bool ContainsKey(TKeyBit key)
        {
            lock (this)
            {
                if (this._children != null)
                {
                    return this._children.ContainsKey(key);
                } 
                else
                {
                    return this.MatchesSingleton(key);
                }
            }
        }

        /// <summary>
        /// Clears the Value (if any) stored at this node
        /// </summary>
        public void ClearValue()
        {
            this.Value = null;
        }

        /// <summary>
        /// Clears the Value (if any) stored at this node and removes all child nodes
        /// </summary>
        public void Clear()
        {
            this.ClearValue();
            this.Trim();
        }

        /// <summary>
        /// Removes all child nodes
        /// </summary>
        public void Trim()
        {
            this.Trim(0);
        }

        /// <summary>
        /// Removes all descendant nodes which are at the given depth below the current node
        /// </summary>
        /// <param name="depth">Depth</param>
        /// <exception cref="ArgumentException">Thrown if depth is less than zero</exception>
        public void Trim(int depth)
        {
            if (depth == 0)
            {
                lock (this)
                {
                    if (this._children != null) this._children.Clear();
                    this._singleton = null;
                    this._singletonNode = null;
                }
            }
            else if (depth > 0)
            {
                lock (this)
                {
                    foreach (ITrieNode<TKeyBit, TValue> node in this.Children)
                    {
                        node.Trim(depth - 1);
                    }
                }
            }
            else
            {
                throw new ArgumentException("Depth must be >= 0");
            }
        }

        /// <summary>
        /// Add a child node associated with a key to this node and return the node.
        /// </summary>
        /// <param name="key">Key to associated with the child node.</param>
        /// <returns>If given key already exists then return the existing child node, else return the new child node.</returns>
        public ITrieNode<TKeyBit, TValue> MoveToChild(TKeyBit key)
        {
            ITrieNode<TKeyBit, TValue> child;
            lock (this)
            {
                if (this._children != null)
                {
                    // Get from existing children adding new child if necessary
                    if (this._children.TryGetValue(key, out child))
                    {
                        return child;
                    }
                    else
                    {
                        child = new SparseTrieNode<TKeyBit, TValue>(this, key);
                        this._children.Add(key, child);
                        return child;
                    }
                }
                else if (this.MatchesSingleton(key))
                {
                    // Can use existing singleton
                    return this._singletonNode;
                }
                else if (this._singleton.HasValue)
                {
                    // Make non-singleton
                    this._children = new Dictionary<TKeyBit, ITrieNode<TKeyBit, TValue>>();
                    this._children.Add(this._singleton.Value, this._singletonNode);
                    child = new SparseTrieNode<TKeyBit, TValue>(this, key);
                    this._children.Add(key, child);
                    return child;
                }
                else
                {
                    // Make singleton
                    this._singleton = key;
                    this._singletonNode = new SparseTrieNode<TKeyBit, TValue>(this, key);
                    return this._singletonNode;
                }
            }
        }

        /// <summary>
        /// Remove the child of a node associated with a key along with all its descendents.
        /// </summary>
        /// <param name="key">The key associated with the child to remove.</param>
        public void RemoveChild(TKeyBit key)
        {
            lock (this)
            {
                if (this._children != null)
                {
                    this._children.Remove(key);
                }
                else if (this.MatchesSingleton(key))
                {
                    this._singleton = null;
                    this._singletonNode = null;
                }
            }
        }

        /// <summary>
        /// Gets the immediate children of this Node
        /// </summary>
        public IEnumerable<ITrieNode<TKeyBit, TValue>> Children
        {
            get
            {
                lock (this)
                {
                    if (this._children != null)
                    {
                        return this._children.Values.ToList();
                    }
                    else if (this._singleton.HasValue)
                    {
                        return this._singletonNode.AsEnumerable();
                    }
                    else
                    {
                        return Enumerable.Empty<ITrieNode<TKeyBit, TValue>>();
                    }
                }
            }
        }

        /// <summary>
        /// Gets all descended children of this Node
        /// </summary>
        public IEnumerable<ITrieNode<TKeyBit, TValue>> Descendants
        {
            get
            {
                lock (this)
                {
                    IEnumerable<ITrieNode<TKeyBit, TValue>> cs = this.Children;
                    return cs.Concat(from c in cs
                                     from d in c.Children
                                     select d); 
                }
            }
        }

        /// <summary>
        /// Get an enumerable of values contained in this node and all its descendants
        /// </summary>
        public virtual IEnumerable<TValue> Values
        {
            get
            {
                return this.Descendants.Select(d => d.Value);
            }
        }
    }
}