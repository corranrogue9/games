namespace Fx.Game
{
    public sealed class Twos<TPlayer> : IGameWithHiddenInformation<TwosDirection, TPlayer, Twos<TPlayer>>
    {
        private readonly TPlayer player;

        private readonly int[][] board;

        private readonly Random random;

        private readonly int win;

        public Twos(TPlayer player, Random random, int win = 11)
        {
            this.win = win;

            this.player = player;
            this.random = random;

            this.board = new int[4][];
            for (int i = 0; i < this.board.Length; ++i)
            {
                this.board[i] = new int[4];
            }

            for (int i = 0; i < 2; ++i)
            {
                while (true)
                {
                    //// TODO you can probably do better
                    var row = this.random.Next(0, this.board.Length);
                    var column = this.random.Next(0, this.board[row].Length);
                    if (this.board[row][column] == 0)
                    {
                        if (this.random.Next(0, 10) == 0)
                        {
                            this.board[row][column] = 2;
                        }
                        else
                        {
                            this.board[row][column] = 1;
                        }

                        break;
                    }
                }
            }
        }

        private Twos(TPlayer player, int[][] board, Random random, int win)
        {
            this.win = win;

            this.player = player;
            this.random = random;

            this.board = new int[board.Length][];
            for (int i = 0; i < this.board.Length; ++i)
            {
                this.board[i] = board[i].Clone() as int[];
            }

            /*this.board = new int[board.Length][];
            for (int i = 0; i < this.board.Length; ++i)
            {
                this.board[i] = new int[board[i].Length];
                for (int j = 0; j < this.board[i].Length; ++j)
                {
                    this.board[i][j] = board[i][j];
                }
            }*/
        }

        public TPlayer CurrentPlayer
        {
            get
            {
                return this.player;
            }
        }

        public int Max
        {
            get
            {
                return this.board.Max(row => row.Max());
            }
        }

        private bool Up()
        {
            var up = false;
            for (int i = this.board.Length - 1; i >= 1; --i)
            {
                for (int j = 0; j < this.board[i].Length; ++j)
                {
                    if (this.board[i][j] != 0)
                    {
                        for (int k = i - 1; k >= 0; --k)
                        {
                            if (this.board[k][j] == 0 || this.board[k][j] == this.board[i][j])
                            {
                                return true;
                            }

                            if (board[k][j] != 0)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool Down()
        {
            var down = false;
            for (int i = 0; i < this.board.Length - 1; ++i)
            {
                for (int j = 0; j < this.board[i].Length; ++j)
                {
                    if (this.board[i][j] != 0)
                    {
                        for (int k = i + 1; k < this.board.Length; ++k)
                        {
                            if (this.board[k][j] == 0 || this.board[k][j] == this.board[i][j])
                            {
                                return true;
                            }

                            if (board[k][j] != 0)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool Left()
        {
            var left = false;
            for (int i = 0; i < this.board.Length; ++i)
            {
                for (int j = this.board[i].Length - 1; j >= 1; --j)
                {
                    if (this.board[i][j] != 0)
                    {
                        for (int k = j - 1; k >= 0; --k)
                        {
                            if (this.board[i][k] == 0 || this.board[i][k] == this.board[i][j])
                            {
                                return true;
                            }

                            if (board[i][k] != 0)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool Right()
        {
            var right = false;
            for (int i = 0; i < this.board.Length; ++i)
            {
                for (int j = 0; j < this.board[i].Length - 1; ++j)
                {
                    if (this.board[i][j] != 0)
                    {
                        for (int k = j + 1; k < this.board[i].Length; ++k)
                        {
                            if (this.board[i][k] == 0 || this.board[i][k] == this.board[i][j])
                            {
                                return true;
                            }

                            if (board[i][k] != 0)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public IEnumerable<TwosDirection> LegalMoves
        {
            get
            {
                if (Up())
                {
                    yield return TwosDirection.Up;
                }

                if (Down())
                {
                    yield return TwosDirection.Down;
                }

                if (Left())
                {
                    yield return TwosDirection.Left;
                }

                if (Right())
                {
                    yield return TwosDirection.Right;
                }
            }
        }

        public Outcome<TPlayer> Outcome
        {
            get
            {
                /*for (int i = 0; i < this.board.Length; ++i)
                {
                    for (int j = 0; j < this.board[i].Length; ++j)
                    {
                        if (this.board[i][j] == this.win)
                        {
                            return new Outcome<TPlayer>(new[] { this.player }, Enumerable.Empty<TPlayer>(), Enumerable.Empty<TPlayer>());
                        }
                    }
                }*/

                if (!this.LegalMoves.Any())
                {
                    return new Outcome<TPlayer>(Enumerable.Empty<TPlayer>());
                }

                return null;
            }
        }

        public Twos<TPlayer> CommitMove(TwosDirection move)
        {
            var newGames = ExploreMove(move).ToList();
            return newGames[this.random.Next(0, newGames.Count)];
        }

        public int[][] Board
        {
            get
            {
                return this.board;
            }
        }

        public void Display()
        {
            for (int i = 0; i < this.board.Length; ++i)
            {
                for (int j = 0; j < this.board[i].Length; ++j)
                {
                    if (this.board[i][j] == 0)
                    {
                        Console.Write("   0 ");
                    }
                    else
                    {
                        Console.Write($"{1 << this.board[i][j], 4} ");
                    }
                }

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
            }

            Console.WriteLine("----------------");
        }

        public IEnumerable<Twos<TPlayer>> ExploreMove(TwosDirection move)
        {
            var newGame = new Twos<TPlayer>(this.player, this.board, this.random, this.win);
            if (move == TwosDirection.Up)
            {
                for (int j = 0; j < newGame.board.Length; ++j)
                {
                    for (int i = 0; i < newGame.board.Length; ++i)
                    {
                        if (newGame.board[i][j] == 0)
                        {
                            for (int k = i + 1; k < newGame.board.Length; ++k)
                            {
                                if (newGame.board[k][j] != 0)
                                {
                                    newGame.board[i][j] = newGame.board[k][j];
                                    newGame.board[k][j] = 0;
                                    break;
                                }
                            }
                        }
                    }
                }

                for (int j = 0; j < newGame.board.Length; ++j)
                {
                    for (int i = 0; i < newGame.board.Length - 1; ++i)
                    {
                        if (newGame.board[i][j] != 0 && newGame.board[i + 1][j] == newGame.board[i][j])
                        {
                            newGame.board[i][j]++;
                            newGame.board[i + 1][j] = 0;
                            for (int k = i + 1; k < newGame.board.Length - 1; ++k)
                            {
                                newGame.board[k][j] = newGame.board[k + 1][j];
                                newGame.board[k + 1][j] = 0;
                            }
                        }
                    }
                }
            }
            else if (move == TwosDirection.Down)
            {
                for (int j = 0; j < newGame.board.Length; ++j)
                {
                    for (int i = newGame.board.Length - 1; i >= 0; --i)
                    {
                        if (newGame.board[i][j] == 0)
                        {
                            for (int k = i - 1; k >= 0; --k)
                            {
                                if (newGame.board[k][j] != 0)
                                {
                                    newGame.board[i][j] = newGame.board[k][j];
                                    newGame.board[k][j] = 0;
                                    break;
                                }
                            }
                        }
                    }
                }

                for (int j = 0; j < newGame.board.Length; ++j)
                {
                    for (int i = newGame.board.Length - 1; i >= 1; --i)
                    {
                        if (newGame.board[i][j] != 0 && newGame.board[i - 1][j] == newGame.board[i][j])
                        {
                            newGame.board[i][j]++;
                            newGame.board[i - 1][j] = 0;
                            for (int k = i - 1; k >= 1; --k)
                            {
                                newGame.board[k][j] = newGame.board[k - 1][j];
                                newGame.board[k - 1][j] = 0;
                            }
                        }
                    }
                }
            }
            else if (move == TwosDirection.Left)
            {
                for (int i = 0; i < newGame.board.Length; ++i)
                {
                    for (int j = 0; j < newGame.board.Length; ++j)
                    {
                        if (newGame.board[i][j] == 0)
                        {
                            for (int k = j + 1; k < newGame.board.Length; ++k)
                            {
                                if (newGame.board[i][k] != 0)
                                {
                                    newGame.board[i][j] = newGame.board[i][k];
                                    newGame.board[i][k] = 0;
                                    break;
                                }
                            }
                        }
                    }
                }

                for (int i = 0; i < newGame.board.Length; ++i)
                {
                    for (int j = 0; j < newGame.board.Length - 1; ++j)
                    {
                        if (newGame.board[i][j] != 0 && newGame.board[i][j + 1] == newGame.board[i][j])
                        {
                            newGame.board[i][j]++;
                            newGame.board[i][j + 1] = 0;
                            for (int k = j + 1; k < newGame.board.Length - 1; ++k)
                            {
                                newGame.board[i][k] = newGame.board[i][k + 1];
                                newGame.board[i][k + 1] = 0;
                            }
                        }
                    }
                }
            }
            else if (move == TwosDirection.Right)
            {
                for (int i = 0; i < newGame.board.Length; ++i)
                {
                    for (int j = newGame.board.Length - 1; j >= 0; --j)
                    {
                        if (newGame.board[i][j] == 0)
                        {
                            for (int k = j - 1; k >= 0; --k)
                            {
                                if (newGame.board[i][k] != 0)
                                {
                                    newGame.board[i][j] = newGame.board[i][k];
                                    newGame.board[i][k] = 0;
                                    break;
                                }
                            }
                        }
                    }
                }

                for (int i = 0; i < newGame.board.Length; ++i)
                {
                    for (int j = newGame.board.Length - 1; j >= 1; --j)
                    {
                        if (newGame.board[i][j] != 0 && newGame.board[i][j - 1] == newGame.board[i][j])
                        {
                            newGame.board[i][j]++;
                            newGame.board[i][j - 1] = 0;
                            for (int k = j - 1; k >= 1; --k)
                            {
                                newGame.board[i][k] = newGame.board[i][k - 1];
                                newGame.board[i][k - 1] = 0;
                            }
                        }
                    }
                }
            }

            var emptySlot = false;
            for (int i = 0; i < newGame.board.Length; ++i)
            {
                for (int j = 0; j < newGame.board[i].Length; ++j)
                {
                    if (newGame.board[i][j] == 0)
                    {
                        /*for (int k = 0; k < 10; ++k)
                        {
                            var explore = new Twos<TPlayer>(newGame.player, newGame.board, newGame.random, this.win);
                            if (k == 0)
                            {
                                explore.board[i][j] = 2;
                            }
                            else
                            {
                                explore.board[i][j] = 1;
                            }

                            emptySlot = true;
                            yield return explore;
                        }*/
                        var explore = new Twos<TPlayer>(newGame.player, newGame.board, newGame.random, this.win);
                        explore.board[i][j] = 1;

                        emptySlot = true;
                        yield return explore;
                    }
                }
            }

            if (!emptySlot)
            {
                yield return newGame;
            }
        }
    }
}