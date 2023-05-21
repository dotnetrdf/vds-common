using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace VDS.Common.Filters
{
    [TestFixture, Category("Filters")]
    public class FastBloomFilterContractTests
        : AbstractBloomFilterContractTests
    {
        protected override IBloomFilter<string> CreateInstance(int numBits, IEnumerable<Func<string, int>> hashFunctions)
        {
            IList<Func<string, int>> funcs = hashFunctions as IList<Func<string, int>> ?? hashFunctions.ToList();
            Func<string, int> h1 = funcs.FirstOrDefault();
            Func<string, int> h2 = funcs.Skip(1).FirstOrDefault();

            return new FastBloomFilter<string>(new BloomFilterParameters(numBits, funcs.Count), h1, h2);
        }
    }
}