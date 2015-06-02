using System;
using VDS.Common.Collections;

namespace VDS.Common.Filters.Storage
{
    public class SparseArrayStorage
        : IBloomFilterStorage
    {
        private readonly ISparseArray<bool> _bits;

        public SparseArrayStorage(IBloomFilterParameters parameters)
        {
            if (parameters == null) throw new ArgumentNullException("parameters");
            if (parameters.NumberOfBits <= 0) throw new ArgumentException("Number of bits must be > 0", "parameters");
            this._bits = new BlockSparseArray<bool>(parameters.NumberOfBits);
        }
        
        public SparseArrayStorage(ISparseArray<bool> bits)
        {
            if (bits.Length <= 0) throw new ArgumentException("Sparse array must have length > 0", "bits");
            this._bits = bits;
        }

        public bool IsSet(int index)
        {
            return this._bits[index];
        }

        public void Set(int index)
        {
            this._bits[index] = true;
        }
    }
}
