using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VDS.Common.Collections
{
    [TestClass]
    public abstract class AbstractCollectionContractTests
    {
        /// <summary>
        /// Method to be implemented by deriving classes to provide the instance to test
        /// </summary>
        /// <returns>Instance to test</returns>
        protected abstract ICollection<String> GetInstance();

        /// <summary>
        /// Method to be implemented by deriving classes to provide the instance to test
        /// </summary>
        /// <param name="contents">Contents for the collection</param>
        /// <returns>Instance to test</returns>
        protected abstract ICollection<String> GetInstance(IEnumerable<String> contents);

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void CollectionContractCopyTo1()
        {
            ICollection<String> c = this.GetInstance();

            c.CopyTo(null, 0);
        }

        [TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CollectionContractCopyTo2()
        {
            ICollection<String> c = this.GetInstance();

            c.CopyTo(new String[10], -1);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void CollectionContractCopyTo3()
        {
            ICollection<String> c = this.GetInstance();

            c.CopyTo(new String[1], 2);
        }

        [TestMethod]
        public void CollectionContractCopyTo4()
        {
            ICollection<String> c = this.GetInstance();

            c.CopyTo(new String[1], 0);
        }

        [TestMethod]
        public void CollectionContractCopyTo5()
        {
            ICollection<String> c = this.GetInstance(new String[] { "test" });

            String[] dest = new String[1];
            c.CopyTo(dest, 0);
            Assert.AreEqual("test", dest[0]);
        }

        [TestMethod]
        public void CollectionContractEnumerate1()
        {
            ICollection<String> c = this.GetInstance();

            using (IEnumerator<String> enumerator = c.GetEnumerator())
            {
                Assert.IsFalse(enumerator.MoveNext());
            }
        }

        [TestMethod]
        public void CollectionContractEnumerate2()
        {
            ICollection<String> c = this.GetInstance(new String[] { "test" });

            using (IEnumerator<String> enumerator = c.GetEnumerator())
            {
                Assert.IsTrue(enumerator.MoveNext());
                Assert.AreEqual("test", enumerator.Current);
                Assert.IsFalse(enumerator.MoveNext());
            }
        }

        [TestMethod]
        public void CollectionContractCount1()
        {
            ICollection<String> c = this.GetInstance();
            Assert.AreEqual(0, c.Count);
        }

        [TestMethod]
        public void CollectionContractCount2()
        {
            ICollection<String> c = this.GetInstance(new String[] { "test" });
            Assert.AreEqual(1, c.Count);
        }

        [TestMethod]
        public void CollectionContractContains1()
        {
            ICollection<String> c = this.GetInstance();
            Assert.IsFalse(c.Contains("test"));
        }

        [TestMethod]
        public void CollectionContractContains2()
        {
            ICollection<String> c = this.GetInstance(new String[] { "test" });
            Assert.IsTrue(c.Contains("test"));
        }
    }

    [TestClass]
    public abstract class AbstractImmutableCollectionContractTests
        : AbstractCollectionContractTests
    {
        [TestMethod, ExpectedException(typeof(NotSupportedException))]
        public void CollectionContractAdd1()
        {
            ICollection<String> c = this.GetInstance();
            c.Add("test");
        }

        [TestMethod, ExpectedException(typeof(NotSupportedException))]
        public void CollectionContractRemove1()
        {
            ICollection<String> c = this.GetInstance();
            c.Remove("test");
        }

        [TestMethod, ExpectedException(typeof(NotSupportedException))]
        public void CollectionContractClear1()
        {
            ICollection<String> c = this.GetInstance();
            c.Clear();
        }
    }

    [TestClass]
    public abstract class AbstractMutableCollectionContractTests
        : AbstractCollectionContractTests
    {
        [TestMethod]
        public void CollectionContractAdd1()
        {
            ICollection<String> c = this.GetInstance();

            c.Add("test");
            Assert.IsTrue(c.Contains("test"));
            Assert.AreEqual(1, c.Count);
        }

        [TestMethod]
        public void CollectionContractAdd2()
        {
            ICollection<String> c = this.GetInstance();

            for (int i = 0; i < 100; i++)
            {
                c.Add("test" + i);
                Assert.IsTrue(c.Contains("test" + i));
                Assert.AreEqual(i + 1, c.Count);
            }
        }

        [TestMethod]
        public void CollectionContractRemove1()
        {
            ICollection<String> c = this.GetInstance();

            c.Add("test");
            Assert.IsTrue(c.Contains("test"));
            c.Remove("test");
            Assert.IsFalse(c.Contains("test"));
        }

        [TestMethod]
        public void CollectionContractRemove2()
        {
            ICollection<String> c = this.GetInstance();

            c.Add("test");
            c.Add("test");
            Assert.IsTrue(c.Contains("test"));
            c.Remove("test");

            //True because only one instance should get removed
            Assert.IsTrue(c.Contains("test"));
        }

        [TestMethod]
        public void CollectionContractClear1()
        {
            ICollection<String> c = this.GetInstance();

            Assert.AreEqual(0, c.Count);
            c.Clear();
            Assert.AreEqual(0, c.Count);
        }

        [TestMethod]
        public void CollectionContractClear2()
        {
            ICollection<String> c = this.GetInstance();

            c.Add("test");
            Assert.IsTrue(c.Contains("test"));
            Assert.AreEqual(1, c.Count);
            c.Clear();
            Assert.IsFalse(c.Contains("test"));
            Assert.AreEqual(0, c.Count);
        }
    }

    [TestClass]
    public class ListContractTests
        : AbstractMutableCollectionContractTests
    {
        protected override ICollection<string> GetInstance()
        {
            return new List<String>();
        }

        protected override ICollection<string> GetInstance(IEnumerable<string> contents)
        {
            return new List<String>(contents);
        }
    }

    [TestClass]
    public class ImmutableViewContractTests
        : AbstractImmutableCollectionContractTests
    {
        protected override ICollection<string> GetInstance()
        {
            return new ImmutableView<String>();
        }

        protected override ICollection<string> GetInstance(IEnumerable<string> contents)
        {
            return new ImmutableView<String>(contents);
        }
    }

    [TestClass]
    public class MaterializedImmutableViewContractTests
        : AbstractImmutableCollectionContractTests
    {
        protected override ICollection<string> GetInstance()
        {
            return new MaterializedImmutableView<String>();
        }

        protected override ICollection<string> GetInstance(IEnumerable<string> contents)
        {
            return new MaterializedImmutableView<String>(contents);
        }
    }
}
