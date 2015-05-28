using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VDS.Common.Filters
{
    public class BloomFilterParameters
        : BaseBloomFilterParameters
    {
        private readonly int _numHashFunctions;

        public BloomFilterParameters(int numBits, int numHashFunctions)
        {
            this.NumberOfBits = numBits;
            this._numHashFunctions = numHashFunctions;
        }

        public override int NumberOfHashFunctions { get { return this._numHashFunctions; } }
    }
}
