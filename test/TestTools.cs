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

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace VDS.Common
{
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public static class TestTools
    {
        public static void PrintEnumerable<T>(IEnumerable<T> items, string sep)
            where T : class
        {
            var first = true;
            foreach (var item in items)
            {
                if (!first)
                {
                    Console.Write(sep);
                }
                else
                {
                    first = false;
                }
                Console.Write((item != null ? item.ToString() : string.Empty));
            }
        }

        public static void PrintEnumerableStruct<T>(IEnumerable<T> items, string sep)
            where T : struct
        {
            var first = true;
            foreach (var item in items)
            {
                if (!first)
                {
                    Console.Write(sep);
                }
                else
                {
                    first = false;
                }
                Console.Write(item.ToString());
            }
        }

        public static void PrintOrderingComparisonEnumerable<T>(IEnumerable<T> enumerable)
            where T : class
        {
            Console.WriteLine("Ascending Order:");
            PrintEnumerable(enumerable.OrderBy(i => i), ",");
            Console.WriteLine();
            Console.WriteLine("Descending Order:");
            PrintEnumerable(enumerable.OrderByDescending(i => i), ",");
            Console.WriteLine();
        }

        public static void PrintOrderingComparisonEnumerableStruct<T>(IEnumerable<T> enumerable)
            where T : struct
        {
            Console.WriteLine("Ascending Order:");
            PrintEnumerableStruct(enumerable.OrderBy(i => i), ",");
            Console.WriteLine();
            Console.WriteLine("Descending Order:");
            PrintEnumerableStruct(enumerable.OrderByDescending(i => i), ",");
            Console.WriteLine();
        }

        public static IEnumerable<T> AsEnumerable<T>(this T item)
        {
            return new[] { item };
        }
    }
}