using System;
using System.Collections.Generic;
using VDS.Common.Filters;

namespace VDS.Common.Collections.Enumerations
{
    /// <summary>
    /// An enumerator that reduces another enumerator by using a bloom filter to give probably distinct results
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// All items returned are guaranteed to be distinct (based upon the hash functions used for the filter) but some items may be erroneously omitted as bloom filters can produce false positives
    /// </remarks>
    public class ProbabilisticDistinctEnumerator<T>
        : AbstractWrapperEnumerator<T>
    {
        /// <summary>
        /// Creates a new enumerator
        /// </summary>
        /// <param name="enumerator">Enumerator to operate over</param>
        /// <param name="filter">Bloom filter to use for distinctness</param>
        public ProbabilisticDistinctEnumerator(IEnumerator<T> enumerator, IBloomFilter<T> filter)
            : base(enumerator)
        {
            if (filter == null) throw new ArgumentNullException("filter");
            this.Filter = filter;
        }

        /// <summary>
        /// Gets/Sets the bloom filter to use
        /// </summary>
        private IBloomFilter<T> Filter { get; set; } 

        /// <summary>
        /// Tries to move next
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns></returns>
        protected override bool TryMoveNext(out T item)
        {
            item = default(T);
            while (this.InnerEnumerator.MoveNext())
            {
                item = this.InnerEnumerator.Current;
                if (this.Filter.Add(item)) return true;
            }
            return false;
        }

        protected override void ResetInternal()
        {
            this.Filter.Clear();
        }
    }
}
