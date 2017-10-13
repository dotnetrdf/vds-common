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

using System.Collections.Generic;

namespace VDS.Common.Collections.Enumerations
{
    /// <summary>
    /// An enumerator that adds an item if it is not present in the inner enumerator
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AddIfMissingEnumerator<T>
            : AbstractEqualityEnumerator<T>
    {
        /// <summary>
        /// Create a new enumerator
        /// </summary>
        /// <param name="enumerator">Enumerator to operate over</param>
        /// <param name="equalityComparer">Equality Comparer to use</param>
        /// <param name="item">Item to add if missing from inner enumerator</param>
        public AddIfMissingEnumerator(IEnumerator<T> enumerator, IEqualityComparer<T> equalityComparer, T item)
            : base(enumerator, equalityComparer)
        {
            this.AdditionalItem = item;
            this.AdditionalItemSeen = false;
        }

        /// <summary>
        /// Gets/Sets whether the item to be added was seen in the inner enumerator
        /// </summary>
        private bool AdditionalItemSeen { get; set; }

        /// <summary>
        /// Gets/Sets whether we are currently returning the 
        /// </summary>
        private bool IsCurrentAdditionalItem { get; set; }

        /// <summary>
        /// Gets/Sets the additional item
        /// </summary>
        private T AdditionalItem { get; set; }

        /// <summary>
        /// Tries to move next
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns></returns>
        protected override bool TryMoveNext(out T item)
        {
            item = default(T);
            if (this.InnerEnumerator.MoveNext())
            {
                item = this.InnerEnumerator.Current;
                if (this.EqualityComparer.Equals(item, this.AdditionalItem)) this.AdditionalItemSeen = true;
                return true;
            }
            if (this.AdditionalItemSeen) return false;
            if (this.IsCurrentAdditionalItem) return false;

            item = this.AdditionalItem;
            this.IsCurrentAdditionalItem = true;
            return true;
        }

        /// <summary>
        /// Resets the enumerator
        /// </summary>
        protected override void ResetInternal()
        {
            this.AdditionalItemSeen = false;
            this.IsCurrentAdditionalItem = false;
        }
    }
}
