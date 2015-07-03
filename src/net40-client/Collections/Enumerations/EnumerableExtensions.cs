/*
VDS.Common is licensed under the MIT License

Copyright (c) 2012-2015 Robert Vesse

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
using VDS.Common.Comparers;

namespace VDS.Common.Collections.Enumerations
{
    /// <summary>
    /// Provides various extension methods which provide useful capabilities on enumerables
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Skips a given number of items
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="enumerable">Enumerable to operate over</param>
        /// <param name="toSkip">Number of items to skip</param>
        /// <returns></returns>
        public static IEnumerable<T> LongSkip<T>(this IEnumerable<T> enumerable, long toSkip)
        {
            return new LongSkipEnumerable<T>(enumerable, toSkip);
        }

        /// <summary>
        /// Takes a given number of items
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="enumerable">Enumerable to operate over</param>
        /// <param name="toTake">Number of items to take</param>
        /// <returns></returns>
        public static IEnumerable<T> LongTake<T>(this IEnumerable<T> enumerable, long toTake)
        {
            return new LongTakeEnumerable<T>(enumerable, toTake);
        }

        /// <summary>
        /// Adds an item to the enumerable provided it is not already present
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="enumerable">Enumerable to operate over</param>
        /// <param name="item">Item to add if it is not already present</param>
        /// <returns></returns>
        public static IEnumerable<T> AddIfMissing<T>(this IEnumerable<T> enumerable, T item)
        {
            return AddIfMissing(enumerable, EqualityComparer<T>.Default, item);
        }

        /// <summary>
        /// Adds an item to the enumerable provided it is not already present
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="enumerable">Enumerable to operate over</param>
        /// <param name="item">Item to add if it is not already present</param>
        /// <returns></returns>
        public static IEnumerable<T> AddIfMissing<T>(this IEnumerable<T> enumerable, IEqualityComparer<T> equalityComprarer, T item)
        {
            return new AddIfMissingEnumerable<T>(enumerable, equalityComprarer, item);
        }

        /// <summary>
        /// Adds an item if the enumerable is empty
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="enumerable">Enumerable to operate over</param>
        /// <param name="item">Item to add if the enumerable is empty</param>
        /// <returns></returns>
        public static IEnumerable<T> AddIfEmpty<T>(this IEnumerable<T> enumerable, T item)
        {
            return new AddIfEmptyEnumerable<T>(enumerable, item);
        }

        /// <summary>
        /// Returns an enumerable which eliminates adjacent duplicates from the given enumerable
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="enumerable">Enumerable</param>
        /// <returns>Enumerable which removes adjacent duplicates</returns>
        public static IEnumerable<T> Reduced<T>(this IEnumerable<T> enumerable)
        {
            return Reduced(enumerable, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Returns an enumerable which eliminates adjacent duplicates from the given enumerable
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="enumerable">Enumerable</param>
        /// <returns>Enumerable which removes adjacent duplicates</returns>
        public static IEnumerable<T> Reduced<T>(this IEnumerable<T> enumerable, IEqualityComparer<T> equalityComparer)
        {
            return new ReducedEnumerable<T>(enumerable, equalityComparer);
        }

        /// <summary>
        /// Gets the top N distinct items according to a given ordering
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="enumerable">Enumerable to operate over</param>
        /// <param name="n">Number of items desired</param>
        /// <param name="comparer">Comparer used to order items</param>
        /// <returns></returns>
        /// <remarks>
        /// This differs from just doing a standard <code>.OrderBy().Take()</code> in that it does not need to hold the entire enumeration in memory at any time, it only ever holds at most <paramref name="n"/> items.
        /// </remarks>
        public static IEnumerable<T> TopDistinct<T>(this IEnumerable<T> enumerable, long n, IComparer<T> comparer)
        {
            return new TopNDistinctEnumerable<T>(enumerable, comparer, n);
        }

        /// <summary>
        /// Gets the top N distinct items according to the default ordering
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="enumerable">Enumerable to operate over</param>
        /// <param name="n">Number of items desired</param>
        /// <returns></returns>
        /// <remarks>
        /// This differs from just doing a standard <code>.OrderBy().Take()</code> in that it does not need to hold the entire enumeration in memory at any time, it only ever holds at most <paramref name="n"/> items.
        /// </remarks>
        public static IEnumerable<T> TopDistinct<T>(this IEnumerable<T> enumerable, long n)
        {
            return TopDistinct(enumerable, n, Comparer<T>.Default);
        }


        /// <summary>
        /// Gets the top N items according to a given ordering
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="enumerable">Enumerable to operate over</param>
        /// <param name="n">Number of items desired</param>
        /// <param name="comparer">Comparer used to order items</param>
        /// <returns></returns>
        /// <remarks>
        /// This differs from just doing a standard <code>.OrderBy().Take()</code> in that it does not need to hold the entire enumeration in memory at any time, it only ever holds at most <paramref name="n"/> items.
        /// </remarks>
        public static IEnumerable<T> Top<T>(this IEnumerable<T> enumerable, long n, IComparer<T> comparer)
        {
            return new TopNEnumerable<T>(enumerable, comparer, n);
        }

        /// <summary>
        /// Gets the top N items according to the default ordering
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="enumerable">Enumerable to operate over</param>
        /// <param name="n">Number of items desired</param>
        /// <returns></returns>
        /// <remarks>
        /// This differs from just doing a standard <code>.OrderBy().Take()</code> in that it does not need to hold the entire enumeration in memory at any time, it only ever holds at most <paramref name="n"/> items.
        /// </remarks>
        public static IEnumerable<T> Top<T>(this IEnumerable<T> enumerable, long n)
        {
            return Top(enumerable, n, Comparer<T>.Default);
        }

        /// <summary>
        /// Gets the bottom N items distinct according to a given ordering
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="enumerable">Enumerable to operate over</param>
        /// <param name="n">Number of items desired</param>
        /// <param name="comparer">Comparer used to order items</param>
        /// <returns></returns>
        /// <remarks>
        /// This differs from just doing a standard <code>.OrderByDescending().Take()</code> in that it does not need to hold the entire enumeration in memory at any time, it only ever holds at most <paramref name="n"/> items.
        /// </remarks>
        public static IEnumerable<T> BottomDistinct<T>(this IEnumerable<T> enumerable, long n, IComparer<T> comparer)
        {
            return new TopNDistinctEnumerable<T>(enumerable, new ReversedComparer<T>(comparer), n);
        }

        /// <summary>
        /// Gets the bottom N items distinct according to the default ordering
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="enumerable">Enumerable to operate over</param>
        /// <param name="n">Number of items desired</param>
        /// <returns></returns>
        /// <remarks>
        /// This differs from just doing a standard <code>.OrderByDescending().Take()</code> in that it does not need to hold the entire enumeration in memory at any time, it only ever holds at most <paramref name="n"/> items.
        /// </remarks>
        public static IEnumerable<T> BottomDistinct<T>(this IEnumerable<T> enumerable, long n)
        {
            return BottomDistinct(enumerable, n, Comparer<T>.Default);
        }

        /// <summary>
        /// Gets the bottom N items according to a given ordering
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="enumerable">Enumerable to operate over</param>
        /// <param name="n">Number of items desired</param>
        /// <param name="comparer">Comparer used to order items</param>
        /// <returns></returns>
        /// <remarks>
        /// This differs from just doing a standard <code>.OrderByDescending().Take()</code> in that it does not need to hold the entire enumeration in memory at any time, it only ever holds at most <paramref name="n"/> items.
        /// </remarks>
        public static IEnumerable<T> Bottom<T>(this IEnumerable<T> enumerable, long n, IComparer<T> comparer)
        {
            return new TopNEnumerable<T>(enumerable, new ReversedComparer<T>(comparer), n);
        }

        /// <summary>
        /// Gets the bottom N items according to a given ordering
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="enumerable">Enumerable to operate over</param>
        /// <param name="n">Number of items desired</param>
        /// <returns></returns>
        /// <remarks>
        /// This differs from just doing a standard <code>.OrderByDescending().Take()</code> in that it does not need to hold the entire enumeration in memory at any time, it only ever holds at most <paramref name="n"/> items.
        /// </remarks>
        public static IEnumerable<T> Bottom<T>(this IEnumerable<T> enumerable, long n)
        {
            return Bottom(enumerable, n, Comparer<T>.Default);
        }
    }
}
