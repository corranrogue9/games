namespace Fx.Games
{
    using System;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    public sealed class ConsoleStrategy<TGame, TBoard, TMove, TPlayer> : IStrategy<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        private readonly Func<TMove, string> toString;

        public ConsoleStrategy(Func<TMove, string> toString)
        {
            this.toString = toString;
        }

        public TMove SelectMove(TGame game)
        {
            Console.WriteLine("Select a move:");
            var moves = game.Moves.ToList();
            int i = 0;
            foreach (var move in moves)
            {
                Console.WriteLine($"{i}: {this.toString(move)}");
            }

            Console.WriteLine();
            return moves[int.Parse(Console.ReadLine())];
        }
    }
}
