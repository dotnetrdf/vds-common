/*
VDS.Common is licensed under the MIT License

Copyright (c) 2009-2014 Robert Vesse

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
using System.Linq;
using System.Text;

namespace VDS.Common.Collections
{
    /// <summary>
    /// A key type for testing, allows hash code to be controlled explicitly to force collisions as desired
    /// </summary>
    public class TestKey<T>
        where T : IComparable<T>
    {
        private int _hashCode;

        /// <summary>
        /// Creates a test key
        /// </summary>
        /// <param name="hashCode">Hash Code</param>
        public TestKey(int hashCode, T value)
        {
            if (value == null) throw new ArgumentNullException("value");
            this._hashCode = hashCode;
            this.Value = value;
        }

        /// <summary>
        /// Gets the value associated with the key
        /// </summary>
        public T Value
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash Code</returns>
        public override int GetHashCode()
        {
            return this._hashCode;
        }

        /// <summary>
        /// Gets string representation of the key
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this._hashCode.ToString();
        }

        /// <summary>
        /// Checks whether the key is equal to another key
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return this._hashCode.Equals(obj.GetHashCode());
        }
    }

    public class TestKeyComparer<T>
        : IComparer<TestKey<T>>, IEqualityComparer<TestKey<T>>
        where T : IComparable<T>
    {

        public int Compare(TestKey<T> x, TestKey<T> y)
        {
            if (x == null)
            {
                if (y == null) return 0;
                return -1;
            }
            else if (y == null)
            {
                return 1;
            }
            else
            {
                return x.Value.CompareTo(y.Value);
            }
        }

        public bool Equals(TestKey<T> x, TestKey<T> y)
        {
            return this.Compare(x, y) == 0;
        }

        public int GetHashCode(TestKey<T> obj)
        {
            return obj.GetHashCode();
        }
    }
}
