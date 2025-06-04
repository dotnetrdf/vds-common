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

namespace VDS.Common.Collections.Enumerations;

/// <summary>
/// An enumerable that adds the given item if it is not seen in the inner enumerable
/// </summary>
/// <typeparam name="T">Item type</typeparam>
public class AddIfMissingEnumerable<T>
    : AbstractEqualityEnumerable<T>
{
    /// <summary>
    /// Creates a new enumerable
    /// </summary>
    /// <param name="enumerable">Enumerable to operate over</param>
    /// <param name="equalityComparer">Equality comparer to use</param>
    /// <param name="item">Item to add if not present in inner enumerable</param>
    public AddIfMissingEnumerable(IEnumerable<T> enumerable, IEqualityComparer<T> equalityComparer, T item)
        : base(enumerable, equalityComparer)
    {
        AdditionalItem = item;
    }

    /// <summary>
    /// Gets the additional item
    /// </summary>
    private T AdditionalItem { get; set; }

    /// <summary>
    /// Gets an enumerator
    /// </summary>
    /// <returns></returns>
    public override IEnumerator<T> GetEnumerator()
    {
        return new AddIfMissingEnumerator<T>(InnerEnumerable.GetEnumerator(), EqualityComparer, AdditionalItem);
    }
}