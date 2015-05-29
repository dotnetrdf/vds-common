using System;
using NUnit.Framework;

namespace VDS.Common.Filters
{
    [TestFixture]
    public class BloomUtilsTests
    {
        // Test cases are based on values calculated at http://hur.st/bloomfilter

        private void CheckErrorRate(long expectedItems, long expectedErrorRate, IBloomFilterParameters parameters)
        {
            long actualErrorRate = CalculateErrorRate(expectedItems, parameters);
            Console.WriteLine("n = {0}, p = 1 in {1}", expectedItems, actualErrorRate);
            Assert.AreEqual(expectedErrorRate, actualErrorRate);

            // If we half the number of items we add the error rate should decrease
            // NB - Since we are expressing error rate as 1 in p actual value will increase
            actualErrorRate = CalculateErrorRate(expectedItems/2, parameters);
            Console.WriteLine("n = {0}, p = 1 in {1}", expectedItems / 2, actualErrorRate);
            Assert.IsTrue(actualErrorRate > expectedErrorRate);

            // If we double the number of items we add the error rate should increase
            // NB - Since we are expressing error rate as 1 in p actual value will decrease
            actualErrorRate = CalculateErrorRate(expectedItems * 2, parameters);
            Console.WriteLine("n = {0}, p = 1 in {1}", expectedItems * 2, actualErrorRate);
            Assert.IsTrue(actualErrorRate < expectedErrorRate);
        }

        private static long CalculateErrorRate(long expectedItems, IBloomFilterParameters parameters)
        {
            double calcErrorRate = BloomUtils.CalculateErrorRate(expectedItems, parameters);
            return Convert.ToInt64(1/calcErrorRate);
        }

        [TestCase(100000, 100000, 2396265, 17)]
        [TestCase(100000, 10000, 1917012, 13)]
        [TestCase(100000, 1000, 1437759, 10)]
        public void CheckParameterCalculation(long expectedItems, long errorRate, int expectedNumBits, int expectedNumHashFunctions)
        {
            IBloomFilterParameters parameters = BloomUtils.CalculateBloomParameters(expectedItems, errorRate);
            Assert.AreEqual(expectedNumBits, parameters.NumberOfBits);
            Assert.AreEqual(expectedNumHashFunctions, parameters.NumberOfHashFunctions);

            CheckErrorRate(expectedItems, errorRate, parameters);
        }
    }
}
