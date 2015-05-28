using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace VDS.Common.Filters
{
    [TestFixture]
    public class BloomUtilsTests
    {
        // Test cases are based on values calculated at http://hur.st/bloomfilter

        [TestCase(100000, 100000, 2396265, 17)]
        [TestCase(100000, 10000, 1917012, 13)]
        [TestCase(100000, 1000, 1437759, 10)]
        public void CheckParameterCalculation(int expectedItems, int errorRate, int expectedNumBits, int expectedNumHashFunctions)
        {
            IBloomFilterParameters parameters = BloomUtils.CalculateBloomParameters(expectedItems, errorRate);
            Assert.AreEqual(expectedNumBits, parameters.NumberOfBits);
            Assert.AreEqual(expectedNumHashFunctions, parameters.NumberOfHashFunctions);
        }
    }
}
