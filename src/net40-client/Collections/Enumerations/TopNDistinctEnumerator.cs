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
            this.TopItems = new AVLTree<T, bool>(comparer);
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
            this.TopItems.Clear();
            while (this.InnerEnumerator.MoveNext())
            {
                T item = this.InnerEnumerator.Current;
                if (this.TopItems.ContainsKey(item)) continue;

                this.TopItems.Add(this.InnerEnumerator.Current, true);
                int count = this.TopItems.Root != null ? this.TopItems.Root.Size : 0;
                if (count > this.N) this.TopItems.RemoveAt(count - 1);
            }
            return this.TopItems.Keys.GetEnumerator();
        }
    }
}