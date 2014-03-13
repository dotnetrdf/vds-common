using System;
using System.Collections;
using System.Collections.Generic;

namespace VDS.Common.Collections
{
    public class RingBuffer<T>
        : IBoundedList<T>
    {
        private readonly T[] _items;
        private int _startIndex = 0;
        private readonly IComparer<T> _comparer;

        public RingBuffer(int capacity)
        {
            Count = 0;
            if (capacity < 0) throw new ArgumentException("Capacity must be >= 0", "capacity");
            this._items = new T[capacity];
            this._comparer = Comparer<T>.Default;
            if (this._comparer == null) throw new InvalidOperationException("Unable to create a RingBuffer since no default comparer is available for the configured element type");
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private int ToActualIndex(int index)
        {
            return (this._startIndex + index)%this._items.Length;
        }

        public void Add(T item)
        {
            int index = this._startIndex + this.Count;
            if (index >= this._items.Length)
            {
                this._startIndex++;
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
            this._startIndex = 0;
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
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public int Count { get; private set; }

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
    }
}