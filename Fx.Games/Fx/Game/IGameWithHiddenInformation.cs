namespace Fx.Game
{
    public interface IGameWithHiddenInformation<TMove, TPlayer, TGame> : Fx.Game.IGame<TMove, TPlayer, TGame> where TGame : IGameWithHiddenInformation<TMove, TPlayer, TGame>
    {
        IEnumerable<TGame> ExploreMove(TMove move);
    }
}