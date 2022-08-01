namespace ConsoleApplication1
{
    using System;
    using System.Collections.Generic;

    using Fx.Games;
    using Fx.Games.TicTacToe;

    class Program
    {
        static void Main(string[] args)
        {
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var consoleStrategy = new UserInterfaceStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>(displayer);
            var randomStrategy = new RandomStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>();
            var chrispre = "chrispre";
            var gdebruin = "gdebruin";
            var game = new TicTacToe<string>(chrispre, gdebruin);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>>
                {
                    { chrispre, consoleStrategy },
                    { gdebruin, randomStrategy },
                }, 
                displayer);
            var result = driver.Run(game);
            Console.ReadLine();
        }
    }
}
