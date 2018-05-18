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

using System.Collections.Generic;

namespace VDS.Common.Collections.Enumerations
{
    /// <summary>
    /// Gets an enumerator that reduces another enumerator by eliminating adjacent duplicates
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    public class ReducedEnumerator<T>
        : AbstractEqualityEnumerator<T>
    {
        /// <summary>
        /// Creates a new enumerator
        /// </summary>
        /// <param name="enumerator">Enumerator to operate over</param>
        /// <param name="equalityComparer">Equality comparer to use</param>
        public ReducedEnumerator(IEnumerator<T> enumerator, IEqualityComparer<T> equalityComparer)
            : base(enumerator, equalityComparer)
        {
            this.First = true;
        }

        /// <summary>
        /// Gets/Sets whether we are at the first item
        /// </summary>
        private bool First { get; set; }

        /// <summary>
        /// Gets the last item seen
        /// </summary>
        private T LastItem { get; set; }

        /// <summary>
        /// Tries to move next
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns></returns>
        protected override bool TryMoveNext(out T item)
        {
            item = default(T);
            if (this.InnerEnumerator.MoveNext()) return false;
            item = this.InnerEnumerator.Current;

            if (this.First)
            {
                this.First = false;
                this.LastItem = item;
                return true;
            }

            while (true)
            {
                // Provided the next item is not the same as the previous return it
                if (!this.EqualityComparer.Equals(this.LastItem, item))
                {
                    this.LastItem = item;
                    return true;
                }

                if (!this.InnerEnumerator.MoveNext()) return false;
                item = this.InnerEnumerator.Current;
            }
        }

        /// <summary>
        /// Resets the 
        /// </summary>
        protected override void ResetInternal()
        {
            this.First = true;
            this.LastItem = default(T);
        }
    }
}