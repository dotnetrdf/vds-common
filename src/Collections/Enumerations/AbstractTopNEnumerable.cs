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
    /// Abstract implementation of a Top N enumerable
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    public abstract class AbstractTopNEnumerable<T> 
        : AbstractOrderedEnumerable<T> 
    {
        /// <summary>
        /// Creates a new enumerable
        /// </summary>
        /// <param name="enumerable">Enumerable to operate over</param>
        /// <param name="comparer">Comparer to use for ordering</param>
        /// <param name="n">Number of items to return</param>
        protected AbstractTopNEnumerable(IEnumerable<T> enumerable, IComparer<T> comparer, long n)
            : base(enumerable, comparer)
        {
            if (n < 1) throw new ArgumentException("N must be >= 1", nameof(n));
            N = n;
        }

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