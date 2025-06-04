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

namespace VDS.Common.Collections
{
    [TestFixture, Category("Dictionaries")]
    public abstract class AbstractSortedDictionaryContractTests
        : AbstractDictionaryContractTests
    {
        protected abstract IDictionary<string, int> GetInstance(IComparer<string> comparer);

        [Test]
        public void DictionaryContractSortedKeys1()
        {
            var dict = GetInstance();

            dict.Add("a", 1);
            dict.Add("b", 2);

            Assert.AreEqual(dict["a"], 1);
            Assert.AreEqual(dict["b"], 2);

            var keys = dict.Keys;
            Assert.AreEqual("a", keys.First());
            Assert.AreEqual("b", keys.Last());
        }

        [Test]
        public void DictionaryContractSortedKeys2()
        {
            var dict = GetInstance();

            dict.Add("b", 1);
            dict.Add("a", 2);

            Assert.AreEqual(dict["b"], 1);
            Assert.AreEqual(dict["a"], 2);

            var keys = dict.Keys;
            Assert.AreEqual("a", keys.First());
            Assert.AreEqual("b", keys.Last());
        }
    }

    [TestFixture, Category("Dictionaries")]
    public class SortedDictionaryContractTests
        : AbstractSortedDictionaryContractTests
    {
        protected override IDictionary<string, int> GetInstance()
        {
            return new SortedDictionary<string, int>();
        }

        protected override IDictionary<string, int> GetInstance(IComparer<string> comparer)
        {
            return new SortedDictionary<string, int>(comparer);
        }
    }

    [TestFixture, Category("Dictionaries")]
    public class TreeSortedDictionaryContractTests2
        : AbstractSortedDictionaryContractTests
    {
        protected override IDictionary<string, int> GetInstance()
        {
            return new TreeSortedDictionary<string, int>();
        }

        protected override IDictionary<string, int> GetInstance(IComparer<string> comparer)
        {
            return new TreeSortedDictionary<string, int>(comparer);
        }
    }
}
