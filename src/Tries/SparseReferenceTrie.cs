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