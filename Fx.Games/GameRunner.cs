namespace ConsoleApplication4
{
    using Fx.Game;

    public sealed class GameRunner<TMove, TPlayer, TGame> where TGame : IGame<TMove, TPlayer, TGame>
    {
        private readonly TGame game;

        private readonly IReadOnlyDictionary<TPlayer, IStrategy<TMove, TPlayer, TGame>> strategies;

        public GameRunner(TGame game, IReadOnlyDictionary<TPlayer, IStrategy<TMove, TPlayer, TGame>> strategies)
        {
            this.game = game;
            this.strategies = strategies;
        }

        public Outcome<TPlayer> Start()
        {
            var games = new List<TGame>();
            var game = this.game;
            games.Add(game);
            game.Display();
            while (game.Outcome == null)
            {
                var moves = game.LegalMoves;
                var strategy = this.strategies[game.CurrentPlayer];
                var move = strategy.SelectMove(game);
                game = game.CommitMove(move);
                games.Add(game);
                ////System.Threading.Thread.Sleep(1000);
                game.Display();
            }

            /*Console.WriteLine("starting game...");
            Console.ReadLine();
            foreach (var gamestate in games)
            {
                gamestate.Display();
                System.Threading.Thread.Sleep(1000);
            }*/

            return game.Outcome;
        }
    }
}