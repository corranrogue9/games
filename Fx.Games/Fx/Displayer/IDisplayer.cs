namespace Fx.Displayer
{
    using Fx.Game;

    /// <summary>
    /// 
    /// </summary>
    /// <threadsafety instance="true"/>
    public interface IDisplayer<in TGame, out TBoard, out TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        void DisplayBoard(TGame game);

        void DisplayOutcome(TGame game);

        void DisplayMoves(TGame game);

        TMove ReadMoveSelection(TGame game);
    }
}
