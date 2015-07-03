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

        public void Clear()
        {
            this._storage.Clear();
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
