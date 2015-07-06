namespace VDS.Common.Trees
{
    /// <summary>
    /// Interface for binary trees
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    public interface IBinaryTree<TKey, TValue>
        : IIndexAccessTree<IBinaryTreeNode<TKey, TValue>, TKey, TValue>
    {
        
    }
}