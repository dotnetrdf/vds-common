using NUnit.Framework;

namespace VDS.Common.Collections
{
    [TestFixture,Category("Arrays")]
    public class LinkedSparseArrayTests
        : AbstractSparseArrayContractTests
    {
        public override ISparseArray<int> CreateInstance(int length)
        {
            return new LinkedSparseArray<int>(length);
        }
    }
}