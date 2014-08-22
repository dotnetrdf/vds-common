using System;
using System.Collections.Generic;
using VDS.Common.Collections;

namespace VDS.Common.Filters
{
    /// <summary>
    /// Bloom filter implementation backed by a <see cref="ISparseArray{T}"/>
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    public class SparseBloomFilter<T>
        : BaseBloomFilter<T>
    {
        private readonly ISparseArray<int> _array;

        public SparseBloomFilter(int bits, IEnumerable<Func<T, int>> hashFunctions)
            : this(bits, hashFunctions, new BlockSparseArray<int>(bits)) {}

        public SparseBloomFilter(int bits, IEnumerable<Func<T, int>> hashFunctions, ISparseArray<int> sparseArray)
            : base(bits, hashFunctions)
        {
            if (sparseArray.Length != bits) throw new ArgumentException("Length of sparse array should be equal to number of bits", "sparseArray");
            this._array = sparseArray;
        }

        protected override void SetBit(int index)
        {
            this._array[index] = 1;
        }

        protected override bool IsBitSet(int index)
        {
            return this._array[index] > 0;
        }
    }
}