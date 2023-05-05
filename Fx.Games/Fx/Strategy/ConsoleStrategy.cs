namespace Fx.Strategy
{
    using Fx.Game;

    public sealed class ConsoleStrategy<TMove, TPlayer, TGame> : IStrategy<TMove, TPlayer, TGame> where TGame : IGame<TMove, TPlayer, TGame>
    {
        private readonly Func<TMove, string> displayMove;

        public ConsoleStrategy(Func<TMove, string> displayMove)
        {
            this.displayMove = displayMove;
        }

        public TMove SelectMove(TGame game)
        {
            var movesList = game.LegalMoves.ToList();
            for (int i = 0; i < movesList.Count; ++i)
            {
                Console.WriteLine($"{i}: {displayMove(movesList[i])}");
            }

            int index;
            var moveSelected = false;
            do
            {
                Console.Write("Select move number: ");
                var indexInput = Console.ReadLine();
                if (!int.TryParse(indexInput, out index) || index < 0 || index >= movesList.Count)
                {
                    ////Console.WriteLine("Invalid input! Please enter a number corresponding to a move.");
                    moveSelected = true;
                }
                else
                {
                    moveSelected = true;
                }
            }
            while (!moveSelected);

            return movesList[index];
        }
    }
}