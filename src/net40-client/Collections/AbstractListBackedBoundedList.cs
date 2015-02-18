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

namespace VDS.Common.Collections
{
    /// <summary>
    /// Abstract implementation of a bounded list backed by a standard <see cref="IList{T}"/>
    /// </summary>
    /// <typeparam name="T">Element type</typeparam>
    public abstract class AbstractListBackedBoundedList<T>
        : IBoundedList<T>
    {
        /// <summary>
        /// Underlying list
        /// </summary>
        protected readonly IList<T> _list;

        /// <summary>
        /// Creates a new list backed bounded list using the given list
        /// </summary>
        /// <param name="list">List</param>
        protected AbstractListBackedBoundedList(IList<T> list)
        {
            if (list == null) throw new ArgumentNullException("list");
            this._list = list;
        }

        /// <summary>
        /// Helper method for selecting an initial capacity for the backing list to avoid preemptively allocating the full capacity needed to hold the maximum number of permitted elements
        /// </summary>
        /// <param name="capacity">MaxCapacity</param>
        /// <returns>Initial capacity</returns>
        protected static int SelectInitialCapacity(int capacity)
        {
            return capacity < 10 ? capacity : Convert.ToInt32(Math.Log(capacity, 2d));
        }

        public virtual int Count
        {
            get { return this._list.Count; }
        }

        public virtual bool IsReadOnly
        {
            get { return this._list.IsReadOnly; }
        }

        public virtual int IndexOf(T item)
        {
            return this._list.IndexOf(item);
        }

        public abstract void Insert(int index, T item);

        public virtual T this[int index]
        {
            get { return this._list[index]; }
            set { this._list[index] = value; }
        }

        public abstract void Add(T item);

        public virtual void Clear()
        {
            this._list.Clear();
        }

        public virtual bool Contains(T item)
        {
            return this._list.Contains(item);
        }

        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            this._list.CopyTo(array, arrayIndex);
        }

        public virtual bool Remove(T item)
        {
            return this._list.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this._list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this._list.GetEnumerator();
        }

        /// <summary>
        /// Gets the overflow policy that is in-use
        /// </summary>
        public abstract BoundedListOverflowPolicy OverflowPolicy { get; }

        /// <summary>
        /// Gets the maximum capacity of the list
        /// </summary>
        public abstract int MaxCapacity { get; protected set; }

        public virtual void RemoveAt(int index)
        {
            this._list.RemoveAt(index);
        }
    }
}