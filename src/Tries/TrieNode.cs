/*
VDS.Common is licensed under the MIT License

Copyright (c) 2012-2015 Robert Vesse
Copyright (c) 2016-2025 dotNetRDF Project (https://dotnetrdf.org/)

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

namespace VDS.Common.Tries;

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
    : ITrieNode<TKeyBit, TValue>
    where TValue : class
{
    private readonly Dictionary<TKeyBit, ITrieNode<TKeyBit, TValue>> _children;
#if !PORTABLE
    private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
#endif

    /// <summary>
    /// Create an empty node with no children and null value
    /// </summary>
    /// <param name="parent">Parent node of this node</param>
    /// <param name="key">Key Bit</param>
    public TrieNode(ITrieNode<TKeyBit, TValue> parent, TKeyBit key)
    {
        KeyBit = key;
        Value = null;
        Parent = parent;
        _children = new Dictionary<TKeyBit, ITrieNode<TKeyBit, TValue>>();
    }

    /// <summary>
    /// Enters a read lock on this node
    /// </summary>
    protected internal void EnterReadLock()
    {
#if !PORTABLE
        _lock.EnterReadLock();
#else
            Monitor.Enter(this._children);
#endif
    }

    /// <summary>
    /// Exits a read lock on this node
    /// </summary>
    protected internal void ExitReadLock()
    {
#if !PORTABLE
        _lock.ExitReadLock();
#else
            Monitor.Exit(this._children);
#endif
    }

    /// <summary>
    /// Enters an upgradeable read lock on this node
    /// </summary>
    protected internal void EnterUpgradeableReadLock()
    {
#if !PORTABLE
        _lock.EnterUpgradeableReadLock();
#else
            Monitor.Enter(this._children);
#endif
    }

    /// <summary>
    /// Exits an upgradeable read lock on this node
    /// </summary>
    protected internal void ExitUpgradeableReadLock()
    {
#if !PORTABLE
        _lock.ExitUpgradeableReadLock();
#else
            Monitor.Enter(this._children);
#endif
    }

    /// <summary>
    /// Enters a write lock on this node
    /// </summary>
    protected internal void EnterWriteLock()
    {
#if !PORTABLE
        _lock.EnterWriteLock();
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
        _lock.ExitWriteLock();
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
        ITrieNode<TKeyBit, TValue> child;
        try
        {
            EnterReadLock();
            if (_children.TryGetValue(key, out child)) return child;
        } 
        finally
        {
            ExitReadLock();
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
            EnterReadLock();
            return _children.TryGetValue(key, out child);
        }
        finally
        {
            ExitReadLock();
        }
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
            try
            {
                EnterReadLock();
                return _children.Count;
            }
            finally
            {
                ExitReadLock();
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
            try
            {
                EnterReadLock();
                return _children.Count == 0;
            }
            finally
            {
                ExitReadLock();
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
            EnterReadLock();
            return _children.ContainsKey(key);
        }
        finally
        {
            ExitReadLock();
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
            try
            {
                EnterWriteLock();
                _children.Clear();
            }
            finally
            {
                ExitWriteLock();
            }
        }
        else if (depth > 0)
        {
            try
            {
                EnterReadLock();
                foreach (var node in _children.Values)
                {
                    node.Trim(depth - 1);
                }
            }
            finally
            {
                ExitReadLock();
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
        try
        {
            EnterReadLock();
            if (_children.TryGetValue(key, out child))
            {
                return child;
            }
        }
        finally
        {
            ExitReadLock();
        }
        try
        {
            EnterWriteLock();

            // There is a race condition where another thread might have entered 
            // the write lock and created the node while we were waiting to 
            // receive the write lock
            if (_children.TryGetValue(key, out child))
                return child;

            // Otherwise go ahead and create the child
            child = new TrieNode<TKeyBit, TValue>(this, key);
            _children.Add(key, child);
            return child;
        }
        finally
        {
            ExitWriteLock();
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
            EnterWriteLock();
            _children.Remove(key);
        }
        finally
        {
            ExitWriteLock();
        }
    }

    /// <summary>
    /// Gets the immediate children of this Node
    /// </summary>
    public IEnumerable<ITrieNode<TKeyBit, TValue>> Children => new TrieNodeChildrenEnumerable<TKeyBit, TValue>(this, _children);

    /// <summary>
    /// Gets all descended children of this Node
    /// </summary>
    public IEnumerable<ITrieNode<TKeyBit, TValue>> Descendants => new DescendantNodesEnumerable<TKeyBit, TValue>(this);

    /// <summary>
    /// Get an enumerable of values contained in this node and all its descendants
    /// </summary>
    public virtual IEnumerable<TValue> Values => new TrieValuesEnumerable<TKeyBit, TValue>(this);
}