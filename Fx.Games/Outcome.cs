namespace Fx.Games
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    public sealed class Outcome<TPlayer>
    {
        private readonly IEnumerable<TPlayer> winners;

        public Outcome(IEnumerable<TPlayer> winners)
        {
            this.winners = winners.ToList();
        }

        public IEnumerable<TPlayer> Winners
        {
            get
            {
                foreach (var element in winners)
                {
                    yield return element;
                }
            }
        }
    }
}
