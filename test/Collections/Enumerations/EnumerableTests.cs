/*
VDS.Common is licensed under the MIT License

Copyright (c) 2012-2015 Robert Vesse
Copyright (c) 2016-2025 dotNetRDF Project (https://dotnetrdf.org/)

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

namespace VDS.Common.Collections.Enumerations;

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

        var expectedEnumerator = expected.GetEnumerator();
        var actualEnumerator = actual.GetEnumerator();

        CompareEnumerators(expectedEnumerator, actualEnumerator);

        // Verify we can reset and re-use the enumerator
        expectedEnumerator = expected.GetEnumerator();
        actualEnumerator.Reset();
        CompareEnumerators(expectedEnumerator, actualEnumerator);
    }

    private static void CompareEnumerators<T>(IEnumerator<T> expectedEnumerator, IEnumerator<T> actualEnumerator) where T : class
    {
        var i = 0;
        while (expectedEnumerator.MoveNext())
        {
            var expectedItem = expectedEnumerator.Current;
            i++;
            if (!actualEnumerator.MoveNext()) Assert.Fail(
                $"Actual enumerator was exhausted at Item {i} when next Item {expectedItem} was expected");
            var actualItem = actualEnumerator.Current;

            Assert.AreEqual(expectedItem, actualItem, $"Enumerators mismatched at Item {i}");
        }
        if (actualEnumerator.MoveNext()) Assert.Fail("Actual enumerator has additional unexpected items");
    }

    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    private static void CheckStruct<T>(IEnumerable<T> expected, IEnumerable<T> actual) where T : struct
    {
        Console.WriteLine("Expected:");
        TestTools.PrintEnumerableStruct(expected, ",");
        Console.WriteLine();
        Console.WriteLine("Actual:");
        TestTools.PrintEnumerableStruct(actual, ",");
        Console.WriteLine();

        var eCount = expected.Count();
        var aCount = actual.Count();
        Assert.AreEqual(eCount, aCount, "Counts differ");

        var expectedEnumerator = expected.GetEnumerator();
        var actualEnumerator = actual.GetEnumerator();

        CompareEnumeratorsStruct(expectedEnumerator, actualEnumerator);

        // Verify we can reset and re-use the enumerator
        expectedEnumerator = expected.GetEnumerator();
        actualEnumerator.Reset();
        CompareEnumeratorsStruct(expectedEnumerator, actualEnumerator);
    }

    private static void CompareEnumeratorsStruct<T>(IEnumerator<T> expectedEnumerator, IEnumerator<T> actualEnumerator) where T : struct
    {
        var i = 0;
        while (expectedEnumerator.MoveNext())
        {
            var expectedItem = expectedEnumerator.Current;
            i++;
            if (!actualEnumerator.MoveNext()) Assert.Fail($"Actual enumerator was exhausted at Item {i} when next Item {expectedItem} was expected");
            var actualItem = actualEnumerator.Current;

            Assert.AreEqual(expectedItem, actualItem, $"Enumerators mismatched at Item {i}");
        }
        if (actualEnumerator.MoveNext()) Assert.Fail("Actual enumerator has additional unexpected items");
    }

    private static void Exhaust<T>(IEnumerator<T> enumerator)
    {
        while (enumerator.MoveNext()) {}
    }

    [Test]
    public void EnumeratorBeforeFirstElement1()
    {
        var data = Enumerable.Range(1, 10);
        IEnumerator<int> enumerator = new LongTakeEnumerator<int>(data.GetEnumerator(), 1);
        Assert.Throws<InvalidOperationException>(() =>
        {
            _ = enumerator.Current;
        });
    }

    [Test]
    public void EnumeratorBeforeFirstElement2()
    {
        IEnumerable<string> data = new[] { "a", "b", "c" };
        IEnumerator<string> enumerator = new LongTakeEnumerator<string>(data.GetEnumerator(), 1);
        Assert.Throws<InvalidOperationException>(() =>
        {
            _ = enumerator.Current;
        });
    }

    [Test]
    public void EnumeratorBeforeFirstElement3()
    {
        var data = Enumerable.Range(1, 10);
        IEnumerator<int> enumerator = new LongSkipEnumerator<int>(data.GetEnumerator(), 1);
        Assert.Throws<InvalidOperationException>(() =>
        {
            _ = enumerator.Current;
        });
    }

    [Test]
    public void EnumeratorBeforeFirstElement4()
    {
        IEnumerable<string> data = new[] { "a", "b", "c" };
        IEnumerator<string> enumerator = new LongSkipEnumerator<string>(data.GetEnumerator(), 1);
        string i = null;
        Assert.Throws<InvalidOperationException>(() => { i = enumerator.Current; });
        Assert.AreEqual(default(string), i);
    }

    [Test]
    public void EnumeratorAfterLastElement1()
    {
        var data = Enumerable.Range(1, 10);
        IEnumerator<int> enumerator = new LongTakeEnumerator<int>(data.GetEnumerator(), 1);
        Exhaust(enumerator);
        Assert.Throws<InvalidOperationException>(() =>
        {
            var _ = enumerator.Current;
        });
    }

    [Test]
    public void EnumeratorAfterLastElement2()
    {
        IEnumerable<string> data = new[] { "a", "b", "c" };
        IEnumerator<string> enumerator = new LongTakeEnumerator<string>(data.GetEnumerator(), 1);
        Exhaust(enumerator);
        string i = null;
        Assert.Throws<InvalidOperationException>(() => { i = enumerator.Current; });
        Assert.AreEqual(default(string), i);
    }

    [Test]
    public void EnumeratorAfterLastElement3()
    {
        var data = Enumerable.Range(1, 10);
        IEnumerator<int> enumerator = new LongSkipEnumerator<int>(data.GetEnumerator(), 1);
        Exhaust(enumerator);
        Assert.Throws<InvalidOperationException>(() =>
        {
            var _ = enumerator.Current;
        });
    }

    [Test]
    public void EnumeratorAfterLastElement4()
    {
        IEnumerable<string> data = new[] { "a", "b", "c" };
        IEnumerator<string> enumerator = new LongSkipEnumerator<string>(data.GetEnumerator(), 1);
        Exhaust(enumerator);
        string i = null;
        Assert.Throws<InvalidOperationException>(() => { i = enumerator.Current; });
        Assert.AreEqual(default(string), i);
    }

    public static readonly object[] SkipAndTakeData =
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
        var data = Enumerable.Range(start, count).ToList();
        var expected = data.Skip(skip);
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
        var data = Enumerable.Range(start, count).ToList();
        var expected = data.Take(take);
        IEnumerable<int> actual = new LongTakeEnumerable<int>(data, take);

        CheckStruct(expected, actual);
    }

    public static readonly object[] AddOmitData =
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
        var data = Enumerable.Range(start, count).ToList();
        var expected = data.Concat(item.AsEnumerable()).Distinct();
        IEnumerable<int> actual = new AddIfMissingEnumerable<int>(data, EqualityComparer<int>.Default, item);

        CheckStruct(expected, actual);
    }

    [Test]
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public void AddIfEmptyEnumerable()
    {
        var data = Enumerable.Empty<int>();
        var actual = data.AddIfEmpty(1);

        Assert.IsFalse(data.Any());
        Assert.IsTrue(actual.Any());
        Assert.AreEqual(1, actual.First());
    }

    public static readonly object[] TopNData =
    {
        new object[] { new[] { 1, 1, 7, 9, 9, 9, 3, 3, 1, 7, 5 }, 1 },
        new object[] { new[] { 1, 1, 7, 9, 9, 9, 3, 3, 1, 7, 5 }, 3 },
        new object[] { new[] { 1, 1, 7, 9, 9, 9, 3, 3, 1, 7, 5 }, 5 },
        new object[] { new[] { 1, 1, 7, 9, 9, 9, 3, 3, 1, 7, 5 }, 100 }
    };

    public static readonly object[] TopNStringData =
    {
        new object[] { new[] { "a", "b", "b", "a", "z", "m", "q", "m", "b", "c", "f" }, 1 }
    };

    [TestCaseSource("TopNData")]
    public void TopNDistinctEnumerable1(int[] data, int n)
    {
        var expected = data.OrderBy(i => i).Distinct().Take(n);
        var actual = data.TopDistinct(n);

        TestTools.PrintOrderingComparisonEnumerableStruct(data);

        CheckStruct(expected, actual);
    }

    [TestCaseSource("TopNData")]
    public void TopNDistinctEnumerable2(int[] data, int n)
    {
        IComparer<int> comparer = new ReversedComparer<int>();
        var expected = data.OrderBy(i => i, comparer).Distinct().Take(n);
        var actual = data.TopDistinct(n, comparer);

        CheckStruct(expected, actual);
    }

    [TestCaseSource("TopNStringData")]
    public void TopNDistinctEnumerable3(string[] data, int n)
    {
        var expected = data.OrderBy(i => i).Distinct().Take(n);
        var actual = data.TopDistinct(n);

        TestTools.PrintOrderingComparisonEnumerable(data);

        Check(expected, actual);
    }

    [TestCaseSource("TopNStringData")]
    public void TopNDistinctEnumerable4(string[] data, int n)
    {
        IComparer<string> comparer = new ReversedComparer<string>();
        var expected = data.OrderBy(i => i, comparer).Distinct().Take(n);
        var actual = data.TopDistinct(n, comparer);

        TestTools.PrintOrderingComparisonEnumerable(data);

        Check(expected, actual);
    }

    [TestCaseSource("TopNData")]
    public void TopNEnumerable1(int[] data, int n)
    {
        var expected = data.OrderBy(i => i).Take(n);
        var actual = data.Top(n);

        TestTools.PrintOrderingComparisonEnumerableStruct(data);

        CheckStruct(expected, actual);
    }

    [TestCaseSource("TopNData")]
    public void TopNEnumerable2(int[] data, int n)
    {
        IComparer<int> comparer = new ReversedComparer<int>();
        var expected = data.OrderBy(i => i, comparer).Take(n);
        var actual = data.Top(n, comparer);

        CheckStruct(expected, actual);
    }

    [TestCaseSource("TopNStringData")]
    public void TopNEnumerable3(string[] data, int n)
    {
        var expected = data.OrderBy(i => i).Take(n);
        var actual = data.Top(n);

        TestTools.PrintOrderingComparisonEnumerable(data);

        Check(expected, actual);
    }

    [TestCaseSource("TopNStringData")]
    public void TopNEnumerable4(string[] data, int n)
    {
        IComparer<string> comparer = new ReversedComparer<string>();
        var expected = data.OrderBy(i => i, comparer).Take(n);
        var actual = data.Top(n, comparer);

        TestTools.PrintOrderingComparisonEnumerable(data);

        Check(expected, actual);
    }

    [TestCaseSource("TopNData")]
    public void BottomNDistinctEnumerable1(int[] data, int n)
    {
        var expected = data.OrderByDescending(i => i).Distinct().Take(n);
        var actual = data.BottomDistinct(n);

        TestTools.PrintOrderingComparisonEnumerableStruct(data);

        CheckStruct(expected, actual);
    }

    [TestCaseSource("TopNData")]
    public void BottomNDistinctEnumerable2(int[] data, int n)
    {
        IComparer<int> comparer = new ReversedComparer<int>();
        var expected = data.OrderByDescending(i => i, comparer).Distinct().Take(n);
        var actual = data.BottomDistinct(n, comparer);

        CheckStruct(expected, actual);
    }

    [TestCaseSource("TopNStringData")]
    public void BottomNDistinctEnumerable3(string[] data, int n)
    {
        var expected = data.OrderByDescending(i => i).Distinct().Take(n);
        var actual = data.BottomDistinct(n);

        TestTools.PrintOrderingComparisonEnumerable(data);

        Check(expected, actual);
    }

    [TestCaseSource("TopNStringData")]
    public void BottomNDistinctEnumerable4(string[] data, int n)
    {
        IComparer<string> comparer = new ReversedComparer<string>();
        var expected = data.OrderByDescending(i => i, comparer).Distinct().Take(n);
        var actual = data.BottomDistinct(n, comparer);

        TestTools.PrintOrderingComparisonEnumerable(data);

        Check(expected, actual);
    }

    [TestCaseSource("TopNData")]
    public void BottomNEnumerable1(int[] data, int n)
    {
        var expected = data.OrderByDescending(i => i).Take(n);
        var actual = data.Bottom(n);

        TestTools.PrintOrderingComparisonEnumerableStruct(data);

        CheckStruct(expected, actual);
    }

    [TestCaseSource("TopNData")]
    public void BottomNEnumerable2(int[] data, int n)
    {
        IComparer<int> comparer = new ReversedComparer<int>();
        var expected = data.OrderByDescending(i => i, comparer).Take(n);
        var actual = data.Bottom(n, comparer);

        CheckStruct(expected, actual);
    }

    [TestCaseSource("TopNStringData")]
    public void BottomNEnumerable3(string[] data, int n)
    {
        var expected = data.OrderByDescending(i => i).Take(n);
        var actual = data.Bottom(n);

        TestTools.PrintOrderingComparisonEnumerable(data);

        Check(expected, actual);
    }

    [TestCaseSource("TopNStringData")]
    public void BottomNEnumerable4(string[] data, int n)
    {
        IComparer<string> comparer = new ReversedComparer<string>();
        var expected = data.OrderByDescending(i => i, comparer).Take(n);
        var actual = data.Bottom(n, comparer);

        TestTools.PrintOrderingComparisonEnumerable(data);

        Check(expected, actual);
    }
}