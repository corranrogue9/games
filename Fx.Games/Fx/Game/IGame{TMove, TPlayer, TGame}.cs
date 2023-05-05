namespace Fx.Game
{
    public interface IGame<TMove, TPlayer, TGame> where TGame : IGame<TMove, TPlayer, TGame>
    {
        IEnumerable<TMove> LegalMoves { get; }

        TGame CommitMove(TMove move);

        TPlayer CurrentPlayer { get; }

        Outcome<TPlayer> Outcome { get; }

        void Display();
    }
}