using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace VDS.Common.Filters
{
    [TestFixture, Category("Filters")]
    public class HybridBloomFilterContractTests
        : AbstractBloomFilterContractTests
    {
        protected override IBloomFilter<string> CreateInstance(int numBits, IEnumerable<Func<string, int>> hashFunctions)
        {
            IList<Func<string,int>> functions = hashFunctions as IList<Func<string, int>> ?? hashFunctions.ToList();
            return new HybridBloomFilter<string>(new BloomFilterParameters(numBits, functions.Count()), functions);
        }
    }
}