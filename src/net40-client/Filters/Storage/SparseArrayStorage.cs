/*
VDS.Common is licensed under the MIT License

Copyright (c) 2012-2015 Robert Vesse

Permission is hereby granted, free of charge, to any person obtaining a copy of this software
and associated documentation files (the "Software"), to deal in the Software without restriction,
including without limitation the rights to use, copy, modify, merge, publish, distribute,
sublicense, and/or sell copies of the Software, and to permit persons to whom the Software 
is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or
substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

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
