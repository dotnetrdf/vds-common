using System;
using System.Collections.Generic;

namespace VDS.Common.Tries;

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