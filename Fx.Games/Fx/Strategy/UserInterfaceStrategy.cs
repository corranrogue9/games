namespace Fx.Game
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    public sealed class UserInterfaceStrategy<TGame, TBoard, TMove, TPlayer> : IStrategy<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        private readonly IDisplayer<TGame, TBoard, TMove, TPlayer> displayer;

        public UserInterfaceStrategy(IDisplayer<TGame, TBoard, TMove, TPlayer> displayer)
        {
            if (displayer == null)
            {
                throw new ArgumentNullException(nameof(displayer));
            }

            this.displayer = displayer;
        }

        public TMove SelectMove(TGame game)
        {
            /*while (true)
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
            }*/

            return this.displayer.ReadMoveSelection(game);
        }
    }
}
