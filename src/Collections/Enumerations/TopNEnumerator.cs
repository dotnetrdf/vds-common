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
    /// An enumerator that returns the top N items based on a given ordering
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    public class TopNEnumerator<T>
        : AbstractTopNEnumerator<T>
    {
        /// <summary>
        /// Creates a new enumerator
        /// </summary>
        /// <param name="enumerator">Enumerator to operate over</param>
        /// <param name="comparer">Comparer to use</param>
        /// <param name="n">Number of items to return</param>
        public TopNEnumerator(IEnumerator<T> enumerator, IComparer<T> comparer, long n)
            : base(enumerator, comparer, n)
        {
            TopItems = new DuplicateSortedList<T>(comparer);
        }

        /// <summary>
        /// Stores the top N items
        /// </summary>
        private DuplicateSortedList<T> TopItems { get; set; }

        /// <summary>
        /// Builds the top items
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator<T> BuildTopItems()
        {
            TopItems.Clear();
            while (InnerEnumerator.MoveNext())
            {
                TopItems.Add(InnerEnumerator.Current);
                if (TopItems.Count > N) TopItems.RemoveAt(TopItems.Count - 1);
            }
            return TopItems.GetEnumerator();
        }
    }
}