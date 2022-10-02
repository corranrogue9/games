namespace ConsoleApplication1
{
    using System;
    using System.Collections.Generic;

    using Fx.Games;
    using Fx.Games.TicTacToe;
    using Fx.Games.Bobble;

    class Program
    {



        static void Main(string[] args)
        {
      

            Console.WriteLine("Provide SKU:");
            var sku = int.Parse(Console.ReadLine());
            switch (sku)
            {
                case 0: // two player console tic tac toe
                    TwoPlayerConsoleTicTacToe();
                    break;
                case 1:
                    var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
                    var computer = "max";
                    var gdebruin = "gdebruin";
                    var game = new TicTacToe<string>(computer, gdebruin);
                    var driver = Driver.Create(
                        new Dictionary<string, IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>>
                        {
                            { computer, MaximizeMovesStrategy.Default<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>() },
                            { gdebruin, new UserInterfaceStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>(displayer) },
                        },
                        displayer);
                    var result = driver.Run(game);
                    break;
                case 2:
                    Bobble();
                    break;
                default:
                    throw new Exception("bad sku given");
            }

            Console.ReadLine();
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




        static void Bobble()
        {
            var displayer = new BobbleConsoleDisplayer<string>(_ => _);
            var computer = "max";
            var gdebruin = "gdebruin";
            var game = new Bobble<string>(computer, gdebruin);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<Bobble<string>, BobbleBoard, BobbleMove, string>>
                {
                    { computer, MaximizeMovesStrategy.Default<Bobble<string>, BobbleBoard, BobbleMove, string>() },
                    { gdebruin, new UserInterfaceStrategy<Bobble<string>, BobbleBoard, BobbleMove, string>(displayer) },                    
                },
                displayer);
            var result = driver.Run(game);
        }


    }
}
