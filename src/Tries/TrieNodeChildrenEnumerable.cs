using System;
using System.Collections.Generic;
using System.Linq;

namespace VDS.Common.Tries;

class TrieNodeChildrenEnumerable<TKeyBit, TValue>
    : IEnumerable<ITrieNode<TKeyBit, TValue>>
    where TValue : class
{
    private readonly Dictionary<TKeyBit, ITrieNode<TKeyBit, TValue>> _children;
    private readonly TrieNode<TKeyBit, TValue> _node;

    public TrieNodeChildrenEnumerable(TrieNode<TKeyBit, TValue> node, Dictionary<TKeyBit, ITrieNode<TKeyBit, TValue>> children)
    {
        _node = node ?? throw new ArgumentNullException(nameof(node));
        _children = children ?? throw new ArgumentNullException(nameof(children));
    }

    public IEnumerator<ITrieNode<TKeyBit, TValue>> GetEnumerator()
    {
        try
        {
            _node.EnterReadLock();
            // Take a copy so we can safely enumerate the children even if another thread is modifying the Trie
            return _children.Values.ToList().GetEnumerator();
        }
        finally
        {
            _node.ExitReadLock();
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}