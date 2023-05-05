namespace Fx.Strategy
{
    using Fx.Game;

    public sealed class RandomStrategy<TMove, TPlayer, TGame> : IStrategy<TMove, TPlayer, TGame> where TGame : IGame<TMove, TPlayer, TGame>
    {
        private readonly Random random;

        public RandomStrategy(Random random)
        {
            this.random = random;
        }

        public TMove SelectMove(TGame game)
        {
            ////System.Threading.Thread.Sleep(500);
            var movesList = game.LegalMoves.ToList();
            var moveIndex = random.Next(0, movesList.Count);
            return movesList[moveIndex];
        }
    }
}