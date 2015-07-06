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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;
using VDS.Common.Comparers;

namespace VDS.Common.Collections.Enumerations
{
    [TestFixture, Category("Collections")]
    public class EnumerableTests
    {
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static void Check<T>(IEnumerable<T> expected, IEnumerable<T> actual)
            where T : class
        {
            Console.WriteLine("Expected:");
            TestTools.PrintEnumerable(expected, ",");
            Console.WriteLine();
            Console.WriteLine("Actual:");
            TestTools.PrintEnumerable(actual, ",");
            Console.WriteLine();

            Assert.AreEqual(expected.Count(), actual.Count());

            IEnumerator<T> expectedEnumerator = expected.GetEnumerator();
            IEnumerator<T> actualEnumerator = actual.GetEnumerator();

            CompareEnumerators(expectedEnumerator, actualEnumerator);

            // Verify we can reset and re-use the enumerator
            expectedEnumerator = expected.GetEnumerator();
            actualEnumerator.Reset();
            CompareEnumerators(expectedEnumerator, actualEnumerator);
        }

        private static void CompareEnumerators<T>(IEnumerator<T> expectedEnumerator, IEnumerator<T> actualEnumerator) where T : class
        {
            int i = 0;
            while (expectedEnumerator.MoveNext())
            {
                T expectedItem = expectedEnumerator.Current;
                i++;
                if (!actualEnumerator.MoveNext()) Assert.Fail(String.Format("Actual enumerator was exhaused at Item {0} when next Item {1} was expected", i, expectedItem));
                T actualItem = actualEnumerator.Current;

                Assert.AreEqual(expectedItem, actualItem, String.Format("Enumerators mismatched at Item {0}", i));
            }
            if (actualEnumerator.MoveNext()) Assert.Fail("Actual enumerator has additional unexpected items");
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static void CheckStruct<T>(IEnumerable<T> expected, IEnumerable<T> actual) where T : struct
        {
            Console.WriteLine("Expected:");
            TestTools.PrintEnumerableStruct(expected, ",");
            Console.WriteLine();
            Console.WriteLine("Actual:");
            TestTools.PrintEnumerableStruct(actual, ",");
            Console.WriteLine();

            int eCount = expected.Count();
            int aCount = actual.Count();
            Assert.AreEqual(eCount, aCount, "Counts differ");

            IEnumerator<T> expectedEnumerator = expected.GetEnumerator();
            IEnumerator<T> actualEnumerator = actual.GetEnumerator();

            CompareEnumeratorsStruct(expectedEnumerator, actualEnumerator);

            // Verify we can reset and re-use the enumerator
            expectedEnumerator = expected.GetEnumerator();
            actualEnumerator.Reset();
            CompareEnumeratorsStruct(expectedEnumerator, actualEnumerator);
        }

        private static void CompareEnumeratorsStruct<T>(IEnumerator<T> expectedEnumerator, IEnumerator<T> actualEnumerator) where T : struct
        {
            int i = 0;
            while (expectedEnumerator.MoveNext())
            {
                T expectedItem = expectedEnumerator.Current;
                i++;
                if (!actualEnumerator.MoveNext()) Assert.Fail("Actual enumerator was exhaused at Item {0} when next Item {1} was expected", i, expectedItem);
                T actualItem = actualEnumerator.Current;

                Assert.AreEqual(expectedItem, actualItem, String.Format("Enumerators mismatched at Item {0}", i));
            }
            if (actualEnumerator.MoveNext()) Assert.Fail("Actual enumerator has additional unexpected items");
        }

        private static void Exhaust<T>(IEnumerator<T> enumerator)
        {
            while (enumerator.MoveNext()) {}
        }

        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void EnumeratorBeforeFirstElement1()
        {
            IEnumerable<int> data = Enumerable.Range(1, 10);
            IEnumerator<int> enumerator = new LongTakeEnumerator<int>(data.GetEnumerator(), 1);
            int i = enumerator.Current;
        }

        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void EnumeratorBeforeFirstElement2()
        {
            IEnumerable<String> data = new String[] { "a", "b", "c" };
            IEnumerator<String> enumerator = new LongTakeEnumerator<String>(data.GetEnumerator(), 1);
            String i = enumerator.Current;
        }

        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void EnumeratorBeforeFirstElement3()
        {
            IEnumerable<int> data = Enumerable.Range(1, 10);
            IEnumerator<int> enumerator = new LongSkipEnumerator<int>(data.GetEnumerator(), 1);
            int i = enumerator.Current;
        }

        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void EnumeratorBeforeFirstElement4()
        {
            IEnumerable<String> data = new String[] { "a", "b", "c" };
            IEnumerator<String> enumerator = new LongSkipEnumerator<String>(data.GetEnumerator(), 1);
            String i = enumerator.Current;
            Assert.AreEqual(default(String), i);
        }

        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void EnumeratorAfterLastElement1()
        {
            IEnumerable<int> data = Enumerable.Range(1, 10);
            IEnumerator<int> enumerator = new LongTakeEnumerator<int>(data.GetEnumerator(), 1);
            Exhaust(enumerator);
            int i = enumerator.Current;
        }

        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void EnumeratorAfterLastElement2()
        {
            IEnumerable<String> data = new String[] { "a", "b", "c" };
            IEnumerator<String> enumerator = new LongTakeEnumerator<String>(data.GetEnumerator(), 1);
            Exhaust(enumerator);
            String i = enumerator.Current;
            Assert.AreEqual(default(String), i);
        }

        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void EnumeratorAfterLastElement3()
        {
            IEnumerable<int> data = Enumerable.Range(1, 10);
            IEnumerator<int> enumerator = new LongSkipEnumerator<int>(data.GetEnumerator(), 1);
            Exhaust(enumerator);
            int i = enumerator.Current;
        }

        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void EnumeratorAfterLastElement4()
        {
            IEnumerable<String> data = new String[] { "a", "b", "c" };
            IEnumerator<String> enumerator = new LongSkipEnumerator<String>(data.GetEnumerator(), 1);
            Exhaust(enumerator);
            String i = enumerator.Current;
            Assert.AreEqual(default(String), i);
        }

        public static readonly Object[] SkipAndTakeData =
        {
            new object[] { 1, 1, 1 },
            new object[] { 1, 50, 10 },
            new object[] { 1, 10, 10 },
            new object[] { 1, 10, 5 },
            new object[] { 1, 10, 20 },
            new object[] { 1, 100, 50 },
        };

        [TestCaseSource("SkipAndTakeData")]
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public void LongSkipEnumerable(int start, int count, int skip)
        {
            List<int> data = Enumerable.Range(start, count).ToList();
            IEnumerable<int> expected = data.Skip(skip);
            IEnumerable<int> actual = new LongSkipEnumerable<int>(data, skip);

            if (skip > count)
            {
                Assert.IsFalse(expected.Any());
                Assert.IsFalse(actual.Any());
            }

            CheckStruct(expected, actual);
        }

        [TestCaseSource("SkipAndTakeData")]
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public void LongTakeEnumerable(int start, int count, int take)
        {
            List<int> data = Enumerable.Range(start, count).ToList();
            IEnumerable<int> expected = data.Take(take);
            IEnumerable<int> actual = new LongTakeEnumerable<int>(data, take);

            CheckStruct(expected, actual);
        }

        public static readonly Object[] AddOmitData =
        {
            new object[] { 1, 1, 1 },
            new object[] { 1, 10, 1 },
            new object[] { 1, 10, 11 },
            new object[] { 1, 10, 100 },
            new object[] { 1, 100, 50 },
            new object[] { 1, 100, 1000 }
        };

        [TestCaseSource("AddOmitData")]
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public void AddDistinctEnumerable(int start, int count, int item)
        {
            List<int> data = Enumerable.Range(start, count).ToList();
            IEnumerable<int> expected = data.Concat(item.AsEnumerable()).Distinct();
            IEnumerable<int> actual = new AddIfMissingEnumerable<int>(data, EqualityComparer<int>.Default, item);

            CheckStruct(expected, actual);
        }

        [Test]
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public void AddIfEmptyEnumerable()
        {
            IEnumerable<int> data = Enumerable.Empty<int>();
            IEnumerable<int> actual = data.AddIfEmpty(1);

            Assert.IsFalse(data.Any());
            Assert.IsTrue(actual.Any());
            Assert.AreEqual(1, actual.First());
        }

        public static readonly Object[] TopNData =
        {
            new object[] { new int[] { 1, 1, 7, 9, 9, 9, 3, 3, 1, 7, 5 }, 1 },
            new object[] { new int[] { 1, 1, 7, 9, 9, 9, 3, 3, 1, 7, 5 }, 3 },
            new object[] { new int[] { 1, 1, 7, 9, 9, 9, 3, 3, 1, 7, 5 }, 5 },
            new object[] { new int[] { 1, 1, 7, 9, 9, 9, 3, 3, 1, 7, 5 }, 100 }
        };

        public static readonly Object[] TopNStringData =
        {
            new object[] { new String[] { "a", "b", "b", "a", "z", "m", "q", "m", "b", "c", "f" }, 1 }
        };

        [TestCaseSource("TopNData")]
        public void TopNDistinctEnumerable1(int[] data, int n)
        {
            IEnumerable<int> expected = data.OrderBy(i => i).Distinct().Take(n);
            IEnumerable<int> actual = data.TopDistinct(n);

            TestTools.PrintOrderingComparisonEnumerableStruct(data);

            CheckStruct(expected, actual);
        }

        [TestCaseSource("TopNData")]
        public void TopNDistinctEnumerable2(int[] data, int n)
        {
            IComparer<int> comparer = new ReversedComparer<int>();
            IEnumerable<int> expected = data.OrderBy(i => i, comparer).Distinct().Take(n);
            IEnumerable<int> actual = data.TopDistinct(n, comparer);

            CheckStruct(expected, actual);
        }

        [TestCaseSource("TopNStringData")]
        public void TopNDistinctEnumerable3(String[] data, int n)
        {
            IEnumerable<String> expected = data.OrderBy(i => i).Distinct().Take(n);
            IEnumerable<String> actual = data.TopDistinct(n);

            TestTools.PrintOrderingComparisonEnumerable(data);

            Check(expected, actual);
        }

        [TestCaseSource("TopNStringData")]
        public void TopNDistinctEnumerable4(String[] data, int n)
        {
            IComparer<String> comparer = new ReversedComparer<String>();
            IEnumerable<String> expected = data.OrderBy(i => i, comparer).Distinct().Take(n);
            IEnumerable<String> actual = data.TopDistinct(n, comparer);

            TestTools.PrintOrderingComparisonEnumerable(data);

            Check(expected, actual);
        }

        [TestCaseSource("TopNData")]
        public void TopNEnumerable1(int[] data, int n)
        {
            IEnumerable<int> expected = data.OrderBy(i => i).Take(n);
            IEnumerable<int> actual = data.Top(n);

            TestTools.PrintOrderingComparisonEnumerableStruct(data);

            CheckStruct(expected, actual);
        }

        [TestCaseSource("TopNData")]
        public void TopNEnumerable2(int[] data, int n)
        {
            IComparer<int> comparer = new ReversedComparer<int>();
            IEnumerable<int> expected = data.OrderBy(i => i, comparer).Take(n);
            IEnumerable<int> actual = data.Top(n, comparer);

            CheckStruct(expected, actual);
        }

        [TestCaseSource("TopNStringData")]
        public void TopNEnumerable3(String[] data, int n)
        {
            IEnumerable<String> expected = data.OrderBy(i => i).Take(n);
            IEnumerable<String> actual = data.Top(n);

            TestTools.PrintOrderingComparisonEnumerable(data);

            Check(expected, actual);
        }

        [TestCaseSource("TopNStringData")]
        public void TopNEnumerable4(String[] data, int n)
        {
            IComparer<String> comparer = new ReversedComparer<String>();
            IEnumerable<String> expected = data.OrderBy(i => i, comparer).Take(n);
            IEnumerable<String> actual = data.Top(n, comparer);

            TestTools.PrintOrderingComparisonEnumerable(data);

            Check(expected, actual);
        }

        [TestCaseSource("TopNData")]
        public void BottomNDistinctEnumerable1(int[] data, int n)
        {
            IEnumerable<int> expected = data.OrderByDescending(i => i).Distinct().Take(n);
            IEnumerable<int> actual = data.BottomDistinct(n);

            TestTools.PrintOrderingComparisonEnumerableStruct(data);

            CheckStruct(expected, actual);
        }

        [TestCaseSource("TopNData")]
        public void BottomNDistinctEnumerable2(int[] data, int n)
        {
            IComparer<int> comparer = new ReversedComparer<int>();
            IEnumerable<int> expected = data.OrderByDescending(i => i, comparer).Distinct().Take(n);
            IEnumerable<int> actual = data.BottomDistinct(n, comparer);

            CheckStruct(expected, actual);
        }

        [TestCaseSource("TopNStringData")]
        public void BottomNDistinctEnumerable3(String[] data, int n)
        {
            IEnumerable<String> expected = data.OrderByDescending(i => i).Distinct().Take(n);
            IEnumerable<String> actual = data.BottomDistinct(n);

            TestTools.PrintOrderingComparisonEnumerable(data);

            Check(expected, actual);
        }

        [TestCaseSource("TopNStringData")]
        public void BottomNDistinctEnumerable4(String[] data, int n)
        {
            IComparer<String> comparer = new ReversedComparer<String>();
            IEnumerable<String> expected = data.OrderByDescending(i => i, comparer).Distinct().Take(n);
            IEnumerable<String> actual = data.BottomDistinct(n, comparer);

            TestTools.PrintOrderingComparisonEnumerable(data);

            Check(expected, actual);
        }

        [TestCaseSource("TopNData")]
        public void BottomNEnumerable1(int[] data, int n)
        {
            IEnumerable<int> expected = data.OrderByDescending(i => i).Take(n);
            IEnumerable<int> actual = data.Bottom(n);

            TestTools.PrintOrderingComparisonEnumerableStruct(data);

            CheckStruct(expected, actual);
        }

        [TestCaseSource("TopNData")]
        public void BottomNEnumerable2(int[] data, int n)
        {
            IComparer<int> comparer = new ReversedComparer<int>();
            IEnumerable<int> expected = data.OrderByDescending(i => i, comparer).Take(n);
            IEnumerable<int> actual = data.Bottom(n, comparer);

            CheckStruct(expected, actual);
        }

        [TestCaseSource("TopNStringData")]
        public void BottomNEnumerable3(String[] data, int n)
        {
            IEnumerable<String> expected = data.OrderByDescending(i => i).Take(n);
            IEnumerable<String> actual = data.Bottom(n);

            TestTools.PrintOrderingComparisonEnumerable(data);

            Check(expected, actual);
        }

        [TestCaseSource("TopNStringData")]
        public void BottomNEnumerable4(String[] data, int n)
        {
            IComparer<String> comparer = new ReversedComparer<String>();
            IEnumerable<String> expected = data.OrderByDescending(i => i, comparer).Take(n);
            IEnumerable<String> actual = data.Bottom(n, comparer);

            TestTools.PrintOrderingComparisonEnumerable(data);

            Check(expected, actual);
        }
    }
}