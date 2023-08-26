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
            Assert.Throws<ArgumentException>(()=> CreateInstance(-1));
        }

        [Test]
        public void SparseArrayEmpty1()
        {
            var array = CreateInstance(0);
            Assert.AreEqual(0, array.Length);
        }

        [Test]
        public void SparseArrayEmpty2()
        {
            var array = CreateInstance(0);
            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                var _ = array[0];
            });
        }

        [Test]
        public void SparseArrayEmpty3()
        {
            var array = CreateInstance(0);
            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                var _ = array[-1];
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
        public void SparseArrayGetSet1(int length)
        {
            var array = CreateInstance(length);
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

        [TestCase(1),
         TestCase(10),
         TestCase(50),
         TestCase(100),
         TestCase(250),
         TestCase(500),
         TestCase(1000),
         TestCase(10000)]
        public void SparseArrayGetSet2(int length)
        {
            var array = CreateInstance(length);
            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                var _ = array[-1];
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
        public void SparseArrayGetSet3(int length)
        {
            var array = CreateInstance(length);
            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                var _ = array[length];
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
            var array = CreateInstance(length);
            Assert.AreEqual(length, array.Length);
            var actualArray = new int[length];
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

            using var sparsEnumerator = array.GetEnumerator();
            var actualEnumerator = actualArray.GetEnumerator();

            var index = -1;
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
            var array = CreateInstance(length);
            Assert.AreEqual(length, array.Length);

            foreach (var t in array)
            {
                // Should have default value
                Assert.AreEqual(default(int), t);
            }

            using var enumerator = array.GetEnumerator();

            var index = -1;
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
            var array = CreateInstance(length);
            Assert.AreEqual(length, array.Length);
            var actualArray = new int[length];
            for (var i = 0; i < array.Length; i++)
            {
                // Should have default value
                Assert.AreEqual(default(int), array[i]);

                // Set all entries
                array[i] = 1;
                actualArray[i] = 1;
                Assert.AreEqual(1, array[i]);
            }

            using var sparsEnumerator = array.GetEnumerator();
            var actualEnumerator = actualArray.GetEnumerator();

            var index = -1;
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
            BlockSize = blockSize;
        }

        private int BlockSize { get; set; }

        public override ISparseArray<int> CreateInstance(int length)
        {
            return new BlockSparseArray<int>(length, BlockSize);
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