using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VDS.Common.Filters
{
    /// <summary>
    /// Provides utility functions for working with bloom filters
    /// </summary>
    public static class BloomUtils
    {
        /*
        n
            Expected number of items to be added to filter
        p
            Probability of false positives, float between 0 and 1 or a number indicating 1-in-p
        m
            Number of bits in the filter
        k
            Number of hash functions needed to yield desired false positive rate
        
        m = ceil((n * log(p)) / log(1.0 / (pow(2.0, log(2.0)))));
        k = round(log(2.0) * m / n);
        */

        /// <summary>
        /// Calculates the necessary parameters for a Bloom Filter
        /// </summary>
        /// <param name="expectedItems">Expected number of items that will be added to the filter</param>
        /// <param name="errorRate">Desired error rate, the value given is treated as a 1 in N error rate</param>
        /// <returns>Bloom Filter parameters</returns>
        public static IBloomFilterParameters CalculateBloomParameters(int expectedItems, int errorRate)
        {
            if (expectedItems < 1) throw new ArgumentException("expectedItems must be >= 1", "expectedItems");
            if (errorRate < 1) throw new ArgumentException("errorRate must be >= 1", "errorRate");
            return CalculateBloomParameters(expectedItems, 1d/errorRate);
        }

        /// <summary>
        /// Calculates the necessary parameters for a Bloom Filter
        /// </summary>
        /// <param name="expectedItems">Expected number of items that will be added to the filter</param>
        /// <param name="errorRate">Desired error rate given as a value between 0 and 1.0</param>
        /// <returns>Bloom Filter parameters</returns>
        public static IBloomFilterParameters CalculateBloomParameters(int expectedItems, double errorRate)
        {
            if (expectedItems < 1) throw new ArgumentException("expectedItems must be >= 1", "expectedItems");
            if (errorRate < 0d || errorRate > 1d) throw new ArgumentException("errorRate must be in the range 0-1", "errorRate");

            double numBits = Math.Ceiling((expectedItems*Math.Log(errorRate))/Math.Log(1d/Math.Pow(2d, Math.Log(2))));
            double numHashFunctions = Math.Round(Math.Log(2d)*(numBits/expectedItems));

            return new BloomFilterParameters(Convert.ToInt32(numBits), Convert.ToInt32(numHashFunctions));
        }
    }
}
