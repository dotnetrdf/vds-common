using System;
using System.Collections.Generic;
using System.Linq;

namespace VDS.Common.Filters
{
    /// <summary>
    /// Abstract implementation of a bloom filter
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    public abstract class BaseBloomFilter<T>
        : BaseBloomFilterParameters, IBloomFilter<T>
    {
        private readonly IBloomFilterStorage _storage;

        protected BaseBloomFilter(IBloomFilterStorage storage)
        {
            if (storage == null) throw new ArgumentNullException("storage", "Storage cannot be null");
            this._storage = storage;
        }

        /// <summary>
        /// Converts an item into a number of bit indexes
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>Bit Indices</returns>
        protected abstract IEnumerable<int> GetBitIndices(T item);

        /// <summary>
        /// Gets whether the filter may have already seen the given item
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>True if the item may have been seen, false otherwise</returns>
        /// <remarks>
        /// Bloom filters may return false positives hence why this method is named <strong>MayContain</strong> but they are guaranteed to never return false negatives
        /// </remarks>
        public bool MayContain(T item)
        {
            IEnumerable<int> indices = this.GetBitIndices(item);
            return indices.All(index => this._storage.IsSet(index));
        }

        /// <summary>
        /// Adds an item to the filter
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>True if the item was added to the filter, false if item may already have been present and was not added</returns>
        public bool Add(T item)
        {
            IEnumerable<int> indices = this.GetBitIndices(item);
            bool alreadySeen = true;
            foreach (int index in indices)
            {
                if (this._storage.IsSet(index)) continue;
                alreadySeen = false;
                this._storage.Set(index);
            }
            return !alreadySeen;
        }
    }
}
