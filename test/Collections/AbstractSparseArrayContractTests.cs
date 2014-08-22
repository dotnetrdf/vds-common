using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace VDS.Common.Collections
{
    [TestFixture]
    public abstract class AbstractSparseArrayContractTests
    {
        /// <summary>
        /// Creates a new sparse array of the given length to test
        /// </summary>
        /// <param name="length">Length</param>
        /// <returns></returns>
        public abstract ISparseArray<int> CreateInstance(int length);

        [Test, ExpectedException(typeof(ArgumentException))]
        public void SparseArrayBadInstantiation()
        {
            this.CreateInstance(-1);
        }

        [Test]
        public void SparseArrayEmpty1()
        {
            ISparseArray<int> array = this.CreateInstance(0);
            Assert.AreEqual(0, array.Length);
        }

        [Test, ExpectedException(typeof(IndexOutOfRangeException))]
        public void SparseArrayEmpty2()
        {
            ISparseArray<int> array = this.CreateInstance(0);
            int i = array[0];
        }

        [TestCase(1),
         TestCase(10),
         TestCase(50),
         TestCase(100),
         TestCase(250),
         TestCase(500),
         TestCase(1000),
         TestCase(10000)]
        public void SparseArrayGetSet1(int length)
        {
            ISparseArray<int> array = this.CreateInstance(length);
            Assert.AreEqual(length, array.Length);
            for (int i = 0, j = 1; i < array.Length; i++, j*=2)
            {
                // Should have default value
                Assert.AreEqual(default(int), array[i]);

                // Set only powers of 2
                if (i != j) continue;
                array[i] = 1;
                Assert.AreEqual(1, array[i]);
            }
        }

        [TestCase(1),
         TestCase(10),
         TestCase(50),
         TestCase(100),
         TestCase(250),
         TestCase(500),
         TestCase(1000),
         TestCase(10000)]
        public void SparseArrayEnumerator1(int length)
        {
            ISparseArray<int> array = this.CreateInstance(length);
            Assert.AreEqual(length, array.Length);
            int[] actualArray = new int[length];
            for (int i = 0, j = 1; i < array.Length; i++, j *= 2)
            {
                // Should have default value
                Assert.AreEqual(default(int), array[i]);

                // Set only powers of 2
                if (i != j) continue;
                array[i] = 1;
                actualArray[i] = 1;
                Assert.AreEqual(1, array[i]);
            }

            IEnumerator<int> sparsEnumerator = array.GetEnumerator();
            IEnumerator actualEnumerator = actualArray.GetEnumerator();

            int index = -1;
            while (actualEnumerator.MoveNext())
            {
                index++;
                Assert.IsTrue(sparsEnumerator.MoveNext(), "Unable to move next at index " + index);
                Assert.AreEqual(actualEnumerator.Current, sparsEnumerator.Current, "Incorrect value at index " + index);
            }
            Assert.AreEqual(length - 1, index);
        }

        public void SparseArrayEnumerator2(int length)
        {
            ISparseArray<int> array = this.CreateInstance(length);
            Assert.AreEqual(length, array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                // Should have default value
                Assert.AreEqual(default(int), i);
            }

            IEnumerator<int> enumerator = array.GetEnumerator();

            int index = -1;
            while (enumerator.MoveNext())
            {
                index++;
                Assert.AreEqual(default(int), enumerator.Current, "Incorrect value at index " + index);
            }
            Assert.AreEqual(length - 1, index);
        }
    }
    
    [TestFixture]
    public class LinkedSparseArrayTests
        : AbstractSparseArrayContractTests
    {
        public override ISparseArray<int> CreateInstance(int length)
        {
            return new LinkedSparseArray<int>(length);
        }
    }

    [TestFixture(1),
     TestFixture(10),
     TestFixture(250),
     TestFixture(1000)]
    public class BlockSparseArrayTests
        : AbstractSparseArrayContractTests
    {
        public BlockSparseArrayTests(int blockSize)
        {
            this.BlockSize = blockSize;
        }

        private int BlockSize { get; set; }

        public override ISparseArray<int> CreateInstance(int length)
        {
            return new BlockSparseArray<int>(length, this.BlockSize);
        }
    }

    public class BinarySparseArrayTests
        : AbstractSparseArrayContractTests
    {
        public override ISparseArray<int> CreateInstance(int length)
        {
            return new BinarySparseArray<int>(length);
        }
    }
}
