using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VDS.Common.Collections
{
    /// <summary>
    /// A simple bounded list implementation that discards items that would exceed the lists capacity
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    public class DiscardingBoundedList<T>
        : AbstractListBackedBoundedList<T>
    {
        /// <summary>
        /// Creates a new bounded list with the given capacity
        /// </summary>
        /// <param name="capacity">MaxCapacity</param>
        public DiscardingBoundedList(int capacity)
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
        /// If the number of items provided exceeds the declared capacity then excess items are discarded
        /// </remarks>
        public DiscardingBoundedList(int capacity, IEnumerable<T> items)
            : this(capacity)
        {
            foreach (T item in items)
            {
                this.Add(item);
            }
        }

        public override void Insert(int index, T item)
        {
            if (this._list.Count == this.MaxCapacity) return;
            this._list.Insert(index, item);
            while (this._list.Count > this.MaxCapacity)
            {
                this._list.RemoveAt(this.MaxCapacity);
            }
        }

        public override void Add(T item)
        {
            if (this._list.Count == this.MaxCapacity) return;
            this._list.Add(item);
        }

        /// <summary>
        /// Gets the overflow policy for this bounded list which is <see cref="BoundedListOverflowPolicy.Discard"/>
        /// </summary>
        public override BoundedListOverflowPolicy OverflowPolicy
        {
            get { return BoundedListOverflowPolicy.Discard; }
        }

        public override int MaxCapacity { get; protected set; }
    }
}