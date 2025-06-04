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

namespace VDS.Common.Collections.Enumerations
{
    /// <summary>
    /// Abstract implementation of a wrapper enumerator that uses item ordering
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    public abstract class AbstractOrderedEnumerator<T>
        : AbstractWrapperEnumerator<T>
    {
        /// <summary>
        /// Creates a new enumerator
        /// </summary>
        /// <param name="enumerator">Enumerator to wrap</param>
        /// <param name="comparer">Comparer to use</param>
        protected AbstractOrderedEnumerator(IEnumerator<T> enumerator, IComparer<T> comparer)
            : base(enumerator)
        {
            Comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        }

        /// <summary>
        /// Gets the comparer to use for ordering
        /// </summary>
        protected IComparer<T> Comparer { get; private set; } 
    }
}