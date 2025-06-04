/*
VDS.Common is licensed under the MIT License

Copyright (c) 2012-2015 Robert Vesse
Copyright (c) 2016-2025 dotNetRDF Project (https://dotnetrdf.org/)

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

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace VDS.Common.Tries;

[TestFixture, Category("Tries")]
public abstract class AbstractTrieContractTests
{
    protected abstract ITrie<string, char, string> GetInstance();

    [Test]
    public void TrieContractAdd1()
    {
        var trie = GetInstance();
        trie.Add("test", "a");

        Assert.AreEqual("a", trie["test"]);
    }

    [Test]
    public void TrieContractAdd2()
    {
        var trie = GetInstance();
        trie.Add("test", "a");
        trie.Add("testing", "b");

        Assert.AreEqual("a", trie["test"]);
        Assert.AreEqual("b", trie["testing"]);
    }

    [Test]
    public void TrieContractClear1()
    {
        var trie = GetInstance();
        trie.Add("test", "a");

        Assert.AreEqual("a", trie["test"]);

        trie.Clear();

        Assert.IsFalse(trie.ContainsKey("test"));
    }

    [Test]
    public void TrieContractClear2()
    {
        var trie = GetInstance();
        trie.Add("test", "a");
        trie.Add("testing", "b");

        Assert.AreEqual("a", trie["test"]);
        Assert.AreEqual("b", trie["testing"]);

        trie.Clear();

        Assert.IsFalse(trie.ContainsKey("test"));
        Assert.IsFalse(trie.ContainsKey("testing"));
    }

    [Test]
    public void TrieContractContains1()
    {
        var trie = GetInstance();
        trie.Add("test", "a");
        trie.Add("testing", "b");

        Assert.IsTrue(trie.ContainsKey("test"));
        Assert.IsTrue(trie.ContainsKey("testing"));
    }

    [Test]
    public void TrieContractContains2()
    {
        var trie = GetInstance();
        var key = "test";
        trie.Add(key, "a");

        Assert.IsTrue(trie.ContainsKey(key));
        for (var i = 1; i < key.Length; i++)
        {
            Assert.IsFalse(trie.ContainsKey(key.Substring(0, i)));
            Assert.IsTrue(trie.ContainsKey(key.Substring(0, i), false));
        }
    }

    [Test]
    public void TrieContractRemove1()
    {
        var trie = GetInstance();
        trie.Add("test", "a");
        trie.Add("testing", "b");

        Assert.AreEqual("a", trie["test"]);
        Assert.AreEqual("b", trie["testing"]);

        trie.Remove("test");

        Assert.AreEqual("b", trie["testing"]);
        Assert.IsNotNull(trie.Find("test"));
    }

    [Test]
    public void TrieContractRemove2()
    {
        var trie = GetInstance();
        trie.Add("test", "a");
        trie.Add("testing", "b");

        Assert.AreEqual("a", trie["test"]);
        Assert.AreEqual("b", trie["testing"]);

        trie.Remove("testing");

        Assert.AreEqual("a", trie["test"]);
        Assert.IsNull(trie.Find("testing"));
    }

    [Test]
    public void TrieContractTryGetValue1()
    {
        var trie = GetInstance();

        Assert.IsFalse(trie.TryGetValue("test", out _));
    }

    [Test]
    public void TrieContractTryGetValue2()
    {
        var trie = GetInstance();
        trie.Add("test", "a");
        trie.Add("testing", "b");

        Assert.IsTrue(trie.TryGetValue("test", out var value));
        Assert.AreEqual("a", value);
        Assert.IsTrue(trie.TryGetValue("testing", out value));
        Assert.AreEqual("b", value);
    }

    [Test]
    public void TrieContractTryGetValue3()
    {
        var trie = GetInstance();
        var key = "test";
        trie.Add(key, "a");

        Assert.IsTrue(trie.TryGetValue(key, out var value));
        Assert.AreEqual("a", value);
        for (var i = 1; i < key.Length; i++)
        {
            Assert.IsFalse(trie.TryGetValue(key.Substring(0, i), out value));
            Assert.IsNull(value);
        }
    }

    [Test]
    public void TrieContractItemGet1()
    {
        var trie = GetInstance();
        trie.Add("test", "a");

        Assert.AreEqual("a", trie["test"]);
    }

    [Test]
    public void TrieContractItemGet2()
    {
        var trie = GetInstance();
        trie.Add("test", "a");
        trie.Add("testing", "b");

        Assert.AreEqual("a", trie["test"]);
        Assert.AreEqual("b", trie["testing"]);
    }

    [Test]
    public void TrieContractItemGet3()
    {
        var trie = GetInstance();
        string result = null;
        Assert.Throws<KeyNotFoundException>(() => result = trie["test"]);
        Assert.AreEqual(null, result);
    }

    [Test]
    public void TrieContractItemSet1()
    {
        var trie = GetInstance();

        Assert.IsFalse(trie.ContainsKey("test"));

        trie["test"] = "a";

        Assert.IsTrue(trie.ContainsKey("test"));
        Assert.AreEqual("a", trie["test"]);
    }

    [Test]
    public void TrieContractItemSet2()
    {
        var trie = GetInstance();

        Assert.IsFalse(trie.ContainsKey("test"));

        trie["test"] = "a";

        Assert.IsTrue(trie.ContainsKey("test"));
        Assert.AreEqual("a", trie["test"]);

        trie["test"] = "b";

        Assert.AreEqual("b", trie["test"]);
    }

    [Test]
    public void TrieContractValues1()
    {
        var trie = GetInstance();

        Assert.IsFalse(trie.Values.Any());
    }

    [Test]
    public void TrieContractValues2()
    {
        var trie = GetInstance();

        Assert.IsFalse(trie.Values.Any());

        trie.Add("test", "a");

        Assert.IsTrue(trie.Values.Any());
        Assert.AreEqual(1, trie.Values.Count());
    }

    [Test]
    public void TrieContractValues3()
    {
        var trie = GetInstance();

        Assert.IsFalse(trie.Values.Any());

        trie.Add("test", "a");

        Assert.IsTrue(trie.Values.Any());
        Assert.AreEqual(1, trie.Values.Count());
    }

    [Test]
    public void TrieContractValues4()
    {
        var trie = GetInstance();

        Assert.IsFalse(trie.Values.Any());

        trie.Add("test", "a");

        Assert.IsTrue(trie.Values.Any());
        Assert.AreEqual(1, trie.Values.Count());

        trie.Add("testing", "b");

        Assert.IsTrue(trie.Values.Any());
        Assert.AreEqual(2, trie.Values.Count());
    }

    [Test]
    public void TrieContractValues5()
    {
        var trie = GetInstance();

        Assert.IsFalse(trie.Values.Any());

        trie.Add("test", "a");

        Assert.IsTrue(trie.Values.Any());
        Assert.AreEqual(1, trie.Values.Count());

        trie.Add("testing", "b");

        Assert.IsTrue(trie.Values.Any());
        Assert.AreEqual(2, trie.Values.Count());
    }

    [Test]
    public void TrieContractDescendants1()
    {
        var trie = GetInstance();

        Assert.IsFalse(trie.Root.Descendants.Any());
    }

    [Test]
    public void TrieContractDescendants2()
    {
        var trie = GetInstance();

        Assert.IsFalse(trie.Root.Descendants.Any());

        trie.Add("test", "a");

        Assert.IsTrue(trie.Root.Descendants.Any());
        Assert.AreEqual(4, trie.Root.Descendants.Count());
    }

    [Test]
    public void TrieContractDescendants3()
    {
        var trie = GetInstance();

        Assert.IsFalse(trie.Root.Descendants.Any());

        trie.Add("test", "a");

        Assert.IsTrue(trie.Root.Descendants.Any());
        Assert.AreEqual(4, trie.Root.Descendants.Count());
    }

    [Test]
    public void TrieContractDescendants4()
    {
        var trie = GetInstance();

        Assert.IsFalse(trie.Root.Descendants.Any());

        trie.Add("test", "a");

        Assert.IsTrue(trie.Root.Descendants.Any());
        Assert.AreEqual(4, trie.Root.Descendants.Count());

        trie.Add("testing", "b");

        Assert.IsTrue(trie.Root.Descendants.Any());
        Assert.AreEqual(7, trie.Root.Descendants.Count());
    }

    [Test]
    public void TrieContractDescendants5()
    {
        var trie = GetInstance();

        Assert.IsFalse(trie.Root.Descendants.Any());

        trie.Add("test", "a");

        Assert.IsTrue(trie.Root.Descendants.Any());
        Assert.AreEqual(4, trie.Root.Descendants.Count());

        trie.Add("testing", "b");

        Assert.IsTrue(trie.Root.Descendants.Any());
        Assert.AreEqual(7, trie.Root.Descendants.Count());
    }

    [Test]
    public void TrieContractMoveToNode1()
    {
        var trie = GetInstance();

        var node = trie.MoveToNode("test");
        Assert.IsNotNull(node);
    }

    [Test]
    public void TrieContractMoveToNode2()
    {
        var trie = GetInstance();

        var node = trie.MoveToNode("test");
        Assert.IsNotNull(node);
        Assert.AreEqual(4, trie.Count);

        node = trie.MoveToNode("test");
        Assert.IsNotNull(node);
        Assert.AreEqual(4, trie.Count);
    }

    [Test]
    public void TrieContractFind1()
    {
        var trie = GetInstance();

        var node = trie.Find("test");
        Assert.IsNull(node);
    }

    [Test]
    public void TrieContractFind2()
    {
        var trie = GetInstance();

        var node = trie.Find("test");
        Assert.IsNull(node);

        trie.Add("test", "a");

        node = trie.Find("test");
        Assert.IsNotNull(node);
        Assert.AreEqual("a", node.Value);
    }

    [Test]
    public void TrieContractFind3()
    {
        var trie = GetInstance();

        var node = trie.Find("test");
        Assert.IsNull(node);

        trie.Add("test", "a");

        node = trie.Find("testing");
        Assert.IsNull(node);
    }

    [Test]
    public void TrieContractFind4()
    {
        var trie = GetInstance();

        var node = trie.Find("test");
        Assert.IsNull(node);

        trie.Add("test", "a");

        //Find with a custom key mapper
        node = trie.Find("testing", (s => s.Substring(0, 4).ToCharArray()));
        Assert.IsNotNull(node);
        Assert.AreEqual("a", node.Value);
    }

    [Test]
    public void TrieContractFindPredecessor1()
    {
        var trie = GetInstance();

        var node = trie.FindPredecessor("test");
        Assert.IsNull(node);

        trie.Add("test", "a");
        trie.Add("testing", "b");
        trie.Add("blah", "c");

        node = trie.FindPredecessor("test");
        Assert.IsNotNull(node);
        Assert.AreEqual("a", node.Value);

        node = trie.FindPredecessor("testi");
        Assert.IsNotNull(node);
        Assert.AreEqual("a", node.Value);

        node = trie.FindPredecessor("testit");
        Assert.IsNotNull(node);
        Assert.AreEqual("a", node.Value);

        node = trie.FindPredecessor("testing");
        Assert.IsNotNull(node);
        Assert.AreEqual("b", node.Value);

        node = trie.FindPredecessor("testingtest");
        Assert.IsNotNull(node);
        Assert.AreEqual("b", node.Value);

        node = trie.FindPredecessor("b");
        Assert.IsNull(node);

        node = trie.FindPredecessor("blahblah");
        Assert.IsNotNull(node);
        Assert.AreEqual("c", node.Value);
    }

    [Test]
    public void TrieContractFindSuccessor1()
    {
        var trie = GetInstance();

        var node = trie.FindSuccessor("test");
        Assert.IsNull(node);

        trie.Add("test", "a");
        trie.Add("testing", "b");
        trie.Add("blah", "c");
        trie.Add("testinh", "d");
        trie.Add("testings", "e");

        node = trie.FindSuccessor("test");
        Assert.IsNotNull(node);
        Assert.AreEqual("a", node.Value);

        node = trie.FindSuccessor("testi");
        Assert.IsNotNull(node);
        Assert.AreEqual("b", node.Value);

        node = trie.FindSuccessor("testit");
        Assert.IsNull(node);

        node = trie.FindSuccessor("testing");
        Assert.IsNotNull(node);
        Assert.AreEqual("b", node.Value);

        node = trie.FindSuccessor("t");
        Assert.IsNotNull(node);
        Assert.AreEqual("a", node.Value);

        node = trie.FindSuccessor("b");
        Assert.IsNotNull(node);
        Assert.AreEqual("c", node.Value);

        node = trie.FindSuccessor("testinga");
        Assert.IsNull(node);
    }
}

[TestFixture, Category("Tries")]
public class TrieContractTests
    : AbstractTrieContractTests
{
    protected override ITrie<string, char, string> GetInstance()
    {
        return new StringTrie<string>();
    }
}

[TestFixture, Category("Tries")]
public class TrieContractTests2
    : AbstractTrieContractTests
{
    protected override ITrie<string, char, string> GetInstance()
    {
        return new Trie<string, char, string>(StringTrie<string>.KeyMapperFunction);
    }
}

[TestFixture, Category("Tries")]
public class SparseTrieContractTests
    : AbstractTrieContractTests
{
    protected override ITrie<string, char, string> GetInstance()
    {
        return new SparseStringTrie<string>();
    }
}

[TestFixture, Category("Tries")]
public class SparseTrieContractTests2
    : AbstractTrieContractTests
{
    protected override ITrie<string, char, string> GetInstance()
    {
        return new SparseCharacterTrie<string, string>(StringTrie<string>.KeyMapperFunction);
    }
}

[TestFixture, Category("Tries")]
public class SparseTrieContractTests3
    : AbstractTrieContractTests
{
    protected override ITrie<string, char, string> GetInstance()
    {
        return new SparseValueTrie<string, char, string>(StringTrie<string>.KeyMapperFunction);
    }
}