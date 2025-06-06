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

namespace VDS.Common.Trees;

/// <summary>
/// An AVL tree implementation
/// </summary>
/// <typeparam name="TKey">Key Type</typeparam>
/// <typeparam name="TValue">Value Type</typeparam>
/// <remarks>
/// <para>
/// Code based in part on various examples from around the web including (but not limited to) <a href="http://www.vcskicks.com/AVL-tree.php">VCSKicks</a> and <a href="http://en.wikipedia.org/wiki/AVL_tree">Wikipedia</a>
/// </para>
/// </remarks>
public sealed class AvlTree<TKey, TValue>
    : BinaryTree<TKey, TValue>
{

    /// <summary>
    /// Creates a new AVL Tree
    /// </summary>
    public AvlTree()
    { }

    /// <summary>
    /// Creates a new AVL Tree using the given key comparer
    /// </summary>
    /// <param name="comparer">Key Comparer</param>
    public AvlTree(IComparer<TKey> comparer)
        : base(comparer) { }

    /// <summary>
    /// Creates a new node
    /// </summary>
    /// <param name="parent">Parent Node</param>
    /// <param name="key">Key</param>
    /// <param name="value">Value</param>
    /// <returns></returns>
    protected override IBinaryTreeNode<TKey, TValue> CreateNode(IBinaryTreeNode<TKey, TValue> parent, TKey key, TValue value)
    {
        return new BinaryTreeNode<TKey, TValue>(parent, key, value);
    }

    /// <summary>
    /// Applies rebalances after inserts
    /// </summary>
    /// <param name="parent">Parent</param>
    /// <param name="node">Newly isnerted node</param>
    protected override void AfterLeftInsert(IBinaryTreeNode<TKey, TValue> parent, IBinaryTreeNode<TKey, TValue> node)
    {
        RebalanceAfterInsert(node);
    }

    /// <summary>
    /// Applies rebalances after inserts
    /// </summary>
    /// <param name="parent">Parent</param>
    /// <param name="node">Newly isnerted node</param>
    protected override void AfterRightInsert(IBinaryTreeNode<TKey, TValue> parent, IBinaryTreeNode<TKey, TValue> node)
    {
        RebalanceAfterInsert(node);   
    }

    /// <summary>
    /// Applies rebalances after inserts
    /// </summary>
    /// <param name="node">Newly isnerted node</param>
    private void RebalanceAfterInsert(IBinaryTreeNode<TKey, TValue> node)
    {
        var current = node.Parent;
        while (current != null)
        {
            var balance = current.GetBalance();
            switch (balance)
            {
                case -2:
                case 2:
                    Rebalance(current, balance);
                    break;
                case -1:
                case 0:
                case 1:
                    //Balanced
                    break;
                default:
                    throw new InvalidOperationException("Illegal AVL Tree state");
            }
            current = current.Parent;
        }
    }

    /// <summary>
    /// Applies rebalances after deletes
    /// </summary>
    /// <param name="node">Node at which the delete occurred</param>
    protected override void AfterDelete(IBinaryTreeNode<TKey, TValue> node)
    {
        var current = node.Parent;
        while (current != null)
        {
            var balance = current.GetBalance();
            if (Math.Abs(balance) == 1) break; //Short circuit where possible
            if (Math.Abs(balance) == 2)
            {
                Rebalance(current, balance);
            }
            current = current.Parent;
        }
    }

    /// <summary>
    /// Applies tree rebalances
    /// </summary>
    /// <param name="node">Node</param>
    /// <param name="balance">Balance at the Node</param>
    private void Rebalance(IBinaryTreeNode<TKey, TValue> node, long balance)
    {
        switch (balance) {
            case 2:
                //Right subtree is heavier
                var rightBalance = node.RightChild.GetBalance();
                switch (rightBalance)
                {
                    case 1:
                    case 0:
                        //Left Rotation
                        RotateLeft(node);
                        break;
                    case -1:
                        //Right Rotation of right child followed by left rotation
                        RotateRight(node.RightChild);
                        RotateLeft(node);
                        break;
                }
                break;
            case -2:
                //Left subtree is heavier
                var leftBalance = node.LeftChild.GetBalance();
                switch (leftBalance) {
                    case 1:
                        //Left rotation of left child followed by right rotation
                        RotateLeft(node.LeftChild);
                        RotateRight(node);
                        break;
                    case -1:
                    case 0:
                        //Right rotation
                        RotateRight(node);
                        break;
                }
                break;
        }
    }

    /// <summary>
    /// Applies left rotation
    /// </summary>
    /// <param name="node">Node</param>
    private void RotateLeft(IBinaryTreeNode<TKey, TValue> node)
    {
        if (node == null) return;

        var pivot = node.RightChild;
        if (pivot == null) return;

        var parent = node.Parent;
        var left = parent != null && ReferenceEquals(node, parent.LeftChild);
        var atRoot = parent == null;

        //Update Parents
        node.Parent = pivot;
        pivot.Parent = parent;

        //Rotate
        node.RightChild = pivot.LeftChild;
        pivot.LeftChild = node;

        if (node.RightChild != null) node.RightChild.Parent = node;
        if (atRoot) Root = pivot;
        if (left)
        {
            parent.LeftChild = pivot;
        }
        else if (parent != null)
        {
            parent.RightChild = pivot;
        }
    }

    /// <summary>
    /// Applies right rotation
    /// </summary>
    /// <param name="node">Node</param>
    private void RotateRight(IBinaryTreeNode<TKey, TValue> node)
    {
        if (node == null) return;

        var pivot = node.LeftChild;
        if (pivot == null) return;

        var parent = node.Parent;
        var left = parent != null && ReferenceEquals(node, parent.LeftChild);
        var atRoot = parent == null;

        //Update Parents
        node.Parent = pivot;
        pivot.Parent = parent;

        //Rotate
        node.LeftChild = pivot.RightChild;
        pivot.RightChild = node;

        if (node.LeftChild != null) node.LeftChild.Parent = node;
        if (atRoot) Root = pivot;
        if (left)
        {
            parent.LeftChild = pivot;
        }
        else if (parent != null)
        {
            parent.RightChild = pivot;
        }
    }
}