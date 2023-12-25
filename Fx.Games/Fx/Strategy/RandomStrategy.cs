namespace Fx.Strategy
{
    using Fx.Game;

    public static class RandomStrategyExtensions
    {
        public static RandomStrategy<TGame, TBoard, TMove, TPlayer> RandomStrategy<TGame, TBoard, TMove, TPlayer>(this IGame<TGame, TBoard, TMove, TPlayer> game)
            where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            return new RandomStrategy<TGame, TBoard, TMove, TPlayer>();
        }
    }
}
