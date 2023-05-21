using System;
using System.Collections.Generic;
using System.Linq;

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

        [Test]
        public override void ThrowsOnNullHashFunctions([Values(0,1)]int numHashFunctions)
        {
            Assert.That(() =>
            {
                CreateInstance(2, Enumerable.Repeat((Func<string, int>)( s => s.GetHashCode() ), numHashFunctions));
            }, Throws.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}