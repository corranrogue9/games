namespace Fx.Displayer
{
    using Fx.Game;

    /// <summary>
    /// Represents the input/output interactions between a user and a <typeparamref name="TGame"/>
    /// </summary>
    /// <typeparam name="TGame">The type of the game that is being played</typeparam>
    /// <typeparam name="TBoard">The type of the board that the <typeparamref name="TGame"/> uses</typeparam>
    /// <typeparam name="TMove">The type of the moves that the <typeparamref name="TGame"/> uses</typeparam>
    /// <typeparam name="TPlayer">The type of the player that is playing the <typeparamref name="TGame"/></typeparam>
    /// <threadsafety instance="true"/>
    public interface IDisplayer<in TGame, out TBoard, out TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        /// <summary>
        /// Outputs the board for current state of <paramref name="game"/>
        /// </summary>
        /// <param name="game">The game to display the board of</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="game"/> is <see langword="null"/></exception>
        void DisplayBoard(TGame game);

        /// <summary>
        /// Outputs the outcome for the current state of <paramref name="game"/>
        /// </summary>
        /// <param name="game">The game to display the outcome of</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="game"/> is <see langword="null"/></exception>
        void DisplayOutcome(TGame game);

        /// <summary>
        /// Outputs the moves available for the current state of <paramref name="game"/>
        /// </summary>
        /// <param name="game">The game to display the moves of</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="game"/> is <see langword="null"/></exception>
        void DisplayMoves(TGame game);

        /// <summary>
        /// Retrieves the move selected by the current player of <paramref name="game"/>
        /// </summary>
        /// <param name="game">The game to get a move for</param>
        /// <returns>The move that was selected by the player</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="game"/> is <see langword="null"/></exception>
        TMove ReadMoveSelection(TGame game);
    }
}
