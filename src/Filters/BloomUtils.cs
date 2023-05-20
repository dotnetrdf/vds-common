/*
VDS.Common is licensed under the MIT License

Copyright (c) 2012-2015 Robert Vesse
Copyright (c) 2016-2018 dotNetRDF Project (http://dotnetrdf.org/)

Permission is hereby granted, free of charge, to any person obtaining a copy of this software
and associated documentation files (the "Software"), to deal in the Software without restriction,
including without limitation the rights to use, copy, modify, merge, publish, distribute,
sublicense, and/or sell copies of the Software, and to permit persons to whom the Software 
is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or
substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;

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
        public static IBloomFilterParameters CalculateBloomParameters(long expectedItems, long errorRate)
        {
            if (expectedItems < 1) throw new ArgumentException("expectedItems must be >= 1", nameof(expectedItems));
            if (errorRate < 1) throw new ArgumentException("errorRate must be >= 1", nameof(errorRate));
            return CalculateBloomParameters(expectedItems, 1d/errorRate);
        }

        /// <summary>
        /// Calculates the necessary parameters for a Bloom Filter
        /// </summary>
        /// <param name="expectedItems">Expected number of items that will be added to the filter</param>
        /// <param name="errorRate">Desired error rate given as a value between 0 and 1.0</param>
        /// <returns>Bloom Filter parameters</returns>
        public static IBloomFilterParameters CalculateBloomParameters(long expectedItems, double errorRate)
        {
            if (expectedItems < 1) throw new ArgumentException("expectedItems must be >= 1", nameof(expectedItems));
            if (errorRate < 0d || errorRate > 1d) throw new ArgumentException("errorRate must be in the range 0-1", nameof(errorRate));

            double numBits = Math.Ceiling((expectedItems*Math.Log(errorRate))/Math.Log(1d/Math.Pow(2d, Math.Log(2))));
            double numHashFunctions = Math.Round(Math.Log(2d)*(numBits/expectedItems));

            try
            {
                return new BloomFilterParameters(Convert.ToInt32(numBits), Convert.ToInt32(numHashFunctions));
            }
            catch (OverflowException)
            {
                throw new ArgumentException(String.Format("The given parameters would result in a Bloom filter that required more than {0} bits/hash functions", Int32.MaxValue));
            }
        }

        /*
         ln p = -(m/n) * ((ln 2)^2).
         */

        /// <summary>
        /// Given some parameters and the expected number of items calculates the error rate
        /// </summary>
        /// <param name="expectedItems">Expected number of items that will be added to the filter</param>
        /// <param name="parameters">Bloom Filter Parameters</param>
        /// <returns>Error Rate as a value between 0 and 1.0</returns>
        public static double CalculateErrorRate(long expectedItems, IBloomFilterParameters parameters)
        {
            if (expectedItems < 1) throw new ArgumentException("expectedItems must be >= 1", nameof(expectedItems));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            double lnP = (-1d*((double) parameters.NumberOfBits / expectedItems))*Math.Pow(Math.Log(2), 2d);
            return Math.Pow(Math.E, lnP);
        }
    }
}
