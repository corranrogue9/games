namespace Fx.Strategy
{
    using Fx.Game;

    public static class TwosHeuristics //// TODO does this go in fx.strategy or fx.game?
    {
        public static double Heuristic1(Twos<string> game)
        {
            return game.LegalMoves.Count();
        }

        public static double Heuristic2(Twos<string> game)
        {
            return game.Max;
        }

        public static double Heuristic3(Twos<string> game)
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

        public static double Heuristic4(Twos<string> game)
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

        public static double Heuristic5(Twos<string> game)
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

        public static double Heuristic6(Twos<string> game)
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

        public static double Heuristic7(Twos<string> game)
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

        public static double Heuristic8(Twos<string> game)
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

        public static double Heuristic9(Twos<string> game)
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

        public static double Heuristic10(Twos<string> game)
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

        public static double Heuristic11(Twos<string> game)
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

        public static double Heuristic12(Twos<string> game)
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

        public static double Heuristic12(NewTwos<string> game)
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
    }
}
