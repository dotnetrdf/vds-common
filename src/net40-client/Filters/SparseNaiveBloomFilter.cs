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
using VDS.Common.Collections;

namespace VDS.Common.Filters
{
    /// <summary>
    /// Bloom filter implementation backed by a <see cref="ISparseArray{T}"/>
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    public class SparseNaiveBloomFilter<T>
        : BaseNaiveBloomFilter<T>
    {
        private readonly ISparseArray<bool> _array;

        public SparseNaiveBloomFilter(int bits, IEnumerable<Func<T, int>> hashFunctions)
            : this(bits, hashFunctions, new BlockSparseArray<bool>(bits)) {}

        public SparseNaiveBloomFilter(int bits, IEnumerable<Func<T, int>> hashFunctions, ISparseArray<bool> sparseArray)
            : base(bits, hashFunctions)
        {
            if (sparseArray == null) throw new ArgumentNullException("sparseArray");
            if (sparseArray.Length != bits) throw new ArgumentException("Length of sparse array should be equal to number of bits", "sparseArray");
            this._array = sparseArray;
        }

        protected override void SetBit(int index)
        {
            this._array[index] = true;
        }

        protected override bool IsBitSet(int index)
        {
            return this._array[index];
        }
    }
}