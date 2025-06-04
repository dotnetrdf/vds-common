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

namespace VDS.Common.Collections
{
    /// <summary>
    /// Interface for bounded lists which are extensions to the standard list contract with some additional constraints
    /// </summary>
    /// <remarks>
    /// <para>
    /// The primary constraint on a bounded list is that it is guaranteed to never grow beyond it's configured maximum capacity.  Implementations may internally grow the data structures used to store the elements up to/above that limit using whatever strategy they desire but they <strong>must</strong> never allow the size of the list to grow beyond the maximum capacity.
    /// </para>
    /// <para>
    /// The second constraint on a bounded list is with regards to its behaviour when a user attempts to assert more elements than the list has capacity for.  The behaviour in this regard is declared via the <see cref="OverflowPolicy"/> property, depending on the policy declared different behaviours may occur.
    /// </para>
    /// </remarks>
    /// <typeparam name="T">Element type</typeparam>
    public interface IBoundedList<T> 
        : IList<T>
    {
        /// <summary>
        /// Gets the overflow policy that applies when attempting to add more elements to the list than there is capacity for
        /// </summary>
        BoundedListOverflowPolicy OverflowPolicy { get; }

        /// <summary>
        /// Gets the maximum capacity of the bounded list i.e. the maximum number of elements it holds
        /// </summary>
        int MaxCapacity { get; }
    }
}
