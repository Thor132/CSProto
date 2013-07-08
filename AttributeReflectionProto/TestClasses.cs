// -----------------------------------------------------------------------
// <copyright file="TestClasses.cs" company="N/A">
// (C) Travis Thornton 2013
// </copyright>
// -----------------------------------------------------------------------

namespace AttributeReflectionProto
{
    public class TestClassA : TestClassBase
    {
        [TestAttributeClass("CustomNameFromAttribute")]
        public int IntPropertyA { get; set; }

        public int IntPropertyB { get; set; }

        [TestAttributeClass]
        public int IntPropertyC { get; set; }
    }

    public class TestClassB : TestClassBase
    {
        [TestAttributeClass]
        public float FloatPropertyA { get; set; }

        [TestAttributeClass]
        public string StringPropertyA { get; set; }
    }

    public class TestClassC : TestClassBase
    {
        public int IntPropertyA { get; set; }

        [TestAttributeClass]
        public string StringPropertyA { get; set; }
    }
}