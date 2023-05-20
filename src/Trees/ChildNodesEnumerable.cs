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

namespace VDS.Common.Trees
{
    /// <summary>
    /// An enumerable over a binary tree which ensures that each time it is enumeraeted the latest state of the tree is enumerated
    /// </summary>
    /// <typeparam name="TNode">Node type</typeparam>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    internal class NodesEnumerable<TNode, TKey, TValue>
        : IEnumerable<IBinaryTreeNode<TKey, TValue>>
        where TNode : class, IBinaryTreeNode<TKey, TValue>
    {
        private readonly IBinaryTree<TKey, TValue> _tree;

        /// <summary>
        /// Creates a new nodes enumerable for a binary tree
        /// </summary>
        /// <param name="tree">Binary Tree</param>
        public NodesEnumerable(IBinaryTree<TKey, TValue> tree)
        {
            this._tree = tree ?? throw new ArgumentNullException(nameof(tree), "Tree cannot be null");
        }

        /// <summary>
        /// Gets an enumerator over the current state of the tree
        /// </summary>
        /// <returns>Enumerator over nodes</returns>
        public IEnumerator<IBinaryTreeNode<TKey, TValue>> GetEnumerator()
        {
            if (this._tree.Root == null)
            {
#if NET40
                return Enumerable.Empty<TNode>().GetEnumerator();
#else
                return Enumerable.Empty<IBinaryTreeNode<TKey, TValue>>().GetEnumerator();
#endif
            }
            else
            {
#if NET40
                return new LeftChildNodeEnumerable<TKey, TValue>(this._tree.Root).OfType<TNode>().Concat(this._tree.Root.AsEnumerable()).Concat(new RightChildNodeEnumerable<TKey, TValue>(this._tree.Root).OfType<TNode>()).GetEnumerator();
#else
                return (IEnumerator<IBinaryTreeNode<TKey, TValue>>)new LeftChildNodeEnumerable<TKey, TValue>(this._tree.Root).Concat(this._tree.Root.AsEnumerable()).Concat(new RightChildNodeEnumerable<TKey, TValue>(this._tree.Root)).GetEnumerator();
#endif
            }
        }

        /// <summary>
        /// Gets an enumerator over the current state of the tree
        /// </summary>
        /// <returns>Enumerator over nodes</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// An enumerable over a binary tree nodes children which ensures that each time it is enumerated the latest state of the tree is enumerated
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    internal abstract class ChildNodesEnumerable<TKey, TValue>
        : IEnumerable<IBinaryTreeNode<TKey,TValue>>
    {
        /// <summary>
        /// Parent node
        /// </summary>
        protected IBinaryTreeNode<TKey, TValue> _parent;

        /// <summary>
        /// Creates a new enumerable
        /// </summary>
        /// <param name="parent">Parent node</param>
        public ChildNodesEnumerable(IBinaryTreeNode<TKey, TValue> parent)
        {
            this._parent = parent ?? throw new ArgumentNullException(nameof(parent), "Parent cannot be null");
        }

        /// <summary>
        /// Get the child whose nodes we want to enumerate
        /// </summary>
        protected abstract IBinaryTreeNode<TKey, TValue> Child
        {
            get;
        }

        /// <summary>
        /// Gets an enumerator over the current state of one of the nodes children
        /// </summary>
        /// <returns>Enumerator over nodes</returns>
        public IEnumerator<IBinaryTreeNode<TKey, TValue>> GetEnumerator()
        {
            IBinaryTreeNode<TKey, TValue> child = this.Child;
            if (child == null) return Enumerable.Empty<IBinaryTreeNode<TKey, TValue>>().GetEnumerator();
            return child.Nodes.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator over the current state of one of the nodes children
        /// </summary>
        /// <returns>Enumerator over nodes</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// An enumerable over the left child of a binary tree node which ensures that each time it is enumerated the latest state of the tree is enumerated
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    internal class LeftChildNodeEnumerable<TKey, TValue>
        : ChildNodesEnumerable<TKey, TValue>
    {
        /// <summary>
        /// Creates a new enumerable
        /// </summary>
        /// <param name="parent">Parent node</param>
        public LeftChildNodeEnumerable(IBinaryTreeNode<TKey, TValue> parent)
            : base(parent) { }

        /// <summary>
        /// Gets the left child
        /// </summary>
        protected override IBinaryTreeNode<TKey, TValue> Child
        {
            get
            {
                return this._parent.LeftChild;
            }
        }
    }

    /// <summary>
    /// An enumerable over the right child of a binary tree node which ensures that each time it is enumerated the latest state of the tree is enumerated
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    internal class RightChildNodeEnumerable<TKey, TValue>
    : ChildNodesEnumerable<TKey, TValue>
    {
        /// <summary>
        /// Creates a new enumerable
        /// </summary>
        /// <param name="parent">Parent node</param>
        public RightChildNodeEnumerable(IBinaryTreeNode<TKey, TValue> parent)
            : base(parent) { }

        /// <summary>
        /// Gets the right child
        /// </summary>
        protected override IBinaryTreeNode<TKey, TValue> Child
        {
            get
            {
                return this._parent.RightChild;
            }
        }
    }
}
