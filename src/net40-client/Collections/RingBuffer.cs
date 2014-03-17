using System;
using System.Collections;
using System.Collections.Generic;

namespace VDS.Common.Collections
{
    public class RingBuffer<T>
        : IBoundedList<T>
    {
        private readonly T[] _items;
        private readonly IComparer<T> _comparer;

        public RingBuffer(int capacity)
        {
            StartIndex = 0;
            Count = 0;
            if (capacity < 0) throw new ArgumentException("Capacity must be >= 0", "capacity");
            this._items = new T[capacity];
            this._comparer = Comparer<T>.Default;
            if (this._comparer == null) throw new InvalidOperationException("Unable to create a RingBuffer since no default comparer is available for the configured element type");
        }

        public RingBuffer(int capacity, IEnumerable<T> items)
            : this(capacity)
        {
            foreach (T item in items)
            {
                this.Add(item);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new RingBufferEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private int ToActualIndex(int index)
        {
            return (this.StartIndex + index)%this._items.Length;
        }

        public void Add(T item)
        {
            int index = this.StartIndex + this.Count;
            if (index >= this._items.Length)
            {
                this.StartIndex++;
                index = this.Count - 1;
            }
            else
            {
                this.Count++;
            }
            this._items[ToActualIndex(index)] = item;
        }

        public void Clear()
        {
            for (int i = 0; i < this._items.Length; i++)
            {
                this._items[i] = default(T);
            }
            this.StartIndex = 0;
            this.Count = 0;
        }

        public bool Contains(T item)
        {
            if (this.Count == 0) return false;
            for (int i = 0; i < this.Count; i++)
            {
                if (this._comparer.Compare(item, this._items[this.ToActualIndex(i)]) == 0) return true;
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException("array", "Cannot copy to a null array");
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException("arrayIndex", "Cannot start copying at index < 0");
            if (this.Count > array.Length - arrayIndex) throw new ArgumentException("Insufficient space in array", "array");

            for (int destIndex = arrayIndex, srcIndex = 0; srcIndex < this.Count; srcIndex++, destIndex++)
            {
                array[destIndex] = this._items[ToActualIndex(srcIndex)];
            }
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public int Count { get; private set; }

        public int Capacity { get { return this._items.Length; } }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public int IndexOf(T item)
        {
            if (this.Count == 0) return -1;
            for (int i = 0; i < this.Count; i++)
            {
                if (this._comparer.Compare(item, this._items[this.ToActualIndex(i)]) == 0) return i;
            }
            return -1;
        }

        public void Insert(int index, T item)
        {
            if (index < 0 || index >= this.Count) throw new IndexOutOfRangeException("Index must be in range 0-" + this.Count);
            // Shift items forward
            for (int i = index; i < this.Count - 1; i++)
            {
                this._items[ToActualIndex(i + 1)] = this._items[ToActualIndex(i)];
            }
            // Insert new item
            this._items[ToActualIndex(index)] = item;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= this.Count) throw new IndexOutOfRangeException("Index must be in range 0-" + this.Count);
            // Shift items backwards
            for (int i = index; i < this.Count - 1; i++)
            {
                this._items[ToActualIndex(i)] = this._items[ToActualIndex(i + 1)];
            }
            this.Count--;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= this.Count) throw new IndexOutOfRangeException("Index must be in range 0-" + this.Count);
                return this._items[ToActualIndex(index)];
            }
            set
            {
                if (index < 0 || index >= this.Count) throw new IndexOutOfRangeException("Index must be in range 0-" + this.Count);
                this._items[ToActualIndex(index)] = value;
            }
        }

        /// <summary>
        /// Gets that the overflow policy of this bounded list is to overwrite elements, the elements overwritten are the least recently added elements
        /// </summary>
        public BoundedListOverflowPolicy OverflowPolicy
        {
            get { return BoundedListOverflowPolicy.Overwrite; }
        }

        internal int StartIndex { get; set; }
    }

    class RingBufferEnumerator<T>
        : IEnumerator<T>
    {
        private readonly RingBuffer<T> _buffer;
        private readonly int _startIndex, _count;
        private int _currIndex;

        public RingBufferEnumerator(RingBuffer<T> buffer)
        {
            this._startIndex = buffer.StartIndex;
            this._currIndex = -1;
            this._count = buffer.Count;
            this._buffer = buffer;
        }

        private void CheckNotModified()
        {
            if (this._count != this._buffer.Count) throw new InvalidOperationException("Ring Buffer was modified during enumeration");
            if (this._startIndex != this._buffer.StartIndex) throw new InvalidOperationException("Ring Buffer was modified during enumeration");
        }

        public T Current
        {
            get
            {
                this.CheckNotModified();
                return this._buffer[this._currIndex];
            }
        }

        public void Dispose()
        {
            // Nothing to do
        }

        object IEnumerator.Current
        {
            get
            {
                return this.Current;
            }
        }

        public bool MoveNext()
        {
            if (this._currIndex >= this._count - 1) return false;
            this.CheckNotModified();
            this._currIndex++;
            return true;
        }

        public void Reset()
        {
            this._currIndex = -1;
        }
    }
}