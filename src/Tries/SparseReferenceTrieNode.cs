using System;

namespace VDS.Common.Tries;

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

    /// <summary>
    /// Gets whether the key matches the singleton
    /// </summary>
    /// <param name="key">Key Bit</param>
    /// <returns>True if it matches, false otherwise</returns>
    protected override bool MatchesSingleton(TKeyBit key)
    {
        return _singleton != null && _singleton.Equals(key);
    }

    /// <summary>
    /// Clears the singleton
    /// </summary>
    protected override void ClearSingleton()
    {
        _singleton = null;
        _singletonNode = null;
    }

    /// <summary>
    /// Creates a new child node
    /// </summary>
    /// <param name="key">Key Bit</param>
    /// <returns>New Child</returns>
    protected override ITrieNode<TKeyBit, TValue> CreateNewChild(TKeyBit key)
    {
        return new SparseReferenceTrieNode<TKeyBit, TValue>(this, key);
    }

    /// <summary>
    /// Gets/Sets the singleton child
    /// </summary>
    protected override ITrieNode<TKeyBit, TValue> SingletonChild
    {
        get => _singletonNode;
        set
        {
            _singleton = value.KeyBit;
            _singletonNode = value;
        }
    }

}