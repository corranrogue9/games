namespace Fx.Displayer
{
    using Fx.Game;

    public sealed class PegGameConsoleDisplayer<TPlayer> : IDisplayer<PegGame<TPlayer>, PegBoard, PegMove, TPlayer>
    {
        public void DisplayBoard(PegGame<TPlayer> game)
        {
            for (int i = 0; i < game.Board.Triangle.Length; ++i)
            {
                Console.Write(new string(' ', game.Board.Triangle.Length - i));
                for (int j = 0; j < game.Board.Triangle[i].Length; ++j)
                {
                    Console.Write(game.Board.Triangle[i][j] == Peg.Empty ? "o " : "* ");
                }

                Console.WriteLine();
            }
        }

        public void DisplayMoves(PegGame<TPlayer> game)
        {
            int i = 0;
            foreach (var move in game.Moves)
            {
                Console.WriteLine($"{i++}: ({move.Start.Item1},{move.Start.Item2}) => ({move.End.Item1}, {move.End.Item2})");
            }
        }

        public void DisplayOutcome(PegGame<TPlayer> game)
        {
            if (game.Outcome.Winners.Any())
            {
                Console.WriteLine("You win!");
            }
            else
            {
                Console.WriteLine("You lose!");
            }
        }

        public PegMove ReadMoveSelection(PegGame<TPlayer> game)
        {
            var moves = game.Moves.ToList();
            while (true)
            {
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
