/*
VDS.Common is licensed under the MIT License

Copyright (c) 2012-2015 Robert Vesse
Copyright (c) 2016-2017 dotNetRDF Project (http://dotnetrdf.org/)

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

namespace VDS.Common.Trees
{
    /// <summary>
    /// Interface for Trees
    /// </summary>
    /// <typeparam name="TNode">Node Type</typeparam>
    /// <typeparam name="TKey">Key Type</typeparam>
    /// <typeparam name="TValue">Value Type</typeparam>
    /// <remarks>
    /// A Tree is a mapping from keys to values stored in a tree structure, individual implementations control how the tree is stored and navigated
    /// </remarks>
    public interface ITree<TNode, TKey, TValue>
        where TNode : class, ITreeNode<TKey, TValue>
    {
        /// <summary>
        /// Gets the root of the tree
        /// </summary>
        TNode Root
        {
            get;
            set;
        }

        /// <summary>
        /// Adds a Key Value pair to the tree or replaces the existing value associated with a key
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <returns>True if a new node was created, false otherwise.</returns>
        /// <exception cref="ArgumentException">Thrown if a duplicate key is added</exception>
        bool Add(TKey key, TValue value);

        /// <summary>
        /// Finds a Node based on the key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Node associated with the given Key or null if the key is not present in the tree</returns>
        TNode Find(TKey key);

        /// <summary>
        /// Moves to a Node based on the key inserting a new Node if necessary
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="created">Whether a new node was inserted</param>
        /// <returns>Node associated with the given Key which may be newly inserted</returns>
        TNode MoveToNode(TKey key, out bool created);

        /// <summary>
        /// Removes a Node based on the Key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>True if a Node was removed</returns>
        bool Remove(TKey key);

        /// <summary>
        /// Determines whether a given Key exists in the Tree
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>True if the key exists in the Tree</returns>
        bool ContainsKey(TKey key);

        /// <summary>
        /// Gets/Sets the value for a key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Returns the value associated with the key</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the key doesn't exist</exception>
        TValue this[TKey key]
        {
            get;
            set;
        }

        /// <summary>
        /// Tries to get a value based on a key
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value or default for the value type if the key is not present</param>
        /// <returns>True if there is a value associated with the key</returns>
        bool TryGetValue(TKey key, out TValue value);

        /// <summary>
        /// Gets the Nodes of the tree
        /// </summary>
        IEnumerable<TNode> Nodes
        {
            get;
        }

        /// <summary>
        /// Gets the keys in the tree
        /// </summary>
        IEnumerable<TKey> Keys
        {
            get;
        }

        /// <summary>
        /// Gets the values in the tree
        /// </summary>
        IEnumerable<TValue> Values
        {
            get;
        }

        /// <summary>
        /// Empty the tree
        /// </summary>
        void Clear();
    }
}
