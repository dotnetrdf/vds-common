/*
VDS.Common is licensed under the MIT License

Copyright (c) 2012-2015 Robert Vesse

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

using System.Collections.Generic;

namespace VDS.Common.Trees
{
    /// <summary>
    /// Interface for Binary Tree Nodes
    /// </summary>
    /// <typeparam name="TKey">Key Type</typeparam>
    /// <typeparam name="TValue">Value Type</typeparam>
    public interface IBinaryTreeNode<TKey, TValue>
        : ITreeNode<TKey, TValue>
    {
        /// <summary>
        /// Gets the left child of this node
        /// </summary>
        IBinaryTreeNode<TKey, TValue> LeftChild
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the right child of this node
        /// </summary>
        IBinaryTreeNode<TKey, TValue> RightChild
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the parent of the node
        /// </summary>
        IBinaryTreeNode<TKey, TValue> Parent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the nodes present for the entire subtree (including this node)
        /// </summary>
        IEnumerable<IBinaryTreeNode<TKey, TValue>> Nodes
        {
            get;
        }

        /// <summary>
        /// Gets the Height of the subtree this node represents
        /// </summary>
        long Height
        {
            get;
        }

        /// <summary>
        /// Indicates that the node should recalculate the height of the subtree it represents
        /// </summary>
        void RecalculateHeight();

        /// <summary>
        /// Gets the size of the subtree this node represents i.e. number of nodes including this node
        /// </summary>
        long Size { get; }

        /// <summary>
        /// Indicates that the node should recalculate the size of the subtree it represents
        /// </summary>
        void RecalculateSize();
    }
}