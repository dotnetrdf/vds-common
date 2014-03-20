using System;
using System.Collections.Generic;

namespace VDS.Common.Collections
{
    /// <summary>
    /// A simple bounded list implementation that errors if users attempt to add more items than there is capacity for
    /// </summary>
    /// <typeparam name="T">Element type</typeparam>
    public class CappedBoundedList<T>
        : AbstractListBackedBoundedList<T>
    {
        /// <summary>
        /// Creates a new bounded list with the given capacity
        /// </summary>
        /// <param name="capacity">MaxCapacity</param>
        public CappedBoundedList(int capacity)
            : base(new List<T>(SelectInitialCapacity(capacity)))
        {
            if (capacity < 0) throw new ArgumentException("MaxCapacity must be >= 0", "capacity");
            this.MaxCapacity = capacity;
        }

        /// <summary>
        /// Creates a new bounded list with the given capacity and items
        /// </summary>
        /// <param name="capacity">MaxCapacity</param>
        /// <param name="items">Items</param>
        /// <remarks>
        /// If the number of items provided exceeds the declared capacity then an error will be thrown from the constructor
        /// </remarks>
        public CappedBoundedList(int capacity, IEnumerable<T> items)
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

        public override int MaxCapacity { get; protected set; }

        public override void Insert(int index, T item)
        {
            if (this._list.Count == this.MaxCapacity) throw new InvalidOperationException("Cannot insert an item to this bounded list since it would cause the configured capacity of " + this.MaxCapacity + " to be exceeded");
            this._list.Insert(index, item);
        }

        public override void Add(T item)
        {
            if (this._list.Count == this.MaxCapacity) throw new InvalidOperationException("Cannot add an item to this bounded list since it would cause the configured capacity of " + this.MaxCapacity + " to be exceeded");
            this._list.Add(item);
        }
    }
}
