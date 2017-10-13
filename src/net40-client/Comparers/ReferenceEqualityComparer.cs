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
using System.Runtime.CompilerServices;

namespace VDS.Common.Comparers
{
    /// <summary>
    /// An equality comparer based purely on Object reference
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public sealed class ReferenceEqualityComparer<T>
        : IEqualityComparer<T>
    {
        /// <summary>
        /// Gets whether two items are reference equals
        /// </summary>
        /// <param name="x">Item</param>
        /// <param name="y">Other item</param>
        /// <returns>True if items are reference equals, false otherwise</returns>
        public bool Equals(T x, T y)
        {
            return ReferenceEquals(x, y);
        }

        /// <summary>
        /// Gets a hash code for an item
        /// </summary>
        /// <param name="obj">Item</param>
        /// <returns>Hash code</returns>
        /// <remarks>
        /// Defers to <see cref="RuntimeHelpers.GetHashCode(object)"/>
        /// </remarks>
        public int GetHashCode(T obj)
        {
            return RuntimeHelpers.GetHashCode(obj);
        }
    }
}
