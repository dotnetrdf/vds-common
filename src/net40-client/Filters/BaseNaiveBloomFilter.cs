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
using System.Collections.Generic;

namespace VDS.Common.Filters
{
    /// <summary>
    /// Abstract implementation of a bloom filter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// This implementation is considered naive because it gives full control over the hash functions and number of bits to the end user
    /// </remarks>
    public abstract class BaseNaiveBloomFilter<T>
        : BaseBloomFilter<T>
    {
        private readonly List<Func<T, int>> _hashFunctions;

        /// <summary>
        /// Creates a new filter
        /// </summary>
        /// <param name="storage">Bloom Filter storage</param>
        /// <param name="bits">Number of Bits</param>
        /// <param name="hashFunctions">Hash Functions</param>
        protected BaseNaiveBloomFilter(IBloomFilterStorage storage, int bits, IEnumerable<Func<T, int>> hashFunctions)
            : base(storage)
        {
            if (bits <= 0) throw new ArgumentException("Bits must be a positive value", "bits");
            if (hashFunctions == null) throw new ArgumentNullException("hashFunctions");
            this._hashFunctions = new List<Func<T, int>>(hashFunctions);
            this._hashFunctions.RemoveAll(f => f == null);
            if (this._hashFunctions.Count <= 1) throw new ArgumentException("A bloom filter requires at least 2 hash functions", "hashFunctions");
            if (bits <= this._hashFunctions.Count) throw new ArgumentException("Bits must be bigger than the number of hash functions", "bits");

            this.NumberOfBits = bits;
        }

        public override int NumberOfHashFunctions
        {
            get { return this._hashFunctions.Count; }
        }

        /// <summary>
        /// Converts an item into a number of bit indexes
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>Bit Indices</returns>
        protected override IEnumerable<int> GetBitIndices(T item)
        {
            int[] indices = new int[this._hashFunctions.Count];
            for (int i = 0; i < indices.Length; i++)
            {
                indices[i] = Math.Abs(this._hashFunctions[i](item)) % this.NumberOfBits;
            }
            return indices;
        }
    }
}