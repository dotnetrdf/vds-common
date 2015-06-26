using System;
using System.Collections.Generic;

namespace VDS.Common.Collections.Enumerations
{
    public abstract class AbstractTopNEnumerable<T> 
        : WrapperEnumerable<T> 
    {
        protected AbstractTopNEnumerable(IEnumerable<T> enumerable, IComparer<T> comparer, long n)
            : base(enumerable)
        {
            if (comparer == null) throw new ArgumentNullException("comparer");
            this.Comparer = comparer;
            if (n < 1) throw new ArgumentException("N must be >= 1", "n");
            this.N = n;
        }

        /// <summary>
        /// Gets the comparer to use
        /// </summary>
        public IComparer<T> Comparer { get; private set; }

        /// <summary>
        /// Gets the maximum number of items to be yielded
        /// </summary>
        public long N { get; private set; }

        /// <summary>
        /// Gets an enumerator
        /// </summary>
        /// <returns>Enumerator</returns>
        public abstract override IEnumerator<T> GetEnumerator();
    }
}