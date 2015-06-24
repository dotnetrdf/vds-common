/*
VDS.Common is licensed under the MIT License

Copyright (c) 2012-2015 Robert Vesse

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

namespace VDS.Common.Trees
{
    /// <summary>
    /// Interface for trees that support indexed access
    /// </summary>
    /// <remarks>
    /// Indexes may not be stable depending on the underlying tree type
    /// </remarks>
    /// <typeparam name="TNode">Node type</typeparam>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    public interface IIndexAccessTree<TNode, TKey, TValue>
        : ITree<TNode, TKey, TValue>
        where TNode : class, ITreeNode<TKey, TValue>
    {
        /// <summary>
        /// Gets the value at the given index
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Value at the given index</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index is not within the acceptable range for this tree</exception>
        TValue GetValueAt(int index);

        /// <summary>
        /// Sets the value at the given index
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="value">Value</param>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index is not within the acceptable range for this tree</exception>
        void SetValueAt(int index, TValue value);

        /// <summary>
        /// Removes the value (and corresponding key) at the given index
        /// </summary>
        /// <param name="index">Index</param>
        /// <exception cref="IndexOutOfRangeException">Thrown if the index is not within the acceptable range for this tree</exception>
        void RemoveAt(int index);
    }
}
