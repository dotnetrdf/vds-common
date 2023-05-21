using NUnit.Framework;

namespace VDS.Common.Collections
{
    [TestFixture(1),
     TestFixture(10),
     TestFixture(250),
     TestFixture(1000),
     Category("Arrays")]
    public class BlockSparseArrayTests
        : AbstractSparseArrayContractTests
    {
        public BlockSparseArrayTests(int blockSize)
        {
            this.BlockSize = blockSize;
        }

        private int BlockSize { get; set; }

        public override ISparseArray<int> CreateInstance(int length)
        {
            return new BlockSparseArray<int>(length, this.BlockSize);
        }
    }
}