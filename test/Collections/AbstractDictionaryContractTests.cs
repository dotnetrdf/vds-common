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
        protected abstract IDictionary<string, int> GetInstance();

        [Test]
        public void DictionaryContractAdd1()
        {
            var dict = GetInstance();

            dict.Add("key", 1);
            Assert.Throws<ArgumentException>(() => dict.Add("key", 2));
        }
        
        [Test]
        public void DictionaryContractAdd2()
        {
            var dict = GetInstance();

            dict.Add(new KeyValuePair<string, int>("key", 1));
            Assert.Throws<ArgumentException>(() => dict.Add(new KeyValuePair<string, int>("key", 2)));
        }

        [Test]
        public void DictionaryContractRemove1()
        {
            var dict = GetInstance();

            dict.Add("key", 1);
            Assert.IsTrue(dict.Remove("key"));
        }

        [Test]
        public void DictionaryContractRemove2()
        {
            var dict = GetInstance();

            Assert.IsFalse(dict.Remove("key"));
        }

        [Test]
        public void DictionaryContractRemove3()
        {
            var dict = GetInstance();

            dict.Add("key", 1);
            Assert.IsTrue(dict.Remove(new KeyValuePair<string, int>("key", 1)));
        }

        [Test]
        public void DictionaryContractRemove4()
        {
            var dict = GetInstance();

            dict.Add("key", 1);
            Assert.IsFalse(dict.Remove(new KeyValuePair<string, int>("key", 2)));
        }

        [Test]
        public void DictionaryContractContains1()
        {
            var dict = GetInstance();

            Assert.IsFalse(dict.ContainsKey("key"));
        }

        [Test]
        public void DictionaryContractContains2()
        {
            var dict = GetInstance();

            dict.Add("key", 1);
            Assert.IsTrue(dict.ContainsKey("key"));
        }

        [Test]
        public void DictionaryContractContains3()
        {
            var dict = GetInstance();

            dict.Add("key", 1);
            Assert.IsTrue(dict.Contains(new KeyValuePair<string, int>("key", 1)));
        }

        [Test]
        public void DictionaryContractContains4()
        {
            var dict = GetInstance();

            dict.Add("key", 1);
            Assert.IsFalse(dict.Contains(new KeyValuePair<string, int>("key", 2)));
        }

        [Test]
        public void DictionaryContractItemGet1()
        {
            var dict = GetInstance();

            Assert.Throws<KeyNotFoundException>(() =>
            {
                var _ = dict["key"];
            });
        }

        [Test]
        public void DictionaryContractItemGet2()
        {
            var dict = GetInstance();

            dict.Add("key", 1);
            Assert.AreEqual(1, dict["key"]);
        }

        [Test]
        public void DictionaryContractItemGet3()
        {
            var dict = GetInstance();

            dict.Add("key", 1);
            dict.Remove("key");
            Assert.Throws<KeyNotFoundException>(() =>
            {
                var _ = dict["key"];
            });
        }

        [Test]
        public void DictionaryContractItemSet1()
        {
            var dict = GetInstance();

            dict["key"] = 1;
            Assert.AreEqual(1, dict["key"]);
        }

        [Test]
        public void DictionaryContractItemSet2()
        {
            var dict = GetInstance();

            dict.Add("key", 1);
            dict["key"] = 2;
            Assert.AreEqual(2, dict["key"]);
        }

        [Test]
        public void DictionaryContractItemSet3()
        {
            var dict = GetInstance();

            dict["key"] = 1;
            dict.Remove("key");
            dict["key"] = 2;
            Assert.AreEqual(2, dict["key"]);
        }

        [Test]
        public void DictionaryContractTryGetValue1()
        {
            var dict = GetInstance();

            Assert.IsFalse(dict.TryGetValue("key", out _));
        }

        [Test]
        public void DictionaryContractTryGetValue2()
        {
            var dict = GetInstance();

            dict.Add("key", 1);
            Assert.IsTrue(dict.TryGetValue("key", out var value));
            Assert.AreEqual(1, value);
        }

        [Test]
        public void DictionaryContractKeys1()
        {
            var dict = GetInstance();

            Assert.IsFalse(dict.Keys.Any());
        }

        [Test]
        public void DictionaryContractKeys2()
        {
            var dict = GetInstance();

            dict.Add("key", 1);
            Assert.IsTrue(dict.Keys.Any());
            Assert.IsTrue(dict.Keys.Contains("key"));
        }

        [Test]
        // ,ExpectedException(typeof(NotSupportedException))
        public void DictionaryContractKeys3()
        {
            var dict = GetInstance();

            Assert.IsFalse(dict.Keys.Any());
            Assert.Throws<NotSupportedException>(() => dict.Keys.Add("key"));
            Assert.IsFalse(dict.ContainsKey("key"));
        }

        [Test]
        public void DictionaryContractKeys4()
        {
            var dict = GetInstance();

            var keys = dict.Keys;
            Assert.AreEqual(0, keys.Count);
            dict.Add("key", 1);
            Assert.AreEqual(1, keys.Count);
            Assert.AreEqual(1, dict.Count);
        }

        [Test]
        public void DictionaryContractValues1()
        {
            var dict = GetInstance();

            Assert.IsFalse(dict.Values.Any());
        }

        [Test]
        public void DictionaryContractValues2()
        {
            var dict = GetInstance();

            dict.Add("key", 1);
            Assert.IsTrue(dict.Values.Any());
            Assert.IsTrue(dict.Values.Contains(1));
        }

        [Test]
        public void DictionaryContractValues3()
        {
            var dict = GetInstance();

            Assert.IsFalse(dict.Values.Any());
            Assert.Throws<NotSupportedException>(() => dict.Values.Add(1));
            Assert.IsFalse(dict.Values.Contains(1));
        }

        [Test]
        public void DictionaryContractValues4()
        {
            var dict = GetInstance();

            var values = dict.Values;
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
        [Test]
        public void DictionaryContractAddNullKey1()
        {
            var dict = GetInstance();

            dict.Add(null, 1);
            Assert.AreEqual(1, dict[null]);
        }

        [Test]
        public void DictionaryContractRemoveNullKey1()
        {
            var dict = GetInstance();

            Assert.IsFalse(dict.Remove(null));
        }

        [Test]
        public void DictionaryContractRemoveNullKey2()
        {
            var dict = GetInstance();

            dict.Add(null, 1);
            Assert.IsTrue(dict.Remove(null));
        }

        [Test]
        public void DictionaryContractContainsNullKey1()
        {
            var dict = GetInstance();

            Assert.IsFalse(dict.ContainsKey(null));
        }

        [Test]
        public void DictionaryContractContainsNullKey2()
        {
            var dict = GetInstance();

            dict.Add(null, 1);
            Assert.IsTrue(dict.ContainsKey(null));
        }

        [Test]
        public void DictionaryContractItemGetNullKey1()
        {
            var dict = GetInstance();

            Assert.Throws<KeyNotFoundException>(() =>
            {
                var _= dict[null];
            });
        }

        [Test]
        public void DictionaryContractItemGetNullKey2()
        {
            var dict = GetInstance();

            dict.Add(null, 1);
            var value = dict[null];
            Assert.AreEqual(1, value);
        }

        [Test]
        public void DictionaryContractItemSetNullKey1()
        {
            var dict = GetInstance();

            dict[null] = 1;
            Assert.AreEqual(1, dict[null]);
        }

        [Test]
        public void DictionaryContractItemSetNullKey2()
        {
            var dict = GetInstance();

            dict[null] = 1;
            Assert.AreEqual(1, dict[null]);
            dict[null] = 2;
            Assert.AreEqual(2, dict[null]);
        }

        [Test]
        public void DictionaryContractTryGetValueNullKey1()
        {
            var dict = GetInstance();

            Assert.IsFalse(dict.TryGetValue(null, out _));
        }

        [Test]
        public void DictionaryContractTryGetValueNullKey2()
        {
            var dict = GetInstance();

            dict.Add(null, 1);
            Assert.IsTrue(dict.TryGetValue(null, out var value));
            Assert.AreEqual(1, value);
        }
    }

    [TestFixture,Category("Dictionaries")]
    public abstract class AbstractDictionaryWithNullKeysForbiddenContractTests
        : AbstractDictionaryContractTests
    {
        [Test]
        public void DictionaryContractAddNullKey1()
        {
            var dict = GetInstance();

            Assert.Throws<ArgumentNullException>(() => dict.Add(null, 1));
        }

        [Test]
        public void DictionaryContractRemoveNullKey1()
        {
            var dict = GetInstance();

            Assert.Throws<ArgumentNullException>(() =>dict.Remove(null));
        }

        [Test]
        public void DictionaryContractContainsNullKey1()
        {
            var dict = GetInstance();

            Assert.Throws<ArgumentNullException>(() => dict.ContainsKey(null));
        }

        [Test]
        public void DictionaryContractItemGetNullKey1()
        {
            var dict = GetInstance();

            Assert.Throws<ArgumentNullException>(() =>
            {
                var _= dict[null];
            });
        }

        [Test]
        public void DictionaryContractItemSetNullKey1()
        {
            var dict = GetInstance();

            Assert.Throws<ArgumentNullException>(() => dict[null] = 1);
        }

        [Test]
        public void DictionaryContractTryGetValueNullKey1()
        {
            var dict = GetInstance();

            Assert.Throws<ArgumentNullException>(() => dict.TryGetValue(null, out var value));
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
            return new MultiDictionary<string, int>(s => s?.GetHashCode() ?? 0, true);
        }
    }

    [TestFixture, Category("Dictionaries")]
    public class TreeSortedDictionaryContractTests
        : AbstractDictionaryWithNullKeysAllowedContractTests
    {
        protected override IDictionary<string, int> GetInstance()
        {
            return new TreeSortedDictionary<string, int>();
        }
    }
}
