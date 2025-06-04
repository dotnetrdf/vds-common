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
    /// A scapegoat tree implementation
    /// </summary>
    /// <typeparam name="TKey">Key Type</typeparam>
    /// <typeparam name="TValue">Value Type</typeparam>
    public sealed class ScapegoatTree<TKey, TValue>
        : BinaryTree<TKey, TValue>
    {
        private readonly double _balanceFactor = 0.75d;
        private long _nodeCount, _maxNodeCount;
        private readonly double _logBase = 1d / 0.75d;

        /// <summary>
        /// Creates a new Scapegoat Tree
        /// </summary>
        public ScapegoatTree()
        { }

        /// <summary>
        /// Creates a new Scapegoat Tree
        /// </summary>
        /// <param name="comparer">Key Comparer</param>
        public ScapegoatTree(IComparer<TKey> comparer)
            : base(comparer) { }

        /// <summary>
        /// Creates a new Scapegoat Tree
        /// </summary>
        /// <param name="balanceFactor">Balance Factor</param>
        public ScapegoatTree(double balanceFactor)
            : this(null, balanceFactor) { }

        /// <summary>
        /// Creates a new Scapegoat Tree
        /// </summary>
        /// <param name="comparer">Key Comparer</param>
        /// <param name="balanceFactor">Balance Factor</param>
        public ScapegoatTree(IComparer<TKey> comparer, double balanceFactor)
            : base(comparer)
        {
            if (balanceFactor < 0.5d || balanceFactor > 1.0d) throw new ArgumentOutOfRangeException(nameof(balanceFactor), "Must meet the condition 0.5 < balanceFactor < 1");
            _balanceFactor = balanceFactor;
            _logBase = 1d / _balanceFactor;
        }

        /// <summary>
        /// Creates a new Node
        /// </summary>
        /// <param name="parent">Parent</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <returns></returns>
        protected override IBinaryTreeNode<TKey, TValue> CreateNode(IBinaryTreeNode<TKey, TValue> parent, TKey key, TValue value)
        {
            return new BinaryTreeNode<TKey, TValue>(parent, key, value);
        }

        /// <summary>
        /// Applies rebalances after left inserts
        /// </summary>
        /// <param name="parent">Parent</param>
        /// <param name="node">Newly inserted nodes</param>
        protected override void AfterLeftInsert(IBinaryTreeNode<TKey, TValue> parent, IBinaryTreeNode<TKey, TValue> node)
        {
            _nodeCount++;
            _maxNodeCount = Math.Max(_maxNodeCount, _nodeCount);

            var depth = node.GetDepth();
            if (depth > Math.Log(_nodeCount, _logBase))
            {
                RebalanceAfterInsert(node);
            }
        }

        /// <summary>
        /// Applies rebalances after right inserts
        /// </summary>
        /// <param name="parent">Parent</param>
        /// <param name="node">Newly inserted nodes</param>
        protected override void AfterRightInsert(IBinaryTreeNode<TKey, TValue> parent, IBinaryTreeNode<TKey, TValue> node)
        {
            _nodeCount++;
            _maxNodeCount = Math.Max(_maxNodeCount, _nodeCount);

            var depth = node.GetDepth();
            if (depth > Math.Log(_nodeCount, _logBase))
            {
                RebalanceAfterInsert(node);
            }
        }

        /// <summary>
        /// Applies rebalances after inserts
        /// </summary>
        /// <param name="node">Newly inserted node</param>
        private void RebalanceAfterInsert(IBinaryTreeNode<TKey, TValue> node)
        {
            Rebalance(node, 1);
        }

        /// <summary>
        /// Applies rebalances after deletes
        /// </summary>
        /// <param name="node">Node the delete occurred at</param>
        private void RebalanceAfterDelete(IBinaryTreeNode<TKey, TValue> node)
        {
            Rebalance(node, node.GetSize());
        }

        /// <summary>
        /// Applies rebalances
        /// </summary>
        /// <param name="node">Node</param>
        /// <param name="selfSize">Size of the subtree the node represents</param>
        private void Rebalance(IBinaryTreeNode<TKey, TValue> node, long selfSize)
        {
            //Find the scapegoat
            var currSize = selfSize;
            long siblingSize, nodeSize;
            var current = node;
            do
            {
                //Get the sibling subtree size
                siblingSize = current.GetSibling().GetSize();
                if (current.Parent != null)
                {
                    //Total size of the Node is Current size of this subtree plus size of
                    //sibling subtree plus one for the current node
                    nodeSize = currSize + siblingSize + 1;
                    current = current.Parent;

                    //Is the current node weight balanced?
                    if (currSize <= (_balanceFactor * nodeSize) && siblingSize <= (_balanceFactor * siblingSize))
                    {
                        //Weight balanced so continue on
                        currSize = nodeSize;
                    }
                    else
                    {
                        //Not weight balanced so this is the scapegoat we rebalance from
                        break;
                    }
                }
                else
                {
                    //Rebalance at the root is gonna be O(n) for sure
                    break;
                }
            } while (current != null);

            //Check how we need to rebuild after the rebalance
            var parent = current.Parent;
            var rebuildLeft = false;
            if (parent != null)
            {
                rebuildLeft = ReferenceEquals(current, parent.LeftChild);
            }

            //Now do a rebalance of the scapegoat which will be whatever current is set to
            var nodes = current.Nodes.ToArray();
            foreach (var n in nodes)
            {
                n.Isolate();
            }

            var median = nodes.Length / 2;
            //Console.WriteLine("m = " + median);
            var root = nodes[median];
            root.LeftChild = RebalanceLeftSubtree(nodes, 0, median - 1);
            root.RightChild = RebalanceRightSubtree(nodes, median + 1, nodes.Length - 1);

            //Don't use this check because it's expensive, may be useful to turn of for debugging if you ever have issues with the ScapegoatTree
            //if (root.Nodes.Count() != nodes.Length) throw new InvalidOperationException("Scapegoat rebalance lost data, expected " + nodes.Length + " Nodes in rebalanced sub-tree but got " + root.Nodes.Count());

            //Use the rebalanced tree in place of the current node
            if (parent == null)
            {
                //Replace entire tree
                Root = root;
            }
            else
            {
                //Replace subtree
                if (rebuildLeft)
                {
                    parent.LeftChild = root;
                }
                else
                {
                    parent.RightChild = root;
                }
            }

            //Reset Max Node code after a rebalance
            _maxNodeCount = _nodeCount;
        }

        /// <summary>
        /// Rebalances a left subtree
        /// </summary>
        /// <param name="nodes">Nodes</param>
        /// <param name="start">Range start</param>
        /// <param name="end">Range end</param>
        /// <returns></returns>
        private IBinaryTreeNode<TKey, TValue> RebalanceLeftSubtree(IBinaryTreeNode<TKey, TValue>[] nodes, int start, int end)
        {
            if (start > end) return null;
            if (end == start) return nodes[start];
            if (end - start == 1)
            {
                var root = nodes[end];
                root.LeftChild = nodes[start];
                return root;
            }
            else if (end - start == 2)
            {
                var root = nodes[start + 1];
                root.LeftChild = nodes[start];
                root.RightChild = nodes[end];
                return root;
            }
            else
            {

                //Rebuild the tree
                var median = start + ((end - start) / 2);
                //Console.WriteLine("m = " + median);
                var root = nodes[median];
                root.LeftChild = RebalanceLeftSubtree(nodes, start, median - 1);
                root.RightChild = RebalanceRightSubtree(nodes, median + 1, end);
                return root;
            }
        }

        /// <summary>
        /// Rebalances a right subtree
        /// </summary>
        /// <param name="nodes">Nodes</param>
        /// <param name="start">Range start</param>
        /// <param name="end">Range end</param>
        /// <returns></returns>
        private IBinaryTreeNode<TKey, TValue> RebalanceRightSubtree(IBinaryTreeNode<TKey, TValue>[] nodes, int start, int end)
        {
            if (start > end) return null;
            if (end == start) return nodes[start];
            if (end - start == 1)
            {
                var root = nodes[start];
                root.RightChild = nodes[end];
                return root;
            }
            else if (end - start == 2)
            {
                var root = nodes[start + 1];
                root.LeftChild = nodes[start];
                root.RightChild = nodes[end];
                return root;
            }
            else
            {
                //Rebuild the tree
                var median = start + ((end - start) / 2);
                //Console.WriteLine("m = " + median);
                var root = nodes[median];
                root.LeftChild = RebalanceLeftSubtree(nodes, start, median - 1);
                root.RightChild = RebalanceRightSubtree(nodes, median + 1, end);
                return root;
            }
        }

        /// <summary>
        /// Applies rebalances after deletes
        /// </summary>
        /// <param name="node">Node the delete occurred at</param>
        protected override void AfterDelete(IBinaryTreeNode<TKey, TValue> node)
        {
            _nodeCount--;

            if (_nodeCount <= (_maxNodeCount / 2))
            {
                RebalanceAfterDelete(node);
            }
        }

        /// <summary>
        /// Resets node counts after a clear
        /// </summary>
        protected override void AfterClear()
        {
            _nodeCount = 0;
            _maxNodeCount = 0;
        }
    }
}
