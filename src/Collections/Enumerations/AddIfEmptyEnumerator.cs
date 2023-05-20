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
    /// An enumerator that adds an item if the inner enumerator is empty
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    public class AddIfEmptyEnumerator<T>
        : AbstractWrapperEnumerator<T>
    {
        /// <summary>
        /// Creates a new enumerator
        /// </summary>
        /// <param name="enumerator">Enumerator</param>
        /// <param name="item">Item to add</param>
        public AddIfEmptyEnumerator(IEnumerator<T> enumerator, T item)
            : base(enumerator)
        {
            this.AdditionalItem = item;
        }

        /// <summary>
        /// Gets the item to be added
        /// </summary>
        private T AdditionalItem { get; set; }

        /// <summary>
        /// Gets/Sets whether any items were seen
        /// </summary>
        private bool AnyItemsSeen { get; set; }

        /// <summary>
        /// Gets whether we are currently returning the additional item
        /// </summary>
        private bool IsCurrentAdditionalItem { get; set; }

        /// <summary>
        /// Tries to move next
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>True if an item is available, false otherwise</returns>
        protected override bool TryMoveNext(out T item)
        {
            item = default;
            if (this.InnerEnumerator.MoveNext())
            {
                this.AnyItemsSeen = true;
                item = this.InnerEnumerator.Current;
                return true;
            }
            if (this.AnyItemsSeen) return false;
            if (this.IsCurrentAdditionalItem) return false;

            this.IsCurrentAdditionalItem = true;
            item = this.AdditionalItem;
            return true;
        }

        /// <summary>
        /// Resets the enumerator
        /// </summary>
        protected override void ResetInternal()
        {
            this.AnyItemsSeen = false;
            this.IsCurrentAdditionalItem = false;
        }
    }
}