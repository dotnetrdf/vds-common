using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VDS.Common.Collections
{
    [TestClass]
    public abstract class AbstractSortedDictionaryContractTests
        : AbstractDictionaryContractTests
    {
        protected abstract IDictionary<String, int> GetInstance(IComparer<String> comparer);

        [TestMethod]
        public void DictionaryContractSortedKeys1()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add("a", 1);
            dict.Add("b", 2);

            Assert.AreEqual(dict["a"], 1);
            Assert.AreEqual(dict["b"], 2);

            ICollection<String> keys = dict.Keys;
            Assert.AreEqual("a", keys.First());
            Assert.AreEqual("b", keys.Last());
        }

        [TestMethod]
        public void DictionaryContractSortedKeys2()
        {
            IDictionary<String, int> dict = this.GetInstance();

            dict.Add("b", 1);
            dict.Add("a", 2);

            Assert.AreEqual(dict["b"], 1);
            Assert.AreEqual(dict["a"], 2);

            ICollection<String> keys = dict.Keys;
            Assert.AreEqual("a", keys.First());
            Assert.AreEqual("b", keys.Last());
        }
    }

    [TestClass]
    public class SortedDictionaryContractTests
        : AbstractSortedDictionaryContractTests
    {
        protected override IDictionary<string, int> GetInstance()
        {
            return new SortedDictionary<String, int>();
        }

        protected override IDictionary<string, int> GetInstance(IComparer<string> comparer)
        {
            return new SortedDictionary<String, int>(comparer);
        }
    }

    [TestClass]
    public class TreeSortedDictionaryContractTests2
        : AbstractSortedDictionaryContractTests
    {
        protected override IDictionary<string, int> GetInstance()
        {
            return new TreeSortedDictionary<String, int>();
        }

        protected override IDictionary<string, int> GetInstance(IComparer<string> comparer)
        {
            return new TreeSortedDictionary<String, int>(comparer);
        }
    }
}
