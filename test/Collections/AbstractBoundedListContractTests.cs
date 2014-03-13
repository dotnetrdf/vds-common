using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VDS.Common.Collections
{
    [TestClass]
    public abstract class AbstractBoundedListContractTests
        : AbstractCollectionContractTests
    {
        protected abstract IBoundedList<string> GetInstance(int capacity);

        protected abstract IBoundedList<string> GetInstance(int capacity, IEnumerable<string> contents);

        protected override ICollection<string> GetInstance()
        {
            return GetInstance(10);
        }

        protected override ICollection<string> GetInstance(IEnumerable<string> contents)
        {
            var enumerable = contents as IList<string> ?? contents.ToList();
            return GetInstance(enumerable.Count, enumerable);
        }

        [TestMethod]
        public void BoundedListContractAdd1()
        {
            IBoundedList<string> list = this.GetInstance(2);
            list.Add("a");
            Assert.AreEqual(1, list.Count);
            list.Add("b");
            Assert.AreEqual(2, list.Count);

            Assert.AreEqual("a", list[0]);
            Assert.AreEqual("b", list[1]);
        }
    }

    [TestClass]
    public class BoundedListTests
        : AbstractBoundedListContractTests
    {

        protected override IBoundedList<string> GetInstance(int capacity)
        {
            return new BoundedList<string>(capacity);
        }

        protected override IBoundedList<string> GetInstance(int capacity, IEnumerable<string> contents)
        {
            return new BoundedList<string>(capacity, contents);
        }
    }

    [TestClass]
    public class RingBufferTests
        : AbstractBoundedListContractTests
    {

        protected override IBoundedList<string> GetInstance(int capacity)
        {
            return new RingBuffer<string>(capacity);
        }

        protected override IBoundedList<string> GetInstance(int capacity, IEnumerable<string> contents)
        {
            return new RingBuffer<string>(capacity, contents);
        }
    }
}
