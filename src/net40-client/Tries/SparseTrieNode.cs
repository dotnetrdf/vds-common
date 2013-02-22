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
    /// Sparse Node of a Trie
    /// </summary>
    /// <typeparam name="TKeyBit">Key Bit Type</typeparam>
    /// <typeparam name="TValue">Value Type</typeparam>
    public class SparseValueTrieNode<TKeyBit, TValue> 
        : AbstractSparseTrieNode<TKeyBit, TValue>
        where TKeyBit : struct, IEquatable<TKeyBit>
        where TValue : class
    {
        private Nullable<TKeyBit> _singleton;
        private ITrieNode<TKeyBit, TValue> _singletonNode;

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
    /// Sparse Node of a Trie
    /// </summary>
    /// <typeparam name="TKeyBit">Key Bit Type</typeparam>
    /// <typeparam name="TValue">Value Type</typeparam>
    public class SparseReferenceTrieNode<TKeyBit, TValue>
        : AbstractSparseTrieNode<TKeyBit, TValue>
        where TKeyBit : class, IEquatable<TKeyBit>
        where TValue : class
    {
        private TKeyBit _singleton;
        private ITrieNode<TKeyBit, TValue> _singletonNode;

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

    /// <summary>
    /// Sparse Node of a Trie
    /// </summary>
    /// <typeparam name="TValue">Value Type</typeparam>
    public class SparseCharacterTrieNode<TValue>
        : AbstractSparseTrieNode<char, TValue>
        where TValue : class
    {
        private char _singleton = '\0';
        private ITrieNode<char, TValue> _singletonNode;

        /// <summary>
        /// Creates a new Sparse Character Trie Node
        /// </summary>
        /// <param name="parent">Parent Node</param>
        /// <param name="key">Key Bit</param>
        public SparseCharacterTrieNode(ITrieNode<char, TValue> parent, char key)
            : base(parent, key) { }

        protected override bool MatchesSingleton(char key)
        {
            return key == this._singleton;
        }

        protected override void ClearSingleton()
        {
            this._singleton = '\0';
            this._singletonNode = null;
        }

        protected override ITrieNode<char, TValue> CreateNewChild(char key)
        {
            return new SparseCharacterTrieNode<TValue>(this, key);
        }

        protected override ITrieNode<char, TValue> SingletonChild
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