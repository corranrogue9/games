using System;

namespace Fx.Games
{
    /// <summary>
    /// 
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    public sealed class CheckerConsoleDisplayer : IDisplayer<CheckerBoard, string>
    {
        public void DisplayBoard(CheckerBoard board)
        {
            for (int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    var piece = board.GetPiece(new Position(i, j));
                    if (piece == null)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        var character = piece.IsBlack ? 'b' : 'w';
                        if (piece.IsQueen)
                        {
                            character = char.ToUpper(character);
                        }

                        Console.Write(character);
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public void DisplayOutcome(Outcome<string> player)
        {
            Console.WriteLine($"Winner: {string.Join(",", player.Winners)}");
        }
    }
}
