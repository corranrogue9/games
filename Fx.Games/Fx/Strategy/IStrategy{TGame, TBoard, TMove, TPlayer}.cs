namespace Fx.Strategy
{
    using Fx.Game;

    /// <summary>
    /// A strategy to employ when playing a <see cref="IGame{TGame, TBoard, TMove, TPlayer}"/>
    /// </summary>
    /// <threadsafety instance="true"/>
    public interface IStrategy<in TGame, out TBoard, out TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        /// <summary>
        /// Determine which move of <paramref name="game"/> should be committed next
        /// </summary>
        /// <param name="game">The game which another move needs to be committed to</param>
        /// <returns>The move that should be committed to <paramref name="game"/></returns>
        TMove SelectMove(TGame game);
    }
}
