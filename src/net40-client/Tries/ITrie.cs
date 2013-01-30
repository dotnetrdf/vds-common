using System;
using System.Collections.Generic;

namespace VDS.Common.Tries
{
    /// <summary>
    /// Interface for Tries, this is a data structure which maps decomposable keys to values
    /// </summary>
    /// <typeparam name="TKey">Key Type</typeparam>
    /// <typeparam name="TKeyBit">Key Bit Type</typeparam>
    /// <typeparam name="TValue">Value Type</typeparam>
    public interface ITrie<TKey, TKeyBit, TValue>
        where TValue : class
    {
        void Add(TKey key, TValue value);
        void Clear();
        int Count { get; }
        ITrieNode<TKeyBit, TValue> Find(IEnumerable<TKeyBit> bs);
        ITrieNode<TKeyBit, TValue> Find(TKey key);
        ITrieNode<TKeyBit, TValue> Find(TKey key, Func<TKey, IEnumerable<TKeyBit>> keyMapper);
        ITrieNode<TKeyBit, TValue> MoveToNode(TKey key);
        void Remove(TKey key);
        ITrieNode<TKeyBit, TValue> Root { get; }
        TValue this[TKey key] { get; set; }
        bool TryGetValue(TKey key, out TValue value);
        IEnumerable<TValue> Values { get; }
    }
}
