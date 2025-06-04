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
using VDS.Common.Filters;

namespace VDS.Common.Collections.Enumerations
{
    /// <summary>
    /// An enumerator that reduces another enumerator by using a bloom filter to give probably distinct results
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// All items returned are guaranteed to be distinct (based upon the hash functions used for the filter) but some items may be erroneously omitted as bloom filters can produce false positives
    /// </remarks>
    public class ProbabilisticDistinctEnumerator<T>
        : AbstractWrapperEnumerator<T>
    {
        /// <summary>
        /// Creates a new enumerator
        /// </summary>
        /// <param name="enumerator">Enumerator to operate over</param>
        /// <param name="filter">Bloom filter to use for distinctness</param>
        public ProbabilisticDistinctEnumerator(IEnumerator<T> enumerator, IBloomFilter<T> filter)
            : base(enumerator)
        {
            Filter = filter ?? throw new ArgumentNullException(nameof(filter));
        }

        /// <summary>
        /// Gets/Sets the bloom filter to use
        /// </summary>
        private IBloomFilter<T> Filter { get; set; } 

        /// <summary>
        /// Tries to move next
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns></returns>
        protected override bool TryMoveNext(out T item)
        {
            item = default(T);
            while (InnerEnumerator.MoveNext())
            {
                item = InnerEnumerator.Current;
                if (Filter.Add(item)) return true;
            }
            return false;
        }

        /// <summary>
        /// Resets the bloom filter used
        /// </summary>
        protected override void ResetInternal()
        {
            Filter.Clear();
        }
    }
}
