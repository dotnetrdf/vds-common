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
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VDS.Common.Collections
{
    [TestClass]
    public abstract class AbstractDictionaryContractTests
    {
        /// <summary>
        /// Gets the instance of a dictionary for use in a test
        /// </summary>
        /// <returns></returns>
        protected abstract IDictionary<String, int> GetInstance();

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void DictionaryContractAdd1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add("key", 1);
            dict.Add("key", 2);
        }
        
        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void DictionaryContractAdd2()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add(new KeyValuePair<String, int>("key", 1));
            dict.Add(new KeyValuePair<String, int>("key", 2));
        }

        [TestMethod]
        public void DictionaryContractRemove1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add("key", 1);
            Assert.IsTrue(dict.Remove("key"));
        }

        [TestMethod]
        public void DictionaryContractRemove2()
        {
            IDictionary<String, int> dict = this.GetInstance();

            Assert.IsFalse(dict.Remove("key"));
        }

        [TestMethod]
        public void DictionaryContractRemove3()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add("key", 1);
            Assert.IsTrue(dict.Remove(new KeyValuePair<String, int>("key", 1)));
        }

        [TestMethod]
        public void DictionaryContractRemove4()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add("key", 1);
            Assert.IsFalse(dict.Remove(new KeyValuePair<String, int>("key", 2)));
        }

        [TestMethod]
        public void DictionaryContractContains1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            Assert.IsFalse(dict.ContainsKey("key"));
        }

        [TestMethod]
        public void DictionaryContractContains2()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add("key", 1);
            Assert.IsTrue(dict.ContainsKey("key"));
        }

        [TestMethod]
        public void DictionaryContractContains3()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add("key", 1);
            Assert.IsTrue(dict.Contains(new KeyValuePair<String, int>("key", 1)));
        }

        [TestMethod]
        public void DictionaryContractContains4()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add("key", 1);
            Assert.IsFalse(dict.Contains(new KeyValuePair<String, int>("key", 2)));
        }

        [TestMethod, ExpectedException(typeof(KeyNotFoundException))]
        public void DictionaryContractItemGet1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            int value = dict["key"];
        }

        [TestMethod]
        public void DictionaryContractItemGet2()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add("key", 1);
            Assert.AreEqual(1, dict["key"]);
        }

        [TestMethod,ExpectedException(typeof(KeyNotFoundException))]
        public void DictionaryContractItemGet3()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add("key", 1);
            dict.Remove("key");
            int value = dict["key"];
        }

        [TestMethod]
        public void DictionaryContractItemSet1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict["key"] = 1;
            Assert.AreEqual(1, dict["key"]);
        }

        [TestMethod]
        public void DictionaryContractItemSet2()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add("key", 1);
            dict["key"] = 2;
            Assert.AreEqual(2, dict["key"]);
        }

        [TestMethod]
        public void DictionaryContractItemSet3()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict["key"] = 1;
            dict.Remove("key");
            dict["key"] = 2;
            Assert.AreEqual(2, dict["key"]);
        }

        [TestMethod]
        public void DictionaryContractTryGetValue1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            int value;
            Assert.IsFalse(dict.TryGetValue("key", out value));
        }

        [TestMethod]
        public void DictionaryContractTryGetValue2()
        {
            IDictionary<String, int> dict = this.GetInstance();

            int value;
            dict.Add("key", 1);
            Assert.IsTrue(dict.TryGetValue("key", out value));
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public void DictionaryContractKeys1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            Assert.IsFalse(dict.Keys.Any());
        }

        [TestMethod]
        public void DictionaryContractKeys2()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add("key", 1);
            Assert.IsTrue(dict.Keys.Any());
            Assert.IsTrue(dict.Keys.Contains("key"));
        }

        [TestMethod,ExpectedException(typeof(NotSupportedException))]
        public void DictionaryContractKeys3()
        {
            IDictionary<String, int> dict = this.GetInstance();

            Assert.IsFalse(dict.Keys.Any());
            dict.Keys.Add("key");
            Assert.IsFalse(dict.ContainsKey("key"));
        }

        [TestMethod]
        public void DictionaryContractKeys4()
        {
            IDictionary<String, int> dict = this.GetInstance();

            ICollection<String> keys = dict.Keys;
            Assert.AreEqual(0, keys.Count);
            dict.Add("key", 1);
            Assert.AreEqual(1, keys.Count);
            Assert.AreEqual(1, dict.Count);
        }

        [TestMethod]
        public void DictionaryContractValues1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            Assert.IsFalse(dict.Values.Any());
        }

        [TestMethod]
        public void DictionaryContractValues2()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add("key", 1);
            Assert.IsTrue(dict.Values.Any());
            Assert.IsTrue(dict.Values.Contains(1));
        }

        [TestMethod, ExpectedException(typeof(NotSupportedException))]
        public void DictionaryContractValues3()
        {
            IDictionary<String, int> dict = this.GetInstance();

            Assert.IsFalse(dict.Values.Any());
            dict.Values.Add(1);
            Assert.IsFalse(dict.Values.Contains(1));
        }

        [TestMethod]
        public void DictionaryContractValues4()
        {
            IDictionary<String, int> dict = this.GetInstance();

            ICollection<int> values = dict.Values;
            Assert.AreEqual(0, values.Count);
            dict.Add("key", 1);
            Assert.AreEqual(1, values.Count);
            Assert.AreEqual(1, dict.Count);
        }
    }

    [TestClass]
    public abstract class AbstractDictionaryWithNullKeysAllowedContractTests
        : AbstractDictionaryContractTests
    {
        [TestMethod]
        public void DictionaryContractAddNullKey1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add(null, 1);
            Assert.AreEqual(1, dict[null]);
        }

        [TestMethod]
        public void DictionaryContractRemoveNullKey1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            Assert.IsFalse(dict.Remove(null));
        }

        [TestMethod]
        public void DictionaryContractRemoveNullKey2()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add(null, 1);
            Assert.IsTrue(dict.Remove(null));
        }

        [TestMethod]
        public void DictionaryContractContainsNullKey1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            Assert.IsFalse(dict.ContainsKey(null));
        }

        [TestMethod]
        public void DictionaryContractContainsNullKey2()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add(null, 1);
            Assert.IsTrue(dict.ContainsKey(null));
        }

        [TestMethod, ExpectedException(typeof(KeyNotFoundException))]
        public void DictionaryContractItemGetNullKey1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            int value = dict[null];
        }

        [TestMethod]
        public void DictionaryContractItemGetNullKey2()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add(null, 1);
            int value = dict[null];
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public void DictionaryContractItemSetNullKey1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict[null] = 1;
            Assert.AreEqual(1, dict[null]);
        }

        [TestMethod]
        public void DictionaryContractItemSetNullKey2()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict[null] = 1;
            Assert.AreEqual(1, dict[null]);
            dict[null] = 2;
            Assert.AreEqual(2, dict[null]);
        }

        [TestMethod]
        public void DictionaryContractTryGetValueNullKey1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            int value;
            Assert.IsFalse(dict.TryGetValue(null, out value));
        }

        [TestMethod]
        public void DictionaryContractTryGetValueNullKey2()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add(null, 1);
            int value;
            Assert.IsTrue(dict.TryGetValue(null, out value));
            Assert.AreEqual(1, value);
        }
    }

    [TestClass]
    public abstract class AbstractDictionaryWithNullKeysForbiddenContractTests
        : AbstractDictionaryContractTests
    {
        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void DictionaryContractAddNullKey1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add(null, 1);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void DictionaryContractRemoveNullKey1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            Assert.IsFalse(dict.Remove(null));
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void DictionaryContractContainsNullKey1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            Assert.IsFalse(dict.ContainsKey(null));
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void DictionaryContractItemGetNullKey1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            int value = dict[null];
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void DictionaryContractItemSetNullKey1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict[null] = 1;
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void DictionaryContractTryGetValueNullKey1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            int value;
            Assert.IsFalse(dict.TryGetValue(null, out value));
        }
    }

    [TestClass]
    public class DictionaryContractTests
        : AbstractDictionaryWithNullKeysForbiddenContractTests
    {
        protected override IDictionary<string, int> GetInstance()
        {
            return new Dictionary<string, int>();
        }
    }

    [TestClass]
    public class MultiDictionaryContractTests
        : AbstractDictionaryWithNullKeysForbiddenContractTests
    {
        protected override IDictionary<string, int> GetInstance()
        {
            return new MultiDictionary<string, int>();
        }
    }

    [TestClass]
    public class MultiDictionaryWithNullableKeysContractTests
        : AbstractDictionaryWithNullKeysAllowedContractTests
    {
        protected override IDictionary<string, int> GetInstance()
        {
            return new MultiDictionary<string, int>(s => s != null ? s.GetHashCode() : 0, true);
        }
    }

    [TestClass]
    public class TreeSortedDictionaryContractTests
        : AbstractDictionaryWithNullKeysAllowedContractTests
    {
        protected override IDictionary<string, int> GetInstance()
        {
            return new TreeSortedDictionary<String, int>();
        }
    }
}
