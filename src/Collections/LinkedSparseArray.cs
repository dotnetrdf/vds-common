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
using System.Collections;
using System.Collections.Generic;

namespace VDS.Common.Collections;

/// <summary>
/// A memory efficient sparse array backed by a <see cref="LinkedList{T}"/> thus it trades lookup performance off against memory.  
/// </summary>
/// <remarks>
/// <para>
/// This implementation is extremely memory efficient for sparse arrays since it only stores entries where the value has been set.  However the use of a linked list means lookup of a specific index is a worse case O(n) operation where n is the length of the list.
/// </para>
/// <para>
/// If fast access is preferred over memory efficiency then the <see cref="BlockSparseArray{T}"/> may be a better option
/// </para>
/// </remarks>
/// <typeparam name="T">Value type</typeparam>
public class LinkedSparseArray<T>
    : ISparseArray<T>
{
    private readonly LinkedList<SparseArrayEntry<T>> _list = new LinkedList<SparseArrayEntry<T>>();

    /// <summary>
    /// Creates a new sparse array with the given length
    /// </summary>
    /// <param name="length">Length</param>
    public LinkedSparseArray(int length)
    {
        if (length < 0) throw new ArgumentException("Length must be >= 0", nameof(length));
        Length = length;
    }

    /// <summary>
    /// Gets an enumerator
    /// </summary>
    /// <returns></returns>
    public IEnumerator<T> GetEnumerator()
    {
        return new LinkedSparseArrayEnumerator<T>(_list, Length);
    }

    /// <summary>
    /// Gets an enumerator
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Gets/Sets the value at the given index
    /// </summary>
    /// <param name="index">Index</param>
    /// <returns>Value</returns>
    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= Length) throw new IndexOutOfRangeException(string.Format("Index must be in range 0 to {0}", Length - 1));
            var node = MoveToNode(index, false);
            return node == null ? default(T) : node.Value.Value;
        }
        set
        {
            if (index < 0 || index >= Length) throw new IndexOutOfRangeException(string.Format("Index must be in range 0 to {0}", Length - 1));
            var node = MoveToNode(index, true);
            node.Value.Value = value;
        }
    }

    private LinkedListNode<SparseArrayEntry<T>> MoveToNode(int index, bool createIfNotExists)
    {
        var node = _list.First;

        // Try to move to the appropriate point in the list
        while (node != null)
        {
            // Reached the requested index
            if (node.Value.Index == index) return node;

            if (node.Value.Index > index)
            {
                // We have moved past the required index
                // Insert a new node if permitted to do so
                return createIfNotExists ? _list.AddBefore(node, new SparseArrayEntry<T>(index)) : null;
            }

            // Otherwise we have not yet reached the required index so move to the next node
            node = node.Next;
        }

        // Reached the end of list without finding the required index
        // Insert a new node if permitted to do so
        return createIfNotExists ? _list.AddLast(new SparseArrayEntry<T>(index)) : null;
    }

    /// <summary>
    /// Gets the length of the array
    /// </summary>
    public int Length { get; private set; }

    /// <summary>
    /// Clears the array
    /// </summary>
    public void Clear()
    {
        _list.Clear();
    }
}