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
using System.Diagnostics;
using NUnit.Framework;

namespace VDS.Common.Collections
{
    [TestFixture, Category("Dictionaries")]
    public class MultiDictionaryTests
    {
        [Test]
        public void MultiDictionaryInstantiation1()
        {
            MultiDictionary<string, int> dict = new MultiDictionary<string, int>();
        }

        [Test]
        public void MultiDictionaryNullKeyHandling1()
        {
            MultiDictionary<object, int> dict = new MultiDictionary<object, int>();
            Assert.Throws<ArgumentNullException>(() => dict.Add(null, 1));
        }

        [Test]
        public void MultiDictionaryNullKeyHandling2()
        {
            MultiDictionary<object, int> dict = new MultiDictionary<object, int>();
            Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = dict[null];
            });
        }

        [Test]
        public void MultiDictionaryNullKeyHandling3()
        {
            MultiDictionary<object, int> dict = new MultiDictionary<object, int>();
            Assert.Throws<ArgumentNullException>(() => dict.TryGetValue(null, out var i));
        }

        [Test]
        public void MultiDictionaryNullKeyHandling4()
        {
            MultiDictionary<object, int> dict = new MultiDictionary<object, int>();
            Assert.Throws<ArgumentNullException>(() => { dict[null] = 1; });
        }

        [Test]
        public void MultiDictionaryNullKeyHandling5()
        {
            MultiDictionary<object, int> dict = new MultiDictionary<object, int>();
            Assert.Throws<ArgumentNullException>(() => { dict.Add(new KeyValuePair<object, int>(null, 1)); });
        }

        [Test]
        public void MultiDictionaryNullKeyHandling6()
        {
            MultiDictionary<object, int> dict = new MultiDictionary<object, int>();
            Assert.Throws<ArgumentNullException>(() => dict.Remove(null));
        }

        [Test]
        public void MultiDictionaryNullKeyHandling7()
        {
            MultiDictionary<object, int> dict = new MultiDictionary<object, int>();
            Assert.Throws<ArgumentNullException>(() => dict.Remove(new KeyValuePair<object, int>(null, 1)));
        }

        [Test]
        public void MultiDictionaryNullKeyHandling8()
        {
            MultiDictionary<object, int> dict = new MultiDictionary<object, int>();
            Assert.Throws<ArgumentNullException>(() => { dict.ContainsKey(null); });
        }

        [Test]
        public void MultiDictionaryNullKeyHandling10()
        {
            MultiDictionary<object, int> dict = new MultiDictionary<object, int>(x => (x == null ? 0 : x.GetHashCode()), true);
            dict.Contains(new KeyValuePair<object, int>(null, 1));
        }

        [Test]
        public void MultiDictionaryNullKeyHandling11()
        {
            MultiDictionary<object, int> dict = new MultiDictionary<object, int>(x => (x == null ? 0 : x.GetHashCode()), true);
            dict.Add(null, 1);
        }

        [Test]
        public void MultiDictionaryNullKeyHandling12()
        {
            MultiDictionary<object, int> dict = new MultiDictionary<object, int>(x => (x == null ? 0 : x.GetHashCode()), true);
            Assert.Throws<KeyNotFoundException>(() =>
            {
                var _ = dict[null];
            });
        }

        [Test]
        public void MultiDictionaryNullKeyHandling13()
        {
            MultiDictionary<object, int> dict = new MultiDictionary<object, int>(x => (x == null ? 0 : x.GetHashCode()), true);
            dict.TryGetValue(null, out var i);
        }

        [Test]
        public void MultiDictionaryNullKeyHandling14()
        {
            MultiDictionary<object, int> dict = new MultiDictionary<object, int>(x => (x == null ? 0 : x.GetHashCode()), true);
            dict[null] = 1;
        }

        [Test]
        public void MultiDictionaryNullKeyHandling15()
        {
            MultiDictionary<object, int> dict = new MultiDictionary<object, int>(x => (x == null ? 0 : x.GetHashCode()), true);
            dict.Add(new KeyValuePair<object, int>(null, 1));
        }

        [Test]
        public void MultiDictionaryNullKeyHandling16()
        {
            MultiDictionary<object, int> dict = new MultiDictionary<object, int>(x => (x == null ? 0 : x.GetHashCode()), true);
            dict.Remove(null);
        }

        [Test]
        public void MultiDictionaryNullKeyHandling17()
        {
            MultiDictionary<object, int> dict = new MultiDictionary<object, int>(x => (x == null ? 0 : x.GetHashCode()), true);
            dict.Remove(new KeyValuePair<object, int>(null, 1));
        }

        [Test]
        public void MultiDictionaryNullKeyHandling18()
        {
            MultiDictionary<object, int> dict = new MultiDictionary<object, int>(x => (x == null ? 0 : x.GetHashCode()), true);
            dict.ContainsKey(null);
        }

        [Test]
        public void MultiDictionaryNullKeyHandling19()
        {
            MultiDictionary<object, int> dict = new MultiDictionary<object, int>(x => (x == null ? 0 : x.GetHashCode()), true);
            dict.Contains(new KeyValuePair<object, int>(null, 1));
        }

        [Test]
        public void MultiDictionaryVsDictionaryInsertBasic1()
        {
            Dictionary<TestKey<string>, int> dict = new Dictionary<TestKey<string>, int>();
            MultiDictionary<TestKey<string>, int> mDict = new MultiDictionary<TestKey<string>, int>(new TestKeyComparer<string>());

            TestKey<string> a = new TestKey<string>(1, "a");
            TestKey<string> b = new TestKey<string>(1, "b");

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
        }

        [Test]
        public void MultiDictionaryVsDictionaryInsertBasic2()
        {
            Dictionary<TestKey<string>, int> dict = new Dictionary<TestKey<string>, int>(new TestKeyComparer<string>());
            MultiDictionary<TestKey<string>, int> mDict = new MultiDictionary<TestKey<string>, int>(new TestKeyComparer<string>());

            TestKey<string> a = new TestKey<string>(1, "a");
            TestKey<string> b = new TestKey<string>(1, "b");

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
            Dictionary<TestKey<int>, int> dict = new Dictionary<TestKey<int>, int>(new TestKeyComparer<int>());
            MultiDictionary<TestKey<int>, int> mDict = new MultiDictionary<TestKey<int>, int>(new TestKeyComparer<int>());

            //Build dictionaries with 10000 keys in them
            List<TestKey<int>> keys = new List<TestKey<int>>();
            for (int i = 0; i < 10000; i++)
            {
                TestKey<int> key = new TestKey<int>(0, i);
                keys.Add(key);
                dict.Add(key, i);
                mDict.Add(key, i);
            }

            Stopwatch timer = new Stopwatch();

            //Lookup all keys in dictionary
            timer.Start();
            foreach (TestKey<int> key in keys)
            {
                dict.ContainsKey(key);
            }
            timer.Stop();

            TimeSpan dictTime = timer.Elapsed;
            Console.WriteLine("Dictionary took " + timer.Elapsed);
            timer.Reset();

            //Lookup all keys in multi-dictionary
            timer.Start();
            foreach (TestKey<int> key in keys)
            {
                mDict.ContainsKey(key);
            }
            timer.Stop();

            TimeSpan mDictTime = timer.Elapsed;
            Console.WriteLine("MultiDictionary took " + timer.Elapsed);

            Assert.IsTrue(mDictTime < dictTime);
        }

        [Test]
        [Category("Timing")]
        public void MultiDictionaryVsDictionaryLookupPathological2()
        {
            Dictionary<TestKey<int>, int> dict = new Dictionary<TestKey<int>, int>(new TestKeyComparer<int>());
            MultiDictionary<TestKey<int>, int> mDict = new MultiDictionary<TestKey<int>, int>(new TestKeyComparer<int>());

            //Build dictionaries with 10000 keys in them
            List<TestKey<int>> keys = new List<TestKey<int>>();
            for (int i = 0; i < 10000; i++)
            {
                TestKey<int> key = new TestKey<int>(0, i);
                keys.Add(key);
                dict.Add(key, i);
                mDict.Add(key, i);
            }

            Stopwatch timer = new Stopwatch();

            //Lookup all keys in multi-dictionary
            timer.Start();
            foreach (TestKey<int> key in keys)
            {
                mDict.ContainsKey(key);
            }
            timer.Stop();

            TimeSpan mDictTime = timer.Elapsed;
            Console.WriteLine("MultiDictionary took " + timer.Elapsed);

            timer.Reset();

            //Lookup all keys in dictionary
            timer.Start();
            foreach (TestKey<int> key in keys)
            {
                dict.ContainsKey(key);
            }
            timer.Stop();

            TimeSpan dictTime = timer.Elapsed;
            Console.WriteLine("Dictionary took " + timer.Elapsed);

            Assert.IsTrue(mDictTime < dictTime);
        }

        [Test]
        [Category("Timing")]
        public void MultiDictionaryVsDictionaryInsertPathological1()
        {
            Dictionary<TestKey<int>, int> dict = new Dictionary<TestKey<int>, int>(new TestKeyComparer<int>());
            MultiDictionary<TestKey<int>, int> mDict = new MultiDictionary<TestKey<int>, int>(new TestKeyComparer<int>());

            //Generate 10000 keys
            List<TestKey<int>> keys = new List<TestKey<int>>();
            for (int i = 0; i < 10000; i++)
            {
                TestKey<int> key = new TestKey<int>(0, i);
                keys.Add(key);
            }

            Stopwatch timer = new Stopwatch();

            //Add to dictionary
            timer.Start();
            foreach (TestKey<int> key in keys)
            {
                dict.Add(key, key.Value);
            }
            timer.Stop();

            TimeSpan dictTime = timer.Elapsed;
            Console.WriteLine("Dictionary took " + timer.Elapsed);

            timer.Reset();

            //Add to multi-dictionary
            timer.Start();
            foreach (TestKey<int> key in keys)
            {
                mDict.Add(key, key.Value);
            }
            timer.Stop();

            TimeSpan mDictTime = timer.Elapsed;
            Console.WriteLine("MutliDictionary took " + timer.Elapsed);

            Assert.IsTrue(mDictTime < dictTime);
        }

        [TestCase(50000), TestCase(100000), TestCase(250000)]
        [Category("Timing")]
        public void MultiDictionaryVsDictionaryInsertNormal1(int numKeys)
        {
            Dictionary<TestKey<int>, int> dict = new Dictionary<TestKey<int>, int>(new TestKeyComparer<int>());
            MultiDictionary<TestKey<int>, int> mDict = new MultiDictionary<TestKey<int>, int>(new TestKeyComparer<int>());

            //Generate some number of keys
            List<TestKey<int>> keys = new List<TestKey<int>>();
            for (int i = 0; i < numKeys; i++)
            {
                TestKey<int> key = new TestKey<int>(i, i);
                keys.Add(key);
            }

            Stopwatch timer = new Stopwatch();

            //Add to dictionary
            timer.Start();
            foreach (TestKey<int> key in keys)
            {
                dict.Add(key, key.Value);
            }
            timer.Stop();

            TimeSpan dictTime = timer.Elapsed;
            Console.WriteLine("Dictionary took " + timer.Elapsed);

            timer.Reset();

            //Add to multi-dictionary
            timer.Start();
            foreach (TestKey<int> key in keys)
            {
                mDict.Add(key, key.Value);
            }
            timer.Stop();

            TimeSpan mDictTime = timer.Elapsed;
            Console.WriteLine("MultiDictionary took " + timer.Elapsed);

            Assert.IsTrue(mDictTime > dictTime);
        }

        [Test]
        [Category("Timing")]
        public void MultiDictionaryVsDictionaryLookupPool1()
        {
            Dictionary<TestKey<int>, int> dict = new Dictionary<TestKey<int>, int>(new TestKeyComparer<int>());
            MultiDictionary<TestKey<int>, int> mDict = new MultiDictionary<TestKey<int>, int>(new TestKeyComparer<int>());

            //Build dictionaries with 10000 keys in them
            List<TestKey<int>> keys = new List<TestKey<int>>();
            for (int i = 0; i < 10000; i++)
            {
                TestKey<int> key = new TestKey<int>(i%100, i);
                keys.Add(key);
                dict.Add(key, i);
                mDict.Add(key, i);
            }

            Stopwatch timer = new Stopwatch();

            //Lookup all keys in dictionary
            timer.Start();
            foreach (TestKey<int> key in keys)
            {
                dict.ContainsKey(key);
            }
            timer.Stop();

            TimeSpan dictTime = timer.Elapsed;
            Console.WriteLine("Dictionary took " + timer.Elapsed);
            timer.Reset();

            //Lookup all keys in multi-dictionary
            timer.Start();
            foreach (TestKey<int> key in keys)
            {
                mDict.ContainsKey(key);
            }
            timer.Stop();

            TimeSpan mDictTime = timer.Elapsed;
            Console.WriteLine("MultiDictionary took " + timer.Elapsed);

            Assert.IsTrue(mDictTime < dictTime);
        }

        [Test]
        [Category("Timing")]
        public void MultiDictionaryVsDictionaryLookupPool2()
        {
            Dictionary<TestKey<int>, int> dict = new Dictionary<TestKey<int>, int>(new TestKeyComparer<int>());
            MultiDictionary<TestKey<int>, int> mDict = new MultiDictionary<TestKey<int>, int>(new TestKeyComparer<int>());

            //Build dictionaries with 10000 keys in them
            List<TestKey<int>> keys = new List<TestKey<int>>();
            for (int i = 0; i < 10000; i++)
            {
                TestKey<int> key = new TestKey<int>(i%100, i);
                keys.Add(key);
                dict.Add(key, i);
                mDict.Add(key, i);
            }

            Stopwatch timer = new Stopwatch();

            //Lookup all keys in multi-dictionary
            timer.Start();
            foreach (TestKey<int> key in keys)
            {
                mDict.ContainsKey(key);
            }
            timer.Stop();

            TimeSpan mDictTime = timer.Elapsed;
            Console.WriteLine("MultiDictionary took " + timer.Elapsed);

            timer.Reset();

            //Lookup all keys in dictionary
            timer.Start();
            foreach (TestKey<int> key in keys)
            {
                dict.ContainsKey(key);
            }
            timer.Stop();

            TimeSpan dictTime = timer.Elapsed;
            Console.WriteLine("Dictionary took " + timer.Elapsed);

            Assert.IsTrue(mDictTime < dictTime);
        }

        [TestCase(1000), TestCase(10000), TestCase(100000), TestCase(250000)]
        [Category("Timing")]
        public void MultiDictionaryVsDictionaryInsertPool1(int numKeys)
        {
            Dictionary<TestKey<int>, int> dict = new Dictionary<TestKey<int>, int>(new TestKeyComparer<int>());
            MultiDictionary<TestKey<int>, int> mDict = new MultiDictionary<TestKey<int>, int>(new TestKeyComparer<int>());

            //Generate 10000 keys
            List<TestKey<int>> keys = new List<TestKey<int>>();
            for (int i = 0; i < numKeys; i++)
            {
                TestKey<int> key = new TestKey<int>(i%100, i);
                keys.Add(key);
            }

            Stopwatch timer = new Stopwatch();

            //Add to dictionary
            timer.Start();
            foreach (TestKey<int> key in keys)
            {
                dict.Add(key, key.Value);
            }
            timer.Stop();

            TimeSpan dictTime = timer.Elapsed;
            Console.WriteLine("Dictionary took " + timer.Elapsed);

            timer.Reset();

            //Add to multi-dictionary
            timer.Start();
            foreach (TestKey<int> key in keys)
            {
                mDict.Add(key, key.Value);
            }
            timer.Stop();

            TimeSpan mDictTime = timer.Elapsed;
            Console.WriteLine("MutliDictionary took " + timer.Elapsed);
            Assert.IsTrue(mDictTime - dictTime < new TimeSpan(0, 0, 0, 0, 100));
        }
    }
}