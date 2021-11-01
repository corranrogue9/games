namespace Fx.Games
{
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
}
