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
using System.Diagnostics;
using NUnit.Framework;
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local

namespace VDS.Common.Collections;

[TestFixture, Category("Dictionaries")]
public class MultiDictionaryTests
{
    [Test]
    public void MultiDictionaryInstantiation1()
    {
        _ = new MultiDictionary<string, int>();
    }

    [Test]
    public void MultiDictionaryNullKeyHandling1()
    {
        var dict = new MultiDictionary<object, int>();
        Assert.Throws<ArgumentNullException>(() => dict.Add(null, 1));
    }

    [Test]
    public void MultiDictionaryNullKeyHandling2()
    {
        var dict = new MultiDictionary<object, int>();
        Assert.Throws<ArgumentNullException>(() => { _ = dict[null]; });
    }

    [Test]
    public void MultiDictionaryNullKeyHandling3()
    {
        var dict = new MultiDictionary<object, int>();
        Assert.Throws<ArgumentNullException>(() => _ = dict.TryGetValue(null, out _));
    }

    [Test]
    public void MultiDictionaryNullKeyHandling4()
    {
        var dict = new MultiDictionary<object, int>();
        Assert.Throws<ArgumentNullException>(() => { dict[null] = 1; });
    }

    [Test]
    public void MultiDictionaryNullKeyHandling5()
    {
        var dict = new MultiDictionary<object, int>();
        Assert.Throws<ArgumentException>(() => { dict.Add(new KeyValuePair<object, int>(null, 1)); });
    }

    [Test]
    public void MultiDictionaryNullKeyHandling6()
    {
        var dict = new MultiDictionary<object, int>();
        Assert.Throws<ArgumentNullException>(() => dict.Remove(null));
    }

    [Test]
    public void MultiDictionaryNullKeyHandling7()
    {
        var dict = new MultiDictionary<object, int>();
        Assert.Throws<ArgumentException>(() => dict.Remove(new KeyValuePair<object, int>(null, 1)));
    }

    [Test]
    public void MultiDictionaryNullKeyHandling8()
    {
        var dict = new MultiDictionary<object, int>();
        Assert.Throws<ArgumentNullException>(() => { _ = dict.ContainsKey(null); });
    }

    [Test]
    public void MultiDictionaryNullKeyHandling10()
    {
        var dict = new MultiDictionary<object, int>(x => x?.GetHashCode() ?? 0, true);
        _ = dict.Contains(new KeyValuePair<object, int>(null, 1));
    }

    [Test]
    public void MultiDictionaryNullKeyHandling11()
    {
        _ = new MultiDictionary<object, int>(x => x?.GetHashCode() ?? 0, true) { { null, 1 } };
    }

    [Test]
    public void MultiDictionaryNullKeyHandling12()
    {
        var dict = new MultiDictionary<object, int>(x => x?.GetHashCode() ?? 0, true);
        Assert.Throws<KeyNotFoundException>(() =>
        {
            _ = dict[null];
        });
    }

    [Test]
    public void MultiDictionaryNullKeyHandling13()
    {
        var dict = new MultiDictionary<object, int>(x => x?.GetHashCode() ?? 0, true);
        _ = dict.TryGetValue(null, out _);
    }

    [Test]
    public void MultiDictionaryNullKeyHandling14()
    {
        _ = new MultiDictionary<object, int>(x => x?.GetHashCode() ?? 0, true)
        {
            [null] = 1
        };
    }

    [Test]
    public void MultiDictionaryNullKeyHandling15()
    {
        _ = new MultiDictionary<object, int>(x => x?.GetHashCode() ?? 0, true) { new(null, 1) };
    }

    [Test]
    public void MultiDictionaryNullKeyHandling16()
    {
        var dict = new MultiDictionary<object, int>(x => x?.GetHashCode() ?? 0, true);
        dict.Remove(null);
    }

    [Test]
    public void MultiDictionaryNullKeyHandling17()
    {
        var dict = new MultiDictionary<object, int>(x => x?.GetHashCode() ?? 0, true);
        dict.Remove(new KeyValuePair<object, int>(null, 1));
    }

    [Test]
    public void MultiDictionaryNullKeyHandling18()
    {
        var dict = new MultiDictionary<object, int>(x => x?.GetHashCode() ?? 0, true);
        _ = dict.ContainsKey(null);
    }

    [Test]
    public void MultiDictionaryNullKeyHandling19()
    {
        var dict = new MultiDictionary<object, int>(x => x?.GetHashCode() ?? 0, true);
        _ = dict.Contains(new KeyValuePair<object, int>(null, 1));
    }

    [Test]
    public void MultiDictionaryVsDictionaryInsertBasic1()
    {
        var dict = new Dictionary<TestKey<string>, int>();
        var mDict = new MultiDictionary<TestKey<string>, int>(new TestKeyComparer<string>());

        var a = new TestKey<string>(1, "a");
        var b = new TestKey<string>(1, "b");
        var c = new TestKey<string>(1, "a");

        dict.Add(a, 1);
        try
        {
            dict.Add(b, 2);
            Assert.Fail("ArgumentException not thrown");
        }
        catch (ArgumentException)
        {
            //Ignore and continue
        }
        mDict.Add(a, 1);
        mDict.Add(b, 2);

        Assert.AreEqual(1, dict.Count);
        Assert.AreEqual(1, dict[a]);
        Assert.AreEqual(1, dict[b]);
        Assert.AreEqual(2, mDict.Count);
        Assert.AreEqual(1, mDict[a]);
        Assert.AreEqual(2, mDict[b]);
        Assert.IsFalse(ReferenceEquals(c, a));
        Assert.IsTrue(mDict.TryGetKey(c, out var d));
        Assert.IsTrue(ReferenceEquals(a, d));
    }

    [Test]
    public void MultiDictionaryVsDictionaryInsertBasic2()
    {
        var dict = new Dictionary<TestKey<string>, int>(new TestKeyComparer<string>());
        var mDict = new MultiDictionary<TestKey<string>, int>(new TestKeyComparer<string>());

        var a = new TestKey<string>(1, "a");
        var b = new TestKey<string>(1, "b");

        dict.Add(a, 1);
        dict.Add(b, 2);
        mDict.Add(a, 1);
        mDict.Add(b, 2);

        Assert.AreEqual(2, dict.Count);
        Assert.AreEqual(1, dict[a]);
        Assert.AreEqual(2, dict[b]);
        Assert.AreEqual(2, mDict.Count);
        Assert.AreEqual(1, mDict[a]);
        Assert.AreEqual(2, mDict[b]);
    }

    [Test]
    [Category("Timing")]
    public void MultiDictionaryVsDictionaryLookupPathological1()
    {
        var dict = new Dictionary<TestKey<int>, int>(new TestKeyComparer<int>());
        var mDict = new MultiDictionary<TestKey<int>, int>(new TestKeyComparer<int>());

        //Build dictionaries with 10000 keys in them
        var keys = new List<TestKey<int>>();
        for (var i = 0; i < 10000; i++)
        {
            var key = new TestKey<int>(0, i);
            keys.Add(key);
            dict.Add(key, i);
            mDict.Add(key, i);
        }

        var timer = new Stopwatch();

        //Lookup all keys in dictionary
        timer.Start();
        foreach (var key in keys)
        {
            _ = dict.ContainsKey(key);
        }
        timer.Stop();

        var dictTime = timer.Elapsed;
        Console.WriteLine("Dictionary took " + timer.Elapsed);
        timer.Reset();

        //Lookup all keys in multi-dictionary
        timer.Start();
        foreach (var key in keys)
        {
            _ = mDict.ContainsKey(key);
        }
        timer.Stop();

        var mDictTime = timer.Elapsed;
        Console.WriteLine("MultiDictionary took " + timer.Elapsed);

        Assert.IsTrue(mDictTime < dictTime);
    }

    [Test]
    [Category("Timing")]
    public void MultiDictionaryVsDictionaryLookupPathological2()
    {
        var dict = new Dictionary<TestKey<int>, int>(new TestKeyComparer<int>());
        var mDict = new MultiDictionary<TestKey<int>, int>(new TestKeyComparer<int>());

        //Build dictionaries with 10000 keys in them
        var keys = new List<TestKey<int>>();
        for (var i = 0; i < 10000; i++)
        {
            var key = new TestKey<int>(0, i);
            keys.Add(key);
            dict.Add(key, i);
            mDict.Add(key, i);
        }

        var timer = new Stopwatch();

        //Lookup all keys in multi-dictionary
        timer.Start();
        foreach (var key in keys)
        {
            _ = mDict.ContainsKey(key);
        }
        timer.Stop();

        var mDictTime = timer.Elapsed;
        Console.WriteLine("MultiDictionary took " + timer.Elapsed);

        timer.Reset();

        //Lookup all keys in dictionary
        timer.Start();
        foreach (var key in keys)
        {
            _ = dict.ContainsKey(key);
        }
        timer.Stop();

        var dictTime = timer.Elapsed;
        Console.WriteLine("Dictionary took " + timer.Elapsed);

        Assert.IsTrue(mDictTime < dictTime);
    }

    [Test]
    [Category("Timing")]
    public void MultiDictionaryVsDictionaryInsertPathological1()
    {
        var dict = new Dictionary<TestKey<int>, int>(new TestKeyComparer<int>());
        var mDict = new MultiDictionary<TestKey<int>, int>(new TestKeyComparer<int>());

        //Generate 10000 keys
        var keys = new List<TestKey<int>>();
        for (var i = 0; i < 10000; i++)
        {
            var key = new TestKey<int>(0, i);
            keys.Add(key);
        }

        var timer = new Stopwatch();

        //Add to dictionary
        timer.Start();
        foreach (var key in keys)
        {
            dict.Add(key, key.Value);
        }
        timer.Stop();

        var dictTime = timer.Elapsed;
        Console.WriteLine("Dictionary took " + timer.Elapsed);

        timer.Reset();

        //Add to multi-dictionary
        timer.Start();
        foreach (var key in keys)
        {
            mDict.Add(key, key.Value);
        }
        timer.Stop();

        var mDictTime = timer.Elapsed;
        Console.WriteLine("MultiDictionary took " + timer.Elapsed);

        Assert.IsTrue(mDictTime < dictTime);
    }

    [TestCase(50000), TestCase(100000), TestCase(250000)]
    [Category("Timing")]
    public void MultiDictionaryVsDictionaryInsertNormal1(int numKeys)
    {
        var dict = new Dictionary<TestKey<int>, int>(new TestKeyComparer<int>());
        var mDict = new MultiDictionary<TestKey<int>, int>(new TestKeyComparer<int>());

        //Generate some number of keys
        var keys = new List<TestKey<int>>();
        for (var i = 0; i < numKeys; i++)
        {
            var key = new TestKey<int>(i, i);
            keys.Add(key);
        }

        var timer = new Stopwatch();

        //Add to dictionary
        timer.Start();
        foreach (var key in keys)
        {
            dict.Add(key, key.Value);
        }
        timer.Stop();

        var dictTime = timer.Elapsed;
        Console.WriteLine("Dictionary took " + timer.Elapsed);

        timer.Reset();

        //Add to multi-dictionary
        timer.Start();
        foreach (var key in keys)
        {
            mDict.Add(key, key.Value);
        }
        timer.Stop();

        var mDictTime = timer.Elapsed;
        Console.WriteLine("MultiDictionary took " + timer.Elapsed);

        Assert.IsTrue(mDictTime > dictTime);
    }

    [Test]
    [Category("Timing")]
    public void MultiDictionaryVsDictionaryLookupPool1()
    {
        var dict = new Dictionary<TestKey<int>, int>(new TestKeyComparer<int>());
        var mDict = new MultiDictionary<TestKey<int>, int>(new TestKeyComparer<int>());

        //Build dictionaries with 10000 keys in them
        var keys = new List<TestKey<int>>();
        for (var i = 0; i < 10000; i++)
        {
            var key = new TestKey<int>(i%100, i);
            keys.Add(key);
            dict.Add(key, i);
            mDict.Add(key, i);
        }

        var timer = new Stopwatch();

        //Lookup all keys in dictionary
        timer.Start();
        foreach (var key in keys)
        {
            _ = dict.ContainsKey(key);
        }
        timer.Stop();

        var dictTime = timer.Elapsed;
        Console.WriteLine("Dictionary took " + timer.Elapsed);
        timer.Reset();

        //Lookup all keys in multi-dictionary
        timer.Start();
        foreach (var key in keys)
        {
            _ = mDict.ContainsKey(key);
        }
        timer.Stop();

        var mDictTime = timer.Elapsed;
        Console.WriteLine("MultiDictionary took " + timer.Elapsed);

        Assert.IsTrue(mDictTime < dictTime);
    }

    [Test]
    [Category("Timing")]
    public void MultiDictionaryVsDictionaryLookupPool2()
    {
        var dict = new Dictionary<TestKey<int>, int>(new TestKeyComparer<int>());
        var mDict = new MultiDictionary<TestKey<int>, int>(new TestKeyComparer<int>());

        //Build dictionaries with 10000 keys in them
        var keys = new List<TestKey<int>>();
        for (var i = 0; i < 10000; i++)
        {
            var key = new TestKey<int>(i%100, i);
            keys.Add(key);
            dict.Add(key, i);
            mDict.Add(key, i);
        }

        var timer = new Stopwatch();

        //Lookup all keys in multi-dictionary
        timer.Start();
        foreach (var key in keys)
        {
            _ = mDict.ContainsKey(key);
        }
        timer.Stop();

        var mDictTime = timer.Elapsed;
        Console.WriteLine("MultiDictionary took " + timer.Elapsed);

        timer.Reset();

        //Lookup all keys in dictionary
        timer.Start();
        foreach (var key in keys)
        {
            _ = dict.ContainsKey(key);
        }
        timer.Stop();

        var dictTime = timer.Elapsed;
        Console.WriteLine("Dictionary took " + timer.Elapsed);

        Assert.IsTrue(mDictTime < dictTime);
    }

    [TestCase(1000), TestCase(10000), TestCase(100000), TestCase(250000)]
    [Category("Timing")]
    public void MultiDictionaryVsDictionaryInsertPool1(int numKeys)
    {
        var dict = new Dictionary<TestKey<int>, int>(new TestKeyComparer<int>());
        var mDict = new MultiDictionary<TestKey<int>, int>(new TestKeyComparer<int>());

        //Generate 10000 keys
        var keys = new List<TestKey<int>>();
        for (var i = 0; i < numKeys; i++)
        {
            var key = new TestKey<int>(i%100, i);
            keys.Add(key);
        }

        var timer = new Stopwatch();

        //Add to dictionary
        timer.Start();
        foreach (var key in keys)
        {
            dict.Add(key, key.Value);
        }
        timer.Stop();

        var dictTime = timer.Elapsed;
        Console.WriteLine("Dictionary took " + timer.Elapsed);

        timer.Reset();

        //Add to multi-dictionary
        timer.Start();
        foreach (var key in keys)
        {
            mDict.Add(key, key.Value);
        }
        timer.Stop();

        var mDictTime = timer.Elapsed;
        Console.WriteLine("MultiDictionary took " + timer.Elapsed);
        Assert.IsTrue(mDictTime - dictTime < new TimeSpan(0, 0, 0, 0, 100));
    }
}