namespace ConsoleApplication1
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using Fx;
    using Fx.Displayer;
    using Fx.Driver;
    using Fx.Game;
    using Fx.Strategy;
    using Fx.Todo;
    using Fx.Tree;

    internal static class Extensions
    {
        public static IReadOnlyList<(TFirst First, TSecond Second)> Zip<TFirst, TSecond, TResult>(this IReadOnlyList<TFirst> first, IReadOnlyList<TSecond> second)
        {
            return first.Zip(second, (a, b) => (a, b));
        }

        public static IReadOnlyList<TResult> Zip<TFirst, TSecond, TResult>(this IReadOnlyList<TFirst> first, IReadOnlyList<TSecond> second, Func<TFirst, TSecond, TResult> zipper)
        {
            Ensure.NotNull(first, nameof(first));
            Ensure.NotNull(second, nameof(second));
            Ensure.NotNull(zipper, nameof(zipper));

            return new ZipList<TFirst, TSecond, TResult>(first, second, zipper);
        }

        private sealed class ZipList<TFirst, TSecond, TResult> : IReadOnlyList<TResult>
        {
            private readonly IReadOnlyList<TFirst> first;

            private readonly IReadOnlyList<TSecond> second;

            private readonly Func<TFirst, TSecond, TResult> zipper;

            public ZipList(IReadOnlyList<TFirst> first, IReadOnlyList<TSecond> second, Func<TFirst, TSecond, TResult> zipper)
            {
                this.first = first;
                this.second = second;
                this.zipper = zipper;
            }

            public TResult this[int index]
            {
                get
                {
                    return this.zipper(this.first[index], this.second[index]);
                }
            }

            public int Count
            {
                get
                {
                    return Math.Min(this.first.Count, this.second.Count);
                }
            }

            public IEnumerator<TResult> GetEnumerator()
            {
                return this.first.AsEnumerable().Zip(this.second, this.zipper).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        public static IReadOnlyList<TResult> Select<TSource, TResult>(this IReadOnlyList<TSource> source, Func<TSource, TResult> selector)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(selector, nameof(selector));

            return new SelectList<TSource, TResult>(source, selector);
        }

        private sealed class SelectList<TSource, TResult> : IReadOnlyList<TResult>
        {
            private readonly IReadOnlyList<TSource> source;

            private readonly Func<TSource, TResult> selector;

            public SelectList(IReadOnlyList<TSource> source, Func<TSource, TResult> selector)
            {
                this.source = source;
                this.selector = selector;
            }

            public TResult this[int index]
            {
                get
                {
                    return this.selector(this.source[index]);
                }
            }

            public int Count
            {
                get
                {
                    return this.source.Count;
                }
            }

            public IEnumerator<TResult> GetEnumerator()
            {
                return this.source.AsEnumerable().Select(this.selector).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        public static IReadOnlyList<T> Concat<T>(this IReadOnlyList<T> first, IReadOnlyList<T> second)
        {
            Ensure.NotNull(first, nameof(first));
            Ensure.NotNull(second, nameof(second));

            return new ConcatList<T>(first, second);
        }

        private sealed class ConcatList<T> : IReadOnlyList<T>
        {
            private readonly IReadOnlyList<T> first;

            private readonly IReadOnlyList<T> second;

            public ConcatList(IReadOnlyList<T> first, IReadOnlyList<T> second)
            {
                this.first = first;
                this.second = second;
            }

            public T this[int index]
            {
                get
                {
                    if (index < this.first.Count)
                    {
                        return this.first[index];
                    }

                    return this.second[index - this.first.Count];
                }
            }

            public int Count
            {
                get
                {
                    return this.first.Count + this.second.Count;
                }
            }

            public IEnumerator<T> GetEnumerator()
            {
                return this.first.AsEnumerable().Concat(this.second).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }

    internal static class Program
    {
        /*private static readonly IReadOnlyList<(string, Action)> games = new (string, Action)[]
        {
            (nameof(TicTacToeHumanVsHuman), TicTacToeHumanVsHuman),
            (nameof(TicTacToeHumanVsDecisionTree), TicTacToeHumanVsDecisionTree),
            (nameof(TicTacToeDecisionTreeVsHuman), TicTacToeDecisionTreeVsHuman),
            (nameof(TicTacToeTwoRandom), TicTacToeTwoRandom),
            (nameof(PegsHuman), PegsHuman),
            (nameof(PegsDecisionTree), PegsDecisionTree),
            (nameof(GobbleHumanVsHuman), GobbleHumanVsHuman),
            (nameof(GobbleHumanVsRandom), GobbleHumanVsRandom),
            (nameof(TwosHuman), TwosHuman),
            (nameof(TwosSevenMovesHeuristic12), TwosSevenMovesHeuristic12),
        };*/

        private delegate IStrategy<TGame, TBoard, TMove, TPlayer> StrategyFactory<TGame, TBoard, TMove, TPlayer>(TPlayer player) where TGame : IGame<TGame, TBoard, TMove, TPlayer>;

        private static IReadOnlyList<(string, StrategyFactory<TGame, TBoard, TMove, TPlayer>)> GeneralStrategyFactories<TGame, TBoard, TMove, TPlayer>(IEqualityComparer<TPlayer> comparer) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            return new (string, StrategyFactory<TGame, TBoard, TMove, TPlayer>)[]
            {
                (nameof(DecisionTreeStrategy<TGame, TBoard, TMove, TPlayer>), player => new DecisionTreeStrategy<TGame, TBoard, TMove, TPlayer>(player, comparer)),
                (nameof(MaximizeMovesStrategy), player => MaximizeMovesStrategy.Default<TGame, TBoard, TMove, TPlayer>()),
                (nameof(MinimizeMovesStrategy), player => MinimizeMovesStrategy.Default<TGame, TBoard, TMove, TPlayer>()),
                (nameof(MonteCarloStrategy<TGame, TBoard, TMove, TPlayer>), player => new MonteCarloStrategy<TGame, TBoard, TMove, TPlayer>(player, 0.2, comparer, new Random())),
                (nameof(RandomStrategy<TGame, TBoard, TMove, TPlayer>), player => new RandomStrategy<TGame, TBoard, TMove, TPlayer>()),
            };
        }

        private static IReadOnlyList<IStrategy<TGame, TBoard, TMove, string>> GetStrategiesFromConsole<TGame, TBoard, TMove>(
            IReadOnlyList<string> players,
            IReadOnlyList<(string, StrategyFactory<TGame, TBoard, TMove, string>)> strategyFactories)
            where TGame : IGame<TGame, TBoard, TMove, string>
        {
            return players.Select(player =>
            {
                Console.WriteLine($"Please select a strategy for {player}:");
                var choice = GetChoiceFromConsole(strategyFactories.Select(strategy => strategy.Item1));
                var strategyFactory = strategyFactories[choice];
                Console.WriteLine();
                return strategyFactory.Item2(player);
            });
        }

        #region TicTacToe
        private static IReadOnlyList<(string, StrategyFactory<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>)> TicTacToeStrategyFactories()
        {
            var generalStrategies = GeneralStrategyFactories<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>(StringComparer.OrdinalIgnoreCase);

            var specificStrategies = new (string, StrategyFactory<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>)[]
            {
                (nameof(GameTreeDepthStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>), player => new GameTreeDepthStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>(game => 0, null, player, StringComparer.OrdinalIgnoreCase)),
                (nameof(UserInterfaceStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>), player => new UserInterfaceStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>(new TicTacToeConsoleDisplayer<string>(_ => _))),
            };

            return generalStrategies.Concat(specificStrategies);
        }

        private static void TicTacToe()
        {
            var exes = "exes";
            var ohs = "ohs";
            Game(
                DelegateGameFactory.Create<TicTacToe<string>, TicTacToeBoard, TicTacToeMove>((player) => new TicTacToe<string>(exes, ohs)), //// TODO the fact that this ignores the player parameter indicates that something is very wrong...
                DelegateStrategyFactoryFactory.Create(TicTacToeStrategyFactories),
                new DelegatePlayerFactory(() => new[] { exes, ohs }),
                DelegateDisplayerFactory.Create(() => new TicTacToeConsoleDisplayer<string>(_ => _)));
        }
        #endregion

        #region Pegs
        private static IReadOnlyList<(string, StrategyFactory<PegGame<string>, PegBoard, PegMove, string>)> PegsStrategyFactories()
        {
            var generalStrategies = GeneralStrategyFactories<PegGame<string>, PegBoard, PegMove, string>(StringComparer.OrdinalIgnoreCase);

            var specificStrategies = new (string, StrategyFactory<PegGame<string>, PegBoard, PegMove, string>)[]
            {
                (nameof(GameTreeDepthStrategy<PegGame<string>, PegBoard, PegMove, string>), player => new GameTreeDepthStrategy<PegGame<string>, PegBoard, PegMove, string>(game => 0, null, player, StringComparer.OrdinalIgnoreCase)),
                (nameof(UserInterfaceStrategy<PegGame<string>, PegBoard, PegMove, string>), player => new UserInterfaceStrategy<PegGame<string>, PegBoard, PegMove, string>(new PegGameConsoleDisplayer<string>())),
            };

            return generalStrategies.Concat(specificStrategies);
        }

        private static void Pegs()
        {
            Game(
                DelegateGameFactory.Create<PegGame<string>, PegBoard, PegMove>((player) => new PegGame<string>(player)), //// TODO having to specify the generic type parameters here seems like a failure
                DelegateStrategyFactoryFactory.Create(PegsStrategyFactories),
                new DelegatePlayerFactory(() => new[] { "player" }),
                DelegateDisplayerFactory.Create(() => new PegGameConsoleDisplayer<string>()));
        }
        #endregion Pegs

        private interface IGameFactory<TGame, TBoard, TMove> where TGame : IGame<TGame, TBoard, TMove, string>
        {
            TGame Create(string player);
        }

        private static class DelegateGameFactory
        {
            public static DelegateGameFactory<TGame, TBoard, TMove> Create<TGame, TBoard, TMove>(Func<string, TGame> factory) where TGame : IGame<TGame, TBoard, TMove, string>
            {
                return new DelegateGameFactory<TGame, TBoard, TMove>(factory);
            }
        }

        private sealed class DelegateGameFactory<TGame, TBoard, TMove> : IGameFactory<TGame, TBoard, TMove> where TGame : IGame<TGame, TBoard, TMove, string>
        {
            private readonly Func<string, TGame> factory;

            public DelegateGameFactory(Func<string, TGame> factory) //// TODO try an implicit operator on delegate
            {
                Ensure.NotNull(factory, nameof(factory));

                this.factory = factory;
            }

            public TGame Create(string player)
            {
                return this.factory(player);
            }
        }

        private interface IDisplayerFactory<TGame, TBoard, TMove> where TGame : IGame<TGame, TBoard, TMove, string>
        {
            IDisplayer<TGame, TBoard, TMove, string> Create();
        }

        private static class DelegateDisplayerFactory
        {
            public static DelegateDisplayerFactory<TGame, TBoard, TMove> Create<TGame, TBoard, TMove>(Func<IDisplayer<TGame, TBoard, TMove, string>> factory) where TGame : IGame<TGame, TBoard, TMove, string>
            {
                return new DelegateDisplayerFactory<TGame, TBoard, TMove>(factory);
            }
        }

        private sealed class DelegateDisplayerFactory<TGame, TBoard, TMove> : IDisplayerFactory<TGame, TBoard, TMove> where TGame : IGame<TGame, TBoard, TMove, string>
        {
            private readonly Func<IDisplayer<TGame, TBoard, TMove, string>> factory;

            public DelegateDisplayerFactory(Func<IDisplayer<TGame, TBoard, TMove, string>> factory)
            {
                Ensure.NotNull(factory, nameof(factory));

                this.factory = factory;
            }

            public IDisplayer<TGame, TBoard, TMove, string> Create()
            {
                return this.factory();
            }
        }

        private interface IStrategyFactoryFactory<TGame, TBoard, TMove> where TGame : IGame<TGame, TBoard, TMove, string>
        {
            IReadOnlyList<(string, StrategyFactory<TGame, TBoard, TMove, string>)> Create();
        }

        private static class DelegateStrategyFactoryFactory
        {
            public static DelegateStrategyFactoryFactory<TGame, TBoard, TMove> Create<TGame, TBoard, TMove>(Func<IReadOnlyList<(string, StrategyFactory<TGame, TBoard, TMove, string>)>> factory) where TGame : IGame<TGame, TBoard, TMove, string>
            {
                return new DelegateStrategyFactoryFactory<TGame, TBoard, TMove>(factory);
            }
        }

        private sealed class DelegateStrategyFactoryFactory<TGame, TBoard, TMove> : IStrategyFactoryFactory<TGame, TBoard, TMove> where TGame : IGame<TGame, TBoard, TMove, string>
        {
            private readonly Func<IReadOnlyList<(string, StrategyFactory<TGame, TBoard, TMove, string>)>> factory;

            public DelegateStrategyFactoryFactory(Func<IReadOnlyList<(string, StrategyFactory<TGame, TBoard, TMove, string>)>> factory)
            {
                Ensure.NotNull(factory, nameof(factory));

                this.factory = factory;
            }

            public IReadOnlyList<(string, StrategyFactory<TGame, TBoard, TMove, string>)> Create()
            {
                return this.factory();
            }
        }

        private interface IPlayerFactory
        {
            IReadOnlyList<string> Create();
        }

        private sealed class DelegatePlayerFactory : IPlayerFactory
        {
            private readonly Func<IReadOnlyList<string>> factory;

            public DelegatePlayerFactory(Func<IReadOnlyList<string>> factory)
            {
                Ensure.NotNull(factory, nameof(factory));

                this.factory = factory;
            }

            public IReadOnlyList<string> Create()
            {
                return this.factory();
            }
        }

        private static void Game<TGame, TBoard, TMove>(
            IGameFactory<TGame, TBoard, TMove> gameFactory,
            IStrategyFactoryFactory<TGame, TBoard, TMove> strategyFactoryFactory,
            IPlayerFactory playerFactory,
            IDisplayerFactory<TGame, TBoard, TMove> displayerFactory) 
            where TGame : IGame<TGame, TBoard, TMove, string>
        {
            var players = playerFactory.Create();
            var strategyFactories = strategyFactoryFactory.Create();
            var strategies = GetStrategiesFromConsole(players, strategyFactories); //// TODO this is the only part using the factories that takes a hard dependency on the console //// TODO this also probably goes hand in hand with TPlayer being assumed to be string
            var displayer = displayerFactory.Create(); //// TODO two of these get instantiated, one for the strategy and one for the driver
            var game = gameFactory.Create(players[0]);

            var driver = Driver.Create(
                players.Zip(strategies).ToDictionary(tuple => tuple.First, tuple => tuple.Second),
                displayer);
            driver.Run(game);
        }

        static void Main(string[] args)
        {
            var games = new (string, Action)[]
            {
                (nameof(TicTacToe), TicTacToe),
                (nameof(Pegs), Pegs),
            };

            while (true)
            {
                Console.Write("Please select a game. ");
                var gameChoice = GetChoiceFromConsole(games.Select(game => game.Item1));
                games[gameChoice].Item2();
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        private static int GetChoiceFromConsole(IReadOnlyList<string> choices)
        {
            Console.WriteLine("Available choices:");
            for (int i = 0; i < choices.Count; ++i)
            {
                Console.WriteLine($"{i}: {choices[i]}");
            }

            do
            {
                Console.WriteLine();
                Console.WriteLine("Provide choice:");
                if (int.TryParse(Console.ReadLine(), out var choice) && choice >= 0 && choice < choices.Count)
                {
                    return choice;
                }
            }
            while (true);
        }

        private static void TicTacToeHumanVsHuman()
        {
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var consoleStrategy = new UserInterfaceStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>(displayer);
            var exes = "exes";
            var ohs = "ohs";
            var game = new TicTacToe<string>(exes, ohs);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>>
                {
                    { exes, consoleStrategy },
                    { ohs, consoleStrategy },
                },
                displayer);
            var result = driver.Run(game);
        }

        private static void TicTacToeHumanVsDecisionTree()
        {
            TicTacToeHumanVsDecisionTree(1);
        }

        private static void TicTacToeDecisionTreeVsHuman()
        {
            TicTacToeHumanVsDecisionTree(0);
        }

        private static void TicTacToeHumanVsDecisionTree(int first)
        {
            var exes = "exes";
            var ohs = "ohs";
            var players = new[] { exes, ohs };
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var game = new TicTacToe<string>(exes, ohs);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>>
                {
                            { players[first % 2], new DecisionTreeStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>(players[first % 2], StringComparer.OrdinalIgnoreCase) },
                            { players[(first + 1) % 2], new UserInterfaceStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>(displayer) },
                },
                displayer);
            var result = driver.Run(game);
        }

        private static void TicTacToeTwoRandom()
        {
            var exes = "exes";
            var ohs = "ohs";
            var strategyX = new RandomStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>();
            var strategyO = new RandomStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>();

            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var game = new TicTacToe<string>(exes, ohs);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<TicTacToe<string>, TicTacToeBoard, TicTacToeMove, string>>
                {
                    { exes, strategyX },
                    { ohs, strategyO },
                },
                displayer);
            var result = driver.Run(game);
        }

        private static void PegsHuman()
        {
            var displayer = new PegGameConsoleDisplayer<string>();
            var player = "player";
            var game = new PegGame<string>(player);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<PegGame<string>, PegBoard, PegMove, string>>
                {
                    { player, new UserInterfaceStrategy<PegGame<string>, PegBoard, PegMove, string>(displayer) },
                },
                displayer);
            var result = driver.Run(game);
        }

        private static void PegsDecisionTree()
        {
            var displayer = new PegGameConsoleDisplayer<string>();
            var computer = "computer";
            var game = new PegGame<string>(computer);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<PegGame<string>, PegBoard, PegMove, string>>
                {
                    { computer, new GameTreeDepthStrategy<PegGame<string>, PegBoard, PegMove, string>(game => game.ToTree().Decide(computer, StringComparer.OrdinalIgnoreCase).Value.Item3.Item2, null, computer, StringComparer.OrdinalIgnoreCase) },
                },
                displayer);
            var result = driver.Run(game);
        }

        private static void GobbleHumanVsHuman()
        {
            var displayer = new GobbleConsoleDisplayer<string>(_ => _);
            var exes = "exes";
            var ohs = "ohs";
            var game = new Gobble<string>(exes, ohs);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<Gobble<string>, GobbleBoard, GobbleMove, string>>
                {
                    { exes, new UserInterfaceStrategy<Gobble<string>, GobbleBoard, GobbleMove, string>(displayer) },
                    { ohs, new UserInterfaceStrategy<Gobble<string>, GobbleBoard, GobbleMove, string>(displayer) },
                },
                displayer);
            var result = driver.Run(game);
        }

        private static void GobbleHumanVsRandom()
        {
            var displayer = new GobbleConsoleDisplayer<string>(_ => _);
            var exes = "exes";
            var ohs = "ohs";
            var game = new Gobble<string>(exes, ohs);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<Gobble<string>, GobbleBoard, GobbleMove, string>>
                {
                    { exes, new UserInterfaceStrategy<Gobble<string>, GobbleBoard, GobbleMove, string>(displayer) },
                    { ohs, new RandomStrategy<Gobble<string>, GobbleBoard, GobbleMove, string>() }
                },
                displayer);
            var result = driver.Run(game);
        }

        public static void TwosHuman()
        {
            var displayer = new TwosConsoleDisplayer<string>(_ => _);
            var player = "player";
            var game = new Twos<string>(player, new Random(), 12);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<Twos<string>, int[][], TwosDirection, string>>
                {
                    { player, new UserInterfaceStrategy<Twos<string>, int[][], TwosDirection, string>(displayer) },
                },
                displayer);
            var result = driver.Run(game);
        }

        public static void TwosSevenMovesHeuristic12()
        {
            var displayer = new TwosConsoleDisplayer<string>(_ => _);
            var computer = "computer";
            var game = new Twos<string>(computer, new Random(), 12);
            var driver = Driver.Create(
                new Dictionary<string, IStrategy<Twos<string>, int[][], TwosDirection, string>>
                {
                    { computer, new GameTreeDepthStrategy<Twos<string>, int[][], TwosDirection, string>(TwosHeuristics.Heuristic12, Node.TreeFactory, computer, StringComparer.OrdinalIgnoreCase)},
                },
                displayer);
            var result = driver.Run(game);
        }
    }
}
