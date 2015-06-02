using System;

namespace VDS.Common.Filters
{
    /// <summary>
    /// Interface for bloom filter storage
    /// </summary>
    public interface IBloomFilterStorage
    {
        /// <summary>
        /// Gets whether the given bit is set
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>True if set, false otherwise</returns>
        bool IsSet(int index);

        /// <summary>
        /// Sets the given bit
        /// </summary>
        /// <param name="index">Index</param>
        void Set(int index);
    }
}