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

namespace VDS.Common.Comparers
{
    /// <summary>
    /// A comparer that reverses the ordering provided by another comparer
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    public class ReversedComparer<T>
        : IComparer<T>
    {
        /// <summary>
        /// Creates a new comparer that reverses the default ordering
        /// </summary>
        public ReversedComparer()
            : this(Comparer<T>.Default) { }

        /// <summary>
        /// Creates a new comparer that reverses the given ordering
        /// </summary>
        /// <param name="comparer">Comparer</param>
        public ReversedComparer(IComparer<T> comparer)
        {
            if (comparer == null) throw new ArgumentNullException("comparer");
            this.InnerComparer = comparer;
        }

        /// <summary>
        /// Gets the inner comparer
        /// </summary>
        public IComparer<T> InnerComparer { get; private set; }

        /// <summary>
        /// Compares two items
        /// </summary>
        /// <param name="x">Item</param>
        /// <param name="y">Other item</param>
        /// <returns></returns>
        public int Compare(T x, T y)
        {
            return this.InnerComparer.Compare(y, x);
        }
    }
}
