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

namespace VDS.Common.Filters
{
    /// <summary>
    /// Interface for bloom filters which are a probabilistic data structure for detecting whether items have already been seen
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    /// <remarks>
    /// A bloom filter is a data structure that can be used to determine whether an item may have previously been seen in a memory efficient way that does not require storing the actual items.  The trade off is that it may yield false positives however it is guaranteed to never yield false negatives.  This makes it an ideal data structure for implementings things like distinctness where it doesn't matter if a few non-distinct items are discarded.
    /// </remarks>
    public interface IBloomFilter<T> : IBloomFilterParameters
    {
        /// <summary>
        /// Adds an item to the filter
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>True if the item was added to the filter, false if item may already have been present and was not added</returns>
        bool Add(T item);

        /// <summary>
        /// Gets whether the filter may have already seen the given item
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>True if the item may have been seen, false otherwise</returns>
        /// <remarks>
        /// Bloom filters may return false positives hence why this method is named <strong>MayContain</strong> but they are guaranteed to never return false negatives
        /// </remarks>
        bool MayContain(T item);

        /// <summary>
        /// Clears the bloom filter i.e. resets it to a state where it has seen no items
        /// </summary>
        void Clear();
    }
}
