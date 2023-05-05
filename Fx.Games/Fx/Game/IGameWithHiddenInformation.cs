namespace ConsoleApplication4
{
    public interface IGameWithHiddenInformation<TMove, TPlayer, TGame> : IGame<TMove, TPlayer, TGame> where TGame : IGameWithHiddenInformation<TMove, TPlayer, TGame>
    {
        IEnumerable<TGame> ExploreMove(TMove move);
    }
}