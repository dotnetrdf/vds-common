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
using System.Collections;
using System.Collections.Generic;
using VDS.Common.Trees;

namespace VDS.Common.Collections
{
    /// <summary>
    /// A sparse array implementation backed by a binary tree
    /// </summary>
    /// <remarks>
    /// This implementation provides a trade off between look up time and memory usage and so provides a compromise between the <see cref="BlockSparseArray{T}"/> and <see cref="LinkedSparseArray{T}"/>
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public class BinarySparseArray<T>
        : ISparseArray<T>
    {
        private readonly ITree<IBinaryTreeNode<int, T>, int, T> _tree;

        /// <summary>
        /// Creates a new sparse array
        /// </summary>
        /// <param name="length">Length</param>
        public BinarySparseArray(int length)
        {
            if (length < 0) throw new ArgumentException("Length must be >= 0", "length");
            this._tree = new AVLTree<int, T>(Comparer<Int32>.Default);
            this.Length = length;
        }


        /// <summary>
        /// Gets an enumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new BinarySparseArrayEnumerator<T>(this.Length, this._tree.Nodes.GetEnumerator());
        }

        /// <summary>
        /// Gets an enumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets/Sets the value at a given index
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Value</returns>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= this.Length) throw new IndexOutOfRangeException(String.Format("Index must be in the range 0 to {0}", this.Length - 1));
                T value;
                return this._tree.TryGetValue(index, out value) ? value : default(T);
            }
            set
            {
                if (index < 0 || index >= this.Length) throw new IndexOutOfRangeException(String.Format("Index must be in the range 0 to {0}", this.Length - 1));
                this._tree.Add(index, value);
            }
        }

        /// <summary>
        /// Gets the length of the array
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Clears the array
        /// </summary>
        public void Clear()
        {
            this._tree.Clear();
        }
    }

    class BinarySparseArrayEnumerator<T>
        : IEnumerator<T>
    {
        public BinarySparseArrayEnumerator(int length, IEnumerator<IBinaryTreeNode<int, T>> nodesEnumerator)
        {
            this.Length = length;
            this.Nodes = nodesEnumerator;
            this.Index = -1;
        }

        private IEnumerator<IBinaryTreeNode<int, T>> Nodes { get; set; }

        private IBinaryTreeNode<int, T> CurrentNode { get; set; } 

        private int Length { get; set; }

        private int Index { get; set; }

        public void Dispose()
        {
            // No dispose actions
        }

        public bool MoveNext()
        {
            if (this.Index == -1)
            {
                if (this.Nodes.MoveNext()) this.CurrentNode = this.Nodes.Current;
            }
            if (this.CurrentNode != null)
            {
                if (this.CurrentNode.Key == this.Index)
                {
                    this.CurrentNode = this.Nodes.MoveNext() ? this.Nodes.Current : null;
                }
            }
            this.Index++;
            return this.Index < this.Length;
        }

        public void Reset()
        {
            this.Index = -1;
            this.CurrentNode = null;
            this.Nodes.Reset();
        }

        public T Current
        {
            get
            {
                if (this.Index == -1) throw new InvalidOperationException("Currently before the start of the enumerator, please call MoveNext() before accessing this property");
                if (this.Index >= this.Length) throw new InvalidOperationException("Past the end of the enumerator");

                // If no node either the linked list is empty or we've reached the end of it in which case simply return the default value
                if (this.CurrentNode == null) return default(T);
                // If we reached the index of the current node then return the value otherwise we have not reached it yet and we return the default value
                return this.CurrentNode.Key == this.Index ? this.CurrentNode.Value : default(T);
            }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
}
