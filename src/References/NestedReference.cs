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

using System;
using System.Collections.Generic;

namespace VDS.Common.References
{
    /// <summary>
    /// Represents a reference whose value may change based on nesting level
    /// </summary>
    /// <typeparam name="T">Reference Type</typeparam>
    public class NestedReference<T> 
        where T : class
    {
        private readonly Dictionary<int, T> _values = new();
        private int _currLevel;
        private T _currRef;

        /// <summary>
        /// Creates a new Nested Reference with an initial null value
        /// </summary>
        public NestedReference()
        {
            _currLevel++;
        }

        /// <summary>
        /// Creates a new nested reference with an initial value
        /// </summary>
        /// <param name="initValue">Initial Value</param>
        public NestedReference(T initValue)
        {
            _values.Add(_currLevel, initValue);
            _currRef = initValue;
            _currLevel++;
        }

        /// <summary>
        /// Gets/Sets the value based on the current nesting level
        /// </summary>
        public T Value
        {
            get => _currRef;
            set
            {
                _values[_currLevel] = value;
                _currRef = value;
            }
        }

        /// <summary>
        /// Increments the nesting level
        /// </summary>
        public void IncrementNesting()
        {
            _currLevel++;
        }

        /// <summary>
        /// Decrements the nesting level
        /// </summary>
        public void DecrementNesting()
        {
            if (_currLevel == 0) throw new InvalidOperationException("Cannot decrement nesting when current nesting level is 0");

            //Revert to the most recent reference
            if (_values.ContainsKey(_currLevel))
            {
                _values.Remove(_currLevel);
                var i = _currLevel;
                while (i > 1)
                {
                    i--;
                    if (_values.TryGetValue(i, out var value))
                    {
                        _currRef = value;
                        break;
                    }
                    else
                    {
                        _currRef = null;
                    }
                }
            }
            //Finally decrement the level
            _currLevel--;
        }
    }
}
