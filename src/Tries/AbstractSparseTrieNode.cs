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

namespace VDS.Common.Tries
{
    /// <summary>
    /// Node of a Sparse Trie
    /// </summary>
    /// <typeparam name="TKeyBit">Key Bit Type</typeparam>
    /// <typeparam name="TValue">Value Type</typeparam>
    /// <remarks>
    /// </remarks>
    public abstract class AbstractSparseTrieNode<TKeyBit, TValue> 
        : ITrieNode<TKeyBit, TValue>
        where TKeyBit : IEquatable<TKeyBit>
        where TValue : class
    {
        private Dictionary<TKeyBit, ITrieNode<TKeyBit, TValue>> _children;

        /// <summary>
        /// Create an empty node with no children and null value
        /// </summary>
        /// <param name="parent">Parent node of this node</param>
        /// <param name="key">Key Bit</param>
        protected AbstractSparseTrieNode(ITrieNode<TKeyBit, TValue> parent, TKeyBit key)
        {
            KeyBit = key;
            Value = null;
            Parent = parent;
        }

        /// <summary>
        /// Gets whether the given key bit matches the current singleton
        /// </summary>
        /// <param name="key">Key Bit</param>
        /// <returns>True if it matches, false otherwise</returns>
        protected abstract bool MatchesSingleton(TKeyBit key);

        /// <summary>
        /// Creates a new child
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns></returns>
        protected abstract ITrieNode<TKeyBit, TValue> CreateNewChild(TKeyBit key);

        /// <summary>
        /// Gets/Sets the singleton child node
        /// </summary>
        protected abstract ITrieNode<TKeyBit, TValue> SingletonChild
        {
            get;
            set;
        }

        /// <summary>
        /// Clears the singleton
        /// </summary>
        protected abstract void ClearSingleton();

        /// <summary>
        /// Gets/Sets value stored by this node
        /// </summary>
        public TValue Value { get; set; }

        /// <summary>
        /// Gets the key bit that is associated with this node
        /// </summary>
        public TKeyBit KeyBit { get; }

        /// <summary>
        /// Get the parent of this node
        /// </summary>
        public ITrieNode<TKeyBit, TValue> Parent { get; }

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
                if (_children != null)
                {
                    return _children.TryGetValue(key, out child);
                }
                else if (MatchesSingleton(key))
                {
                    child = SingletonChild;
                    return true;
                }
            }
            child = null;
            return false;
        }

        /// <summary>
        /// Check whether this Node is the Root Node
        /// </summary>
        public bool IsRoot => Parent == null;

        /// <summary>
        /// Check whether or not this node has a value associated with it
        /// </summary>
        public bool HasValue => Value != null;

        /// <summary>
        /// Gets the number of immediate child nodes this node has
        /// </summary>
        public int Count
        {
            get
            {
                lock (this)
                {
                    if (_children != null)
                    {
                        return _children.Count;
                    }
                    else if (SingletonChild != null)
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
        public int CountAll => Descendants.Count();

        /// <summary>
        /// Check whether or not this node has any children.
        /// </summary>
        public bool IsLeaf
        {
            get
            {
                lock (this)
                {
                    if (_children != null)
                    {
                        return _children.Count == 0;
                    }
                    else
                    {
                        return SingletonChild == null;
                    }
                }
            }
        }

        /// <summary>
        /// Clears the Value (if any) stored at this node
        /// </summary>
        public void ClearValue()
        {
            Value = null;
        }

        /// <summary>
        /// Clears the Value (if any) stored at this node and removes all child nodes
        /// </summary>
        public void Clear()
        {
            ClearValue();
            Trim();
        }

        /// <summary>
        /// Removes all child nodes
        /// </summary>
        public void Trim()
        {
            Trim(0);
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
                    if (_children != null) _children.Clear();
                    ClearSingleton();
                }
            }
            else if (depth > 0)
            {
                lock (this)
                {
                    foreach (var node in Children)
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
            lock (this)
            {
                ITrieNode<TKeyBit, TValue> child;
                if (_children != null)
                {
                    // Get from existing children adding new child if necessary
                    if (_children.TryGetValue(key, out child))
                    {
                        return child;
                    }
                    else
                    {
                        child = CreateNewChild(key);
                        _children.Add(key, child);
                        return child;
                    }
                }
                else if (MatchesSingleton(key))
                {
                    // Can use existing singleton
                    return SingletonChild;
                }
                else if (SingletonChild != null)
                {
                    // Make non-singleton
                    _children = new Dictionary<TKeyBit, ITrieNode<TKeyBit, TValue>> { { SingletonChild.KeyBit, SingletonChild } };
                    child = CreateNewChild(key);
                    _children.Add(key, child);
                    return child;
                }
                else
                {
                    // Make singleton
                    SingletonChild = CreateNewChild(key);
                    return SingletonChild;
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
                if (_children != null)
                {
                    _children.Remove(key);
                }
                else if (MatchesSingleton(key))
                {
                    ClearSingleton();
                }
            }
        }

        /// <summary>
        /// Gets the immediate children of this Node
        /// </summary>
        public IEnumerable<ITrieNode<TKeyBit, TValue>> Children => new AbstractSparseTrieNodeChildrenEnumerable(this);

        /// <summary>
        /// Gets all descended children of this Node
        /// </summary>
        public IEnumerable<ITrieNode<TKeyBit, TValue>> Descendants => new DescendantNodesEnumerable<TKeyBit, TValue>(this);

        /// <summary>
        /// Get an enumerable of values contained in this node and all its descendants
        /// </summary>
        public IEnumerable<TValue> Values => new TrieValuesEnumerable<TKeyBit, TValue>(this);

        private class AbstractSparseTrieNodeChildrenEnumerable
            : IEnumerable<ITrieNode<TKeyBit, TValue>>
        {
            private readonly AbstractSparseTrieNode<TKeyBit, TValue> _node;

            public AbstractSparseTrieNodeChildrenEnumerable(AbstractSparseTrieNode<TKeyBit, TValue> node)
            {
                _node = node ?? throw new ArgumentNullException(nameof(node));
            }

            public IEnumerator<ITrieNode<TKeyBit, TValue>> GetEnumerator()
            {
                if (_node.IsLeaf)
                {
                    return Enumerable.Empty<ITrieNode<TKeyBit, TValue>>().GetEnumerator();
                }
                else if (_node._children != null)
                {
                    //May be multiple children present
                    return _node._children.Values.GetEnumerator();
                }
                else
                {
                    //Must be singleton child
                    return _node.SingletonChild.AsEnumerable().GetEnumerator();
                }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }

    
}