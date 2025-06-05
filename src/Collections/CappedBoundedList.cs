/*
VDS.Common is licensed under the MIT License

Copyright (c) 2012-2015 Robert Vesse
Copyright (c) 2016-2025 dotNetRDF Project (https://dotnetrdf.org/)

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

namespace VDS.Common.Collections;

/// <summary>
/// A simple bounded list implementation that errors if users attempt to add more items than there is capacity for
/// </summary>
/// <typeparam name="T">Element type</typeparam>
public class CappedBoundedList<T>
    : AbstractListBackedBoundedList<T>
{
    private int _capacity;

    /// <summary>
    /// Creates a new bounded list with the given capacity
    /// </summary>
    /// <param name="capacity">MaxCapacity</param>
    public CappedBoundedList(int capacity)
        : base(new List<T>(SelectInitialCapacity(capacity)))
    {
        if (capacity < 0) throw new ArgumentException("MaxCapacity must be >= 0", nameof(capacity));
        _capacity = capacity;
    }

    /// <summary>
    /// Gets the overflow policy for this bounded list which is <see cref="BoundedListOverflowPolicy.Error"/>
    /// </summary>
    public override BoundedListOverflowPolicy OverflowPolicy => BoundedListOverflowPolicy.Error;

    /// <summary>
    /// Gets the maximum capacity of the list
    /// </summary>
    public override int MaxCapacity
    {
        get => _capacity;
        protected set => _capacity = value;
    }

    /// <summary>
    /// Inserts an item into the list at the given index
    /// </summary>
    /// <param name="index">Index</param>
    /// <param name="item">Item to insert</param>
    public override void Insert(int index, T item)
    {
        CheckCapacity();
        List.Insert(index, item);
    }

    /// <summary>
    /// Adds an item to the list
    /// </summary>
    /// <param name="item">Item</param>
    public override void Add(T item)
    {
        CheckCapacity();
        List.Add(item);
    }

    /// <summary>
    /// Add each of the items in the provided enumeration to the list.
    /// </summary>
    /// <param name="items">Items to add to the list</param>
    /// <exception cref="InvalidOperationException">Raised if the capacity of this list is exceeded.</exception>
    public virtual void Add(IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            Add(item);
        }
    }

    private void CheckCapacity()
    {
        if (List.Count == MaxCapacity)
        {
            throw new InvalidOperationException("Cannot add an item to this bounded list since it would cause the configured capacity of " + MaxCapacity + " to be exceeded");
        }
    }

}