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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VDS.Common.Trees;

namespace VDS.Common.Collections
{
    /// <summary>
    /// A sorted list that supports duplicate entries
    /// </summary>
    /// <remarks>
    /// Note that duplicates are not stored directly so this should <strong>only</strong> be used if the user is happy to receive multiple instances of the first instance of duplicates seen
    /// </remarks>
    /// <typeparam name="T">Item type</typeparam>
    public class DuplicateSortedList<T>
        : IList<T>
    {
        private readonly IBinaryTree<T, int> _data;
        private readonly IComparer<T> _comparer; 
        private int _total;

        /// <summary>
        /// Creates a new list
        /// </summary>
        public DuplicateSortedList()
            : this(null, null) { }

        /// <summary>
        /// Creates a new list with the given items
        /// </summary>
        /// <param name="items">Items</param>
        public DuplicateSortedList(IEnumerable<T> items)
            : this(null, items) { }

        /// <summary>
        /// Creates a new list that uses the given comparer
        /// </summary>
        /// <param name="comparer">Comparer</param>
        public DuplicateSortedList(IComparer<T> comparer)
            : this(comparer, null) { }

        /// <summary>
        /// Creates a new list that uses the given comparer and has the given items
        /// </summary>
        /// <param name="comparer">Comparer</param>
        /// <param name="items">Items</param>
        public DuplicateSortedList(IComparer<T> comparer, IEnumerable<T> items)
        {
            this._comparer = comparer ?? Comparer<T>.Default;
            this._data = new AVLTree<T, int>(this._comparer);
            if (items == null) return;
            foreach (T item in items)
            {
                this.Add(item);
            }
        }

        /// <summary>
        /// Gets an enumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this._data.Nodes.SelectMany(n => Enumerable.Repeat(n.Key, n.Value)).GetEnumerator();
        }

        /// <summary>
        /// Gets ane enumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Adds an item to the list
        /// </summary>
        /// <param name="item">Item to add</param>
        public void Add(T item)
        {
            bool created;
            IBinaryTreeNode<T, int> node = this._data.MoveToNode(item, out created);
            if (created)
            {
                node.Value = 1;
            }
            else
            {
                node.Value++;
            }
            this._total++;
        }

        /// <summary>
        /// Clears the list
        /// </summary>
        public void Clear()
        {
            this._data.Clear();
            this._total = 0;
        }

        /// <summary>
        /// Gets if the list contains the given item
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>True if contained in the list, false otherwise</returns>
        public bool Contains(T item)
        {
            return this._data.ContainsKey(item);
        }

        /// <summary>
        /// Copies the list to the given array
        /// </summary>
        /// <param name="array">Array</param>
        /// <param name="arrayIndex">Array Index to start the copy at</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array), "Cannot copy to a null array");
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Cannot start copying at index < 0");
            if (this.Count > array.Length - arrayIndex) throw new ArgumentException("Insufficient space in array");

            int i = arrayIndex;
            foreach (T item in this)
            {
                array[i] = item;
                i++;
            }
        }

        /// <summary>
        /// Removes an item from the list
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>True if item was removed, false otherwise</returns>
        public bool Remove(T item)
        {
            int count;
            if (!this._data.TryGetValue(item, out count)) return false;
            if (count == 1)
            {
                this._data.Remove(item);
            }
            else
            {
                this._data[item] = count - 1;
            }
            this._total--;
            return true;
        }

        /// <summary>
        /// Gets the number of items in the list
        /// </summary>
        public int Count
        {
            get { return this._total; }
        }

        /// <summary>
        /// Gets whether the list is read only
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        private IBinaryTreeNode<T, int> MoveToIndex(int index)
        {
            if (this._data.Root == null) throw new InvalidOperationException("Root node is null");
            if (index < 0 || index >= this.Count) throw new ArgumentOutOfRangeException(nameof(index));

            // Special cases
            // Index 0 and only one node, return the root
            if (index == 0 && this.Count == 1) return this._data.Root;
            // Index 0, return the left most child
            if (index == 0) return this._data.Root.FindLeftmostChild();
            // Index == count - 1, return the right most child
            if (index == this.Count - 1) return this._data.Root.FindRightmostChild();

            long baseIndex = 0;
            IBinaryTreeNode<T, int> currentNode = this._data.Root;
            while (true)
            {
                long currentIndex = currentNode.LeftChild != null ? baseIndex + currentNode.LeftChild.Size + currentNode.LeftChild.Value - 1 : baseIndex;
                if (currentIndex == index) return currentNode;
                if (currentNode.LeftChild != null && index > currentIndex && index < currentIndex + currentNode.LeftChild.Value) return currentNode;

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
                    baseIndex = currentIndex + currentNode.Value;
                    currentNode = currentNode.RightChild;
                    continue;
                }
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets the index of a given item in the list
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns></returns>
        public int IndexOf(T item)
        {
            if (this._data.Root == null) return -1;

            IEnumerator<IBinaryTreeNode<T, int>> enumerator = this._data.Nodes.GetEnumerator();
            int index = 0;
            while (enumerator.MoveNext())
            {
                if (this._comparer.Compare(item, enumerator.Current.Key) == 0) return index;
                index += enumerator.Current.Value;
            }
            return -1;
        }

        /// <summary>
        /// Unsupported for this list
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="item">Item to insert</param>
        public void Insert(int index, T item)
        {
            throw new NotSupportedException("Cannot insert into a sorted list");
        }

        /// <summary>
        /// Removes the item at the given index
        /// </summary>
        /// <param name="index">Index</param>
        public void RemoveAt(int index)
        {
            IBinaryTreeNode<T, int> node = this.MoveToIndex(index);
            if (node.Value > 1)
            {
                node.Value--;
                this._total--;
            }
            else
            {
                this._data.Remove(node.Key);
            }
        }

        /// <summary>
        /// Gets the value at the given index, setting the value at an index is unsupported
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Value</returns>
        public T this[int index]
        {
            get { return this.MoveToIndex(index).Key; }
            set { throw new NotSupportedException("Cannot set value at a specific index in a sorted list"); }
        }
    }
}