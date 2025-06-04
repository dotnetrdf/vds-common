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

using System;
using System.Collections.Generic;

namespace VDS.Common.Tries;

/// <summary>
/// Sparse implementation of a Trie data structure
/// </summary>
/// <typeparam name="TKey">Type of keys</typeparam>
/// <typeparam name="TKeyBit">Type of key bits</typeparam>
/// <typeparam name="TValue">Type of values</typeparam>
/// <remarks>
/// </remarks>
public class SparseValueTrie<TKey, TKeyBit, TValue>
    : AbstractTrie<TKey, TKeyBit, TValue>
    where TKeyBit : struct, IEquatable<TKeyBit>
    where TValue : class
{   
    /// <summary>
    /// Create an empty trie with an empty root node.
    /// </summary>
    public SparseValueTrie(Func<TKey, IEnumerable<TKeyBit>> keyMapper)
        : base(keyMapper, new SparseValueTrieNode<TKeyBit, TValue>(null, default)) { }

    /// <summary>
    /// Method which creates a new child node
    /// </summary>
    /// <param name="key">Key Bit</param>
    /// <returns></returns>
    protected override ITrieNode<TKeyBit, TValue> CreateRoot(TKeyBit key)
    {
        return new SparseValueTrieNode<TKeyBit, TValue>(null, key);
    }
}

/// <summary>
/// Sparse implementation of a Trie data structure
/// </summary>
/// <typeparam name="TKey">Type of keys</typeparam>
/// <typeparam name="TKeyBit">Type of key bits</typeparam>
/// <typeparam name="TValue">Type of values</typeparam>
/// <remarks>
/// </remarks>
public class SparseReferenceTrie<TKey, TKeyBit, TValue>
    : AbstractTrie<TKey, TKeyBit, TValue>
    where TKeyBit : class, IEquatable<TKeyBit>
    where TValue : class
{
    /// <summary>
    /// Create an empty trie with an empty root node.
    /// </summary>
    public SparseReferenceTrie(Func<TKey, IEnumerable<TKeyBit>> keyMapper)
        : base(keyMapper, new SparseReferenceTrieNode<TKeyBit, TValue>(null, default)) { }

    /// <summary>
    /// Method which creates a new child node
    /// </summary>
    /// <param name="key">Key Bit</param>
    /// <returns></returns>
    protected override ITrieNode<TKeyBit, TValue> CreateRoot(TKeyBit key)
    {
        return new SparseReferenceTrieNode<TKeyBit, TValue>(null, key);
    }
}

/// <summary>
/// Sparse Implementation of a Trie data structure optimized for the common case of the key bits being characters
/// </summary>
/// <typeparam name="TKey">Type of keys</typeparam>
/// <typeparam name="TValue">Type of values</typeparam>
public class SparseCharacterTrie<TKey, TValue>
    : AbstractTrie<TKey, char, TValue>
    where TValue : class
{
    /// <summary>
    /// Creates a new sparse character trie
    /// </summary>
    /// <param name="keyMapper">Key Mapper</param>
    public SparseCharacterTrie(Func<TKey, IEnumerable<char>> keyMapper)
        : base(keyMapper, new SparseCharacterTrieNode<TValue>(null, default)) { }

    /// <summary>
    /// Creates the root node of the trie
    /// </summary>
    /// <param name="key">Key</param>
    /// <returns>Root Node</returns>
    protected override ITrieNode<char, TValue> CreateRoot(char key)
    {
        return new SparseCharacterTrieNode<TValue>(null, key);
    }
}