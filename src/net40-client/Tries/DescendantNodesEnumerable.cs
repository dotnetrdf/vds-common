/*
VDS.Common is licensed under the MIT License

Copyright (c) 2012-2015 Robert Vesse
Copyright (c) 2016-2017 dotNetRDF Project (http://dotnetrdf.org/)

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
using System.Text;

namespace VDS.Common.Tries
{
    /// <summary>
    /// An enumerable over a Trie which ensures every time it is enumerated the latest state of the Trie is seen
    /// </summary>
    /// <typeparam name="TKeyBit">Key Bit Type</typeparam>
    /// <typeparam name="TValue">Valye Type</typeparam>
    public class DescendantNodesEnumerable<TKeyBit, TValue>
        : IEnumerable<ITrieNode<TKeyBit, TValue>>
        where TValue : class
    {
        private ITrieNode<TKeyBit, TValue> _node;

        /// <summary>
        /// Creates a descendant nodes enumable
        /// </summary>
        /// <param name="node">Node</param>
        public DescendantNodesEnumerable(ITrieNode<TKeyBit, TValue> node)
        {
            if (node == null) throw new ArgumentNullException("node");
            this._node = node;
        }

        /// <summary>
        /// Gets the enumerator over the descendants
        /// </summary>
        /// <returns></returns>
        public IEnumerator<ITrieNode<TKeyBit, TValue>> GetEnumerator()
        {
            if (this._node.IsLeaf)
            {
                return Enumerable.Empty<ITrieNode<TKeyBit, TValue>>().GetEnumerator();
            }
            else
            {
                return this._node.Children.Concat(this._node.Children.SelectMany(c => c.Descendants)).GetEnumerator();
            }
        }

        /// <summary>
        /// Gets the enumerator over the descendants
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
