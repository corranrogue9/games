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
            int sku = GetSkuFromArgsOrConsole(args);
            switch (sku)
            {
                case 0: // two player console tic tac toe
                    TwoPlayerConsoleTicTacToe();
                    break;
                case 1:
                    TicTacToeHumanVsMaximizeMoves();
                    break;
                default:
                    throw new Exception("bad sku given");
            }

            Console.WriteLine("hit enter to exit");
            Console.ReadLine();
        }

        private static void TicTacToeHumanVsMaximizeMoves()
        {
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var computer = "max";
            var human = "human";
            var game = new TicTacToe<string>(computer, human);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>>
                {
                            { computer, MaximizeMovesStrategy.Default<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>() },
                            { human, new UserInterfaceStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>(displayer) },
                },
                displayer);
            var result = driver.Run(game);
        }

        private static void TwoPlayerConsoleTicTacToe()
        {
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var consoleStrategy = new UserInterfaceStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>(displayer);
            var chrispre = "chrispre";
            var gdebruin = "gdebruin";
            var game = new TicTacToe<string>(chrispre, gdebruin);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>>
                {
                    { chrispre, consoleStrategy },
                    { gdebruin, consoleStrategy },
                },
                displayer);
            var result = driver.Run(game);
        }

        private static int GetSkuFromArgsOrConsole(string[] args)
        {
            if (args.Length == 1 && int.TryParse(args[0], out var num))
            {
                return num;
            }
            Console.WriteLine("Provide SKU:");
            var sku = int.Parse(Console.ReadLine());
            return sku;
        }
    }
}
