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
using System.Threading;

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
    public class TrieNode<TKeyBit, TValue> 
        : ITrieNode<TKeyBit, TValue>, IDisposable
        where TValue : class
    {
        private readonly Dictionary<TKeyBit, ITrieNode<TKeyBit, TValue>> _children;
#if !PORTABLE
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
#else
        private readonly object _monitorObject = new object();
#endif
        /// <summary>
        /// Create an empty node with no children and null value
        /// </summary>
        /// <param name="parent">Parent node of this node</param>
        /// <param name="key">Key Bit</param>
        public TrieNode(ITrieNode<TKeyBit, TValue> parent, TKeyBit key)
        {
            this.KeyBit = key;
            this.Value = null;
            this.Parent = parent;
            this._children = new Dictionary<TKeyBit, ITrieNode<TKeyBit, TValue>>();
        }

        /// <summary>
        /// Enters a read lock on this node
        /// </summary>
        protected internal void EnterReadLock()
        {
#if !PORTABLE
            this._lock.EnterReadLock();
#else
            Monitor.Enter(this._monitorObject);
#endif
        }

        /// <summary>
        /// Exits a read lock on this node
        /// </summary>
        protected internal void ExitReadLock()
        {
#if !PORTABLE
            if(this._lock.IsReadLockHeld)
                this._lock.ExitReadLock();
#else
            if(Monitor.IsEntered(this._monitorObject))
                Monitor.Exit(this._monitorObject);
#endif
        }

        /// <summary>
        /// Enters an upgradeable read lock on this node
        /// </summary>
        protected internal void EnterUpgradeableReadLock()
        {
#if !PORTABLE
            this._lock.EnterUpgradeableReadLock();
#else
            Monitor.Enter(this._monitorObject );
#endif
        }

        /// <summary>
        /// Exits an upgradeable read lock on this node
        /// </summary>
        protected internal void ExitUpgradeableReadLock()
        {
#if !PORTABLE
            if(this._lock.IsUpgradeableReadLockHeld)
                this._lock.ExitUpgradeableReadLock();
#else
            if(Monitor.IsEntered(this._monitorObject))
                Monitor.Exit(this._monitorObject );
#endif
        }

        /// <summary>
        /// Enters a write lock on this node
        /// </summary>
        protected internal void EnterWriteLock()
        {
#if !PORTABLE
            this._lock.EnterWriteLock();
#else
            // Since we use a Monitor under PCL there is no difference between a read and write lock
            this.EnterReadLock();
#endif
        }

        /// <summary>
        /// Exits a write lock on this node
        /// </summary>
        protected internal void ExitWriteLock()
        {
#if !PORTABLE
            this._lock.ExitWriteLock();
#else
            // Since we use a Monitor under PCL there is no difference between a read and write lock
            this.ExitReadLock();
#endif
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

        /// <summary>
        /// Get a child of this node which is associated with a key bit
        /// </summary>
        /// <param name="key">Key associated with the child of interest</param>
        /// <returns>The child or null if no child is associated with the given key</returns>
        internal ITrieNode<TKeyBit, TValue> GetChild(TKeyBit key)
        {
            try
            {
                this.EnterReadLock();
                if (this._children.TryGetValue(key, out ITrieNode<TKeyBit, TValue> child)) return child;
            } 
            finally
            {
                this.ExitReadLock();
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
            try
            {
                this.EnterReadLock();
                return this._children.TryGetValue(key, out child);
            }
            finally
            {
                this.ExitReadLock();
            }
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
                try
                {
                    this.EnterReadLock();
                    return this._children.Count;
                }
                finally
                {
                    this.ExitReadLock();
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
                try
                {
                    this.EnterReadLock();
                    return this._children.Count == 0;
                }
                finally
                {
                    this.ExitReadLock();
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
            try
            {
                this.EnterReadLock();
                return this._children.ContainsKey(key);
            }
            finally
            {
                this.ExitReadLock();
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
            switch (depth)
            {
                case 0:
                    try
                    {
                        this.EnterWriteLock();
                        this._children.Clear();
                    }
                    finally
                    {
                        this.ExitWriteLock();
                    }

                    break;
                case > 0:
                    try
                    {
                        this.EnterReadLock();
                        foreach (ITrieNode<TKeyBit, TValue> node in this._children.Values)
                        {
                            node.Trim(depth - 1);
                        }
                    }
                    finally
                    {
                        this.ExitReadLock();
                    }

                    break;
                default:
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
            try
            {
                this.EnterReadLock();
                if (this._children.TryGetValue(key, out child))
                {
                    return child;
                }
            }
            finally
            {
                this.ExitReadLock();
            }
            try
            {
                this.EnterWriteLock();

                // There is a race condition where another thread might have entered 
                // the write lock and created the node while we were waiting to 
                // receive the write lock
                if (this._children.TryGetValue(key, out child))
                    return child;

                // Otherwise go ahead and create the child
                child = new TrieNode<TKeyBit, TValue>(this, key);
                this._children.Add(key, child);
                return child;
            }
            finally
            {
                this.ExitWriteLock();
            }

            // Alternative strategy using upgradeable locks
            // The problem is that this optimises the write case at the cost of decreasing performance
            // for the read case and reducing thread throughput for read biased workflows
            // This is because an upgradeable lock is equivalent to an exclusive lock

            //try
            //{
            //    this.EnterUpgradeableReadLock();

            //    if (this._children.TryGetValue(key, out child))
            //        return child;

            //    try
            //    {
            //        this.EnterWriteLock();

            //        // Otherwise go ahead and create the child
            //        child = new TrieNode<TKeyBit, TValue>(this, key);
            //        this._children.Add(key, child);
            //        return child;
            //    }
            //    finally
            //    {
            //        this.ExitWriteLock();
            //    }
            //}
            //finally
            //{
            //    this.ExitUpgradeableReadLock();
            //}
        }

        /// <summary>
        /// Remove the child of a node associated with a key along with all its descendents.
        /// </summary>
        /// <param name="key">The key associated with the child to remove.</param>
        public void RemoveChild(TKeyBit key)
        {
            try
            {
                this.EnterWriteLock();
                this._children.Remove(key);
            }
            finally
            {
                this.ExitWriteLock();
            }
        }

        /// <summary>
        /// Gets the immediate children of this Node
        /// </summary>
        public IEnumerable<ITrieNode<TKeyBit, TValue>> Children
        {
            get
            {
                return new TrieNodeChildrenEnumerable<TKeyBit, TValue>(this, this._children);
            }
        }

        /// <summary>
        /// Gets all descended children of this Node
        /// </summary>
        public IEnumerable<ITrieNode<TKeyBit, TValue>> Descendants
        {
            get
            {
                return new DescendantNodesEnumerable<TKeyBit, TValue>(this);
            }
        }

        /// <summary>
        /// Get an enumerable of values contained in this node and all its descendants
        /// </summary>
        public virtual IEnumerable<TValue> Values
        {
            get
            {
                return new TrieValuesEnumerable<TKeyBit, TValue>(this);
            }
        }

        /// <summary>
        /// Releases all unmanaged resources owned by this object.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _lock?.Dispose();
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    internal class TrieNodeChildrenEnumerable<TKeyBit, TValue>
        : IEnumerable<ITrieNode<TKeyBit, TValue>>
        where TValue : class
    {
        private readonly Dictionary<TKeyBit, ITrieNode<TKeyBit, TValue>> _children;
        private readonly TrieNode<TKeyBit, TValue> _node;

        public TrieNodeChildrenEnumerable(TrieNode<TKeyBit, TValue> node, Dictionary<TKeyBit, ITrieNode<TKeyBit, TValue>> children)
        {
            this._node = node ?? throw new ArgumentNullException(nameof(node));
            this._children = children ?? throw new ArgumentNullException(nameof(children));
        }

        public IEnumerator<ITrieNode<TKeyBit, TValue>> GetEnumerator()
        {
            try
            {
                this._node.EnterReadLock();
                // Take a copy so we can safely enumerate the children even if another thread is modifying the Trie
                return this._children.Values.ToList().GetEnumerator();
            }
            finally
            {
                this._node.ExitReadLock();
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}