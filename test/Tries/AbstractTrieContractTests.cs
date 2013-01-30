using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VDS.Common.Tries
{
    [TestClass]
    public abstract class AbstractTrieContractTests
    {
        protected abstract ITrie<String, char, String> GetInstance();

        [TestMethod]
        public void TrieContractAdd1()
        {
            ITrie<String, char, String> trie = this.GetInstance();
            trie.Add("test", "a");

            Assert.AreEqual("a", trie["test"]);
        }

        [TestMethod]
        public void TrieContractRemove1()
        {
            ITrie<String, char, String> trie = this.GetInstance();
            trie.Add("test", "a");
            trie.Add("testing", "b");

            Assert.AreEqual("a", trie["test"]);
            Assert.AreEqual("b", trie["testing"]);

            trie.Remove("test");

            Assert.AreEqual("b", trie["testing"]);
        }
    }

    [TestClass]
    public class TrieContractTests
        : AbstractTrieContractTests
    {
        protected override ITrie<string, char, String> GetInstance()
        {
            return new StringTrie<String>();
        }
    }
}
