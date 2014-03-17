using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;

namespace VDS.Common.Collections
{
    [TestFixtureAttribute]
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

        [TestAttribute]
        public void BoundedListContractAdd1()
        {
            IBoundedList<string> list = this.GetInstance(2);
            list.Add("a");
            Assert.AreEqual(1, list.Count);
            Assert.IsTrue(list.Contains("a"));
            list.Add("b");
            Assert.AreEqual(2, list.Count);
            Assert.IsTrue(list.Contains("b"));

            Assert.AreEqual("a", list[0]);
            Assert.AreEqual("b", list[1]);
        }
        
        [TestAttribute, ExpectedException(typeof(InvalidOperationException))]
        public void BoundedListContractAddError1()
        {
            IBoundedList<String> list = this.GetInstance(1);
            if (list.OverflowPolicy != BoundedListOverflowPolicy.Error) Assert.Inconclusive("Test is only applicable to implementations with an OverflowPolicy of Error");

            list.Add("a");
            Assert.AreEqual(1, list.Count);
            Assert.IsTrue(list.Contains("a"));
            Assert.AreEqual("a", list[0]);

            // Adding an additional item should exceed capacity and result in an error
            list.Add("b");
        }

        [TestAttribute]
        public void BoundedListContractAddOverwrite1()
        {
            IBoundedList<String> list = this.GetInstance(2);
            if (list.OverflowPolicy != BoundedListOverflowPolicy.Overwrite) Assert.Inconclusive("Test is only applicable to implementations with an OverflowPolicy of Overwrite");

            list.Add("a");
            Assert.AreEqual(1, list.Count);
            Assert.IsTrue(list.Contains("a"));
            Assert.AreEqual("a", list[0]);

            list.Add("b");
            Assert.AreEqual(2, list.Count);
            Assert.IsTrue(list.Contains("b"));
            Assert.AreEqual("b", list[1]);

            // Third item should overwrite the first
            list.Add("c");
            Assert.AreEqual(2, list.Count);
            Assert.IsTrue(list.Contains("c"));
            Assert.IsFalse(list.Contains("a"));
            Assert.IsTrue(list.Contains("b"));
            // Also the indexes will shift
            Assert.AreEqual("b", list[0]);
            Assert.AreEqual("c", list[1]);
        }

        [TestAttribute]
        public void BoundedListContractAddOverwrite2()
        {
            IBoundedList<String> list = this.GetInstance(10);
            if (list.OverflowPolicy != BoundedListOverflowPolicy.Overwrite) Assert.Inconclusive("Test is only applicable to implementations with an OverflowPolicy of Overwrite");

            List<String> items = new List<string>();
            for (int i = 0; i < 100; i++)
            {
                // Find the oldest item and create a new item (but don't add it yet)
                // Check the oldest item is in the list and the new item isn't yet
                String oldestItem = list.Count > 0 ? list[0] : null;
                String newItem = i.ToString(CultureInfo.InvariantCulture);
                Assert.IsFalse(list.Contains(newItem));
                if (oldestItem != null) Assert.IsTrue(list.Contains(oldestItem));

                // Add the new item
                // Depending on whether we've reached the capacity yet this will ovewrite the oldest item
                bool expectOvewrite = list.Count == list.Capacity;
                list.Add(newItem);
                items.Add(newItem);
                Assert.IsTrue(list.Contains(newItem));
                if (oldestItem != null)
                {
                    if (expectOvewrite)
                    {
                        Assert.IsFalse(list.Contains(oldestItem));
                    }
                    else
                    {
                        Assert.IsTrue(list.Contains(oldestItem));
                    }
                }

                // Check the items
                if (expectOvewrite)
                {
                    // Check old items have been overwritten and are no longer present
                    for (int index = 0; index < items.Count - list.Capacity; index++)
                    {
                        Assert.IsFalse(list.Contains(items[index]));
                    }
                }
                // Check current items are at expected indexes
                for (int index = Math.Max(0, items.Count - list.Count); index < items.Count; index++)
                {
                    Assert.IsTrue(list.Contains(items[index]));
                    int listIndex = items.Count > list.Capacity ? index - (items.Count - list.Capacity) : index;
                    Assert.AreEqual(items[index], list[listIndex]);
                }
            }
        }
    }

    [TestFixtureAttribute]
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

    [TestFixtureAttribute]
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
