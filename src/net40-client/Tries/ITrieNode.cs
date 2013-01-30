using System;
using System.Collections.Generic;

namespace VDS.Common.Tries
{
    /// <summary>
    /// Interface for Trie nodes, this is the node in a <see cref="ITrie"/>
    /// </summary>
    /// <typeparam name="TKeyBit">Key Bit Type</typeparam>
    /// <typeparam name="TValue">Value Type</typeparam>
    public interface ITrieNode<TKeyBit, TValue>
        where TValue : class
    {
        IEnumerable<ITrieNode<TKeyBit, TValue>> AllChildren { get; }
        IEnumerable<ITrieNode<TKeyBit, TValue>> Children { get; }
        void Clear();
        void ClearValue();
        int Count { get; }
        int CountAll { get; }
        bool HasValue { get; }
        bool IsLeaf { get; }
        bool IsRoot { get; }
        TKeyBit KeyBit { get; }
        ITrieNode<TKeyBit, TValue> Parent { get; }
        ITrieNode<TKeyBit, TValue> MoveToChild(TKeyBit key);
        bool TryGetChild(TKeyBit key, out ITrieNode<TKeyBit, TValue> child);
        void RemoveChild(TKeyBit key);
        void Trim();
        void Trim(int depth);
        TValue Value { get; set; }
        IEnumerable<TValue> Values { get; }
    }
}
