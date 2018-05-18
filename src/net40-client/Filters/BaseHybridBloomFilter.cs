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

namespace VDS.Common.Filters
{
    /// <summary>
    /// Abstract implementation of a bloom filter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// This implementation is a hybrid of the naive and fast approaches provided by <see cref="BaseNaiveBloomFilter{T}"/> and <see cref="BaseFastBloomFilter{T}"/> in that it allows for users to provide more than two hash functions but uses the fast arithmetic technique to create any additional hash functions needed.
    /// </remarks>
    public abstract class BaseHybridBloomFilter<T>
        : BaseBloomFilter<T>
    {
        private readonly List<Func<T, int>> _hashFunctions;
        private readonly IBloomFilterParameters _parameters;

        /// <summary>
        /// Creates a new filter
        /// </summary>
        /// <param name="storage">Bloom Filter storage</param>
        /// <param name="parameters">Parameters</param>
        /// <param name="hashFunctions">Hash Functions</param>
        protected BaseHybridBloomFilter(IBloomFilterStorage storage, IBloomFilterParameters parameters, IEnumerable<Func<T, int>> hashFunctions)
            : base(storage)
        {
            if (parameters.NumberOfBits < 1) throw new ArgumentException("Number of bits must be >= 1", "parameters");
            if (hashFunctions == null) throw new ArgumentNullException("hashFunctions");
            this._hashFunctions = new List<Func<T, int>>(hashFunctions);
            this._hashFunctions.RemoveAll(f => f == null);
            if (this._hashFunctions.Count <= 1) throw new ArgumentException("A bloom filter requires at least 2 hash functions", "hashFunctions");
            if (parameters.NumberOfBits <= this._hashFunctions.Count) throw new ArgumentException("Number of bits must be bigger than the number of hash functions", "parameters");

            this.NumberOfBits = parameters.NumberOfBits;
            this._parameters = parameters;
        }

        /// <summary>
        /// Gets the number of hash functions
        /// </summary>
        public override int NumberOfHashFunctions
        {
            get { return this._parameters.NumberOfHashFunctions; }
        }

        /// <summary>
        /// Converts an item into a number of bit indexes
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>Bit Indices</returns>
        protected override IEnumerable<int> GetBitIndices(T item)
        {
            int[] indices = new int[this._parameters.NumberOfHashFunctions];
            int a = 0, b = 0;
            for (int i = 0; i < indices.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        a = this._hashFunctions[i](item);
                        indices[i] = Math.Abs(a)%this.NumberOfBits;
                        break;
                    case 1:
                        b = this._hashFunctions[i](item);
                        indices[i] = Math.Abs(b) % this.NumberOfBits;
                        break;
                    default:
                        if (i < this._hashFunctions.Count)
                        {
                            // Use user defined hash functions while available
                            indices[i] = Math.Abs(this._hashFunctions[i](item))%this.NumberOfBits;
                        }
                        else
                        {
                            // Use arithmetic combination of the first two hash functions
                            indices[i] = Math.Abs(a + (i*b))%this.NumberOfBits;
                        }
                        break;
                }
            }
            return indices;
        }
    }
}