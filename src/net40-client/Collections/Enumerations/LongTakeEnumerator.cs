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
using System.Collections.Generic;

namespace VDS.Common.Collections.Enumerations
{
    /// <summary>
    /// An enumerator that takes some number of items
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    public class LongTakeEnumerator<T>
        : AbstractWrapperEnumerator<T>
    {
        /// <summary>
        /// Creates a new enumerator
        /// </summary>
        /// <param name="enumerator">Enumerator to operate over</param>
        /// <param name="toTake">Number of items to take</param>
        public LongTakeEnumerator(IEnumerator<T> enumerator, long toTake)
            : base(enumerator)
        {
            if (toTake <= 0) throw new ArgumentException("toTake must be > 0", "toTake");
            this.ToTake = toTake;
            this.Taken = 0;
        }

        /// <summary>
        /// Gets/Sets the number of items to take
        /// </summary>
        private long ToTake { get; set; }

        /// <summary>
        /// Gets/Sets the number of items taken
        /// </summary>
        private long Taken { get; set; }

        /// <summary>
        /// Tries to move next
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns></returns>
        /// <remarks>
        /// While the number of items seen is less than the desired number of items the inner enumerator is accessed normally, once that is reached no further items are returned
        /// </remarks>
        protected override bool TryMoveNext(out T item)
        {
            item = default(T);
            if (this.Taken >= this.ToTake) return false;
            if (!this.InnerEnumerator.MoveNext()) return false;

            this.Taken++;
            item = this.InnerEnumerator.Current;
            return true;
        }

        /// <summary>
        /// Resets the enumerator
        /// </summary>
        protected override void ResetInternal()
        {
            this.Taken = 0;
        }
    }
}