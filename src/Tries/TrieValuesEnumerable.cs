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
using System.Linq;

namespace VDS.Common.Tries
{
    /// <summary>
    /// An enumerable over the values of a Trie which ensures that the latest state of the Trie is always enumerated
    /// </summary>
    /// <typeparam name="TKeyBit">Key Bit Type</typeparam>
    /// <typeparam name="TValue">Valye Type</typeparam>
    public class TrieValuesEnumerable<TKeyBit, TValue>
        : IEnumerable<TValue>
        where TValue : class
    {
        private ITrieNode<TKeyBit, TValue> _node;

        /// <summary>
        /// Creates a new values enumerator
        /// </summary>
        /// <param name="node">Node to start enumeration from</param>
        public TrieValuesEnumerable(ITrieNode<TKeyBit, TValue> node)
        {
            this._node = node ?? throw new ArgumentNullException(nameof(node));
        }

        /// <summary>
        /// Gets an enumerator over the values in the Trie
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TValue> GetEnumerator()
        {
            return this._node.HasValue switch
            {
                true => this._node.Value.AsEnumerable().Concat(this._node.Descendants.Where(n => n.HasValue).Select(n => n.Value)).GetEnumerator(),
                _ => this._node.Descendants.Where(n => n.HasValue).Select(n => n.Value).GetEnumerator()
            };
        }

        /// <summary>
        /// Gets an enumerator over the values in the Trie
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
