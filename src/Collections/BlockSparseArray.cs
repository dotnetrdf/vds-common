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

namespace VDS.Common.Collections;

/// <summary>
/// A memory efficient sparse array implemented as a sequence of blocks
/// </summary>
/// <remarks>
/// <para>
/// This sparse array is implemented as a series of potentially empty blocks such that only blocks which contain values
/// have any memory allocated to them.  This means it is extremely memory efficient for sparsely populated arrays.
/// The block size may be tweaked to limit the amount of memory that might be newly allocated by setting a value since
/// setting a value in an as yet unpopulated block requires allocating the memory for that block.  Since at minimum the
/// sequence of blocks must be maintained this implementation can be less memory efficient than the
/// <see cref="LinkedSparseArray{T}"/> for some usages.
/// </para>
/// <para>
/// Since the sequence of blocks and the contents of each block are implemented using standard arrays,
/// element access is O(1) regardless of how empty/full the sparse array is.
/// </para>
/// </remarks>
/// <typeparam name="T">Value type</typeparam>
public class BlockSparseArray<T>
    : ISparseArray<T>
{
    /// <summary>
    /// Default block size used if one is not explicitly specified
    /// </summary>
    public const int DefaultBlockSize = 100;

    private readonly SparseBlock<T>[] _blocks;

    /// <summary>
    /// Creates a new sparse array with the default block size
    /// </summary>
    /// <param name="length">Length of the array</param>
    public BlockSparseArray(int length)
        : this(length, DefaultBlockSize) { }

    /// <summary>
    /// Creates a new sparse array
    /// </summary>
    /// <param name="length">Length</param>
    /// <param name="blockSize">Block Size</param>
    public BlockSparseArray(int length, int blockSize)
    {
        if (length < 0) throw new ArgumentException("Length must be >= 0", nameof(length));
        if (blockSize < 1) throw new ArgumentException("Block Size must be >= 1", nameof(blockSize));
        var numBlocks = (length/blockSize) + (length%blockSize);
        _blocks = new SparseBlock<T>[numBlocks];

        BlockSize = blockSize;
        Length = length;
    }

    /// <summary>
    /// Gets an enumerator
    /// </summary>
    /// <returns></returns>
    public IEnumerator<T> GetEnumerator()
    {
        return new BlockSparseArrayEnumerator<T>(_blocks.GetEnumerator(), Length, BlockSize);
    }

    /// <summary>
    /// Gets an enumerator
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Gets/Sets the value at the given index
    /// </summary>
    /// <param name="index">Index</param>
    /// <returns>Value</returns>
    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= Length) throw new IndexOutOfRangeException(string.Format("Index must be in the range 0 to {0}", Length - 1));
            var blockIndex = index / BlockSize;
            var block = _blocks[blockIndex];
            return block != null ? block[index] : default(T);
        }
        set
        {
            if (index < 0 || index >= Length) throw new IndexOutOfRangeException(string.Format("Index must be in the range 0 to {0}", Length - 1));
            var blockIndex = index / BlockSize;
            var block = _blocks[blockIndex];
            if (block == null)
            {
                // Calculate start index and block size
                // Remember that if length is not a multiple of the block size the final block must be truncated
                var startIndex = blockIndex*BlockSize;
                var blockSize = blockIndex < _blocks.Length - 1 ? BlockSize : Math.Min(BlockSize, Length - startIndex);
                block = new SparseBlock<T>(startIndex, blockSize);
                _blocks[blockIndex] = block;
            }
            block[index] = value;

        }
    }

    /// <summary>
    /// Gets/Sets the block size
    /// </summary>
    private int BlockSize { get; set; }

    /// <summary>
    /// Gets the length of the array
    /// </summary>
    public int Length { get; private set; }

    /// <summary>
    /// Clears the array
    /// </summary>
    public void Clear()
    {
        Array.Clear(_blocks, 0, _blocks.Length);
    }
}