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
using System.Linq;
using NUnit.Framework;

namespace VDS.Common.Collections
{
    [TestFixture, Category("Dictionaries")]
    public abstract class AbstractDictionaryContractTests
    {
        /// <summary>
        /// Gets the instance of a dictionary for use in a test
        /// </summary>
        /// <returns></returns>
        protected abstract IDictionary<String, int> GetInstance();

        [TestAttribute, ExpectedException(typeof(ArgumentException))]
        public void DictionaryContractAdd1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add("key", 1);
            dict.Add("key", 2);
        }
        
        [TestAttribute, ExpectedException(typeof(ArgumentException))]
        public void DictionaryContractAdd2()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add(new KeyValuePair<String, int>("key", 1));
            dict.Add(new KeyValuePair<String, int>("key", 2));
        }

        [TestAttribute]
        public void DictionaryContractRemove1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add("key", 1);
            Assert.IsTrue(dict.Remove("key"));
        }

        [TestAttribute]
        public void DictionaryContractRemove2()
        {
            IDictionary<String, int> dict = this.GetInstance();

            Assert.IsFalse(dict.Remove("key"));
        }

        [TestAttribute]
        public void DictionaryContractRemove3()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add("key", 1);
            Assert.IsTrue(dict.Remove(new KeyValuePair<String, int>("key", 1)));
        }

        [TestAttribute]
        public void DictionaryContractRemove4()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add("key", 1);
            Assert.IsFalse(dict.Remove(new KeyValuePair<String, int>("key", 2)));
        }

        [TestAttribute]
        public void DictionaryContractContains1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            Assert.IsFalse(dict.ContainsKey("key"));
        }

        [TestAttribute]
        public void DictionaryContractContains2()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add("key", 1);
            Assert.IsTrue(dict.ContainsKey("key"));
        }

        [TestAttribute]
        public void DictionaryContractContains3()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add("key", 1);
            Assert.IsTrue(dict.Contains(new KeyValuePair<String, int>("key", 1)));
        }

        [TestAttribute]
        public void DictionaryContractContains4()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add("key", 1);
            Assert.IsFalse(dict.Contains(new KeyValuePair<String, int>("key", 2)));
        }

        [TestAttribute, ExpectedException(typeof(KeyNotFoundException))]
        public void DictionaryContractItemGet1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            int value = dict["key"];
        }

        [TestAttribute]
        public void DictionaryContractItemGet2()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add("key", 1);
            Assert.AreEqual(1, dict["key"]);
        }

        [TestAttribute,ExpectedException(typeof(KeyNotFoundException))]
        public void DictionaryContractItemGet3()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add("key", 1);
            dict.Remove("key");
            int value = dict["key"];
        }

        [TestAttribute]
        public void DictionaryContractItemSet1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict["key"] = 1;
            Assert.AreEqual(1, dict["key"]);
        }

        [TestAttribute]
        public void DictionaryContractItemSet2()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add("key", 1);
            dict["key"] = 2;
            Assert.AreEqual(2, dict["key"]);
        }

        [TestAttribute]
        public void DictionaryContractItemSet3()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict["key"] = 1;
            dict.Remove("key");
            dict["key"] = 2;
            Assert.AreEqual(2, dict["key"]);
        }

        [TestAttribute]
        public void DictionaryContractTryGetValue1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            int value;
            Assert.IsFalse(dict.TryGetValue("key", out value));
        }

        [TestAttribute]
        public void DictionaryContractTryGetValue2()
        {
            IDictionary<String, int> dict = this.GetInstance();

            int value;
            dict.Add("key", 1);
            Assert.IsTrue(dict.TryGetValue("key", out value));
            Assert.AreEqual(1, value);
        }

        [TestAttribute]
        public void DictionaryContractKeys1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            Assert.IsFalse(dict.Keys.Any());
        }

        [TestAttribute]
        public void DictionaryContractKeys2()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add("key", 1);
            Assert.IsTrue(dict.Keys.Any());
            Assert.IsTrue(dict.Keys.Contains("key"));
        }

        [TestAttribute,ExpectedException(typeof(NotSupportedException))]
        public void DictionaryContractKeys3()
        {
            IDictionary<String, int> dict = this.GetInstance();

            Assert.IsFalse(dict.Keys.Any());
            dict.Keys.Add("key");
            Assert.IsFalse(dict.ContainsKey("key"));
        }

        [TestAttribute]
        public void DictionaryContractKeys4()
        {
            IDictionary<String, int> dict = this.GetInstance();

            ICollection<String> keys = dict.Keys;
            Assert.AreEqual(0, keys.Count);
            dict.Add("key", 1);
            Assert.AreEqual(1, keys.Count);
            Assert.AreEqual(1, dict.Count);
        }

        [TestAttribute]
        public void DictionaryContractValues1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            Assert.IsFalse(dict.Values.Any());
        }

        [TestAttribute]
        public void DictionaryContractValues2()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add("key", 1);
            Assert.IsTrue(dict.Values.Any());
            Assert.IsTrue(dict.Values.Contains(1));
        }

        [TestAttribute, ExpectedException(typeof(NotSupportedException))]
        public void DictionaryContractValues3()
        {
            IDictionary<String, int> dict = this.GetInstance();

            Assert.IsFalse(dict.Values.Any());
            dict.Values.Add(1);
            Assert.IsFalse(dict.Values.Contains(1));
        }

        [TestAttribute]
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

    [TestFixture,Category("Dictionaries")]
    public abstract class AbstractDictionaryWithNullKeysAllowedContractTests
        : AbstractDictionaryContractTests
    {
        [TestAttribute]
        public void DictionaryContractAddNullKey1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add(null, 1);
            Assert.AreEqual(1, dict[null]);
        }

        [TestAttribute]
        public void DictionaryContractRemoveNullKey1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            Assert.IsFalse(dict.Remove(null));
        }

        [TestAttribute]
        public void DictionaryContractRemoveNullKey2()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add(null, 1);
            Assert.IsTrue(dict.Remove(null));
        }

        [TestAttribute]
        public void DictionaryContractContainsNullKey1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            Assert.IsFalse(dict.ContainsKey(null));
        }

        [TestAttribute]
        public void DictionaryContractContainsNullKey2()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add(null, 1);
            Assert.IsTrue(dict.ContainsKey(null));
        }

        [TestAttribute, ExpectedException(typeof(KeyNotFoundException))]
        public void DictionaryContractItemGetNullKey1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            int value = dict[null];
        }

        [TestAttribute]
        public void DictionaryContractItemGetNullKey2()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add(null, 1);
            int value = dict[null];
            Assert.AreEqual(1, value);
        }

        [TestAttribute]
        public void DictionaryContractItemSetNullKey1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict[null] = 1;
            Assert.AreEqual(1, dict[null]);
        }

        [TestAttribute]
        public void DictionaryContractItemSetNullKey2()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict[null] = 1;
            Assert.AreEqual(1, dict[null]);
            dict[null] = 2;
            Assert.AreEqual(2, dict[null]);
        }

        [TestAttribute]
        public void DictionaryContractTryGetValueNullKey1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            int value;
            Assert.IsFalse(dict.TryGetValue(null, out value));
        }

        [TestAttribute]
        public void DictionaryContractTryGetValueNullKey2()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add(null, 1);
            int value;
            Assert.IsTrue(dict.TryGetValue(null, out value));
            Assert.AreEqual(1, value);
        }
    }

    [TestFixture,Category("Dictionaries")]
    public abstract class AbstractDictionaryWithNullKeysForbiddenContractTests
        : AbstractDictionaryContractTests
    {
        [TestAttribute, ExpectedException(typeof(ArgumentNullException))]
        public void DictionaryContractAddNullKey1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add(null, 1);
        }

        [TestAttribute, ExpectedException(typeof(ArgumentNullException))]
        public void DictionaryContractRemoveNullKey1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            Assert.IsFalse(dict.Remove(null));
        }

        [TestAttribute, ExpectedException(typeof(ArgumentNullException))]
        public void DictionaryContractContainsNullKey1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            Assert.IsFalse(dict.ContainsKey(null));
        }

        [TestAttribute, ExpectedException(typeof(ArgumentNullException))]
        public void DictionaryContractItemGetNullKey1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            int value = dict[null];
        }

        [TestAttribute, ExpectedException(typeof(ArgumentNullException))]
        public void DictionaryContractItemSetNullKey1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict[null] = 1;
        }

        [TestAttribute, ExpectedException(typeof(ArgumentNullException))]
        public void DictionaryContractTryGetValueNullKey1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            int value;
            Assert.IsFalse(dict.TryGetValue(null, out value));
        }
    }

    [TestFixture, Category("Dictionaries")]
    public class DictionaryContractTests
        : AbstractDictionaryWithNullKeysForbiddenContractTests
    {
        protected override IDictionary<string, int> GetInstance()
        {
            return new Dictionary<string, int>();
        }
    }

    [TestFixture, Category("Dictionaries")]
    public class MultiDictionaryContractTests
        : AbstractDictionaryWithNullKeysForbiddenContractTests
    {
        protected override IDictionary<string, int> GetInstance()
        {
            return new MultiDictionary<string, int>();
        }
    }

    [TestFixture, Category("Dictionaries")]
    public class MultiDictionaryWithNullableKeysContractTests
        : AbstractDictionaryWithNullKeysAllowedContractTests
    {
        protected override IDictionary<string, int> GetInstance()
        {
            return new MultiDictionary<string, int>(s => s != null ? s.GetHashCode() : 0, true);
        }
    }

    [TestFixture, Category("Dictionaries")]
    public class TreeSortedDictionaryContractTests
        : AbstractDictionaryWithNullKeysAllowedContractTests
    {
        protected override IDictionary<string, int> GetInstance()
        {
            return new TreeSortedDictionary<String, int>();
        }
    }
}
