using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VDS.Common.Filters
{
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

        public static IBloomFilterParameters CalculateBloomParameters
    }
}
