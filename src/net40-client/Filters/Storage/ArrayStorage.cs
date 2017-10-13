/*
VDS.Common is licensed under the MIT License

Copyright (c) 2012-2015 Robert Vesse
Copyright (c) 2016-2017 dotNetRDF Project (http://dotnetrdf.org/)

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

namespace VDS.Common.Filters.Storage
{
    /// <summary>
    /// Array based storage for Bloom Filters
    /// </summary>
    public class ArrayStorage
        : IBloomFilterStorage
    {
        private readonly bool[] _bits;

        /// <summary>
        /// Creates new array storage
        /// </summary>
        /// <param name="size">Size in bits of the storage</param>
        public ArrayStorage(int size)
        {
            if (size <= 0) throw new ArgumentException("Size must be > 0", "size");
            this._bits = new bool[size];
        }

        /// <summary>
        /// Gets whether a given bit is set
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>True if set, false otherwise</returns>
        public bool IsSet(int index)
        {
            return this._bits[index];
        }

        /// <summary>
        /// Sets a given bit
        /// </summary>
        /// <param name="index">Index</param>
        public void Set(int index)
        {
            this._bits[index] = true;
        }

        /// <summary>
        /// Clears the storage
        /// </summary>
        public void Clear()
        {
            Array.Clear(this._bits, 0, this._bits.Length);
        }
    }
}
