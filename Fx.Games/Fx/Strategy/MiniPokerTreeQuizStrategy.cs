namespace Fx.Strategy
{
    using Fx.Game;

    public sealed class MiniPokerTreeQuizStrategy<TPlayer> : IStrategy<MiniPoker<TPlayer>, MiniPokerBoard, MiniPokerMove, TPlayer>
    {
        public MiniPokerMove SelectMove(MiniPoker<TPlayer> game)
        {
            throw new NotImplementedException();
        }
    }
}
