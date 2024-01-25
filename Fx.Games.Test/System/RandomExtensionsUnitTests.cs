namespace System
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RandomExtensionsUnitTests
    {
        [TestMethod]
        public void Choose()
        {
            var random = new Random(0);
            var list = new[] { 5, 4, 1, 7 };
            Assert.AreEqual(1, random.Choose(list));
        }
    }
}