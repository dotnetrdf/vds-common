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

namespace VDS.Common.Tries
{
    /// <summary>
    /// Represents the classic use case of a Trie data structure, keys are strings with a character stored at each Node
    /// </summary>
    /// <typeparam name="T">Type of values to be stored</typeparam>
    public class StringTrie<T>
        : Trie<string, char, T>
        where T : class
    {
        /// <summary>
        /// Creates a new String Trie
        /// </summary>
        public StringTrie()
            : base(StringTrie<T>.KeyMapper) { }

        /// <summary>
        /// Key Mapper function for String Trie
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Array of characters</returns>
        public static IEnumerable<char> KeyMapper(string key)
        {
            return key.ToCharArray();
        }
    }

    /// <summary>
    /// Represents the classic use case of a Trie data structure, keys are strings with a character stored at each Node
    /// </summary>
    /// <typeparam name="T">Type of values to be stored</typeparam>
    /// <remarks>
    /// This is a sparse implementation so should be more memory efficient than the <see cref="StringTrie{T}"/> for many use cases
    /// </remarks>
    public class SparseStringTrie<T>
        : SparseCharacterTrie<string, T>
        where T : class
    {
        /// <summary>
        /// Creates a new sparse String Trie
        /// </summary>
        public SparseStringTrie()
            : base(StringTrie<T>.KeyMapper) { }
    }
}
