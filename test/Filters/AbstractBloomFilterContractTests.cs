/*
VDS.Common is licensed under the MIT License

Copyright (c) 2012-2015 Robert Vesse

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
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace VDS.Common.Filters
{
    [TestFixture,Category("Filters")]
    public abstract class AbstractBloomFilterContractTests
    {
        /// <summary>
        /// Creates an instance of a bloom filter for testing
        /// </summary>
        /// <param name="numBits">Number of bits</param>
        /// <param name="hashFunctions">Hash Functions</param>
        /// <returns>Bloom Filter</returns>
        protected abstract IBloomFilter<string> CreateInstance(int numBits, IEnumerable<Func<string, int>> hashFunctions);

        [Test]
        public void ThrowsOnNumberOfBitsOutOfRange([Values(-1,0)]int numBits)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => CreateInstance(numBits, Enumerable.Empty<Func<string, int>>()));
        }

        [Test]
        public void ThrowsOnNullHashFunctions([Values(0,1)]int numHashFunctions)
        {
            Assert.That(() =>
            {
                CreateInstance(2, Enumerable.Repeat((Func<string, int>)( s => s.GetHashCode() ), numHashFunctions));
            }, Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [TestCase(2,2)]
        [TestCase(2,3)]
        public void ThrowsOnFewerOrEqualHashFunctions(int numBits,int numHashFunctions)
        {
            Assert.That(() =>
            {
                CreateInstance(numBits, Enumerable.Repeat((Func<string, int>)( s => s.GetHashCode() ), numHashFunctions));
            }, Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [TestCase(3,2)]
        [TestCase(4,2)]
        [TestCase(4,3)]
        public void DoesNotThrowOnValidParameters(int numBits,int numHashFunctions)
        {
            Assert.That(() =>
            {
                CreateInstance(numBits, Enumerable.Repeat((Func<string, int>)( s => s.GetHashCode() ), numHashFunctions));
            }, Throws.Nothing);
        }

        [Test]
        public void BloomFilterMayContain1()
        {
            const string item1 = "test";

            IBloomFilter<string> filter = this.CreateInstance(100, Enumerable.Repeat(((Func<string, int>) (x => x.GetHashCode())), 2));
            Assert.IsFalse(filter.MayContain(item1));

            Assert.IsTrue(filter.Add(item1));
            Assert.IsTrue(filter.MayContain(item1));
        }

        [Test]
        public void BloomFilterMayContain2()
        {
            const string item1 = "test";
            const string item2 = "other";

            IBloomFilter<string> filter = this.CreateInstance(100, Enumerable.Repeat(((Func<string, int>)(x => x.GetHashCode())), 2));
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
            IBloomFilter<string> filter = this.CreateInstance(100, Enumerable.Repeat(((Func<string, int>)(_ => 0)), 2));
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
            IBloomFilter<string> filter = this.CreateInstance(100, new Func<string, int>[] { x => x.ToCharArray()[0], x => x.ToCharArray()[x.Length - 1] });
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
            IBloomFilter<string> filter = this.CreateInstance(100, new Func<string, int>[] { x => x.ToCharArray()[0], x => x.ToCharArray()[x.Length - 1] });
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
            IBloomFilter<string> filter = this.CreateInstance(100, new Func<string, int>[] { x => x.ToCharArray()[0], x => x.ToCharArray()[x.Length - 1] });
            Assert.IsFalse(filter.MayContain(item1));
            Assert.IsFalse(filter.MayContain(item2));

            Assert.IsTrue(filter.Add(item1));
            Assert.IsTrue(filter.MayContain(item1));
            // Both hash functions give equal values so should be a false positive
            Assert.IsTrue(filter.MayContain(item2));
        }

        [Test]
        public void BloomFilterAdd1()
        {
            const string item1 = "test";

            IBloomFilter<string> filter = this.CreateInstance(100, Enumerable.Repeat(((Func<string, int>)(x => x.GetHashCode())), 2));
            Assert.IsFalse(filter.MayContain(item1));

            Assert.IsTrue(filter.Add(item1));
            Assert.IsTrue(filter.MayContain(item1));

            // Adding the item again should report false since it is already in the filter
            Assert.IsFalse(filter.Add(item1));
            Assert.IsTrue(filter.MayContain(item1));
        }

        [Test]
        public void BloomFilterAdd2()
        {
            const string item1 = "test";
            const string item2 = "other";

            IBloomFilter<string> filter = this.CreateInstance(100, Enumerable.Repeat(((Func<string, int>)(x => x.GetHashCode())), 2));
            Assert.IsFalse(filter.MayContain(item1));
            Assert.IsFalse(filter.MayContain(item2));

            Assert.IsTrue(filter.Add(item1));
            Assert.IsTrue(filter.MayContain(item1));

            // Adding the second item should report true because it is considered different
            Assert.IsTrue(filter.Add(item2));
            Assert.IsTrue(filter.MayContain(item2));
        }

        [Test]
        public void BloomFilterAdd3()
        {
            const string item1 = "test";
            const string item2 = "other";

            // In this test use has functions that will map all items to the same hash values
            IBloomFilter<string> filter = this.CreateInstance(100, Enumerable.Repeat(((Func<string, int>)(_ => 0)), 2));
            Assert.IsFalse(filter.MayContain(item1));
            Assert.IsFalse(filter.MayContain(item2));

            Assert.IsTrue(filter.Add(item1));
            Assert.IsTrue(filter.MayContain(item1));

            // Due to dud hash functions should get false here since the items are considered equivalent
            // This is a false positive
            Assert.IsFalse(filter.Add(item2));
            Assert.IsTrue(filter.MayContain(item2));
        }

        [Test]
        public void BloomFilterAdd4()
        {
            const string item1 = "test";
            const string item2 = "other";

            // In this test use hash functions that will map the first and last letters to their character values
            IBloomFilter<string> filter = this.CreateInstance(100, new Func<string, int>[] { x => x.ToCharArray()[0], x => x.ToCharArray()[x.Length - 1] });
            Assert.IsFalse(filter.MayContain(item1));
            Assert.IsFalse(filter.MayContain(item2));

            Assert.IsTrue(filter.Add(item1));
            Assert.IsTrue(filter.MayContain(item1));
            Assert.IsFalse(filter.MayContain(item2));

            // Should be possible to add the second item
            Assert.IsTrue(filter.Add(item2));
            Assert.IsTrue(filter.MayContain(item2));
        }

        [Test]
        public void BloomFilterAdd5()
        {
            const string item1 = "test";
            const string item2 = "time";

            // In this test use hash functions that will map the first and last letters to their character values
            IBloomFilter<string> filter = this.CreateInstance(100, new Func<string, int>[] { x => x.ToCharArray()[0], x => x.ToCharArray()[x.Length - 1] });
            Assert.IsFalse(filter.MayContain(item1));
            Assert.IsFalse(filter.MayContain(item2));

            Assert.IsTrue(filter.Add(item1));
            Assert.IsTrue(filter.MayContain(item1));
            Assert.IsFalse(filter.MayContain(item2));

            // Only one hash should be equivalent so should be possible to add the additional item
            Assert.IsTrue(filter.Add(item2));
            Assert.IsTrue(filter.MayContain(item1));
        }

        [Test]
        public void BloomFilterAdd6()
        {
            const string item1 = "test";
            const string item2 = "tat";

            // In this test use hash functions that will map the first and last letters to their character values
            IBloomFilter<string> filter = this.CreateInstance(100, new Func<string, int>[] { x => x.ToCharArray()[0], x => x.ToCharArray()[x.Length - 1] });
            Assert.IsFalse(filter.MayContain(item1));
            Assert.IsFalse(filter.MayContain(item2));

            Assert.IsTrue(filter.Add(item1));
            Assert.IsTrue(filter.MayContain(item1));

            // Both hash functions give equal values so should return false since it is already considered to be in the filter
            // This is a false positive
            Assert.IsFalse(filter.Add(item2));
            Assert.IsTrue(filter.MayContain(item2));
        }
    }
}