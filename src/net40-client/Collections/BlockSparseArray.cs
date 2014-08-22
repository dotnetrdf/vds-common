using System;
using System.Collections;
using System.Collections.Generic;

namespace VDS.Common.Collections
{
    /// <summary>
    /// A memory efficient sparse array implemented as a sequence of blocks
    /// </summary>
    /// <remarks>
    /// <para>
    /// This sparse array is implemented as a series of potentially empty blocks such that only blocks which contain values have any memory allocated to them.  This means it is extremely memory efficient for sparsely populated arrays.  The block size may be tweaked to limit the amount of memory that might be newly allocated by setting a value since setting a value in an as yet unpopulated block requires allocating the memory for that block.  Since at minimum the sequence of blocks must be maintained this implementation can be less memory efficient than the <see cref="LinkedSparseArray{T}"/> for some usages.
    /// </para>
    /// <para>
    /// Since the sequence of blocks and the contents of each block are implemented using standard arrays an element may be accessed by index in linear time regardless of how empty/full the sparse array is.
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
            if (length < 0) throw new ArgumentException("Length must be >= 0", "length");
            if (blockSize < 1) throw new ArgumentException("Block Size must be >= 1", "blockSize");
            int numBlocks = (length/blockSize) + (length%blockSize);
            this._blocks = new SparseBlock<T>[numBlocks];

            this.BlockSize = blockSize;
            this.Length = length;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new BlockSparseArrayEnumerator<T>(this._blocks.GetEnumerator(), this.Length, this.BlockSize);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= this.Length) throw new IndexOutOfRangeException(String.Format("Index must be in the range 0 to {0}", this.Length - 1));
                int blockIndex = index / this.BlockSize;
                SparseBlock<T> block = this._blocks[blockIndex];
                return block != null ? block[index] : default(T);
            }
            set
            {
                if (index < 0 || index >= this.Length) throw new IndexOutOfRangeException(String.Format("Index must be in the range 0 to {0}", this.Length - 1));
                int blockIndex = index / this.BlockSize;
                SparseBlock<T> block = this._blocks[blockIndex];
                if (block == null)
                {
                    // Calculate start index and block size
                    // Remember that if length is not a multiple of the block size the final block must be truncated
                    int startIndex = blockIndex*this.BlockSize;
                    int blockSize = blockIndex < this._blocks.Length - 1 ? this.BlockSize : Math.Min(this.BlockSize, this.Length - startIndex);
                    block = new SparseBlock<T>(startIndex, blockSize);
                    this._blocks[blockIndex] = block;
                }
                block[index] = value;

            }
        }

        private int BlockSize { get; set; }

        public int Length { get; private set; }
    }

    class SparseBlock<T>
    {
        private readonly T[] _block;

        public SparseBlock(int startIndex, int length)
        {
            if (startIndex < 0) throw new ArgumentException("Start Index must be >= 0", "startIndex");
            if (length <= 0) throw new ArgumentException("Length must be >= 1", "length");
            this.StartIndex = startIndex;
            this.Length = length;
            this._block = new T[length];
        }

        public int StartIndex { get; private set; }

        public int Length { get; private set; }

        public T this[int index]
        {
            get { return this._block[index - this.StartIndex]; }
            set { this._block[index - this.StartIndex] = value; }
        }
    }

    class BlockSparseArrayEnumerator<T>
        : IEnumerator<T>
    {
        public BlockSparseArrayEnumerator(IEnumerator blocks, int length, int blockSize)
        {
            this.Blocks = blocks;
            this.Length = length;
            this.Index = -1;
            this.BlockSize = blockSize;
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
            if (this.Index == -1)
            {
                this.Blocks.MoveNext();
            }
            this.Index++;
            if (this.Index > 0 && this.Index % this.BlockSize == 0)
            {
                // Need to move to next block
                if (!this.Blocks.MoveNext()) return false;
            }
            return this.Index < this.Length;
        }

        public void Reset()
        {
            this.Index = -1;
            this.Blocks.Reset();
        }

        public T Current
        {
            get
            {
                if (this.Index == -1) throw new InvalidOperationException("Currently before the start of the enumerator, please call MoveNext() before accessing this property");
                if (this.Index >= this.Length) throw new InvalidOperationException("Past the end of the enumerator");

                // If no current block return default value
                SparseBlock<T> currentBlock = (SparseBlock<T>) this.Blocks.Current;
                if (currentBlock == null) return default(T);

                // Otherwise return the value within the block
                return currentBlock[this.Index];
            }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
}
