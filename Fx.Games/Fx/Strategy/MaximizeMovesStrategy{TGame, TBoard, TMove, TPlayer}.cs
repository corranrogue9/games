namespace Fx.Strategy
{
    using System.Linq;

    using Fx.Game;

    /// <summary>
    /// 
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    public sealed class MaximizeMovesStrategy<TGame, TBoard, TMove, TPlayer> : IStrategy<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        private MaximizeMovesStrategy()
        {
        }

        internal static readonly MaximizeMovesStrategy<TGame, TBoard, TMove, TPlayer> Default = new MaximizeMovesStrategy<TGame, TBoard, TMove, TPlayer>();

        public TMove SelectMove(TGame game)
        {
            return game.Moves.Maximum(myMove =>
            {
                var newGame = game.CommitMove(myMove);
                var newMoves = newGame.Moves.ToList();
                if (newMoves.Count == 0)
                {
                    return 0;
                }

                return newMoves.Average(yourMove => newGame.CommitMove(yourMove).Moves.Count());
            });
        }
    }
}
