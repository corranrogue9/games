using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    using Fx.Games;

    class Program
    {
        static void Main(string[] args)
        {
            var games = new[]
            {
                "Checkers",
            };

            for (int i = 0; i < games.Length; ++i)
            {
                Console.WriteLine($"{i}: {games[i]}");
            }

            Console.WriteLine();
            Console.Write("Select game: ");
            var gameIndex = Console.ReadLine();
            var game = games[int.Parse(gameIndex)];
            if (game == "Checkers")
            {
                Checkers();
            }
        }

        static void Checkers()
        {
            var whiteStrategy = new ConsoleStrategy<Checkers<string>, CheckerBoard, CheckerMove, string>(
                move => $"From ({move.Initial.Row},{move.Initial.Column}) to ({move.Final.Row},{move.Final.Column})");
            var driver = Driver.Create(new[] { whiteStrategy, whiteStrategy }, new CheckerConsoleDisplayer());
            driver.Run(new Checkers<string>("garrett", "christof"));
        }
    }
}
