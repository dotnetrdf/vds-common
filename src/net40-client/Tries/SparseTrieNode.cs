/*
VDS.Common is licensed under the MIT License

Copyright (c) 2009-2013 Robert Vesse

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
    /// Node of a Trie
    /// </summary>
    /// <typeparam name="TKeyBit">Key Bit Type</typeparam>
    /// <typeparam name="TValue">Value Type</typeparam>
    /// <remarks>
    /// <para>
    /// Original code taken from <a href="http://code.google.com/p/typocalypse/source/browse/#hg/Trie">Typocolypse</a> but has been heavily rewritten to be much more generic and LINQ friendly
    /// </para>
    /// </remarks>
    public class SparseValueTrieNode<TKeyBit, TValue> 
        : AbstractSparseTrieNode<TKeyBit, TValue>
        where TKeyBit : struct, IEquatable<TKeyBit>
        where TValue : class
    {
        private Nullable<TKeyBit> _singleton;
        private ITrieNode<TKeyBit, TValue> _singletonNode;
        private Dictionary<TKeyBit, ITrieNode<TKeyBit, TValue>> _children;

        /// <summary>
        /// Create an empty node with no children and null value
        /// </summary>
        /// <param name="parent">Parent node of this node</param>
        /// <param name="key">Key Bit</param>
        public SparseValueTrieNode(ITrieNode<TKeyBit, TValue> parent, TKeyBit key)
            : base(parent, key) { }

        protected override bool MatchesSingleton(TKeyBit key)
        {
            return this._singleton.HasValue && this._singleton.Value.Equals(key);
        }

        protected override void ClearSingleton()
        {
            this._singleton = null;
            this._singletonNode = null;
        }

        protected override ITrieNode<TKeyBit, TValue> CreateNewChild(TKeyBit key)
        {
            return new SparseValueTrieNode<TKeyBit, TValue>(this, key);
        }

        protected override ITrieNode<TKeyBit, TValue> SingletonChild
        {
            get
            {
                return this._singletonNode;
            }
            set
            {
                this._singleton = value.KeyBit;
                this._singletonNode = value;
            }
        }

    }

    /// <summary>
    /// Node of a Trie
    /// </summary>
    /// <typeparam name="TKeyBit">Key Bit Type</typeparam>
    /// <typeparam name="TValue">Value Type</typeparam>
    /// <remarks>
    /// <para>
    /// Original code taken from <a href="http://code.google.com/p/typocalypse/source/browse/#hg/Trie">Typocolypse</a> but has been heavily rewritten to be much more generic and LINQ friendly
    /// </para>
    /// </remarks>
    public class SparseReferenceTrieNode<TKeyBit, TValue>
        : AbstractSparseTrieNode<TKeyBit, TValue>
        where TKeyBit : class, IEquatable<TKeyBit>
        where TValue : class
    {
        private TKeyBit _singleton;
        private ITrieNode<TKeyBit, TValue> _singletonNode;
        private Dictionary<TKeyBit, ITrieNode<TKeyBit, TValue>> _children;

        /// <summary>
        /// Create an empty node with no children and null value
        /// </summary>
        /// <param name="parent">Parent node of this node</param>
        /// <param name="key">Key Bit</param>
        public SparseReferenceTrieNode(ITrieNode<TKeyBit, TValue> parent, TKeyBit key)
            : base(parent, key) { }

        protected override bool MatchesSingleton(TKeyBit key)
        {
            return this._singleton != null && this._singleton.Equals(key);
        }

        protected override void ClearSingleton()
        {
            this._singleton = null;
            this._singletonNode = null;
        }

        protected override ITrieNode<TKeyBit, TValue> CreateNewChild(TKeyBit key)
        {
            return new SparseReferenceTrieNode<TKeyBit, TValue>(this, key);
        }

        protected override ITrieNode<TKeyBit, TValue> SingletonChild
        {
            get
            {
                return this._singletonNode;
            }
            set
            {
                this._singleton = value.KeyBit;
                this._singletonNode = value;
            }
        }

    }
}