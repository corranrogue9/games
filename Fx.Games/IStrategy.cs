namespace Fx.Games
{
    /// <summary>
    /// 
    /// </summary>
    /// <threadsafety instance="true"/>
    public interface IStrategy<in TGame, out TBoard, out TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        TMove SelectMove(TGame game);
    }
}
