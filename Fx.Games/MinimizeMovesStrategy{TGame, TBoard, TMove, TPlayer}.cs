namespace Fx.Games
{
    using System;
    using System.Collections;
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
            return game.Moves.Select(move => Tuple.Create(move, GetWins(game.CommitMove(move)))).Maximum(tuple => tuple.Item2).Item1;
        }

        private IEnumerable<Tuple<TMove, int>> FindWins(TGame game)
        {
        }

        private int GetWins(TGame game)
        {
            if (game.Outcome != null)
            {
                if (game.Outcome.Winners.Contains(this.player))
                {
                    return 1;
                }
                else if (!game.Outcome.Winners.Any())
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }

            return game
                .Moves
                .Where(move => this.rng.NextDouble() < this.branchingFactor)
                .Select(move => GetWins(game.CommitMove(move)))
                .Sum();
        }
    }
}
