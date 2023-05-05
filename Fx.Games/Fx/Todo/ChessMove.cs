namespace Fx.Todo
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    public sealed class ChessMove
    {
        public Tuple<int, int> Start { get; }

        public Tuple<int, int> End { get; }
    }
}
