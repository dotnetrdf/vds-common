using System;
using System.Collections.Generic;
using VDS.Common.Filters;

namespace VDS.Common.Collections.Enumerations
{
    /// <summary>
    /// An enumerable that reduces another enumerable by using a bloom filter to give probably distinct results
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    /// <remarks>
    /// All items returned are guaranteed to be distinct (based upon the hash functions used for the filter) but some items may be erroneously omitted as bloom filters can produce false positives
    /// </remarks>
    public class ProbabilisticDistinctEnumerable<T>
        : AbstractWrapperEnumerable<T>
    {
        /// <summary>
        /// Creates a new enumerable
        /// </summary>
        /// <param name="enumerable">Enumerable</param>
        /// <param name="filterFactory">Filter factory function</param>
        public ProbabilisticDistinctEnumerable(IEnumerable<T> enumerable, Func<IBloomFilter<T>> filterFactory)
            : base(enumerable)
        {
            if (filterFactory == null) throw new ArgumentNullException("filterFactory");
            this.FilterFactory = filterFactory;
        }

        /// <summary>
        /// Gets/Sets a factory method that produces a bloom filter to use
        /// </summary>
        private Func<IBloomFilter<T>> FilterFactory { get; set; } 

        /// <summary>
        /// Gets an enumerator
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<T> GetEnumerator()
        {
            return new ProbabilisticDistinctEnumerator<T>(this.InnerEnumerable.GetEnumerator(), this.FilterFactory());
        }
    }
}
