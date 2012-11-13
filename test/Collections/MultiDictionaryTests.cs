/*
VDS.Common is licensed under the MIT License

Copyright (c) 2009-2012 Rob Vesse

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
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VDS.Common.Collections
{
    [TestClass]
    public class MultiDictionaryTests
    {
        [TestMethod]
        public void MultiDictionaryVsDictionary1()
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

        [TestMethod]
        public void MultiDictionaryVsDictionary2()
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
    }
}
