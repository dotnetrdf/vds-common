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
using System.Collections.Generic;

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
            if (capacity < 0) throw new ArgumentException("MaxCapacity must be >= 0", nameof(capacity));
            MaxCapacity = capacity;
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
            foreach (var item in items)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Inserts an item at the given index
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="item">Item</param>
        public override void Insert(int index, T item)
        {
            if (List.Count == MaxCapacity && index == List.Count) return;
            List.Insert(index, item);
            while (List.Count > MaxCapacity)
            {
                List.RemoveAt(MaxCapacity);
            }
        }

        /// <summary>
        /// Adds an item
        /// </summary>
        /// <param name="item">Item</param>
        public override sealed void Add(T item)
        {
            if (List.Count == MaxCapacity) return;
            List.Add(item);
        }

        /// <summary>
        /// Gets the overflow policy for this bounded list which is <see cref="BoundedListOverflowPolicy.Discard"/>
        /// </summary>
        public override BoundedListOverflowPolicy OverflowPolicy => BoundedListOverflowPolicy.Discard;

        /// <summary>
        /// Gets the maximum capacity of the list
        /// </summary>
        public override sealed int MaxCapacity { get; protected set; }
    }
}