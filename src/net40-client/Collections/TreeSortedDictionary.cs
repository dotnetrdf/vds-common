using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VDS.Common.Trees;

namespace VDS.Common.Collections
{
    /// <summary>
    /// An implementation of a dictionary where the sort order of keys is preserved
    /// </summary>
    /// <typeparam name="TKey">Key Type</typeparam>
    /// <typeparam name="TValue">Value Type</typeparam>
    public class TreeSortedDictionary<TKey, TValue>
        : IDictionary<TKey, TValue>, IEnumerable<TValue>
    {
        private ITree<IBinaryTreeNode<TKey, TValue>, TKey, TValue> _tree;
        private int _count = 0;

        public TreeSortedDictionary()
            : this(Comparer<TKey>.Default) { }

        public TreeSortedDictionary(IComparer<TKey> comparer)
        {
            if (comparer == null) throw new ArgumentNullException("comparer", "Comparer cannot be null");
            this._tree = new AVLTree<TKey, TValue>(comparer);
        }

        #region IDictionary<TKey,TValue> Members

        public void Add(TKey key, TValue value)
        {
            if (this._tree.Add(key, value))
            {
                this._count++;
            }
        }

        public bool ContainsKey(TKey key)
        {
            return this._tree.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get 
            {
                return new ImmutableView<TKey>(this._tree.Keys);
            }
        }

        public bool Remove(TKey key)
        {
            if (this._tree.Remove(key))
            {
                this._count--;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return this._tree.TryGetValue(key, out value);
        }

        public ICollection<TValue> Values
        {
            get
            {
                return new ImmutableView<TValue>(this._tree.Values); 
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                return this._tree[key];
            }
            set
            {
                this._tree[key] = value;
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            this._tree.Clear();
            this._count = 0;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            TValue value;

            if (this.TryGetValue(item.Key, out value))
            {
                if (value != null) return value.Equals(item.Value);
                if (item.Value == null) return true; //Both null so equal
                return false; //One is null so not equal
            }
            else
            {
                return false;
            }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException("Cannot copy to a null array");
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException("Cannot start copying at index < 0");
            if (this.Count > array.Length - arrayIndex) throw new ArgumentException("Insufficient space in array");

            int i = arrayIndex;
            foreach (ITreeNode<TKey, TValue> node in this._tree.Nodes)
            {
                array[i] = new KeyValuePair<TKey, TValue>(node.Key, node.Value);
                i++;
            }
        }

        public int Count
        {
            get 
            {
                return this._count;
            }
        }

        public bool IsReadOnly
        {
            get 
            {
                return false;
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            TValue value;
            if (this.TryGetValue(item.Key, out value))
            {
                if (value != null && value.Equals(item.Value)) return this.Remove(item.Key);
                if (item.Value == null) return this.Remove(item.Key);
                return false;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return (from node in this._tree.Nodes
                    select new KeyValuePair<TKey, TValue>(node.Key, node.Value)).GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
        {
            return this._tree.Values.GetEnumerator();
        }
    }
}
