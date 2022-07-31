namespace Fx.Games
{
    /// <summary>
    /// 
    /// </summary>
    /// <threadsafety instance="true"/>
    public interface IDisplayer<in TBoard, TPlayer>
    {
        void DisplayBoard(TBoard board);

        void DisplayOutcome(Outcome<TPlayer> outcome);
    }
}
