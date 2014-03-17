using System;
using System.Collections.Generic;

namespace VDS.Common.Collections
{
    /// <summary>
    /// Abstract implementation of a bounded list backed by a standard <see cref="IList{T}"/>
    /// </summary>
    /// <typeparam name="T">Element type</typeparam>
    public abstract class AbstractListBackedBoundedList<T>
        : IBoundedList<T>
    {
        protected readonly IList<T> _list;

        protected AbstractListBackedBoundedList(IList<T> list)
        {
            if (list == null) throw new ArgumentNullException("list");
            this._list = list;
        }

        public virtual int Count
        {
            get { return this._list.Count; }
        }

        public virtual bool IsReadOnly
        {
            get { return this._list.IsReadOnly; }
        }

        public virtual int IndexOf(T item)
        {
            return this._list.IndexOf(item);
        }

        public abstract void Insert(int index, T item);

        public virtual T this[int index]
        {
            get { return this._list[index]; }
            set { this._list[index] = value; }
        }

        public abstract void Add(T item);

        public virtual void Clear()
        {
            this._list.Clear();
        }

        public virtual bool Contains(T item)
        {
            return this._list.Contains(item);
        }

        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            this._list.CopyTo(array, arrayIndex);
        }

        public virtual bool Remove(T item)
        {
            return this._list.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this._list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this._list.GetEnumerator();
        }

        public abstract BoundedListOverflowPolicy OverflowPolicy { get; }

        public abstract int Capacity { get; protected set; }

        public virtual void RemoveAt(int index)
        {
            this._list.RemoveAt(index);
        }
    }
}