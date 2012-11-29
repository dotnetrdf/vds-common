using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        protected IEnumerable<T> _items;
        private String _errMsg;

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
        public bool Contains(T item)
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
            if (array == null) throw new ArgumentNullException("Cannot copy to a null array");
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException("Cannot start copying at index < 0");
            if (this.Count > array.Length - arrayIndex) throw new ArgumentException("Insufficient space in array");

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
    /// A version of <see cref="ImmutableView"/> where the enumerable is materialized as a list
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
    }
}
