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

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void DictionaryContractAdd3()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add(null, 1);
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

        [TestMethod,ExpectedException(typeof(ArgumentNullException))]
        public void DictionaryContractRemove5()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Remove(null);
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

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void DictionaryContractContains5()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.ContainsKey(null);
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

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void DictionaryContractItemGet4()
        {
            IDictionary<String, int> dict = this.GetInstance();

            int value = dict[null];
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

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void DictionaryContractItemSet4()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict[null] = 1;
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

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void DictionaryContractTryGetValue3()
        {
            IDictionary<String, int> dict = this.GetInstance();

            int value;
            dict.TryGetValue(null, out value);
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
    public class DictionaryContractTests
        : AbstractDictionaryContractTests
    {
        protected override IDictionary<string, int> GetInstance()
        {
            return new Dictionary<string, int>();
        }
    }

    [TestClass]
    public class MultiDictionaryContractTests
        : AbstractDictionaryContractTests
    {
        protected override IDictionary<string, int> GetInstance()
        {
            return new MultiDictionary<string, int>();
        }
    }
}
