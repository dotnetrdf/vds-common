using NUnit.Framework;

namespace VDS.Common.Collections
{
    [TestFixture, Category("Arrays")]
    public class BinarySparseArrayTests
        : AbstractSparseArrayContractTests
    {
        public override ISparseArray<int> CreateInstance(int length)
        {
            return new BinarySparseArray<int>(length);
        }
    }
}