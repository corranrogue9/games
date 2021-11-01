namespace Fx.Games
{
    using System.Linq;

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
                return newGame.Moves.Average(yourMove => newGame.CommitMove(yourMove).Moves.Count());
            });
        }
    }
}
