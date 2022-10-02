namespace Fx.Games
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    public sealed class MinimizeMovesStrategy<TGame, TBoard, TMove, TPlayer> : IStrategy<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        private MinimizeMovesStrategy()
        {
        }

        internal static readonly MinimizeMovesStrategy<TGame, TBoard, TMove, TPlayer> Default = new MinimizeMovesStrategy<TGame, TBoard, TMove, TPlayer>();

        public TMove SelectMove(TGame game)
        {
            return game.Moves.Minimum(move => game.CommitMove(move).Moves.Count());
        }
    }



    public sealed class MonteCarloStrategy<TGame, TBoard, TMove, TPlayer> : IStrategy<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        private readonly double branchingFactor;
        private readonly Random rng;
        private readonly TPlayer player;

        public MonteCarloStrategy(double branchingFactor, System.Random rng, TPlayer player)
        {
            this.branchingFactor = branchingFactor;
            this.rng = rng;
            this.player = player;
        }
        
        public TMove SelectMove(TGame game)
        {
            //// TODO driver doesn't handle a game that isn't over but a player doesn't have legal moves

            ////return FindWins(game).Maximum(tuple => tuple.Item2).Item1;
            var winAggregations = FindWins(game).ToList();

            var opponentDoesntWin = winAggregations.Where(tuple => tuple.Item2.Opponent == 0).ToList();
            if (opponentDoesntWin.Count != 0)
            {
                return opponentDoesntWin[0].Item1;
            }

            return winAggregations.Maximum(tuple => tuple.Item2.Me).Item1;
        }

        private IEnumerable<Tuple<TMove, WinCount>> FindWins(TGame game)
        {
            foreach (var move in game.Moves)
            {

                yield return Tuple.Create(move, GetWins(game.CommitMove(move)));
            }
        }

        private sealed class WinCount
        {
            public int Me { get; set; }

            public int Opponent { get; set; }

            public int Draw { get; set; }
        }

        private WinCount GetWins(TGame game)
        {
            if (game.Outcome != null)
            {
                if (game.Outcome.Winners.Contains(this.player))
                {
                    return new WinCount() { Me = 1 };
                }
                else if (!game.Outcome.Winners.Any())
                {
                    return new WinCount() { Draw = 1 };
                }
                else
                {
                    return new WinCount() { Opponent = 1 };
                }
            }

            var subset = game
                .Moves
                .Where(move => this.rng.NextDouble() < this.branchingFactor);
            return GetSubset(game, subset)
                .Aggregate(new WinCount(), (accumalate, wins) =>
                {
                    accumalate.Me += wins.Me;
                    accumalate.Draw += wins.Draw;
                    accumalate.Opponent += wins.Opponent;
                    return accumalate;
                });
        }

        private IEnumerable<WinCount> GetSubset(TGame game, IEnumerable<TMove> moves)
        {
            foreach (var move in moves)
            {
                yield return GetWins(game.CommitMove(move));
            }
        }
    }
}
