namespace Fx.Game
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Security;

    /// <summary>
    /// A turn-based game engine at a current state within the game it represents
    /// </summary>
    /// <typeparam name="TGame">The type of the game that is being represented</typeparam>
    /// <typeparam name="TBoard">The type of the board that the <typeparamref name="TGame"/> uses</typeparam>
    /// <typeparam name="TMove">The type of the moves that the <typeparamref name="TGame"/> uses</typeparam>
    /// <typeparam name="TPlayer">The type of the player that is playing the <typeparamref name="TGame"/></typeparam>
    /// <threadsafety instance="true"/>
    public interface IGame<out TGame, out TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        /// <summary>
        /// The <typeparamref name="TPlayer"/> whose turn it currently is
        /// </summary>
        TPlayer CurrentPlayer { get; }

        /// <summary>
        /// Commits <paramref name="move"/> to the current board state of the game
        /// </summary>
        /// <param name="move">The move that is being committed for the <see cref="CurrentPlayer"/></param>
        /// <returns>The board state of the game after <paramref name="move"/> has been committed</returns>
        /// <exception cref="IllegalMoveExeption">Thrown if <paramref name="move"/> is not a legal move for the current board state of the game</exception>
        TGame CommitMove(TMove move);

        /// <summary>
        /// The legal moves in the current board state
        /// </summary>
        IEnumerable<TMove> Moves { get; }

        /// <summary>
        /// The current board state of the game
        /// </summary>
        TBoard Board { get; }

        /// <summary>
        /// The results of the game as of the current state. <see langword="null"/> indicates that the game has not yet ended. //// TODO games can have players with an outcome without the game being over (for example, players can have already lost the game without the game being over)
        /// </summary>
        Outcome<TPlayer> Outcome { get; }
    }

    public interface IGameWithHiddenInformation<out TGame, out TBoard, TMove, TPlayer, out TDistribution> : IGame<TGame, TBoard, TMove, TPlayer> where TGame : IGameWithHiddenInformation<TGame, TBoard, TMove, TPlayer, TDistribution> where TDistribution : Distribution<TGame>
    {
        /// <summary>
        /// TODO why does the tdistribution type parameter get around the covariance issue?
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        TDistribution ExploreMove(TMove move);
    }

    public interface IWeightedGame<out TGame, out TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        public TGame Game { get; }

        public double Weight { get; }
    }

    public sealed class WeightedGame<TGame, TBoard, TMove, TPlayer> : IWeightedGame<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        public WeightedGame(TGame game, double weight)
        {
            this.Game = game;
            this.Weight = weight;
        }

        public TGame Game { get; }

        public double Weight { get; }
    }

    /// <summary>
    /// TODO probably this isnt' enough of a distrubiotn to use that name
    /// 
    /// TODO for garrett to explore: if distribution weren't generic, but only the dervied types were, would this actualy give you a convenient way heterogenous generics?
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Distribution<T>
    {
        private Distribution(T value)
        {
            this.Value = value;
        }

        /// <summary>
        /// TODO should this go on the base type? it might give the wrong impression, and moving it to the derived types will require callers to understand the distriminated union
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// TODO different name
        /// </summary>
        public sealed class CompleteDistribution : Distribution<T>
        {
            public CompleteDistribution(T value)
                : base(value)
            {
            }
        }

        /// <summary>
        /// TODO different name
        /// </summary>
        public sealed class PartialDistribution : Distribution<T>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="liklihood">TODO different name</param>
            /// <param name="value"></param>
            /// <param name="remainder">TODO different name</param>
            public PartialDistribution(double liklihood, T value, Distribution<T> remainder)
                : base(value)
            {
                if (liklihood < 0 || liklihood >= 1.0)
                {
                    //// TODO don't want 1.0 because then this is no different froma complete distribution
                    throw new ArgumentOutOfRangeException(nameof(liklihood));
                }

                this.Liklihood = liklihood;
                this.Remainder = remainder;
            }

            public double Liklihood { get; }

            public Distribution<T> Remainder { get; }
        }
    }

    public static class DistributionExtensions
    {
        public static T Sample<T>(this Distribution<T> distribution)
        {
            return Sample(distribution, new Random());
        }

        public static T Sample<T>(this Distribution<T> distribution, Random random)
        {
            if (distribution is Distribution<T>.PartialDistribution partialDistribution)
            {
                //// TODO next double is exclusive of 1.0, so does this break anything?
                var weight = random.NextDouble();
                return Sample(partialDistribution, weight);
            }
            else
            {
                return distribution.Value;
            }
        }

        public static T Sample<T>(this Distribution<T>.PartialDistribution distribution, double weight)
        {
            if (weight < distribution.Liklihood)
            {
                return distribution.Value;
            }
            else
            {
                if (distribution.Remainder is Distribution<T>.PartialDistribution partialDistribution)
                {
                    return Sample(partialDistribution, weight - distribution.Liklihood); //// TODO just subtraction won't work here i think
                }
                else
                {
                    return distribution.Remainder.Value;
                }
            }
        }
    }

    public static class GameWithHiddenInformationExtensions
    {
        public static TGame CommitSpecificMove<TGame, TBoard, TMove, TPlayer, TDistribution>(this IGameWithHiddenInformation<TGame, TBoard, TMove, TPlayer, TDistribution> game, TMove move, Random random)
            where TGame : IGameWithHiddenInformation<TGame, TBoard, TMove, TPlayer, TDistribution>
            where TDistribution : Distribution<TGame>
        {
            var potentialGames = game.ExploreMove(move);
            return potentialGames.Sample(random);
        }
    }
}
