using System;
using System.Collections.Generic;

namespace VDS.Common.Collections
{
    /// <summary>
    /// A simple bounded list implementation that errors if users attempt to add more items than there is capacity for
    /// </summary>
    /// <typeparam name="T">Element type</typeparam>
    public class BoundedList<T>
        : AbstractListBackedBoundedList<T>
    {
        private readonly int _capacity;

        /// <summary>
        /// Creates a new bounded list with the given capacity
        /// </summary>
        /// <param name="capacity">Capacity</param>
        public BoundedList(int capacity)
            : base(new List<T>(capacity))
        {
            if (capacity < 0) throw new ArgumentException("Capacity must be >= 0", "capacity");
            this._capacity = capacity;
        }

        public BoundedList(int capacity, IEnumerable<T> items)
            : this(capacity)
        {
            foreach (T item in items)
            {
                this.Add(item);
            }
        }

        /// <summary>
        /// Gets the overflow policy for this bounded list which is <see cref="BoundedListOverflowPolicy.Error"/>
        /// </summary>
        public override BoundedListOverflowPolicy OverflowPolicy
        {
            get { return BoundedListOverflowPolicy.Error; }
        }

        public override void Insert(int index, T item)
        {
            if (this._list.Count == this._capacity) throw new InvalidOperationException("Cannot insert an item to this bounded list since it would cause the configured capacity of " + this._capacity + " to be exceeded");
            this._list.Insert(index, item);
        }

        public override void Add(T item)
        {
            if (this._list.Count == this._capacity) throw new InvalidOperationException("Cannot add an item to this bounded list since it would cause the configured capacity of " + this._capacity + " to be exceeded");
            this._list.Add(item);
        }
    }
}
