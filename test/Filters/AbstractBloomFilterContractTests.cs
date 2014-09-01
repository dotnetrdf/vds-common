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

        [Test]
        public void BloomFilterMayContain1()
        {
            const string item1 = "test";

            IBloomFilter<String> filter = this.CreateInstance(100, Enumerable.Repeat(((Func<String, int>) (x => x.GetHashCode())), 2));
            Assert.IsFalse(filter.MayContain(item1));

            Assert.IsTrue(filter.Add(item1));
            Assert.IsTrue(filter.MayContain(item1));
        }

        [Test]
        public void BloomFilterMayContain2()
        {
            const string item1 = "test";
            const string item2 = "other";

            IBloomFilter<String> filter = this.CreateInstance(100, Enumerable.Repeat(((Func<String, int>)(x => x.GetHashCode())), 2));
            Assert.IsFalse(filter.MayContain(item1));
            Assert.IsFalse(filter.MayContain(item2));

            Assert.IsTrue(filter.Add(item1));
            Assert.IsTrue(filter.MayContain(item1));
            Assert.IsFalse(filter.MayContain(item2));
        }

        [Test]
        public void BloomFilterMayContain3()
        {
            const string item1 = "test";
            const string item2 = "other";

            // In this test use has functions that will map all items to the same hash values
            IBloomFilter<String> filter = this.CreateInstance(100, Enumerable.Repeat(((Func<String, int>)(_ => 0)), 2));
            Assert.IsFalse(filter.MayContain(item1));
            Assert.IsFalse(filter.MayContain(item2));

            Assert.IsTrue(filter.Add(item1));
            Assert.IsTrue(filter.MayContain(item1));
            // Due to dud hash functions should get a false positive here
            Assert.IsTrue(filter.MayContain(item2));
        }

        [Test]
        public void BloomFilterMayContain4()
        {
            const string item1 = "test";
            const string item2 = "other";

            // In this test use hash functions that will map the first and last letters to their character values
            IBloomFilter<String> filter = this.CreateInstance(100, new Func<String, int>[] { x => x.ToCharArray()[0], x => x.ToCharArray()[x.Length - 1] });
            Assert.IsFalse(filter.MayContain(item1));
            Assert.IsFalse(filter.MayContain(item2));

            Assert.IsTrue(filter.Add(item1));
            Assert.IsTrue(filter.MayContain(item1));
            Assert.IsFalse(filter.MayContain(item2));
        }

        [Test]
        public void BloomFilterMayContain5()
        {
            const string item1 = "test";
            const string item2 = "time";

            // In this test use hash functions that will map the first and last letters to their character values
            IBloomFilter<String> filter = this.CreateInstance(100, new Func<String, int>[] { x => x.ToCharArray()[0], x => x.ToCharArray()[x.Length - 1] });
            Assert.IsFalse(filter.MayContain(item1));
            Assert.IsFalse(filter.MayContain(item2));

            Assert.IsTrue(filter.Add(item1));
            Assert.IsTrue(filter.MayContain(item1));
            // Only one hash should be equivalent so should be negative
            Assert.IsFalse(filter.MayContain(item2));
        }

        [Test]
        public void BloomFilterMayContain6()
        {
            const string item1 = "test";
            const string item2 = "tat";

            // In this test use hash functions that will map the first and last letters to their character values
            IBloomFilter<String> filter = this.CreateInstance(100, new Func<String, int>[] { x => x.ToCharArray()[0], x => x.ToCharArray()[x.Length - 1] });
            Assert.IsFalse(filter.MayContain(item1));
            Assert.IsFalse(filter.MayContain(item2));

            Assert.IsTrue(filter.Add(item1));
            Assert.IsTrue(filter.MayContain(item1));
            // Both hash functions give equal values so should be a false positive
            Assert.IsTrue(filter.MayContain(item2));
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