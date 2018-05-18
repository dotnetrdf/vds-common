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
    /// Interface for sparse arrays which are memory efficient implementations of arrays
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    public interface ISparseArray<T>
        : IEnumerable<T>
    {
        /// <summary>
        /// Gets/Sets the element at the specified index
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Element at the given index</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the given index is out of range</exception>
        T this[int index] { get; set; }

        /// <summary>
        /// Gets the length of the array
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Clears the array i.e. resets all values to the default and frees any unecessary storage
        /// </summary>
        void Clear();
    }
}
