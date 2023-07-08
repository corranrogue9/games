namespace Fx.Game.Chess
{
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using Fx.Game.Chess;
    using Microsoft.VisualBasic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ChessMoveTests
    {
        [DataTestMethod]
        [DataRow("e4")]
        [DataRow("e5")]
        [DataRow("c4")]
        [DataRow("Bc5")]
        [DataRow("Nf3")]
        [DataRow("d6")]
        [DataRow("d3")]
        [DataRow("a6")]
        [DataRow("a3")]
        [DataRow("Bg4")]
        [DataRow("h3")]
        [DataRow("Bxf3")]
        [DataRow("Qxf3")]
        [DataRow("Qf6")]
        [DataRow("Be2")]
        [DataRow("Nd7")]
        [DataRow("Nc3")]
        [DataRow("Ne7")]
        [DataRow("O-O")]
        [DataRow("Bd4")]
        [DataRow("Bd2")]
        [DataRow("h6")]
        [DataRow("g5")]
        [DataRow("Qg4")]
        [DataRow("c6")]
        [DataRow("Be3")]
        [DataRow("Ba7")]
        [DataRow("Bxa7")]
        [DataRow("Rxa7")]
        [DataRow("Qg6")]
        [DataRow("Na4")]
        [DataRow("h5")]
        [DataRow("Qg3")]
        [DataRow("Nf6")]
        [DataRow("Qe3")]
        [DataRow("Ra8")]
        [DataRow("Nb6")]
        [DataRow("Rb8")]
        [DataRow("c5")]
        [DataRow("d5")]
        [DataRow("Qxe5")]
        [DataRow("Rd8")]
        [DataRow("Qxh8+")]
        [DataRow("Ng8")]
        [DataRow("Bxh5")]
        [DataRow("Qh6")]
        [DataRow("Kxe7")]
        [DataRow("Rxe7+")]
        [DataRow("Re1+")]
        [DataRow("Rab1")]
        [DataRow("b4")]
        [DataRow("exd5")]
        [DataRow("Nfxd5")]
        [DataRow("Rbe1+")]
        [DataRow("Ne7")]
        public void ParseHalfMoveTest(string input)
        {
            SANParser.ParseHalfMove(input);
        }


        [DataTestMethod]
        [DataRow("Rbe1+", SANPiece.Rook, "b0", false, "e1", true)]
        [DataRow("Ne7", SANPiece.Knight, "00", false, "e7", false)]
        [DataRow("Nfxd5", SANPiece.Knight, "f0", true, "d5", false)]
        [DataRow("Bxa7", SANPiece.Bishop, "00", true, "a7", false)]
        public void ParseHalfMoveResultTest(string input, SANPiece Piece, string Start, bool Take, string Coord, bool Check)
        {
            var actual = SANParser.ParseHalfMove(input);
            Assert.AreEqual(Piece, actual.Piece);
            Assert.AreEqual(O(Start), actual.Start);
            Assert.AreEqual(Take, actual.Take);
            Assert.AreEqual(C(Coord), actual.Target);
            Assert.AreEqual(Check, actual.Check);

            static (char, short)? O(string coord)
            {
                return coord == "00" ? null : C(coord);
            }

            static (char, short) C(string coord)
            {
                return (coord[0] == '0' ? '\0' : coord[0], (short)(coord[1] - '0'));
            }


        }

        [DataTestMethod]
        [DataRow("1. e4 e5 2. e4 e5")]
        public void ParseFullMoveTest(string input)
        {
            var result = SANParser.FullMove(input, out var remainder, out var actual);
            Assert.IsTrue(result);
            var expected = (1,
                new SANMove(SANPiece.Pawn, null, false, ('e', 4), false),
                new SANMove(SANPiece.Pawn, null, false, ('e', 5), false));
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(remainder.ToString(), "2. e4 e5");
        }

        [DataTestMethod]
        [DataRow("1. e4 e5 2. e4 e5")]
        public void ParseGameTest(string input)
        {
            if (SANParser.Moves(input, out var remainder, out var actual))
            {
                var expected = (1,
                    new SANMove(SANPiece.Pawn, null, false, ('e', 4), false),
                    new SANMove(SANPiece.Pawn, null, false, ('e', 5), false));
                Assert.AreEqual(expected, actual[0]);
            }
            else
            {
                Assert.Fail("failed to parse");
            }
        }

        // [TestMethod]
        // public void SanMoveTest()
        // {
        //     var san = new SANMove(SANPiece.Pawn, null, false, ('e', 4), false);
        //     var move = new ChessMove(ChessPiece.WhitePawn, new Coordinate(4, 1), new Coordinate(4, 3));

        //     Assert.IsTrue(san.Matches(move));
        // }


        [DataTestMethod]
        [DataRow("1. e4 e6 2. d4 b6")]
        [DataRow("1. e4 e6 2. d4 b6 3. a3 Bb7 4. Nc3 Nh6 5. Bxh6 gxh6 6. Be2 Qg5 7. Bg4 h5 8. Nf3 Qg6 9. Nh4 Qg5 10. Bxh5 Qxh4 11. Qf3 Kd8 12. Qxf7 Nc6 13. Qe8# 1-0")]
        public void GamePlayTest(string input)
        {
            var game = new Chess<string>("W", "B");
            foreach (var round in SANParser.ParseMoves(input))
            {
                FindAndPlayMove(ref game, round.Item2);
                FindAndPlayMove(ref game, round.Item3);
            }

            void FindAndPlayMove(ref Chess<string> game, SANMove san)
            {
                var moves = game.Moves;
                var matches = moves.Where(m => san.Matches(m)).ToList();
                if (matches.Count() == 0)
                {
                    Assert.Fail($"unable to find move {san} in {string.Join(", ", moves)}");
                }
                else if (matches.Count() > 1)
                {
                    Assert.Fail($"found multiple matches for move {san}: \n matches: {string.Join(", ", matches)} \nin moves: {string.Join(", ", moves)} \n{game.Board.Board}");
                }
                else
                {
                    var move = matches[0];
                    game = game.CommitMove(move);
                }
            }
        }

    }

}

