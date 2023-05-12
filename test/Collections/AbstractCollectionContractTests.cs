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
    [TestFixture,Category("Collections")]
    public abstract class AbstractCollectionContractTests
    {
        /// <summary>
        /// Method to be implemented by deriving classes to provide the instance to test
        /// </summary>
        /// <returns>Instance to test</returns>
        protected abstract ICollection<string> GetInstance();

        /// <summary>
        /// Method to be implemented by deriving classes to provide the instance to test
        /// </summary>
        /// <param name="contents">Contents for the collection</param>
        /// <returns>Instance to test</returns>
        protected abstract ICollection<string> GetInstance(IEnumerable<string> contents);

        [Test]
        public void CollectionContractCopyTo1()
        {
            ICollection<string> c = this.GetInstance();

            Assert.Throws<ArgumentNullException>(()=>c.CopyTo(null, 0));
        }

        [Test]
        public void CollectionContractCopyTo2()
        {
            ICollection<string> c = this.GetInstance();

            Assert.Throws<ArgumentOutOfRangeException>(()=>c.CopyTo(new string[10], -1));
        }

        [Test]
        public void CollectionContractCopyTo3()
        {
            ICollection<string> c = this.GetInstance();

            Assert.Throws<ArgumentException>(() => c.CopyTo(new string[1], 2));
        }

        [Test]
        public void CollectionContractCopyTo4()
        {
            ICollection<string> c = this.GetInstance();

            c.CopyTo(new string[1], 0);
        }

        [Test]
        public void CollectionContractCopyTo5()
        {
            ICollection<string> c = this.GetInstance(new string[] { "test" });

            string[] dest = new string[1];
            c.CopyTo(dest, 0);
            Assert.AreEqual("test", dest[0]);
        }

        [Test]
        public void CollectionContractCopyTo6()
        {
            string[] data = new string[] { "a", "a", "b", "c" };
            ICollection<string> c = this.GetInstance(data);

            string[] dest = new string[data.Length];
            c.CopyTo(dest, 0);
            Assert.AreEqual(data, dest);
        }

        [Test]
        public void CollectionContractEnumerate1()
        {
            ICollection<string> c = this.GetInstance();

            using (IEnumerator<string> enumerator = c.GetEnumerator())
            {
                Assert.IsFalse(enumerator.MoveNext());
            }
        }

        [Test]
        public void CollectionContractEnumerate2()
        {
            ICollection<string> c = this.GetInstance(new string[] { "test" });

            using (IEnumerator<string> enumerator = c.GetEnumerator())
            {
                Assert.IsTrue(enumerator.MoveNext());
                Assert.AreEqual("test", enumerator.Current);
                Assert.IsFalse(enumerator.MoveNext());
            }
        }

        [Test]
        public void CollectionContractEnumerate3()
        {
            ICollection<string> c = this.GetInstance(new string[] { "a", "b" });

            using (IEnumerator<string> enumerator = c.GetEnumerator())
            {
                Assert.IsTrue(enumerator.MoveNext());
                Assert.AreEqual("a", enumerator.Current);
                Assert.IsTrue(enumerator.MoveNext());
                Assert.AreEqual("b", enumerator.Current);
                Assert.IsFalse(enumerator.MoveNext());
            }
        }

        [Test]
        public void CollectionContractEnumerate4()
        {
            string[] values = new string[] { "a", "a", "b", "c", "d", "e", "e" };
            ICollection<string> c = this.GetInstance(values);

            using (IEnumerator<string> enumerator = c.GetEnumerator())
            {
                int index = 0;
                while (index < values.Length)
                {
                    Assert.IsTrue(enumerator.MoveNext(), "Failed to move next at Index " + index);
                    Assert.AreEqual(values[index], enumerator.Current);
                    index++;
                }
            }
        }

        [Test]
        public void CollectionContractCount1()
        {
            ICollection<string> c = this.GetInstance();
            Assert.AreEqual(0, c.Count);
        }

        [Test]
        public void CollectionContractCount2()
        {
            ICollection<string> c = this.GetInstance(new string[] { "test" });
            Assert.AreEqual(1, c.Count);
        }

        [Test]
        public void CollectionContractContains1()
        {
            ICollection<string> c = this.GetInstance();
            Assert.IsFalse(c.Contains("test"));
        }

        [Test]
        public void CollectionContractContains2()
        {
            ICollection<string> c = this.GetInstance(new string[] { "test" });
            Assert.IsTrue(c.Contains("test"));
        }
    }

    [TestFixture,Category("Collections")]
    public abstract class AbstractImmutableCollectionContractTests
        : AbstractCollectionContractTests
    {
        [Test]
        public void CollectionContractAdd1()
        {
            ICollection<string> c = this.GetInstance();
            Assert.Throws<NotSupportedException>(()=>c.Add("test"));
        }

        [Test]
        public void CollectionContractRemove1()
        {
            ICollection<string> c = this.GetInstance();
            Assert.Throws<NotSupportedException>(() => c.Remove("test"));
        }

        [Test]
        public void CollectionContractClear1()
        {
            ICollection<string> c = this.GetInstance();
            Assert.Throws<NotSupportedException>(() => c.Clear());
        }
    }

    [TestFixture,Category("Collections")]
    public abstract class AbstractMutableCollectionContractTests
        : AbstractCollectionContractTests
    {
        [Test]
        public void CollectionContractAdd1()
        {
            ICollection<string> c = this.GetInstance();

            c.Add("test");
            Assert.IsTrue(c.Contains("test"));
            Assert.AreEqual(1, c.Count);
        }

        [Test]
        public void CollectionContractAdd2()
        {
            ICollection<string> c = this.GetInstance();

            for (int i = 0; i < 100; i++)
            {
                c.Add("test" + i);
                Assert.IsTrue(c.Contains("test" + i));
                Assert.AreEqual(i + 1, c.Count);
            }
        }

        [Test]
        public void CollectionContractRemove1()
        {
            ICollection<string> c = this.GetInstance();

            c.Add("test");
            Assert.IsTrue(c.Contains("test"));
            c.Remove("test");
            Assert.IsFalse(c.Contains("test"));
        }

        [Test]
        public void CollectionContractRemove2()
        {
            ICollection<string> c = this.GetInstance();

            c.Add("test");
            c.Add("test");
            Assert.IsTrue(c.Contains("test"));
            c.Remove("test");

            //True because only one instance should get removed
            Assert.IsTrue(c.Contains("test"));
        }

        [Test]
        public void CollectionContractClear1()
        {
            ICollection<string> c = this.GetInstance();

            Assert.AreEqual(0, c.Count);
            c.Clear();
            Assert.AreEqual(0, c.Count);
        }

        [Test]
        public void CollectionContractClear2()
        {
            ICollection<string> c = this.GetInstance();

            c.Add("test");
            Assert.IsTrue(c.Contains("test"));
            Assert.AreEqual(1, c.Count);
            c.Clear();
            Assert.IsFalse(c.Contains("test"));
            Assert.AreEqual(0, c.Count);
        }
    }

    [TestFixture,Category("Lists")]
    public class ListContractTests
        : AbstractMutableCollectionContractTests
    {
        protected override ICollection<string> GetInstance()
        {
            return new List<string>();
        }

        protected override ICollection<string> GetInstance(IEnumerable<string> contents)
        {
            return new List<string>(contents);
        }
    }

    [TestFixture,Category("Collections")]
    public class ImmutableViewContractTests
        : AbstractImmutableCollectionContractTests
    {
        protected override ICollection<string> GetInstance()
        {
            return new ImmutableView<string>();
        }

        protected override ICollection<string> GetInstance(IEnumerable<string> contents)
        {
            return new ImmutableView<string>(contents);
        }
    }

    [TestFixture,Category("Collections")]
    public class MaterializedImmutableViewContractTests
        : AbstractImmutableCollectionContractTests
    {
        protected override ICollection<string> GetInstance()
        {
            return new MaterializedImmutableView<string>();
        }

        protected override ICollection<string> GetInstance(IEnumerable<string> contents)
        {
            return new MaterializedImmutableView<string>(contents);
        }
    }

    [TestFixture, Category("Collections")]
    public class DuplicateSortedListContractTests
        : AbstractMutableCollectionContractTests
    {
        protected override ICollection<string> GetInstance()
        {
            return new DuplicateSortedList<string>();
        }

        protected override ICollection<string> GetInstance(IEnumerable<string> contents)
        {
            return new DuplicateSortedList<string>(contents);
        }
    }
}
