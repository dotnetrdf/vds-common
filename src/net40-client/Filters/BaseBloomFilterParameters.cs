namespace VDS.Common.Filters
{
    public abstract class BaseBloomFilterParameters
        : IBloomFilterParameters
    {
        public int NumberOfBits { get; protected set; }

        public abstract int NumberOfHashFunctions { get; }
    }
}