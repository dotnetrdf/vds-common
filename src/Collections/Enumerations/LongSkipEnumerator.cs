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
    /// An enumerator that skips items
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    public class LongSkipEnumerator<T>
        : AbstractWrapperEnumerator<T>
    {
        /// <summary>
        /// Creates a new enumerator
        /// </summary>
        /// <param name="enumerator">Enumerator to operate over</param>
        /// <param name="toSkip">Number of items to skip</param>
        public LongSkipEnumerator(IEnumerator<T> enumerator, long toSkip)
            : base(enumerator)
        {
            if (toSkip <= 0) throw new ArgumentException("toSkip must be > 0", nameof(toSkip));
            this.ToSkip = toSkip;
            this.Skipped = 0;
        }

        /// <summary>
        /// Gets/Sets the number of items to skip
        /// </summary>
        private long ToSkip { get; set; }

        /// <summary>
        /// Gets/Sets the number of items skipped
        /// </summary>
        private long Skipped { get; set; }

        /// <summary>
        /// Tries to skip the desired number of items
        /// </summary>
        /// <returns></returns>
        private bool TrySkip()
        {
            while (this.Skipped < this.ToSkip)
            {
                if (!this.InnerEnumerator.MoveNext()) return false;
                this.Skipped++;
            }
            return this.Skipped == this.ToSkip;
        }

        /// <summary>
        /// Tries to move next
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns></returns>
        /// <remarks>
        /// The first time this is called it will try to skip the requisite number of items from the inner enumerator, if that succeeds it will then start returning items from the inner enumerator.
        /// </remarks>
        protected override bool TryMoveNext(out T item)
        {
            item = default(T);

            // If we've previously done the skipping so can just defer to inner enumerator
            if (this.Skipped == this.ToSkip)
            {
                if (!this.InnerEnumerator.MoveNext()) return false;
                item = this.InnerEnumerator.Current;
                return true;
            }

            // First time being accessed so attempt to skip if possible
            return this.TrySkip() && this.TryMoveNext(out item);
        }

        /// <summary>
        /// Resets the enumerator
        /// </summary>
        protected override void ResetInternal()
        {
            this.Skipped = 0;
        }
    }
}