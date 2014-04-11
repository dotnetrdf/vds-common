/*
VDS.Common is licensed under the MIT License

Copyright (c) 2012-2014 Robert Vesse

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

namespace VDS.Common.Collections
{
    /// <summary>
    /// A bounded list implementation where items added above the capacity overwrite the oldest items in the list
    /// </summary>
    /// <remarks>
    /// This differs in internal implementation from <see cref="RingBuffer{T}"/> in that it doesn't pre-allocate all the memory for the list so performs better when the capacity is set to a high value or where the capacity will rarely be exceeded.  Some overwriting operations may be slower while others may be faster.  Generally we suggest that you benchmark to see which performs better in your usage scenario.
    /// </remarks>
    /// <typeparam name="T">Item type</typeparam>
    public class OverwritingBoundedList<T>
        : IBoundedList<T>
    {
        private readonly LinkedList<T> _list;

        public OverwritingBoundedList(int capacity)
        {
            this.MaxCapacity = capacity;
            this._list = new LinkedList<T>();
        }

        public OverwritingBoundedList(int capacity, IEnumerable<T> items)
            : this(capacity)
        {
            foreach (T item in items)
            {
                this.Add(item);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            if (this._list.Count == this.MaxCapacity) this._list.RemoveFirst();
            this._list.AddLast(item);
        }

        public void Clear()
        {
            this._list.Clear();
        }

        public bool Contains(T item)
        {
            return this._list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            // This is a workaround for the fact that LinkedList throws the wrong exception in several cases and doesn't do a proper null check
            if (array == null) throw new ArgumentNullException("array", "Cannot copy to a null array");
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException("arrayIndex", "Cannot start copying at index < 0");
            if (this.Count > array.Length - arrayIndex) throw new ArgumentException("Insufficient space in array", "array");

            this._list.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return this._list.Remove(item);
        }

        public int Count
        {
            get { return this._list.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public int IndexOf(T item)
        {
            if (this._list.Count == 0) return -1;

            int i = 0;
            LinkedListNode<T> node = this._list.First;
            do
            {
                if (node.Value.Equals(item)) return i;
                i++;
                node = node.Next;
            } while (node != null);
            return -1;
        }

        public void Insert(int index, T item)
        {
            if (index == this.Count)
            {
                this.Add(item);
            }
            else
            {
                LinkedListNode<T> node = FindNode(index);
                this._list.AddBefore(node, item);
                if (this._list.Count > this.MaxCapacity) this._list.RemoveFirst();
            }
        }

        public void RemoveAt(int index)
        {
            LinkedListNode<T> node = FindNode(index);
            this._list.Remove(node);
        }

        public T this[int index]
        {
            get
            {
                LinkedListNode<T> node = FindNode(index);
                return node.Value;
            }
            set
            {
                LinkedListNode<T> node = FindNode(index);
                node.Value = value;
            }
        }

        private LinkedListNode<T> FindNode(int index)
        {
            if (index < 0 || index >= this._list.Count) throw new ArgumentOutOfRangeException("Index must be in the range 0-" + (this._list.Count - 1));
            LinkedListNode<T> node = this._list.First;
            for (int i = 0; i < index; i++)
            {
                node = node.Next;
            }
            return node;
        }

        public BoundedListOverflowPolicy OverflowPolicy
        {
            get { return BoundedListOverflowPolicy.Overwrite; }
        }

        public int MaxCapacity { get; private set; }
    }
}