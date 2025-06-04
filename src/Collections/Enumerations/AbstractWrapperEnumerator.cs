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
using System.Collections;
using System.Collections.Generic;

namespace VDS.Common.Collections.Enumerations
{
    /// <summary>
    /// Abstract implementation of an enumerator that wraps another enumerator
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    public abstract class AbstractWrapperEnumerator<T>
        : IEnumerator<T>
    {
        private T _current;

        /// <summary>
        /// Creates a new enumerator
        /// </summary>
        /// <param name="enumerator">Enumerator to wrap</param>
        protected AbstractWrapperEnumerator(IEnumerator<T> enumerator)
        {
            InnerEnumerator = enumerator ?? throw new ArgumentNullException(nameof(enumerator));
        }

        /// <summary>
        /// Gets the wrapped enumerator
        /// </summary>
        protected IEnumerator<T> InnerEnumerator { get; private set; }

        /// <summary>
        /// Disposes of this enumerator
        /// </summary>
        public virtual void Dispose()
        {
            InnerEnumerator.Dispose();
        }

        /// <summary>
        /// Gets/Sets whether the enumeration has started
        /// </summary>
        private bool Started { get; set; }

        /// <summary>
        /// Gets/Sets whether the enumeration has finished
        /// </summary>
        private bool Finished { get; set; }

        /// <summary>
        /// Gets whether the enumerator can move to the next item
        /// </summary>
        /// <returns>True if another item is available, false otherwise</returns>
        /// <remarks>
        /// This differs to the abstract method <see cref="TryMoveNext"/> which derived classes must implement to determine if it can move next and what the next item is
        /// </remarks>
        public bool MoveNext()
        {
            Started = true;
            if (Finished) return false;
            if (TryMoveNext(out var item))
            {
                Current = item;
                return true;
            }
            Finished = true;
            return false;
        }

        /// <summary>
        /// Tries to move next
        /// </summary>
        /// <param name="item">Next item (if available)</param>
        /// <returns>True if another item is available, false otherwise</returns>
        protected abstract bool TryMoveNext(out T item);

        /// <summary>
        /// Resets the enumerator
        /// </summary>
        public void Reset()
        {
            Started = false;
            Finished = false;
            InnerEnumerator.Reset();
            ResetInternal();
        }

        /// <summary>
        /// Takes any implementation specific reset actions, should be overridden by derived classes that need to reset internal state
        /// </summary>
        protected virtual void ResetInternal() {}

        /// <summary>
        /// Gets/Sets the current item
        /// </summary>
        public T Current
        {
            get
            {
                if (!Started) throw new InvalidOperationException("Currently before the start of the enumerator, call MoveNext() before accessing Current");
                if (Finished) throw new InvalidOperationException("Currently after end of the enumerator");
                return _current;
            }
            private set => _current = value;
        }

        /// <summary>
        /// Gets the current item
        /// </summary>
        object IEnumerator.Current => Current;
    }
}