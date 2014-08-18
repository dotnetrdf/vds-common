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
    }
}
