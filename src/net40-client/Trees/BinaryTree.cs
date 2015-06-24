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

using System;
using System.Collections.Generic;
using System.Linq;

namespace VDS.Common.Trees
{
    /// <summary>
    /// Abstract base implementation of an unbalanced binary search tree
    /// </summary>
    /// <typeparam name="TKey">Key Type</typeparam>
    /// <typeparam name="TValue">Value Type</typeparam>
    public abstract class BinaryTree<TKey, TValue>
        : IBinaryTree<TKey, TValue>
    {
        /// <summary>
        /// Key Comparer
        /// </summary>
        protected IComparer<TKey> _comparer = Comparer<TKey>.Default;

        /// <summary>
        /// Creates a new unbalanced Binary Tree
        /// </summary>
        protected BinaryTree()
            : this(null) {}

        /// <summary>
        /// Creates a new unbalanced Binary Tree
        /// </summary>
        /// <param name="comparer">Comparer for keys</param>
        protected BinaryTree(IComparer<TKey> comparer)
        {
            this._comparer = (comparer ?? this._comparer);
        }

        /// <summary>
        /// Gets/Sets the Root of the Tree
        /// </summary>
        public virtual IBinaryTreeNode<TKey,TValue> Root
        {
            get;
            set;
        }


        /// <summary>
        /// Adds a Key Value pair to the tree, replaces an existing value if the key already exists in the tree
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown if a duplicate key is used</exception>
        public virtual bool Add(TKey key, TValue value)
        {
            if (this.Root == null)
            {
                //No root yet so just insert at the root
                this.Root = this.CreateNode(null, key, value);
                return true;
            }
            //Move to the node
            bool created = false;
            IBinaryTreeNode<TKey, TValue> node = this.MoveToNode(key, out created);
            if (!created) throw new ArgumentException("Duplicate keys are not permitted");
            node.Value = value;
            return true;
        }

        /// <summary>
        /// Creates a new node for the tree
        /// </summary>
        /// <param name="parent">Parent node</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <returns></returns>
        protected abstract IBinaryTreeNode<TKey, TValue> CreateNode(IBinaryTreeNode<TKey, TValue> parent, TKey key, TValue value);

        /// <summary>
        /// Finds a Node based on the key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Node associated with the given Key or null if the key is not present in the tree</returns>
        public virtual IBinaryTreeNode<TKey, TValue> Find(TKey key)
        {
            if (this.Root == null) return null;

            //Iteratively binary search for the key
            IBinaryTreeNode<TKey, TValue> current = this.Root;
            do
            {
                int c = this._comparer.Compare(key, current.Key);
                if (c < 0)
                {
                    current = current.LeftChild;
                }
                else if (c > 0)
                {
                    current = current.RightChild;
                }
                else
                {
                    //If we find a match on the key then return it
                    return current;
                }
            } while (current != null);

            return null;
        }

        /// <summary>
        /// Moves to the node with the given key inserting a new node if necessary
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="created">Whether a new node was inserted</param>
        /// <returns></returns>
        public virtual IBinaryTreeNode<TKey, TValue> MoveToNode(TKey key, out bool created)
        {
            if (this.Root == null)
            {
                this.Root = this.CreateNode(null, key, default(TValue));
                created = true;
                return this.Root;
            }
            //Iteratively binary search for the key
                IBinaryTreeNode<TKey, TValue> current = this.Root;
                IBinaryTreeNode<TKey, TValue> parent = null;
            int c;
            do
            {
                c = this._comparer.Compare(key, current.Key);
                if (c < 0)
                {
                    parent = current;
                        current = current.LeftChild;
                }
                else if (c > 0)
                {
                    parent = current;
                        current = current.RightChild;
                }
                else
                {
                    //If we find a match on the key then return it
                    created = false;
                    return current;
                }
            } while (current != null);

            //Key doesn't exist so need to do an insert
            current = this.CreateNode(parent, key, default(TValue));
            created = true;
            if (c < 0)
            {
                parent.LeftChild = current;
                this.AfterLeftInsert(parent, current);
            }
            else
            {
                parent.RightChild = current;
                this.AfterRightInsert(parent, current);
            }

            //Return the newly inserted node
            return current;
        }

        /// <summary>
        /// Virtual method that can be used by derived implementations to perform tree balances after an insert
        /// </summary>
        /// <param name="parent">Parent</param>
        /// <param name="node">Newly inserted node</param>
        protected virtual void AfterLeftInsert(IBinaryTreeNode<TKey, TValue> parent, IBinaryTreeNode<TKey, TValue> node) {}

        /// <summary>
        /// Virtual method that can be used by derived implementations to perform tree balances after an insert
        /// </summary>
        /// <param name="parent">Parent</param>
        /// <param name="node">Newly inserted node</param>
        protected virtual void AfterRightInsert(IBinaryTreeNode<TKey, TValue> parent, IBinaryTreeNode<TKey, TValue> node) {}

        /// <summary>
        /// Removes a Node based on the Key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>True if a Node was removed</returns>
        public virtual bool Remove(TKey key)
        {
            //If empty tree any remove always returns false
            if (this.Root == null) return false;

            //Iteratively binary search for the key
            IBinaryTreeNode<TKey, TValue> current = this.Root;
            int c;
            do
            {
                c = this._comparer.Compare(key, current.Key);
                if (c < 0)
                {
                    current = current.LeftChild;
                }
                else if (c > 0)
                {
                    current = current.RightChild;
                }
                else
                {
                    //If we find a match on the key then stop searching
                    //Calculate the comparison with the parent key (if there is a parent) as we need this info
                    //for the deletion implementation
                    c = (current.Parent != null ? this._comparer.Compare(current.Key, current.Parent.Key) : c);
                    break;
                }
            } while (current != null);

            // Node not found so nothing to remove
            if (current == null) return false;

            return RemoveNode(current, c);
        }

        /// <summary>
        /// Removes a node
        /// </summary>
        /// <param name="current">Node to be removed</param>
        /// <param name="c">Comparison of this node versus its parent</param>
        /// <returns>True if node removed</returns>
        protected bool RemoveNode(IBinaryTreeNode<TKey, TValue> current, int c)
        {
            //Perform the delete if we found a node
            if (current.ChildCount == 2)
            {
                //Has two children
                //Therefore we need to get the in order successor of the left child (which must exist) and move it's key and value
                //to this node and then delete the successor
                    IBinaryTreeNode<TKey, TValue> successor = this.FindRightmostChild(current.LeftChild);
                if (ReferenceEquals(successor, current.LeftChild))
                {
                    //If the successor is just the left child i.e. the left child has no right children
                    //then we can simply move the left child up to this position
                    current.Key = successor.Key;
                    current.Value = successor.Value;
                    current.LeftChild = successor.LeftChild;
                    this.AfterDelete(current);
                    return true;
                }
                //We've found a successor which is a rightmost child of our left child
                //Move it's value to here and either delete the successor if it was a leaf
                //node or move up it's left child - note it can never have a right child because
                //we traversed to the rightmost child
                current.Key = successor.Key;
                current.Value = successor.Value;
                successor.Parent.RightChild = successor.HasChildren ? successor.LeftChild : null;
                this.AfterDelete(current);
                return true;
            }
            if (current.HasChildren)
            {
                //Is an internal node with a single child
                //Thus just set the appropriate child of the parent to the appropriate child of the node we are deleting
                if (c < 0)
                {
                    current.Parent.LeftChild = (current.LeftChild ?? current.RightChild);
                        this.AfterDelete(current.Parent);
                    return true;
                }
                if (c > 0)
                {
                    current.Parent.RightChild = (current.RightChild ?? current.LeftChild);
                        this.AfterDelete(current.Parent);
                    return true;
                }
                        IBinaryTreeNode<TKey, TValue> successor;
                if (current.LeftChild != null)
                {
                    //Has a left subtree so get the in order successor which is the rightmost child of the left
                    //subtree
                            successor = this.FindRightmostChild(current.LeftChild);
                    if (ReferenceEquals(current.LeftChild, successor))
                    {
                        //There were no right children on the left subtree
                        if (current.Parent == null)
                        {
                            //At Root and no right child of left subtree so can move left child up to root
                                    this.Root = current.LeftChild;
                            if (this.Root != null) this.Root.Parent = null;
                            this.AfterDelete(this.Root);
                            return true;
                        }
                        else
                        {
                            //Not at Root and no right child of left subtree so can move left child up
                            current.Parent.LeftChild = current.LeftChild;
                                    this.AfterDelete(current.Parent.LeftChild);
                            return true;
                        }
                    }
                    //Move value up to this node and delete the rightmost child
                    current.Key = successor.Key;
                    current.Value = successor.Value;

                    //Watch out for the case where the rightmost child had a left child
                    if (successor.Parent.RightChild.HasChildren)
                    {
                        successor.Parent.RightChild = successor.LeftChild;
                    }
                    else
                    {
                        successor.Parent.RightChild = null;
                    }
                    this.AfterDelete(current);
                    return true;
                }
                //Must have a right subtree so find the in order sucessor which is the
                //leftmost child of the right subtree
                            successor = this.FindLeftmostChild(current.RightChild);
                if (ReferenceEquals(current.RightChild, successor))
                {
                    //There were no left children on the right subtree
                    if (current.Parent == null)
                    {
                        //At Root and no left child of right subtree so can move right child up to root
                                    this.Root = current.RightChild;
                        if (this.Root != null) this.Root.Parent = null;
                        this.AfterDelete(this.Root);
                        return true;
                    }
                    //Not at Root and no left child of right subtree so can move right child up
                    current.Parent.RightChild = current.RightChild;
                                    this.AfterDelete(current.Parent.RightChild);
                    return true;
                }
                //Move value up to this node and delete the leftmost child
                current.Key = successor.Key;
                current.Value = successor.Value;

                //Watch out for the case where the lefttmost child had a right child
                successor.Parent.LeftChild = successor.Parent.LeftChild.HasChildren ? successor.RightChild : null;
                this.AfterDelete(current);
                return true;
            }
            //Must be an external node
            //Thus just set the appropriate child of the parent to be null
            if (c < 0)
            {
                current.Parent.LeftChild = null;
                        this.AfterDelete(current.Parent);
                return true;
            }
            if (c > 0)
            {
                current.Parent.RightChild = null;
                        this.AfterDelete(current.Parent);
                return true;
            }
            //Root of tree is only way we can get here so just
            //set root to null
            this.Root = null;
            return true;
        }

        /// <summary>
        /// Finds the leftmost child of the given node
        /// </summary>
        /// <param name="node">Node</param>
        /// <returns></returns>
        protected IBinaryTreeNode<TKey, TValue> FindLeftmostChild(IBinaryTreeNode<TKey, TValue> node)
        {
            while (node.LeftChild != null)
            {
                node = node.LeftChild;
            }
            return node;
        }

        /// <summary>
        /// Finds the rightmost child of the given node
        /// </summary>
        /// <param name="node">Node</param>
        /// <returns></returns>
        protected IBinaryTreeNode<TKey, TValue> FindRightmostChild(IBinaryTreeNode<TKey, TValue> node)
        {
            while (node.RightChild != null)
            {
                node = node.RightChild;
            }
            return node;
        }

        /// <summary>
        /// Virtual method that can be used by derived implementations to perform tree balances after a delete
        /// </summary>
        /// <param name="node">Node at which the deletion happened</param>
        protected virtual void AfterDelete(IBinaryTreeNode<TKey, TValue> node) { }

        /// <summary>
        /// Determines whether a given Key exists in the Tree
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>True if the key exists in the Tree</returns>
        public virtual bool ContainsKey(TKey key)
        {
            return this.Find(key) != null;
        }

        /// <summary>
        /// Gets/Sets the value for a key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Returns the value associated with the key</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the key doesn't exist</exception>
        public TValue this[TKey key]
        {
            get
            {
                IBinaryTreeNode<TKey, TValue> n = this.Find(key);
                if (n != null)
                {
                    return n.Value;
                }
                throw new KeyNotFoundException();
            }
            set
            {
                bool created = false;
                IBinaryTreeNode<TKey, TValue> n = this.MoveToNode(key, out created);
                if (n != null)
                {
                    n.Value = value;
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
        }

        /// <summary>
        /// Tries to get a value based on a key
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value or default for the value type if the key is not present</param>
        /// <returns>True if there is a value associated with the key</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            IBinaryTreeNode<TKey, TValue> n = this.Find(key);
            if (n != null)
            {
                value = n.Value;
                return true;
            }
            value = default(TValue);
            return false;
        }

        /// <summary>
        /// Tries to move to a node based on its index
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Node</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index is out of range</exception>
        protected IBinaryTreeNode<TKey, TValue> MoveToIndex(int index)
        {
            if (this.Root == null) throw new IndexOutOfRangeException();
            int count = this.Root.Size;
            if (index < 0 || index >= count) throw new IndexOutOfRangeException();

            // Special cases
            // Index 0 and only one node, return the root
            if (index == 0 && count == 1) return this.Root;
            // Index 0, return the left most child
            if (index == 0) return FindLeftmostChild(this.Root);
            // Index == count - 1, return the right most child
            if (index == 0) return FindRightmostChild(this.Root);

            long baseIndex = 0;
            IBinaryTreeNode<TKey, TValue> currentNode = this.Root;
            while (true)
            {
                long currentIndex = currentNode.LeftChild != null ? baseIndex + currentNode.LeftChild.Size : baseIndex;
                if (currentIndex == index) return currentNode;

                if (currentIndex > index)
                {
                    // We're at a node where our calculated index is greater than the desired so need to move to the left sub-tree
                    currentNode = currentNode.LeftChild;
                    continue;
                }

                // We're at a node where our calculated index is less than the desired so need to move to the right sub-tree
                // Plus we need to adjust the base appropriately
                if (currentNode.RightChild != null)
                {
                    currentNode = currentNode.RightChild;
                    baseIndex = currentIndex + 1;
                    continue;
                }
                throw new InvalidOperationException();
            }
        }

        public TValue GetValueAt(int index)
        {
            IBinaryTreeNode<TKey, TValue> node = this.MoveToIndex(index);
            return node.Value;
        }

        public void SetValueAt(int index, TValue value)
        {
            IBinaryTreeNode<TKey, TValue> node = this.MoveToIndex(index);
            node.Value = value;
        }

        public void RemoveAt(int index)
        {
            IBinaryTreeNode<TKey, TValue> node = this.MoveToIndex(index);
            int c = node.Parent != null ? this._comparer.Compare(node.Key, node.Parent.Key) : 0;
            this.RemoveNode(node, c);
        }

        /// <summary>
        /// Gets the Nodes of the Tree
        /// </summary>
        public IEnumerable<IBinaryTreeNode<TKey, TValue>> Nodes
        {
            get { return new NodesEnumerable<IBinaryTreeNode<TKey, TValue>, TKey, TValue>(this); }
        }

        /// <summary>
        /// Gets the Keys of the Tree
        /// </summary>
        public IEnumerable<TKey> Keys
        {
            get
            {
                return (from n in this.Nodes
                    select n.Key);
            }
        }

        /// <summary>
        /// Gets the Values of the Tree
        /// </summary>
        public IEnumerable<TValue> Values
        {
            get
            {
                return (from n in this.Nodes
                    select n.Value);
            }
        }

        /// <summary>
        /// Clears the tree
        /// </summary>
        public void Clear()
        {
            this.Root = null;
            this.AfterClear();
        }

        /// <summary>
        /// Virtual method that can be used by derived implementations to perform clean up after a clear
        /// </summary>
        protected virtual void AfterClear() {}
    }
}