using System;
using System.Collections.Generic;
using System.Linq;

namespace VDS.Common.Trees;

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
        _tree = tree ?? throw new ArgumentNullException(nameof(tree), "Tree cannot be null");
    }

    /// <summary>
    /// Gets an enumerator over the current state of the tree
    /// </summary>
    /// <returns>Enumerator over nodes</returns>
    public IEnumerator<IBinaryTreeNode<TKey, TValue>> GetEnumerator()
    {
        if (_tree.Root == null)
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
            return new LeftChildNodeEnumerable<TKey, TValue>(_tree.Root).Concat(_tree.Root.AsEnumerable()).Concat(new RightChildNodeEnumerable<TKey, TValue>(_tree.Root)).GetEnumerator();
#endif
        }
    }

    /// <summary>
    /// Gets an enumerator over the current state of the tree
    /// </summary>
    /// <returns>Enumerator over nodes</returns>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}