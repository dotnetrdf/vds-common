using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VDS.Common.Collections;

namespace VDS.Common.Filters
{
    /// <summary>
    /// A bloom filter backed by an array
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    public class BloomFilter<T>
        : BaseBloomFilter<T>
    {
        private readonly int[] _bits;

        public BloomFilter(int bits, IEnumerable<Func<T, int>> hashFunctions)
            : base(bits, hashFunctions)
        {
            this._bits = new int[bits];
        }

        protected override bool IsBitSet(int index)
        {
            return this._bits[index] > 0;
        }

        protected override void SetBit(int index)
        {
            this._bits[index] = 1;
        }
    }
}