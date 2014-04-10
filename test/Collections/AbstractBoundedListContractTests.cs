/*
VDS.Common is licensed under the MIT License

Copyright (c) 2012-2014 Robert Vesse

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
using System.Globalization;
using System.Linq;
using NUnit.Framework;

namespace VDS.Common.Collections
{
    [TestFixture]
    public abstract class AbstractBoundedListContractTests
        : AbstractMutableCollectionContractTests
    {
        protected abstract IBoundedList<string> GetInstance(int capacity);

        protected abstract IBoundedList<string> GetInstance(int capacity, IEnumerable<string> contents);

        protected override ICollection<string> GetInstance()
        {
            return GetInstance(100);
        }

        protected override ICollection<string> GetInstance(IEnumerable<string> contents)
        {
            var enumerable = contents as IList<string> ?? contents.ToList();
            return GetInstance(100, enumerable);
        }

        [Test]
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

        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void BoundedListContractAddError1()
        {
            IBoundedList<String> list = this.GetInstance(1);
            if (list.OverflowPolicy != BoundedListOverflowPolicy.Error) Assert.Ignore("Test is only applicable to implementations with an OverflowPolicy of Error");

            list.Add("a");
            Assert.AreEqual(1, list.Count);
            Assert.IsTrue(list.Contains("a"));
            Assert.AreEqual("a", list[0]);

            // Adding an additional item should exceed capacity and result in an error
            list.Add("b");
        }

        [TestCase(10, 100),
         TestCase(10, 1000),
         TestCase(1, 100),
         TestCase(100, 10),
         TestCase(100, 1000),
         TestCase(2, 100),
         TestCase(2, 1000)]
        public void BoundedListContractAddError2(int capacity, int iterations)
        {
            IBoundedList<String> list = this.GetInstance(capacity);
            if (list.OverflowPolicy != BoundedListOverflowPolicy.Error) Assert.Ignore("Test is only applicable to implementations with an OverflowPolicy of Error");

            List<String> items = new List<string>();
            for (int i = 0; i < iterations; i++)
            {
                String newItem = i.ToString(CultureInfo.InvariantCulture);
                Assert.IsFalse(list.Contains(newItem));
                items.Add(newItem);

                // Try to add to list, should error once capacity is exceeded
                try
                {
                    list.Add(newItem);
                    Assert.IsTrue(list.Contains(newItem));

                    // Should never exceed list capacity
                    Assert.IsFalse(list.Count > list.MaxCapacity);
                }
                catch (InvalidOperationException ex)
                {
                    // If this error occurs then we expect the list to be full
                    Assert.AreEqual(list.MaxCapacity, list.Count);
                }

                // Check expected items are in list
                for (int index = 0; index < Math.Min(items.Count, list.MaxCapacity); index++)
                {
                    Assert.IsTrue(list.Contains(items[index]));
                    Assert.AreEqual(items[index], list[index]);
                }
                // Check additional items are not in list
                if (items.Count <= list.MaxCapacity) continue;
                for (int index = list.MaxCapacity; index < items.Count; index++)
                {
                    Assert.IsFalse(list.Contains(items[index]));
                }
            }
        }

        [Test]
        public void BoundedListContractAddOverwrite1()
        {
            IBoundedList<String> list = this.GetInstance(2);
            if (list.OverflowPolicy != BoundedListOverflowPolicy.Overwrite) Assert.Ignore("Test is only applicable to implementations with an OverflowPolicy of Overwrite");

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

        [TestCase(10, 100),
         TestCase(10, 1000),
         TestCase(1, 100),
         TestCase(100, 10),
         TestCase(100, 1000),
         TestCase(2, 100),
         TestCase(2, 1000)]
        public void BoundedListContractAddOverwrite2(int capacity, int iterations)
        {
            IBoundedList<String> list = this.GetInstance(capacity);
            if (list.OverflowPolicy != BoundedListOverflowPolicy.Overwrite) Assert.Ignore("Test is only applicable to implementations with an OverflowPolicy of Overwrite");

            List<String> items = new List<string>();
            for (int i = 0; i < iterations; i++)
            {
                // Find the oldest item and create a new item (but don't add it yet)
                // Check the oldest item is in the list and the new item isn't yet
                String oldestItem = list.Count > 0 ? list[0] : null;
                String newItem = i.ToString(CultureInfo.InvariantCulture);
                Assert.IsFalse(list.Contains(newItem));
                if (oldestItem != null) Assert.IsTrue(list.Contains(oldestItem));

                // Add the new item
                // Depending on whether we've reached the capacity yet this will ovewrite the oldest item
                bool expectOvewrite = list.Count == list.MaxCapacity;
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
                    for (int index = 0; index < items.Count - list.MaxCapacity; index++)
                    {
                        Assert.IsFalse(list.Contains(items[index]));
                    }
                }
                // Check current items are at expected indexes
                for (int index = Math.Max(0, items.Count - list.Count); index < items.Count; index++)
                {
                    Assert.IsTrue(list.Contains(items[index]));
                    int listIndex = items.Count > list.MaxCapacity ? index - (items.Count - list.MaxCapacity) : index;
                    Assert.AreEqual(items[index], list[listIndex]);
                }
            }
        }

        [Test]
        public void BoundedListContractAddDiscard1()
        {
            IBoundedList<string> list = this.GetInstance(2);
            if (list.OverflowPolicy != BoundedListOverflowPolicy.Discard) Assert.Ignore("Test is only applicable to implementations with an OverflowPolicy of Discard");

            list.Add("a");
            Assert.AreEqual(1, list.Count);
            Assert.IsTrue(list.Contains("a"));
            list.Add("b");
            Assert.AreEqual(2, list.Count);
            Assert.IsTrue(list.Contains("b"));

            Assert.AreEqual("a", list[0]);
            Assert.AreEqual("b", list[1]);

            // Third item should be discarded
            list.Add("c");
            Assert.IsFalse(list.Contains("c"));
            Assert.AreEqual(2, list.Count);
        }

        [TestCase(10, 100),
         TestCase(10, 1000),
         TestCase(1, 100),
         TestCase(100, 10),
         TestCase(100, 1000),
         TestCase(2, 100),
         TestCase(2, 1000)]
        public void BoundedListContractAddDiscard2(int capacity, int iterations)
        {
            IBoundedList<String> list = this.GetInstance(capacity);
            if (list.OverflowPolicy != BoundedListOverflowPolicy.Discard) Assert.Ignore("Test is only applicable to implementations with an OverflowPolicy of Discard");

            List<String> items = new List<string>();
            for (int i = 0; i < iterations; i++)
            {
                String newItem = i.ToString(CultureInfo.InvariantCulture);
                Assert.IsFalse(list.Contains(newItem));
                items.Add(newItem);
                list.Add(newItem);

                // Check expected items are in list
                for (int index = 0; index < Math.Min(items.Count, list.MaxCapacity); index++)
                {
                    Assert.IsTrue(list.Contains(items[index]));
                    Assert.AreEqual(items[index], list[index]);
                }
                // Check additional items are not in list
                if (items.Count <= list.MaxCapacity) continue;
                for (int index = list.MaxCapacity; index < items.Count; index++)
                {
                    Assert.IsFalse(list.Contains(items[index]));
                }
            }
        }

        [Test]
        public void BoundedListContractRemove1()
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

            // Now remove item a
            Assert.IsTrue(list.Remove("a"));
            Assert.IsFalse(list.Contains("a"));
            Assert.AreEqual("b", list[0]);
            Assert.AreEqual(1, list.Count);

            // Now remove item b
            Assert.IsTrue(list.Remove("b"));
            Assert.IsFalse(list.Contains("b"));
            Assert.AreEqual(0, list.Count);
        }

        [Test]
        public void BoundedListContractRemove2()
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

            // Now remove item b
            Assert.IsTrue(list.Remove("b"));
            Assert.IsFalse(list.Contains("b"));
            Assert.AreEqual("a", list[0]);
            Assert.AreEqual(1, list.Count);

            // Now remove item a
            Assert.IsTrue(list.Remove("a"));
            Assert.IsFalse(list.Contains("a"));
            Assert.AreEqual(0, list.Count);
        }

        [Test]
        public void BoundedListContractRemove3()
        {
            IBoundedList<string> list = this.GetInstance(2);

            // Can't remove non-existent items
            Assert.IsFalse(list.Remove("a"));
        }

        [Test]
        public void BoundedListContractRemove4()
        {
            IBoundedList<string> list = this.GetInstance(2);
            list.Add("a");
            Assert.AreEqual(1, list.Count);
            Assert.IsTrue(list.Contains("a"));

            // Can't remove non-existent items
            Assert.IsFalse(list.Remove("bhg"));
        }

        [Test]
        public void BoundedListContractInsert1()
        {
            IBoundedList<string> list = this.GetInstance(2);
            list.Add("a");
            Assert.AreEqual(1, list.Count);
            Assert.IsTrue(list.Contains("a"));
            Assert.AreEqual("a", list[0]);

            // Insert before
            list.Insert(0, "b");
            Assert.AreEqual(2, list.Count);
            Assert.IsTrue(list.Contains("b"));
            Assert.AreEqual("b", list[0]);
            Assert.AreEqual("a", list[1]);
        }

        [Test]
        public void BoundedListContractInsert2()
        {
            IBoundedList<string> list = this.GetInstance(2);
            list.Add("a");
            Assert.AreEqual(1, list.Count);
            Assert.IsTrue(list.Contains("a"));
            Assert.AreEqual("a", list[0]);

            // Insert after
            list.Insert(1, "b");
            Assert.AreEqual(2, list.Count);
            Assert.IsTrue(list.Contains("b"));
            Assert.AreEqual("a", list[0]);
            Assert.AreEqual("b", list[1]);
        }

        [Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BoundedListContractInsertOutOfRange1()
        {
            IBoundedList<string> list = this.GetInstance(2);

            // Insert out of range
            list.Insert(2, "b");
        }

        [Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BoundedListContractInsertOutOfRange2()
        {
            IBoundedList<string> list = this.GetInstance(2);

            // Insert out of range
            list.Insert(-1, "b");
        }

        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void BoundedListContractInsertError1()
        {
            IBoundedList<String> list = this.GetInstance(1);
            if (list.OverflowPolicy != BoundedListOverflowPolicy.Error) Assert.Ignore("Test is only applicable to implementations with an OverflowPolicy of Error");

            list.Add("a");
            Assert.AreEqual(1, list.Count);
            Assert.IsTrue(list.Contains("a"));
            Assert.AreEqual("a", list[0]);

            // Inserting an additional item should exceed capacity and result in an error
            list.Insert(0, "b");
        }

        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void BoundedListContractInsertError2()
        {
            IBoundedList<String> list = this.GetInstance(1);
            if (list.OverflowPolicy != BoundedListOverflowPolicy.Error) Assert.Ignore("Test is only applicable to implementations with an OverflowPolicy of Error");

            list.Add("a");
            Assert.AreEqual(1, list.Count);
            Assert.IsTrue(list.Contains("a"));
            Assert.AreEqual("a", list[0]);

            // Inserting an additional item should exceed capacity and result in an error
            list.Insert(1, "b");
        }

        [TestCase(10, 100),
         TestCase(10, 1000),
         TestCase(1, 100),
         TestCase(100, 10),
         TestCase(100, 1000),
         TestCase(2, 100),
         TestCase(2, 1000)]
        public void BoundedListContractInsertError3(int capacity, int iterations)
        {
            IBoundedList<String> list = this.GetInstance(capacity);
            if (list.OverflowPolicy != BoundedListOverflowPolicy.Error) Assert.Ignore("Test is only applicable to implementations with an OverflowPolicy of Error");

            List<String> items = new List<string>();
            for (int i = 0; i < iterations; i++)
            {
                String newItem = i.ToString(CultureInfo.InvariantCulture);
                Assert.IsFalse(list.Contains(newItem));
                items.Add(newItem);

                // Try to insert to list, should error once capacity is exceeded
                try
                {
                    list.Insert(0, newItem);
                    Assert.IsTrue(list.Contains(newItem));

                    // Should never exceed list capacity
                    Assert.IsFalse(list.Count > list.MaxCapacity);
                }
                catch (InvalidOperationException ex)
                {
                    // If this error occurs then we expect the list to be full
                    Assert.AreEqual(list.MaxCapacity, list.Count);
                }

                // Check expected items are in list
                for (int index = 0; index < Math.Min(items.Count, list.MaxCapacity); index++)
                {
                    Assert.IsTrue(list.Contains(items[index]));
                    Assert.AreEqual(items[Math.Min(items.Count, list.MaxCapacity) - 1 - index], list[index]);
                }
                // Check additional items are not in list
                if (items.Count <= list.MaxCapacity) continue;
                for (int index = list.MaxCapacity; index < items.Count; index++)
                {
                    Assert.IsFalse(list.Contains(items[index]));
                }
            }
        }

        [TestCase(10, 100),
         TestCase(10, 1000),
         TestCase(1, 100),
         TestCase(100, 10),
         TestCase(100, 1000),
         TestCase(2, 100),
         TestCase(2, 1000)]
        public void BoundedListContractInsertError4(int capacity, int iterations)
        {
            IBoundedList<String> list = this.GetInstance(capacity);
            if (list.OverflowPolicy != BoundedListOverflowPolicy.Error) Assert.Ignore("Test is only applicable to implementations with an OverflowPolicy of Error");

            List<String> items = new List<string>();
            for (int i = 0, insert = 0; i < iterations; i++, insert = (insert + 1) % (capacity + 1))
            {
                String newItem = i.ToString(CultureInfo.InvariantCulture);
                Assert.IsFalse(list.Contains(newItem));
                items.Add(newItem);

                // Try to insert to list, should error once capacity is exceeded
                try
                {
                    // Insert at cycled position
                    list.Insert(insert, newItem);
                    Assert.IsTrue(list.Contains(newItem));

                    // Should never exceed list capacity
                    Assert.IsFalse(list.Count > list.MaxCapacity);
                }
                catch (InvalidOperationException ex)
                {
                    // If this error occurs then we expect the list to be full
                    Assert.AreEqual(list.MaxCapacity, list.Count);
                }

                // Check expected items are in list
                for (int index = 0; index < Math.Min(items.Count, list.MaxCapacity); index++)
                {
                    Assert.IsTrue(list.Contains(items[index]));
                }
                // Check additional items are not in list
                if (items.Count <= list.MaxCapacity) continue;
                for (int index = list.MaxCapacity; index < items.Count; index++)
                {
                    Assert.IsFalse(list.Contains(items[index]));
                }
            }
        }
    }

    [TestFixture]
    public class CappedBoundedListTests
        : AbstractBoundedListContractTests
    {
        protected override IBoundedList<string> GetInstance(int capacity)
        {
            return new CappedBoundedList<string>(capacity);
        }

        protected override IBoundedList<string> GetInstance(int capacity, IEnumerable<string> contents)
        {
            return new CappedBoundedList<string>(capacity, contents);
        }
    }

    [TestFixture]
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

    [TestFixture]
    public class DiscardingBoundedListTests
        : AbstractBoundedListContractTests
    {
        protected override IBoundedList<string> GetInstance(int capacity)
        {
            return new DiscardingBoundedList<string>(capacity);
        }

        protected override IBoundedList<string> GetInstance(int capacity, IEnumerable<string> contents)
        {
            return new DiscardingBoundedList<string>(capacity, contents);
        }
    }

    public class OverwritingBoundedListTests
        : AbstractBoundedListContractTests
    {
        protected override IBoundedList<string> GetInstance(int capacity)
        {
            return new OverwritingBoundedList<string>(capacity);
        }

        protected override IBoundedList<string> GetInstance(int capacity, IEnumerable<string> contents)
        {
            return new OverwritingBoundedList<string>(capacity, contents);
        }
    }
}