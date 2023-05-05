namespace Fx.Games
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    /// <threadsafety instance="true"/>
    public interface IGame<out TGame, out TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard ,TMove, TPlayer>
    {
        TPlayer CurrentPlayer { get; }

        TGame CommitMove(TMove move);

        IEnumerable<TMove> Moves { get; }

        TBoard Board { get; }

        Outcome<TPlayer> Outcome { get; }
    }
}
