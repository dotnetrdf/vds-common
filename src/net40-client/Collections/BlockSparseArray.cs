using System;
using System.Collections;
using System.Collections.Generic;

namespace VDS.Common.Collections
{
    /// <summary>
    /// A memory efficient sparse array implemented as a sequence of blocks
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    public class BlockSparseArray<T>
        : ISparseArray<T>
    {
        public const int DefaultBlockSize = 100;

        private readonly SparseBlock<T>[] _blocks;

        public BlockSparseArray(int length)
            : this(length, DefaultBlockSize) { }

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
            return new BlockSparseArrayEnumerator<T>((IEnumerator<SparseBlock<T>>) this._blocks.GetEnumerator(), this.Length, this.BlockSize);
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
                    block = new SparseBlock<T>(blockIndex * this.BlockSize, this.BlockSize);
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
        public BlockSparseArrayEnumerator(IEnumerator<SparseBlock<T>> blocks, int length, int blockSize)
        {
            this.Blocks = blocks;
            this.Length = length;
            this.Index = -1;
            this.BlockSize = blockSize;
        }

        private IEnumerator<SparseBlock<T>> Blocks { get; set; }

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
                if (!this.Blocks.MoveNext()) return false;
            }
            this.Index++;
            if (this.Index % this.BlockSize == 0)
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
                SparseBlock<T> currentBlock = this.Blocks.Current;
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
