namespace VDS.Common.Collections;

class SparseArrayEntry<T>
{
    public SparseArrayEntry(int index)
        : this(index, default(T)) { }

    public SparseArrayEntry(int index, T value)
    {
        Index = index;
        Value = value;
    }

    public int Index { get; private set; }

    public T Value { get; set; }
}