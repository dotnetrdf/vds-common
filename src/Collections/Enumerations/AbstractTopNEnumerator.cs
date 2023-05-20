/*
VDS.Common is licensed under the MIT License

Copyright (c) 2012-2015 Robert Vesse
Copyright (c) 2016-2018 dotNetRDF Project (http://dotnetrdf.org/)

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

namespace VDS.Common.Collections.Enumerations
{
    /// <summary>
    /// Abstract implementation of a Top N enumerator
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    public abstract class AbstractTopNEnumerator<T> 
        : AbstractOrderedEnumerator<T> 
    {
        /// <summary>
        /// Creates a new enumerator
        /// </summary>
        /// <param name="enumerator">Enumerator to operate over</param>
        /// <param name="comparer">Comparer to use for ordering</param>
        /// <param name="n">Number of items to return</param>
        protected AbstractTopNEnumerator(IEnumerator<T> enumerator, IComparer<T> comparer, long n)
            : base(enumerator, comparer)
        {
            if (n < 1) throw new ArgumentException("N must be >= 1", nameof(n));
            this.N = n;
        }

        /// <summary>
        /// Gets the number of items to be returned
        /// </summary>
        public long N { get; private set; }

        /// <summary>
        /// Gets/Sets the Top Items enumerator
        /// </summary>
        private IEnumerator<T> TopItemsEnumerator { get; set; }

        /// <summary>
        /// Tries to move next
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>True if more items, false otherwise</returns>
        /// <remarks>
        /// If this is the first time the enumerator is trying to move next it will build the top items list by consuming the inner enumerator and storing the top N items in a temporary data structure.  Once it has done that it will then return items from that data structure until they are exhausted
        /// </remarks>
        protected override bool TryMoveNext(out T item)
        {
            // First time this is accessed need to populate the Top N items list
            if (this.TopItemsEnumerator == null)
            {
                this.TopItemsEnumerator = this.BuildTopItems();
            }

            // Afterwards we just pull items from that list
            item = default;
            if (!this.TopItemsEnumerator.MoveNext()) return false;
            item = this.TopItemsEnumerator.Current;
            return true;
        }

        /// <summary>
        /// Resets the enumerator
        /// </summary>
        protected override void ResetInternal()
        {
            if (this.TopItemsEnumerator == null) return;
            this.TopItemsEnumerator.Dispose();
            this.TopItemsEnumerator = null;
        }

        /// <summary>
        /// Requests that the top items enumerator be built
        /// </summary>
        /// <returns>Top Items enumerator</returns>
        protected abstract IEnumerator<T> BuildTopItems();
    }
}