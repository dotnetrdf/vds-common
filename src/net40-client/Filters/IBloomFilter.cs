namespace VDS.Common.Filters
{
    /// <summary>
    /// Interface for bloom filters which are a probabilistic data structure for detecting whether items have already been seen
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    /// <remarks>
    /// A bloom filter is a data structure that can be used to determine whether an item may have previously been seen in a memory efficient way that does not require storing the actual items.  The trade off is that it may yield false positives however it is guaranteed to never yield false negatives.  This makes it an ideal data structure for implementings things like distinctness where it doesn't matter if a few non-distinct items are discarded.
    /// </remarks>
    public interface IBloomFilter<T>
    {
        /// <summary>
        /// Gets the number of bits used for the filter
        /// </summary>
        int NumberOfBits { get; }

        /// <summary>
        /// Gets the number of hash functions used for the filter
        /// </summary>
        int NumberOfHashFunctions { get; }

        /// <summary>
        /// Adds an item to the filter
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>True if the item was added to the filter, false if item may already have been present and was not added</returns>
        bool Add(T item);

        /// <summary>
        /// Gets whether the filter may have already seen the given item
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>True if the item may have been seen, false otherwise</returns>
        /// <remarks>
        /// Bloom filters may return false positives hence why this method is named <strong>MayContain</strong> but they are guaranteed to never return false negatives
        /// </remarks>
        bool MayContain(T item);
    }
}
