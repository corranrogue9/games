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
            while (true)
            {
                Console.WriteLine("Select a move:");
                var moves = game.Moves.ToList();
                int i = 0;
                foreach (var move in moves)
                {
                    Console.WriteLine($"{i++}: {this.toString(move)}");
                }

                Console.WriteLine();

                var input = Console.ReadLine();
                if (!int.TryParse(input, out var selectedMove) || selectedMove >= moves.Count)
                {
                    Console.WriteLine($"The input '{input}' was not the index of a legal move");
                    continue;
                }

                return moves[selectedMove];
            }
        }
    }
}
