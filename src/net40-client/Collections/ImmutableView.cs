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

namespace VDS.Common.Collections
{
    /// <summary>
    /// An implementation of <see cref="ICollection{T}"/> which is an immutable view over some enumerable
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public class ImmutableView<T>
        : ICollection<T>
    {
        private const String DefaultErrorMessage = "This collection is immutable";

        /// <summary>
        /// The enumerable being wrapped
        /// </summary>
        protected IEnumerable<T> _items;
        private readonly String _errMsg;

        /// <summary>
        /// Creates a new immutable view over an empty collection
        /// </summary>
        public ImmutableView()
            : this(Enumerable.Empty<T>(), DefaultErrorMessage) { }

        /// <summary>
        /// Creates a new immutable view over an empty collection
        /// </summary>
        /// <param name="message">Error message to throw when mutation actions are attempted</param>
        public ImmutableView(String message)
            : this(Enumerable.Empty<T>(), message) { }

        /// <summary>
        /// Creates a new immutable view
        /// </summary>
        /// <param name="items">Enumerable to provide view over</param>
        /// <param name="message">Error message to throw when mutation actions are attempted</param>
        public ImmutableView(IEnumerable<T> items, String message)
        {
            this._items = items;
            this._errMsg = (!String.IsNullOrEmpty(message) ? message : DefaultErrorMessage);
        }

        /// <summary>
        /// Creates a new immutable view
        /// </summary>
        /// <param name="items">Enumerable to provide view over</param>
        public ImmutableView(IEnumerable<T> items)
            : this(items, DefaultErrorMessage) { }

        /// <summary>
        /// Throws an error as this collection is immutable
        /// </summary>
        /// <param name="item">Item</param>
        /// <exception cref="NotSupportedException">Thrown because the collection is immutable</exception>
        public void Add(T item)
        {
            throw new NotSupportedException(this._errMsg);
        }

        /// <summary>
        /// Throws an error as this collection is immutable
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown because the collection is immutable</exception>
        public void Clear()
        {
            throw new NotSupportedException(this._errMsg);
        }

        /// <summary>
        /// Checks whether the collection contains a given item
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>True if the item is contained in the collection, false otherwise</returns>
        public virtual bool Contains(T item)
        {
            return this._items.Contains(item);
        }

        /// <summary>
        /// Copies the collection into an array
        /// </summary>
        /// <param name="array">Array</param>
        /// <param name="arrayIndex">Index to start the copying at</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException("array", "Cannot copy to a null array");
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException("arrayIndex", "Cannot start copying at index < 0");
            if (this.Count > array.Length - arrayIndex) throw new ArgumentException("Insufficient space in array", "array");

            int i = arrayIndex;
            foreach (T item in this._items)
            {
                array[i] = item;
                i++;
            }
        }

        /// <summary>
        /// Gets the count of items in the collection
        /// </summary>
        public virtual int Count
        {
            get 
            {
                return this._items.Count();
            }
        }

        /// <summary>
        /// Returns that the collection is read-only
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Throws an error as this collection is immutable
        /// </summary>
        /// <param name="item">Item</param>
        /// <exception cref="NotSupportedException">Thrown because the collection is immutable</exception>
        public bool Remove(T item)
        {
            throw new NotSupportedException(this._errMsg);
        }

        /// <summary>
        /// Gets an enumerator for the collection
        /// </summary>
        /// <returns>Enumerator over the collection</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this._items.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator for the collection
        /// </summary>
        /// <returns>Enumerator over the collection</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// A version of <see cref="ImmutableView{T}"/> where the enumerable is materialized as a list internally
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public class MaterializedImmutableView<T>
        : ImmutableView<T>
    {
         /// <summary>
        /// Creates a new immutable view over an empty collection
        /// </summary>
        public MaterializedImmutableView()
            : base(new List<T>()) { }

        /// <summary>
        /// Creates a new immutable view over an empty collection
        /// </summary>
        /// <param name="message">Error message to throw when mutation actions are attempted</param>
        public MaterializedImmutableView(String message)
            : base(new List<T>(), message) { }

        /// <summary>
        /// Creates a new immutable view
        /// </summary>
        /// <param name="items">Enumerable to provide view over</param>
        /// <param name="message">Error message to throw when mutation actions are attempted</param>
        public MaterializedImmutableView(IEnumerable<T> items, String message)
            : base(items.ToList(), message) { }

        /// <summary>
        /// Creates a new immutable view
        /// </summary>
        /// <param name="items">Enumerable to provide view over</param>
        public MaterializedImmutableView(IEnumerable<T> items)
            : base(items.ToList()) { }

        /// <summary>
        /// Gets the count of items in the collection
        /// </summary>
        public override int Count
        {
            get
            {
                return ((IList<T>)this._items).Count;
            }
        }

        /// <summary>
        /// Checks whether the collection contains a given item
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>True if the item is contained in the collection, false otherwise</returns>
        public override bool Contains(T item)
        {
            return ((IList<T>) this._items).Contains(item);
        }
    }
}
