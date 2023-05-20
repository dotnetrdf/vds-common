/*
VDS.Common is licensed under the MIT License

Copyright (c) 2012-2015 Robert Vesse
Copyright (c) 2016-2018 dotNetRDF Project (http://dotnetrdf.org/)

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

namespace VDS.Common.Tries
{
    /// <summary>
    /// Standard implementation of a Trie data structure
    /// </summary>
    /// <typeparam name="TKey">Type of keys</typeparam>
    /// <typeparam name="TKeyBit">Type of key bits</typeparam>
    /// <typeparam name="TValue">Type of values</typeparam>
    /// <remarks>
    /// </remarks>
    public class Trie<TKey, TKeyBit, TValue>
        : AbstractTrie<TKey, TKeyBit, TValue>
        where TValue : class
    {   
        /// <summary>
        /// Create an empty trie with an empty root node.
        /// </summary>
        public Trie(Func<TKey, IEnumerable<TKeyBit>> keyMapper)
            : base(keyMapper) { }

        /// <inheritdoc />
        protected override ITrieNode<TKeyBit, TValue> _root { get; init; } = new TrieNode<TKeyBit, TValue>(null, default);

        /// <summary>
        /// Method which creates a new child node
        /// </summary>
        /// <param name="key">Key Bit</param>
        /// <returns></returns>
        protected override ITrieNode<TKeyBit, TValue> CreateRoot(TKeyBit key)
        {
            return new TrieNode<TKeyBit, TValue>(null, key);
        }
    }
}