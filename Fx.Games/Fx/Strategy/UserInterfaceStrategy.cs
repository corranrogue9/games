namespace Fx.Strategy
{
    using Fx.Displayer;
    using Fx.Game;

    public static class UserInterfaceStrategy
    {
        public static UserInterfaceStrategy<TGame, TBoard, TMove, TPlayer> Create<TGame, TBoard, TMove, TPlayer>(IDisplayer<TGame, TBoard, TMove, TPlayer> displayer)
            where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            return new UserInterfaceStrategy<TGame, TBoard, TMove, TPlayer>(displayer);
        }
    }
}
