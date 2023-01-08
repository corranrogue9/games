namespace Fx.Game
{
    using System.Collections.Generic;

    public sealed class Outcome<TPlayer>
    {
        public Outcome(IEnumerable<TPlayer> winners)
        {
            this.Winners = winners;
        }

        public IEnumerable<TPlayer> Winners { get; }
    }
}
