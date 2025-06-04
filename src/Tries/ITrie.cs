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
/// Interface for Tries, this is a data structure which maps decomposable keys to values
/// </summary>
/// <typeparam name="TKey">Key Type</typeparam>
/// <typeparam name="TKeyBit">Key Bit Type</typeparam>
/// <typeparam name="TValue">Value Type</typeparam>
public interface ITrie<TKey, TKeyBit, TValue>
    where TValue : class
{
    /// <summary>
    /// Adds a new key value pair, overwriting the existing value if the given key is already in use
    /// </summary>
    /// <param name="key">Key to search for value by</param>
    /// <param name="value">Value associated with key</param>
    void Add(TKey key, TValue value);

    /// <summary>
    /// Clears the Trie
    /// </summary>
    void Clear();

    /// <summary>
    /// Gets whether the Trie contains the given key value pair
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="value">Value</param>
    /// <returns>True if the key value pair exists, false otherwise</returns>
    bool Contains(TKey key, TValue value);

    /// <summary>
    /// Gets whether the Trie contains a specific key and has a value associated with that key
    /// </summary>
    /// <param name="key">Key</param>
    /// <returns>True if the Trie contains a specific key and has a value associated with it, false otherwise</returns>
    bool ContainsKey(TKey key);

    /// <summary>
    /// Gets whether the Trie contains a specific key
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="requireValue">Whether the key is required to have a value associated with it in order to be considered as being contained</param>
    /// <returns>True if the Trie contains the given key and meets the value requirement</returns>
    bool ContainsKey(TKey key, bool requireValue);

    /// <summary>
    /// Gets the Count of all Nodes in the Trie
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Finds and returns a Node for the given sequence of Key Bits
    /// </summary>
    /// <param name="bs">Key Bits</param>
    /// <returns>Null if the Key does not map to a Node</returns>
    /// <remarks>
    /// The ability to provide a specific seqeunce of key bits may be useful for custom lookups where you don't necessarily have a value of the <strong>TKey</strong> type but do have values of the <strong>TKeyBit</strong> type
    /// </remarks>
    ITrieNode<TKeyBit, TValue> Find(IEnumerable<TKeyBit> bs);

    /// <summary>
    /// Finds and returns a Node for the given Key
    /// </summary>
    /// <param name="key">Key</param>
    /// <returns>Null if the Key does not map to a Node</returns>
    ITrieNode<TKeyBit, TValue> Find(TKey key);

    /// <summary>
    /// Finds and returns a Node for the given Key using the given Key to Key Bit mapping function
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="keyMapper">Function to map keys to key bits</param>
    /// <returns>Null if the Key does not map to a Node</returns>
    /// <remarks>
    /// The ability to provide a custom mapping function allows you to do custom lookups into the Trie.  For example you might want to only match some portion of the key rather than the entire key
    /// </remarks>
    ITrieNode<TKeyBit, TValue> Find(TKey key, Func<TKey, IEnumerable<TKeyBit>> keyMapper);

    /// <summary>
    /// Finds and returns a Node that has the longest prefix match for the given sequence of Key Bits
    /// </summary>
    /// <param name="bs">Key Bits</param>
    /// <returns>Null if the Key does not map to a Node</returns>
    /// <remarks>
    /// The ability to provide a specific seqeunce of key bits may be useful for custom lookups where you don't necessarily have a value of the <strong>TKey</strong> type but do have values of the <strong>TKeyBit</strong> type
    /// </remarks>
    ITrieNode<TKeyBit, TValue> FindPredecessor(IEnumerable<TKeyBit> bs);

    /// <summary>
    /// Finds and returns a Node that has the longest prefix match for the given Key
    /// </summary>
    /// <param name="key">Key</param>
    /// <returns>Null if the Key does not map to a Node</returns>
    ITrieNode<TKeyBit, TValue> FindPredecessor(TKey key);

    /// <summary>
    /// Finds and returns a Node that has the longest prefix match for the given Key using the given Key to Key Bit mapping function
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="keyMapper">Function to map keys to key bits</param>
    /// <returns>Null if the Key does not map to a Node</returns>
    /// <remarks>
    /// The ability to provide a custom mapping function allows you to do custom lookups into the Trie.  For example you might want to only match some portion of the key rather than the entire key
    /// </remarks>
    ITrieNode<TKeyBit, TValue> FindPredecessor(TKey key, Func<TKey, IEnumerable<TKeyBit>> keyMapper);

    /// <summary>
    /// Finds and returns a Node that has the shortest prefix match greater than or equal to the given Key
    /// </summary>
    /// <param name="key">Key</param>
    /// <returns>Null if the Key does not map to a Node</returns>
    ITrieNode<TKeyBit, TValue> FindSuccessor(TKey key);
        
    /// <summary>
    /// Finds and returns a Node that has the shortest prefix match greater than or equal to the given Key using the given Key to Key Bit mapping function
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="keyMapper">Function to map keys to key bits</param>
    /// <returns>Null if the Key does not map to a Node</returns>
    /// <remarks>
    /// The ability to provide a custom mapping function allows you to do custom lookups into the Trie.  For example you might want to only match some portion of the key rather than the entire key
    /// </remarks>
    ITrieNode<TKeyBit, TValue> FindSuccessor(TKey key, Func<TKey, IEnumerable<TKeyBit>> keyMapper);

    /// <summary>
    /// Finds and returns a Node that has the shortest prefix match greater than or equal to the given sequence of Key Bits
    /// </summary>
    /// <param name="bs">Key Bits</param>
    /// <returns>Null if the Key does not map to a Node</returns>
    /// <remarks>
    /// The ability to provide a specific seqeunce of key bits may be useful for custom lookups where you don't necessarily have a value of the <strong>TKey</strong> type but do have values of the <strong>TKeyBit</strong> type
    /// </remarks>
    ITrieNode<TKeyBit, TValue> FindSuccessor(IEnumerable<TKeyBit> bs);

    /// <summary>
    /// Moves to the Node associated with the given Key creating new nodes if necessary
    /// </summary>
    /// <param name="key">Key</param>
    /// <returns>Trie Node</returns>
    ITrieNode<TKeyBit, TValue> MoveToNode(TKey key);

    /// <summary>
    /// Remove the value that a key leads to and any redundant nodes which result from this action
    /// </summary>
    /// <param name="key">Key of the value to remove</param>
    void Remove(TKey key);

    /// <summary>
    /// Gets the Root Node of the Trie
    /// </summary>
    ITrieNode<TKeyBit, TValue> Root { get; }

    /// <summary>
    /// Gets/Sets the Value associated with a given Key
    /// </summary>
    /// <param name="key">Key</param>
    /// <returns>Value associated with the given Key, may be null if no value is associated</returns>
    /// <exception cref="KeyNotFoundException">Thrown if you try to get a value for a key that is not in the Trie</exception>
    TValue this[TKey key] { get; set; }

    /// <summary>
    /// Tries to get the Value associated with a given Key
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="value">Value, may be null if the key exists but has no value associated with it</param>
    /// <returns>True if the Key exists in the Trie, False if it does not</returns>
    bool TryGetValue(TKey key, out TValue value);

    /// <summary>
    /// Gets all the Values in the Trie
    /// </summary>
    IEnumerable<TValue> Values { get; }
}