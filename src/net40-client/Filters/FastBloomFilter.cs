using System;
using VDS.Common.Filters.Storage;

namespace VDS.Common.Filters
{
    /// <summary>
    /// A fast bloom filter backed by an array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FastBloomFilter<T>
        : BaseFastBloomFilter<T>
    {
        public FastBloomFilter(IBloomFilterParameters parameters, Func<T, int> h1, Func<T, int> h2)
            : base(new ArrayStorage(parameters.NumberOfBits), parameters, h1, h2) { }
    }
}
