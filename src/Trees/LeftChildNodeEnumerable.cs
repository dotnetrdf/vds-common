namespace VDS.Common.Trees;

/// <summary>
/// An enumerable over the left child of a binary tree node which ensures that each time it is enumerated the latest state of the tree is enumerated
/// </summary>
/// <typeparam name="TKey">Key type</typeparam>
/// <typeparam name="TValue">Value type</typeparam>
internal class LeftChildNodeEnumerable<TKey, TValue>
    : ChildNodesEnumerable<TKey, TValue>
{
    /// <summary>
    /// Creates a new enumerable
    /// </summary>
    /// <param name="parent">Parent node</param>
    public LeftChildNodeEnumerable(IBinaryTreeNode<TKey, TValue> parent)
        : base(parent) { }

    /// <summary>
    /// Gets the left child
    /// </summary>
    protected override IBinaryTreeNode<TKey, TValue> Child => Parent.LeftChild;
}