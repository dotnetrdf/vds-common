/*
VDS.Common is licensed under the MIT License

Copyright (c) 2012-2015 Robert Vesse
Copyright (c) 2016-2025 dotNetRDF Project (https://dotnetrdf.org/)

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
using VDS.Common.Trees;

namespace VDS.Common.Collections.Enumerations
{
    /// <summary>
    /// An enumerator that returns the top N distinct items accoding to some ordering
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    public class TopNDistinctEnumerator<T>
        : AbstractTopNEnumerator<T>
    {
        /// <summary>
        /// Creates a new enumerator
        /// </summary>
        /// <param name="enumerator">Enumerator to operate over</param>
        /// <param name="comparer">Comparer to use</param>
        /// <param name="n">Number of items to return</param>
        public TopNDistinctEnumerator(IEnumerator<T> enumerator, IComparer<T> comparer, long n)
            : base(enumerator, comparer, n)
        {
            TopItems = new AvlTree<T, bool>(comparer);
        }

        /// <summary>
        /// Gets/Sets the tree of stored items
        /// </summary>
        private IBinaryTree<T, bool> TopItems { get; set; }

        /// <summary>
        /// Builds the top items
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator<T> BuildTopItems()
        {
            TopItems.Clear();
            while (InnerEnumerator.MoveNext())
            {
                var item = InnerEnumerator.Current;
                if (TopItems.ContainsKey(item)) continue;

                TopItems.Add(InnerEnumerator.Current, true);
                var count = TopItems.Root?.Size ?? 0;
                if (count > N) TopItems.RemoveAt(count - 1);
            }
            return TopItems.Keys.GetEnumerator();
        }
    }
}