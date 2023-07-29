namespace Fx.Game.Chess
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    // run via `dotnet test -l "console;verbosity=detailed" --filter "TestCategory=Parsers"`
    [TestClass]
    public class ParserTests
    {
        [DataTestMethod]
        [TestCategory("Parsers")]
        [DataRow("ABC", "ABC")]
        [DataRow("AC", "AC")]
        [DataRow("CB", "BC")]
        [DataRow("A", "A")]
        [DataRow("AA", "A")]
        public void TestUnorderedParsers(string input, string expected)
        {
            var parser = Parsers.Unordered2(Parsers.Char('A'), Parsers.Char('B'), Parsers.Char('C'));

            Assert.IsTrue(parser(input, out var rem, out var val));
            Assert.AreEqual("", rem.ToString());

            var expectedSet = expected.ToHashSet();
            Assert.IsTrue(val.SetEquals(expectedSet), $"actual: {string.Join(", ", val)} != {string.Join(", ", expectedSet)}");
        }
    }
}

