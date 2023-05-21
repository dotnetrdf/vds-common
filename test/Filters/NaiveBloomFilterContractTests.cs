using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace VDS.Common.Filters
{
    [TestFixture,Category("Filters")]
    public class NaiveBloomFilterContractTests
        : AbstractBloomFilterContractTests
    {
        protected override IBloomFilter<string> CreateInstance(int numBits, IEnumerable<Func<string, int>> hashFunctions)
        {
            return new NaiveBloomFilter<string>(numBits, hashFunctions);
        }
    }
}