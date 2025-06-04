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

namespace VDS.Common.Trees
{
    /// <summary>
    /// Binary Tree node implementation
    /// </summary>
    /// <typeparam name="TKey">Key Type</typeparam>
    /// <typeparam name="TValue">Value Type</typeparam>
    public class BinaryTreeNode<TKey, TValue>
        : IBinaryTreeNode<TKey, TValue>
    {
        private IBinaryTreeNode<TKey, TValue> _left, _right;
        private long _height = 1;

        /// <summary>
        /// Creates a new Binary Tree Node
        /// </summary>
        /// <param name="parent">Parent</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        public BinaryTreeNode(IBinaryTreeNode<TKey, TValue> parent, TKey key, TValue value)
        {
            Parent = parent;
            Key = key;
            Value = value;
            Size = 1;
        }

        /// <summary>
        /// Gets/Sets the Parent Node (if any)
        /// </summary>
        public IBinaryTreeNode<TKey, TValue> Parent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/Sets the Left Child (if any)
        /// </summary>
        public IBinaryTreeNode<TKey, TValue> LeftChild
        {
            get => _left;
            set
            {
                _left = value;
                if (_left != null) _left.Parent = this;
                RecalculateHeight();
                RecalculateSize();
            }
        }

        /// <summary>
        /// Gets/Sets the Right Child (if any)
        /// </summary>
        public IBinaryTreeNode<TKey, TValue> RightChild
        {
            get => _right;
            set
            {
                _right = value;
                if (_right != null) _right.Parent = this;
                RecalculateHeight();
                RecalculateSize();
            }
        }

        /// <summary>
        /// Gets/Sets the Key
        /// </summary>
        public TKey Key
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/Sets the Value
        /// </summary>
        public TValue Value
        {
            get;
            set;
        }

        /// <summary>
        /// Gets whether this Node has children
        /// </summary>
        public bool HasChildren => LeftChild != null || RightChild != null;

        /// <summary>
        /// Gets the number of child nodes present (0, or 1)
        /// </summary>
        public int ChildCount => (LeftChild != null ? 1 : 0) + (RightChild != null ? 1 : 0);

        /// <summary>
        /// Gets the height of the subtree
        /// </summary>
        public long Height => _height;

        /// <summary>
        /// Recalculates the height of the subtree
        /// </summary>
        public void RecalculateHeight()
        {
            var newHeight = Math.Max(_left?.Height ?? 0, _right?.Height ?? 0) + 1;
            if (newHeight == _height) return;
            _height = newHeight;
            Parent?.RecalculateHeight();
        }

        /// <summary>
        /// Gets the size of the subtree i.e. number of nodes including this node in the count
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// Recalculates the size of the subtree
        /// </summary>
        public void RecalculateSize()
        {
            var leftSize = _left?.Size ?? 0;
            var rightSize = _right?.Size ?? 0;
            Size = leftSize + rightSize + 1;
            Parent?.RecalculateSize();
        }

        /// <summary>
        /// Gets the nodes of the subtree including this node
        /// </summary>
        public IEnumerable<IBinaryTreeNode<TKey, TValue>> Nodes => (LeftChild != null ? LeftChild.Nodes : Enumerable.Empty<IBinaryTreeNode<TKey, TValue>>()).Concat(((IBinaryTreeNode<TKey, TValue>)this).AsEnumerable()).Concat(RightChild != null ? RightChild.Nodes : Enumerable.Empty<IBinaryTreeNode<TKey, TValue>>());

        /// <summary>
        /// Gets a String representation of the node
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Key: " + Key.ToString() + " Value: " + Value.ToSafeString();
        }
    }
}
