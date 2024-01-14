namespace Fx.Game
{
    using System.Collections.Generic;

    public sealed class WinnersAndLosers<TPlayer>
    {
        public WinnersAndLosers(IEnumerable<TPlayer> winners, IEnumerable<TPlayer> losers, IEnumerable<TPlayer> drawers)
        {
            this.Winners = winners;
            this.Losers = losers;
            this.Drawers = drawers;
        }

        public IEnumerable<TPlayer> Winners { get; }

        public IEnumerable<TPlayer> Losers { get; }

        public IEnumerable<TPlayer> Drawers { get; }
    }
}
