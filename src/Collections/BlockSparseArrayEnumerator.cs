using System;
using System.Collections;
using System.Collections.Generic;

namespace VDS.Common.Collections;

class BlockSparseArrayEnumerator<T>
    : IEnumerator<T>
{
    public BlockSparseArrayEnumerator(IEnumerator blocks, int length, int blockSize)
    {
        Blocks = blocks;
        Length = length;
        Index = -1;
        BlockSize = blockSize;
    }

    private IEnumerator Blocks { get; set; }

    private int Index { get; set; }

    private int Length { get; set; }

    private int BlockSize { get; set; }

    public void Dispose()
    {
        // No dispose actions needed
    }

    public bool MoveNext()
    {
        if (Index == -1)
        {
            Blocks.MoveNext();
        }
        Index++;
        if (Index > 0 && Index % BlockSize == 0)
        {
            // Need to move to next block
            if (!Blocks.MoveNext()) return false;
        }
        return Index < Length;
    }

    public void Reset()
    {
        Index = -1;
        Blocks.Reset();
    }

    public T Current
    {
        get
        {
            if (Index == -1) throw new InvalidOperationException("Currently before the start of the enumerator, please call MoveNext() before accessing this property");
            if (Index >= Length) throw new InvalidOperationException("Past the end of the enumerator");

            // If no current block return default value
            var currentBlock = (SparseBlock<T>) Blocks.Current;
            if (currentBlock == null) return default(T);

            // Otherwise return the value within the block
            return currentBlock[Index];
        }
    }

    object IEnumerator.Current => Current;
}