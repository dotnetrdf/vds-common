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
using System.Diagnostics;
using NUnit.Framework;

namespace VDS.Common.Collections
{
    [TestFixture]
    public class MultiDictionaryTests
    {
        [Test]
        public void MultiDictionaryInstantiation1()
        {
            MultiDictionary<String, int> dict = new MultiDictionary<string, int>();
        }

        [Test,ExpectedException(typeof(ArgumentNullException))]
        public void MultiDictionaryNullKeyHandling1()
        {
            MultiDictionary<Object, int> dict = new MultiDictionary<object, int>();
            dict.Add(null, 1);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void MultiDictionaryNullKeyHandling2()
        {
            MultiDictionary<Object, int> dict = new MultiDictionary<object, int>();
            int i = dict[null];
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void MultiDictionaryNullKeyHandling3()
        {
            MultiDictionary<Object, int> dict = new MultiDictionary<object, int>();
            int i;
            dict.TryGetValue(null, out i);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void MultiDictionaryNullKeyHandling4()
        {
            MultiDictionary<Object, int> dict = new MultiDictionary<object, int>();
            dict[null] = 1;
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void MultiDictionaryNullKeyHandling5()
        {
            MultiDictionary<Object, int> dict = new MultiDictionary<object, int>();
            dict.Add(new KeyValuePair<Object, int>(null, 1));
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void MultiDictionaryNullKeyHandling6()
        {
            MultiDictionary<Object, int> dict = new MultiDictionary<object, int>();
            dict.Remove(null);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void MultiDictionaryNullKeyHandling7()
        {
            MultiDictionary<Object, int> dict = new MultiDictionary<object, int>();
            dict.Remove(new KeyValuePair<Object, int>(null, 1));
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void MultiDictionaryNullKeyHandling8()
        {
            MultiDictionary<Object, int> dict = new MultiDictionary<object, int>();
            dict.ContainsKey(null);
        }

        [Test]
        public void MultiDictionaryNullKeyHandling10()
        {
            MultiDictionary<Object, int> dict = new MultiDictionary<object, int>(x => (x == null ? 0 : x.GetHashCode()), true);
            dict.Contains(new KeyValuePair<Object, int>(null, 1));
        }

        [Test]
        public void MultiDictionaryNullKeyHandling11()
        {
            MultiDictionary<Object, int> dict = new MultiDictionary<object, int>(x => (x == null ? 0 : x.GetHashCode()), true);
            dict.Add(null, 1);
        }

        [Test,ExpectedException(typeof(KeyNotFoundException))]
        public void MultiDictionaryNullKeyHandling12()
        {
            MultiDictionary<Object, int> dict = new MultiDictionary<object, int>(x => (x == null ? 0 : x.GetHashCode()), true);
            int i = dict[null];
        }

        [Test]
        public void MultiDictionaryNullKeyHandling13()
        {
            MultiDictionary<Object, int> dict = new MultiDictionary<object, int>(x => (x == null ? 0 : x.GetHashCode()), true);
            int i;
            dict.TryGetValue(null, out i);
        }

        [Test]
        public void MultiDictionaryNullKeyHandling14()
        {
            MultiDictionary<Object, int> dict = new MultiDictionary<object, int>(x => (x == null ? 0 : x.GetHashCode()), true);
            dict[null] = 1;
        }

        [Test]
        public void MultiDictionaryNullKeyHandling15()
        {
            MultiDictionary<Object, int> dict = new MultiDictionary<object, int>(x => (x == null ? 0 : x.GetHashCode()), true);
            dict.Add(new KeyValuePair<Object, int>(null, 1));
        }

        [Test]
        public void MultiDictionaryNullKeyHandling16()
        {
            MultiDictionary<Object, int> dict = new MultiDictionary<object, int>(x => (x == null ? 0 : x.GetHashCode()), true);
            dict.Remove(null);
        }

        [Test]
        public void MultiDictionaryNullKeyHandling17()
        {
            MultiDictionary<Object, int> dict = new MultiDictionary<object, int>(x => (x == null ? 0 : x.GetHashCode()), true);
            dict.Remove(new KeyValuePair<Object, int>(null, 1));
        }

        [Test]
        public void MultiDictionaryNullKeyHandling18()
        {
            MultiDictionary<Object, int> dict = new MultiDictionary<object, int>(x => (x == null ? 0 : x.GetHashCode()), true);
            dict.ContainsKey(null);
        }

        [Test]
        public void MultiDictionaryNullKeyHandling19()
        {
            MultiDictionary<Object, int> dict = new MultiDictionary<object, int>(x => (x == null ? 0 : x.GetHashCode()), true);
            dict.Contains(new KeyValuePair<Object, int>(null, 1));
        }

        [Test]
        public void MultiDictionaryVsDictionaryInsertBasic1()
        {
            Dictionary<TestKey<String>, int> dict = new Dictionary<TestKey<String>, int>();
            MultiDictionary<TestKey<String>, int> mDict = new MultiDictionary<TestKey<String>, int>(new TestKeyComparer<String>());

            TestKey<String> a = new TestKey<String>(1, "a");
            TestKey<String> b = new TestKey<String>(1, "b");

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
            Dictionary<TestKey<String>, int> dict = new Dictionary<TestKey<String>, int>(new TestKeyComparer<String>());
            MultiDictionary<TestKey<String>, int> mDict = new MultiDictionary<TestKey<String>, int>(new TestKeyComparer<String>());

            TestKey<String> a = new TestKey<String>(1, "a");
            TestKey<String> b = new TestKey<String>(1, "b");

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

        [Test]
        public void MultiDictionaryVsDictionaryLookupNormal1()
        {
            Dictionary<TestKey<int>, int> dict = new Dictionary<TestKey<int>, int>(new TestKeyComparer<int>());
            MultiDictionary<TestKey<int>, int> mDict = new MultiDictionary<TestKey<int>, int>(new TestKeyComparer<int>());

            //Build dictionaries with 10000 keys in them
            List<TestKey<int>> keys = new List<TestKey<int>>();
            for (int i = 0; i < 10000; i++)
            {
                TestKey<int> key = new TestKey<int>(i, i);
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

            Assert.IsTrue(mDictTime > dictTime);
        }

        [Test]
        public void MultiDictionaryVsDictionaryLookupNormal2()
        {
            Dictionary<TestKey<int>, int> dict = new Dictionary<TestKey<int>, int>(new TestKeyComparer<int>());
            MultiDictionary<TestKey<int>, int> mDict = new MultiDictionary<TestKey<int>, int>(new TestKeyComparer<int>());

            //Build dictionaries with 10000 keys in them
            List<TestKey<int>> keys = new List<TestKey<int>>();
            for (int i = 0; i < 10000; i++)
            {
                TestKey<int> key = new TestKey<int>(i, i);
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

            Assert.IsTrue(mDictTime > dictTime);
        }

        [Test]
        public void MultiDictionaryVsDictionaryInsertNormal1()
        {
            Dictionary<TestKey<int>, int> dict = new Dictionary<TestKey<int>, int>(new TestKeyComparer<int>());
            MultiDictionary<TestKey<int>, int> mDict = new MultiDictionary<TestKey<int>, int>(new TestKeyComparer<int>());

            //Generate 10000 keys
            List<TestKey<int>> keys = new List<TestKey<int>>();
            for (int i = 0; i < 10000; i++)
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
            Console.WriteLine("MutliDictionary took " + timer.Elapsed);

            Assert.IsTrue(mDictTime > dictTime);
        }

        [Test]
        public void MultiDictionaryVsDictionaryLookupPool1()
        {
            Dictionary<TestKey<int>, int> dict = new Dictionary<TestKey<int>, int>(new TestKeyComparer<int>());
            MultiDictionary<TestKey<int>, int> mDict = new MultiDictionary<TestKey<int>, int>(new TestKeyComparer<int>());

            //Build dictionaries with 10000 keys in them
            List<TestKey<int>> keys = new List<TestKey<int>>();
            for (int i = 0; i < 10000; i++)
            {
                TestKey<int> key = new TestKey<int>(i % 100, i);
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
        public void MultiDictionaryVsDictionaryLookupPool2()
        {
            Dictionary<TestKey<int>, int> dict = new Dictionary<TestKey<int>, int>(new TestKeyComparer<int>());
            MultiDictionary<TestKey<int>, int> mDict = new MultiDictionary<TestKey<int>, int>(new TestKeyComparer<int>());

            //Build dictionaries with 10000 keys in them
            List<TestKey<int>> keys = new List<TestKey<int>>();
            for (int i = 0; i < 10000; i++)
            {
                TestKey<int> key = new TestKey<int>(i % 100, i);
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
        public void MultiDictionaryVsDictionaryInsertPool1()
        {
            Dictionary<TestKey<int>, int> dict = new Dictionary<TestKey<int>, int>(new TestKeyComparer<int>());
            MultiDictionary<TestKey<int>, int> mDict = new MultiDictionary<TestKey<int>, int>(new TestKeyComparer<int>());

            //Generate 10000 keys
            List<TestKey<int>> keys = new List<TestKey<int>>();
            for (int i = 0; i < 10000; i++)
            {
                TestKey<int> key = new TestKey<int>(i % 100, i);
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
