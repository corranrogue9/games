namespace Fx.Games
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    public sealed class ChessMove
    {
        public Tuple<int, int> Start => Tuple.Create(0, 0);

        public Tuple<int, int> End => Tuple.Create(0, 0);
    }
}
