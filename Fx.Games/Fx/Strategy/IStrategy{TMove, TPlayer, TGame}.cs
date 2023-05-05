namespace ConsoleApplication4
{
    using Fx.Game;

    public interface IStrategy<TMove, TPlayer, TGame> where TGame : IGame<TMove, TPlayer, TGame>
    {
        TMove SelectMove(TGame game);
    }
}