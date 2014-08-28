using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace VDS.Common.Filters
{
    [TestFixture]
    public abstract class AbstractBloomFilterContractTests
    {
        /// <summary>
        /// Creates an instance of a bloom filter for testing
        /// </summary>
        /// <param name="numBits">Number of bits</param>
        /// <param name="hashFunctions">Hash Functions</param>
        /// <returns>Bloom Filter</returns>
        protected abstract IBloomFilter<String> CreateInstance(int numBits, IEnumerable<Func<String, int>> hashFunctions);

        [TestCase(-1), 
         TestCase(0), 
         TestCase(1),
         ExpectedException(typeof(ArgumentException))]
        public void BloomFilterInstantiation1(int numBits)
        {
            CreateInstance(numBits, Enumerable.Empty<Func<String, int>>());
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void BloomFilterInstantiation2()
        {
            CreateInstance(2, Enumerable.Empty<Func<String, int>>());
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void BloomFilterInstantiation3()
        {
            CreateInstance(2, Enumerable.Repeat((Func<String, int>)(s => s.GetHashCode()), 1));
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void BloomFilterInstantiation4()
        {
            CreateInstance(2, Enumerable.Repeat((Func<String, int>)(s => s.GetHashCode()), 2));
        }

        [Test]
        public void BloomFilterInstantiation5()
        {
            CreateInstance(3, Enumerable.Repeat((Func<String, int>)(s => s.GetHashCode()), 2));
        }
    }

    [TestFixture]
    public class BloomFilterContractTests
        : AbstractBloomFilterContractTests
    {
        protected override IBloomFilter<string> CreateInstance(int numBits, IEnumerable<Func<string, int>> hashFunctions)
        {
            return new BloomFilter<string>(numBits, hashFunctions);
        }
    }

    public class SparseBloomFilterContractTests
        : AbstractBloomFilterContractTests
    {
        protected override IBloomFilter<string> CreateInstance(int numBits, IEnumerable<Func<string, int>> hashFunctions)
        {
            return new SparseBloomFilter<string>(numBits, hashFunctions);
        }
    }
}