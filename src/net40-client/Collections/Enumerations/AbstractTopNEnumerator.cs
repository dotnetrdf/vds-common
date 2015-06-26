using System;
using System.Collections.Generic;

namespace VDS.Common.Collections.Enumerations
{
    public abstract class AbstractTopNEnumerator<T> 
        : WrapperEnumerator<T> 
    {
        public AbstractTopNEnumerator(IEnumerator<T> enumerator, IComparer<T> comparer, long n)
            : base(enumerator)
        {
            if (comparer == null) throw new ArgumentNullException("comparer");
            if (n < 1) throw new ArgumentException("N must be >= 1", "n");
            this.N = n;
        }

        public long N { get; private set; }

        /// <summary>
        /// Gets/Sets the Top Items enumerator
        /// </summary>
        private IEnumerator<T> TopItemsEnumerator { get; set; }

        protected override bool TryMoveNext(out T item)
        {
            // First time this is accessed need to populate the Top N items list
            if (this.TopItemsEnumerator == null)
            {
                this.TopItemsEnumerator = this.BuildTopItems();
            }

            // Afterwards we just pull items from that list
            item = default(T);
            if (!this.TopItemsEnumerator.MoveNext()) return false;
            item = this.TopItemsEnumerator.Current;
            return true;
        }

        protected override void ResetInternal()
        {
            if (this.TopItemsEnumerator != null)
            {
                this.TopItemsEnumerator.Dispose();
                this.TopItemsEnumerator = null;
            }
        }

        /// <summary>
        /// Requests that the top items enumerator be built
        /// </summary>
        /// <returns>Top Items enumerator</returns>
        protected abstract IEnumerator<T> BuildTopItems();
    }
}