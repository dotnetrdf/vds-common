using System;

namespace VDS.Common.Filters.Storage
{
    public class ArrayStorage
        : IBloomFilterStorage
    {
        private readonly bool[] _bits;

        public ArrayStorage(int size)
        {
            if (size <= 0) throw new ArgumentException("Size must be > 0", "size");
            this._bits = new bool[size];
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
