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
