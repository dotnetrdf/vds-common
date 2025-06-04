using System.Collections.Generic;
using System.Linq;

namespace VDS.Common.Collections;

/// <summary>
/// A version of <see cref="ImmutableView{T}"/> where the enumerable is materialized as a list internally
/// </summary>
/// <typeparam name="T">Type</typeparam>
public class MaterializedImmutableView<T>
    : ImmutableView<T>
{
    /// <summary>
    /// Creates a new immutable view over an empty collection
    /// </summary>
    public MaterializedImmutableView()
        : base(new List<T>()) { }

    /// <summary>
    /// Creates a new immutable view over an empty collection
    /// </summary>
    /// <param name="message">Error message to throw when mutation actions are attempted</param>
    public MaterializedImmutableView(string message)
        : base(new List<T>(), message) { }

    /// <summary>
    /// Creates a new immutable view
    /// </summary>
    /// <param name="items">Enumerable to provide view over</param>
    /// <param name="message">Error message to throw when mutation actions are attempted</param>
    public MaterializedImmutableView(IEnumerable<T> items, string message)
        : base(items.ToList(), message) { }

    /// <summary>
    /// Creates a new immutable view
    /// </summary>
    /// <param name="items">Enumerable to provide view over</param>
    public MaterializedImmutableView(IEnumerable<T> items)
        : base(items.ToList()) { }

    /// <summary>
    /// Gets the count of items in the collection
    /// </summary>
    public override int Count => ((IList<T>)Items).Count;

    /// <summary>
    /// Checks whether the collection contains a given item
    /// </summary>
    /// <param name="item">Item</param>
    /// <returns>True if the item is contained in the collection, false otherwise</returns>
    public override bool Contains(T item)
    {
        return ((IList<T>) Items).Contains(item);
    }
}