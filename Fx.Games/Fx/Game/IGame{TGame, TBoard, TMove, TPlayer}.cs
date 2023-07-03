namespace Fx.Game
{
    using System.Collections.Generic;

    /// <summary>
    /// A turn-based game engine at a current state within the game it represents
    /// </summary>
    /// <typeparam name="TGame">The type of the game that is being represented</typeparam>
    /// <typeparam name="TBoard">The type of the board that the <typeparamref name="TGame"/> uses</typeparam>
    /// <typeparam name="TMove">The type of the moves that the <typeparamref name="TGame"/> uses</typeparam>
    /// <typeparam name="TPlayer">The type of the player that is playing the <typeparamref name="TGame"/></typeparam>
    /// <threadsafety instance="true"/>
    public interface IGame<out TGame, out TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard ,TMove, TPlayer>
    {
        /// <summary>
        /// The <typeparamref name="TPlayer"/> whose turn it currently is
        /// </summary>
        TPlayer CurrentPlayer { get; }

        /// <summary>
        /// Commits <paramref name="move"/> to the current board state of the game
        /// </summary>
        /// <param name="move">The move that is being committed for the <see cref="CurrentPlayer"/></param>
        /// <returns>The board state of the game after <paramref name="move"/> has been committed</returns>
        /// <exception cref="IllegalMoveExeption">Thrown if <paramref name="move"/> is not a legal move for the current board state of the game</exception>
        TGame CommitMove(TMove move);

        /// <summary>
        /// The legal moves in the current board state
        /// </summary>
        IEnumerable<TMove> Moves { get; }

        /// <summary>
        /// The current board state of the game
        /// </summary>
        TBoard Board { get; }

        /// <summary>
        /// The results of the game as of the current state. <see langword="null"/> indicates that the game has not yet ended. //// TODO games can have players with an outcome without the game being over (for example, players can have already lost the game without the game being over)
        /// </summary>
        Outcome<TPlayer> Outcome { get; }
    }

    public interface IGameWithHiddenInformation<out TGame, out TBoard, TMove, TPlayer> : IGame<TGame, TBoard, TMove, TPlayer> where TGame : IGameWithHiddenInformation<TGame, TBoard, TMove, TPlayer>
    {
        /// <summary>
        /// The sum of the weights of the resulting sequence must equal 1.0
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        IEnumerable<IWeightedGame<TGame, TBoard, TMove, TPlayer>> ExploreMove(TMove move);
    }

    public interface IWeightedGame<out TGame, out TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        public TGame Game { get; }

        public double Weight { get; }
    }

    public sealed class WeightedGame<TGame, TBoard, TMove, TPlayer> : IWeightedGame<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        public WeightedGame(TGame game, double weight)
        {
            this.Game = game;
            this.Weight = weight;
        }

        public TGame Game { get; }

        public double Weight { get; }
    }
}
