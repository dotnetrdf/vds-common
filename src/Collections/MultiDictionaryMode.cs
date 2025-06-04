namespace VDS.Common.Collections;

/// <summary>
/// Possible modes to use for the binary search tree based buckets of the <see cref="MultiDictionary{TKey,TValue}"/>
/// </summary>
public enum MultiDictionaryMode
{
    /// <summary>
    /// Use unbalanced trees, best when you expect minimal key collisions and are willing to trade faster insert performance for slower lookup performance
    /// </summary>
    Unbalanced,
    /// <summary>
    /// Use Scapegoat trees, good when there are a few key collisions and key comparisons are inexpensive.  Provides amortized O(log n) performance but ocassional operations may be O(n)
    /// </summary>
    Scapegoat,
    /// <summary>
    /// Use AVL trees, likely gives the best overall performance
    /// </summary>
    Avl   
}