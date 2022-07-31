namespace ConsoleApplication1
{
    using System;

    using Fx.Games;
    using Fx.Games.TicTacToe;

    class Program
    {
        static void Main(string[] args)
        {
            Func<TicTacToeMove, string> moveToString = move => $"{move.Row}, {move.Column}";
            var consoleStrategy = new ConsoleStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>(moveToString);
            var game = new TicTacToe<string>("chrispre", "gdebruin");
            var driver = Driver.Create(new[] { consoleStrategy, consoleStrategy }, new TicTacToeConsoleDisplayer<string>(_ => _));
            var result = driver.Run(game);
            Console.ReadLine();
        }
    }
}
