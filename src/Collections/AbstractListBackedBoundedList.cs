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
/// Abstract implementation of a bounded list backed by a standard <see cref="IList{T}"/>
/// </summary>
/// <typeparam name="T">Element type</typeparam>
public abstract class AbstractListBackedBoundedList<T>
    : IBoundedList<T>
{
    /// <summary>
    /// Underlying list
    /// </summary>
    protected readonly IList<T> List;

    /// <summary>
    /// Creates a new list backed bounded list using the given list
    /// </summary>
    /// <param name="list">List</param>
    protected AbstractListBackedBoundedList(IList<T> list)
    {
        List = list ?? throw new ArgumentNullException(nameof(list));
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

    /// <summary>
    /// Gets the size of the list
    /// </summary>
    public virtual int Count => List.Count;

    /// <summary>
    /// Gets whether the list is ready only
    /// </summary>
    public virtual bool IsReadOnly => List.IsReadOnly;

    /// <summary>
    /// Gets the index of the given item
    /// </summary>
    /// <param name="item">Item</param>
    /// <returns>Index or -1 if not in list</returns>
    public virtual int IndexOf(T item)
    {
        return List.IndexOf(item);
    }

    /// <summary>
    /// Inserts an item at the given index
    /// </summary>
    /// <param name="index">Index</param>
    /// <param name="item">Item</param>
    public abstract void Insert(int index, T item);

    /// <summary>
    /// Gets/Sets the value at the given index
    /// </summary>
    /// <param name="index">Index</param>
    /// <returns>Value</returns>
    public virtual T this[int index]
    {
        get => List[index];
        set => List[index] = value;
    }

    /// <summary>
    /// Adds an item
    /// </summary>
    /// <param name="item">Item</param>
    public abstract void Add(T item);

    /// <summary>
    /// Clears the list
    /// </summary>
    public virtual void Clear()
    {
        List.Clear();
    }

    /// <summary>
    /// Gets whether the list contains the given item
    /// </summary>
    /// <param name="item">Item</param>
    /// <returns>True if contained in the list, false otherwise</returns>
    public virtual bool Contains(T item)
    {
        return List.Contains(item);
    }

    /// <summary>
    /// Copies the list to the given array
    /// </summary>
    /// <param name="array">Array</param>
    /// <param name="arrayIndex">Array Index to start the copy at</param>
    public virtual void CopyTo(T[] array, int arrayIndex)
    {
        List.CopyTo(array, arrayIndex);
    }

    /// <summary>
    /// Removes an item from the list
    /// </summary>
    /// <param name="item">Item</param>
    /// <returns>True if item was removed, false otherwise</returns>
    public virtual bool Remove(T item)
    {
        return List.Remove(item);
    }

    /// <summary>
    /// Gets an enumerator
    /// </summary>
    /// <returns></returns>
    public IEnumerator<T> GetEnumerator()
    {
        return List.GetEnumerator();
    }

    /// <summary>
    /// Gets an enumerator
    /// </summary>
    /// <returns></returns>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return List.GetEnumerator();
    }

    /// <summary>
    /// Gets the overflow policy that is in-use
    /// </summary>
    public abstract BoundedListOverflowPolicy OverflowPolicy { get; }

    /// <summary>
    /// Gets the maximum capacity of the list
    /// </summary>
    public abstract int MaxCapacity { get; protected set; }

    /// <summary>
    /// Removes an item at the given index
    /// </summary>
    /// <param name="index">Index</param>
    public virtual void RemoveAt(int index)
    {
        List.RemoveAt(index);
    }
}