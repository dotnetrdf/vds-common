namespace VDS.Common.Filters
{
    public interface IBloomFilterParameters {
        /// <summary>
        /// Gets the number of bits used for the filter
        /// </summary>
        int NumberOfBits { get; }

        /// <summary>
        /// Gets the number of hash functions used for the filter
        /// </summary>
        int NumberOfHashFunctions { get; }
    }
}