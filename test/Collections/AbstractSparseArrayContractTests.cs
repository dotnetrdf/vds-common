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
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace VDS.Common.Collections
{
    [TestFixture,Category("Arrays")]
    public abstract class AbstractSparseArrayContractTests
    {
        /// <summary>
        /// Creates a new sparse array of the given length to test
        /// </summary>
        /// <param name="length">Length</param>
        /// <returns></returns>
        public abstract ISparseArray<int> CreateInstance(int length);

        [Test]
        public void SparseArrayBadInstantiation()
        {
            Assert.Throws<ArgumentException>(()=> this.CreateInstance(-1));
        }

        [Test]
        public void SparseArrayEmpty1()
        {
            ISparseArray<int> array = this.CreateInstance(0);
            Assert.AreEqual(0, array.Length);
        }

        [Test]
        public void SparseArrayEmpty2()
        {
            ISparseArray<int> array = this.CreateInstance(0);
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var _ = array[0];
            });
        }

        [Test]
        public void SparseArrayEmpty3()
        {
            ISparseArray<int> array = this.CreateInstance(0);
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var _ = array[-1];
            });
        }

        [Test]
        [Parallelizable(ParallelScope.Children)]
        public void SparseArrayGetSet1([Range(0,10000,1000)]int length)
        {
            ISparseArray<int> array = this.CreateInstance(length);
            Assert.AreEqual(length, array.Length);
            for (int i = 0, j = 1; i < array.Length; i++, j *= 2)
            {
                // Should have default value
                Assert.AreEqual(default(int), array[i]);

                // Set only powers of 2
                if (i != j) continue;
                array[i] = 1;
                Assert.AreEqual(1, array[i]);
            }
        }

        [Test]
        [Parallelizable(ParallelScope.Children)]
        public void SparseArrayGetSet2([Range(0,10000,1000)]int length)
        {
            ISparseArray<int> array = this.CreateInstance(length);
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                int _ = array[-1];
            });
        }

        [Test]
        [Parallelizable(ParallelScope.Children)]
        public void SparseArrayGetSet3([Range(0,10000,1000)]int length)
        {
            ISparseArray<int> array = this.CreateInstance(length);
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                int _ = array[length];
            });
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
            Assert.AreNotEqual(default(int), 1);

            // Sparsely filled array
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

            using IEnumerator<int> sparsEnumerator = array.GetEnumerator();
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

        [TestCase(1),
         TestCase(10),
         TestCase(50),
         TestCase(100),
         TestCase(250),
         TestCase(500),
         TestCase(1000),
         TestCase(10000)]
        public void SparseArrayEnumerator2(int length)
        {
            Assert.AreNotEqual(default(int), 1);

            // Completely sparse array i.e. no actual data
            ISparseArray<int> array = this.CreateInstance(length);
            Assert.AreEqual(length, array.Length);

            for (int i = 0; i < array.Length; i++)
            {
                // Should have default value
                Assert.AreEqual(default(int), array[i]);
            }

            using IEnumerator<int> enumerator = array.GetEnumerator();

            int index = -1;
            while (enumerator.MoveNext())
            {
                index++;
                Assert.AreEqual(default(int), enumerator.Current, "Incorrect value at index " + index);
            }
            Assert.AreEqual(length - 1, index);
        }

        [TestCase(1),
         TestCase(10),
         TestCase(50),
         TestCase(100),
         TestCase(250),
         TestCase(500),
         TestCase(1000),
         TestCase(10000)]
        public void SparseArrayEnumerator3(int length)
        {
            Assert.AreNotEqual(default(int), 1);

            // Completely filled array
            ISparseArray<int> array = this.CreateInstance(length);
            Assert.AreEqual(length, array.Length);
            int[] actualArray = new int[length];
            for (int i = 0; i < array.Length; i++)
            {
                // Should have default value
                Assert.AreEqual(default(int), array[i]);

                // Set all entries
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
    }

    [TestFixture,Category("Arrays")]
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
     TestFixture(1000),
     Category("Arrays")]
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

    [TestFixture, Category("Arrays")]
    public class BinarySparseArrayTests
        : AbstractSparseArrayContractTests
    {
        public override ISparseArray<int> CreateInstance(int length)
        {
            return new BinarySparseArray<int>(length);
        }
    }
}