namespace ConsoleApplication4
{
    using Fx.Driver;
    using Fx.Game;
    using Fx.Strategy;

    public static class Main
    {
        private static double Heuristic1(Twos<string> game)
        {
            return game.LegalMoves.Count();
        }

        private static double Heuristic2(Twos<string> game)
        {
            return game.Max;
        }

        private static double Heuristic3(Twos<string> game)
        {
            var max = 0;
            var free = 0;
            foreach (var row in game.Board)
            {
                foreach (var column in row)
                {
                    if (column > max)
                    {
                        max = column;
                    }

                    if (column == 0)
                    {
                        ++free;
                    }
                }
            }

            return max * 100 + free;
        }

        private static double Heuristic4(Twos<string> game)
        {
            //// best so far
            var max = 0;
            var free = 0;
            foreach (var row in game.Board)
            {
                foreach (var column in row)
                {
                    if (column > max)
                    {
                        max = column;
                    }

                    if (column == 0)
                    {
                        ++free;
                    }
                }
            }

            return (game.Board[0][0] == max ? 100000 : 0) + max * 100 + free;
        }

        private static double Heuristic5(Twos<string> game)
        {
            var max = 0;
            var free = 0;
            foreach (var row in game.Board)
            {
                foreach (var column in row)
                {
                    if (column > max)
                    {
                        max = column;
                    }

                    if (column == 0)
                    {
                        ++free;
                    }
                }
            }

            var corner =
                game.Board[0][0] == max ||
                game.Board[0][3] == max ||
                game.Board[3][0] == max ||
                game.Board[3][3] == max;

            return (corner ? 100000 : 0) + max * 100 + free;
        }

        private static double Heuristic6(Twos<string> game)
        {
            var max = 0;
            var free = 0;
            foreach (var row in game.Board)
            {
                foreach (var column in row)
                {
                    if (column > max)
                    {
                        max = column;
                    }

                    if (column == 0)
                    {
                        ++free;
                    }
                }
            }

            var corner =
                game.Board[0][0] == max ||
                game.Board[0][3] == max ||
                game.Board[3][0] == max ||
                game.Board[3][3] == max;

            return (corner ? 100000 : 0) + free * 100 + max;
        }

        private static double Heuristic7(Twos<string> game)
        {
            var max = 0;
            var free = 0;
            foreach (var row in game.Board)
            {
                foreach (var column in row)
                {
                    if (column > max)
                    {
                        max = column;
                    }

                    if (column == 0)
                    {
                        ++free;
                    }
                }
            }

            return free * 100 + max;
        }

        private static double Heuristic8(Twos<string> game)
        {
            // this is intentionally bad
            var sum = 0;
            foreach (var row in game.Board)
            {
                foreach (var column in row)
                {
                    sum += column;
                }
            }

            return sum;
        }

        private static double Heuristic9(Twos<string> game)
        {
            var max = 0;
            var free = 0;
            var combine = 0;
            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    var value = game.Board[i][j];
                    if (value > max)
                    {
                        max = value;
                    }

                    if (value == 0)
                    {
                        ++free;
                    }

                    if (j != 0)
                    {
                        if (game.Board[i][j - 1] == game.Board[i][j])
                        {
                            ++combine;
                        }
                    }

                    if (i != 0)
                    {
                        if (game.Board[i - 1][j] == game.Board[i][j])
                        {
                            ++combine;
                        }
                    }
                }
            }

            return free * 10000 + combine * 100 + max;
        }

        private static double Heuristic10(Twos<string> game)
        {
            var max = 0;
            var free = 0;
            var combine = 0;
            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    var value = game.Board[i][j];
                    if (value > max)
                    {
                        max = value;
                    }

                    if (value == 0)
                    {
                        ++free;
                    }

                    if (j != 0)
                    {
                        if (game.Board[i][j - 1] == game.Board[i][j])
                        {
                            ++combine;
                        }
                    }

                    if (i != 0)
                    {
                        if (game.Board[i - 1][j] == game.Board[i][j])
                        {
                            ++combine;
                        }
                    }
                }
            }

            return combine * 10000 + free * 100 + max;
        }

        private static double Heuristic11(Twos<string> game)
        {
            var max = 0;
            var free = 0;
            var combine = 0;
            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    var value = game.Board[i][j];
                    if (value > max)
                    {
                        max = value;
                    }

                    if (value == 0)
                    {
                        ++free;
                    }

                    if (j != 0)
                    {
                        if (game.Board[i][j - 1] == game.Board[i][j])
                        {
                            ++combine;
                        }
                    }

                    if (i != 0)
                    {
                        if (game.Board[i - 1][j] == game.Board[i][j])
                        {
                            ++combine;
                        }
                    }
                }
            }

            return max * 10000 + free * 100 + combine;
        }

        private static double Heuristic12(Twos<string> game)
        {
            var max = 0;
            var free = 0;
            var combine = 0;
            var position = (0, 0);
            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    var value = game.Board[i][j];
                    if (value > max)
                    {
                        max = value;
                        position = (i, j);
                    }

                    if (value == 0)
                    {
                        ++free;
                    }

                    if (j != 0)
                    {
                        if (game.Board[i][j - 1] == game.Board[i][j])
                        {
                            ++combine;
                        }
                    }

                    if (i != 0)
                    {
                        if (game.Board[i - 1][j] == game.Board[i][j])
                        {
                            ++combine;
                        }
                    }
                }
            }

            return max * (position.Item1 + 1) * (position.Item2 + 1) * 10000 + free * 100 + combine;
        }

        public static void DoWork()
        {
            //// TODO do other display mechanism to formalize "board state"
            //// TODO do game of amazons to fix decision tree with monte carlo simulation
            //// TODO do more games
            Func<TwosDirection, string> displayMove = (move) =>
            {
                return move.ToString();
            };
            var ticks = Environment.TickCount;
            ////var ticks = 551040140;
            ////var ticks = 1071300156;
            File.WriteAllText(@"c:\users\gdebruin\desktop\ticks.txt", ticks.ToString());
            var random = new Random(ticks);
            var consolestat = new ConsoleStrategy<TwosDirection, string, Twos<string>>(displayMove);
            var results = new Driver<TwosDirection, string, Twos<string>>(
                new Twos<string>("gdebruin", new Random(ticks), 12),
                new Dictionary<string, IStrategy<TwosDirection, string, Twos<string>>>
                {
                    ////{ "gdebruin", new ConsoleStrategy<Direction, string, Twos<string>>(displayMove)},
                    ////{ "brett", new BrettStrategy(consolestat) },
                    { "gdebruin", new GarrettGameTreeDepthStrategy<Twos<string>, TwosDirection, string>(Heuristic12, Fx.Tree.Node.TreeFactory, "gdebruin", StringComparer.OrdinalIgnoreCase)},
                    ////{ "gdebruin", new RandomStrategy<Direction, string, Twos<string>>(random) }
                    ////{ "gdebruin", new DecisionTree<Direction, string, Twos<string>>(outcome => outcome.Winners.Contains("gdebruin")) },
                    /*{
                        "gdebruin",
                        new DecisionTreeImp<Direction, string, Twos<string>>(
                            outcome => outcome.Winners.Contains("gdebruin"),
                            2, 
                            game => MontyCarloSimulation<Twos<string>, Direction, string>(game, outcome => outcome.Winners.Contains("gdebruin"), 100000, random))
                    },*/
                    ////{ "gdebruin", new RandomStrategy<Direction, string, Twos<string>>(new Random(ticks)) },
                }).Start();

            /*foreach (var loser in results.Losers)
            {
                Console.WriteLine($"{loser} loses...");
            }*/

            foreach (var winner in results.Winners)
            {
                Console.WriteLine($"{winner} wins!");
            }

            Console.WriteLine(ticks);
            Console.ReadLine();
        }
    }
}