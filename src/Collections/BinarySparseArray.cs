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
using VDS.Common.Trees;

namespace VDS.Common.Collections;

/// <summary>
/// A sparse array implementation backed by a binary tree
/// </summary>
/// <remarks>
/// This implementation provides a trade off between look up time and memory usage and so provides a compromise between the <see cref="BlockSparseArray{T}"/> and <see cref="LinkedSparseArray{T}"/>
/// </remarks>
/// <typeparam name="T"></typeparam>
public class BinarySparseArray<T>
    : ISparseArray<T>
{
    private readonly ITree<IBinaryTreeNode<int, T>, int, T> _tree;

    /// <summary>
    /// Creates a new sparse array
    /// </summary>
    /// <param name="length">Length</param>
    public BinarySparseArray(int length)
    {
        if (length < 0) throw new ArgumentException("Length must be >= 0", nameof(length));
        _tree = new AvlTree<int, T>(Comparer<int>.Default);
        Length = length;
    }


    /// <summary>
    /// Gets an enumerator
    /// </summary>
    /// <returns></returns>
    public IEnumerator<T> GetEnumerator()
    {
        return new BinarySparseArrayEnumerator<T>(Length, _tree.Nodes.GetEnumerator());
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
    /// Gets/Sets the value at a given index
    /// </summary>
    /// <param name="index">Index</param>
    /// <returns>Value</returns>
    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= Length) throw new IndexOutOfRangeException(string.Format("Index must be in the range 0 to {0}", Length - 1));
            return _tree.TryGetValue(index, out var value) ? value : default(T);
        }
        set
        {
            if (index < 0 || index >= Length) throw new IndexOutOfRangeException(string.Format("Index must be in the range 0 to {0}", Length - 1));
            _tree.Add(index, value);
        }
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
        _tree.Clear();
    }
}