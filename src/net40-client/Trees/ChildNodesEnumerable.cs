using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        private BinaryTree<TNode, TKey, TValue> _tree;

        /// <summary>
        /// Creates a new nodes enumerable for a binary tree
        /// </summary>
        /// <param name="tree">Binary Tree</param>
        public NodesEnumerable(BinaryTree<TNode, TKey, TValue> tree)
        {
            if (tree == null) throw new ArgumentNullException("tree", "Tree cannot be null");
            this._tree = tree;
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
                return (IEnumerator<IBinaryTreeNode<TKey, TValue>>)new LeftChildNodeEnumerable<TKey, TValue>(this._tree.Root).OfType<TNode>().Concat(this._tree.Root.AsEnumerable()).Concat(new RightChildNodeEnumerable<TKey, TValue>(this._tree.Root).OfType<TNode>()).GetEnumerator();
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
            if (parent == null) throw new ArgumentNullException("parent", "Parent cannot be null");
            this._parent = parent;
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
