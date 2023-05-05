﻿namespace Fx.Game
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    /// <threadsafety instance="true"/>
    public interface IGame<out TGame, out TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard ,TMove, TPlayer>
    {
        TPlayer CurrentPlayer { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        /// <exception cref="IllegalMoveExeption"></exception>
        TGame CommitMove(TMove move);

        IEnumerable<TMove> Moves { get; }

        TBoard Board { get; }

        Outcome<TPlayer> Outcome { get; }
    }
}
