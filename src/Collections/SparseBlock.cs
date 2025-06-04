using System;

namespace VDS.Common.Collections;

class SparseBlock<T>
{
    private readonly T[] _block;

    public SparseBlock(int startIndex, int length)
    {
        if (startIndex < 0) throw new ArgumentException("Start Index must be >= 0", nameof(startIndex));
        if (length <= 0) throw new ArgumentException("Length must be >= 1", nameof(length));
        StartIndex = startIndex;
        Length = length;
        _block = new T[length];
    }

    public int StartIndex { get; private set; }

    public int Length { get; private set; }

    public T this[int index]
    {
        get => _block[index - StartIndex];
        set => _block[index - StartIndex] = value;
    }
}