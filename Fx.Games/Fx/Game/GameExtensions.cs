namespace Fx.Game
{
    using Fx.Tree;
    using Fx.TreeFactory;

    public static class GameExtensions
    {
        public static ITree<IGame<TGame, TBoard, TMove, TPlayer>> ToTree<TGame, TBoard, TMove, TPlayer>(this IGame<TGame, TBoard, TMove, TPlayer> game) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            return ToTree(game, Node.TreeFactory);
        }

        public static ITree<IGame<TGame, TBoard, TMove, TPlayer>> ToTree<TGame, TBoard, TMove, TPlayer>(this IGame<TGame, TBoard, TMove, TPlayer> game, ITreeFactory treeFactory) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            return ToTree(game, treeFactory, EqualityComparer<IGame<TGame, TBoard, TMove, TPlayer>>.Default);
        }

        public static ITree<IGame<TGame, TBoard, TMove, TPlayer>> ToTree<TGame, TBoard, TMove, TPlayer>(this IGame<TGame, TBoard, TMove, TPlayer> game, ITreeFactory treeFactory, IEqualityComparer<IGame<TGame, TBoard, TMove, TPlayer>> gameComparer) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            return game.ToTree(-1, treeFactory, new HashSet<IGame<TGame, TBoard, TMove, TPlayer>>(gameComparer));
        }

        internal static ITree<IGame<TGame, TBoard, TMove, TPlayer>> ToTree<TGame, TBoard, TMove, TPlayer>(
            this IGame<TGame, TBoard, TMove, TPlayer> game,
            int depth,
            ITreeFactory treeFactory, 
            HashSet<IGame<TGame, TBoard, TMove, TPlayer>> games) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            ////var pointer = new Pointer<ITree<IGame<TGame, TBoard, TMove, TPlayer>>>();
            if (game.Outcome == null && depth != 0)
            {
                var tree = treeFactory.CreateInner(game, game.Moves.Select(move => game.CommitMove(move)).Where(newGame => !games.Contains(newGame)).Select(newGame => newGame.ToTree(depth - 1, treeFactory, games)));
                ////pointer.Value = tree;
                return tree;
            }
            else
            {
                var tree = treeFactory.CreateLeaf(game);
                ////pointer.Value = tree;
                return tree;
            }
        }

        private sealed class Pointer<T>
        {
            public T Value { get; set; }
        }

        public static ITree<TGame> ToTree<TGame, TBoard, TMove, TPlayer>(this TGame game) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            return game.ToTree<TGame, TBoard, TMove, TPlayer>(-1);
        }

        internal static ITree<TGame> ToTree<TGame, TBoard, TMove, TPlayer>(this TGame game, int depth) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            if (game.Outcome == null && depth != 0)
            {
                return Node.CreateTree(game, game.Moves.Select(move => game.CommitMove(move).ToTree<TGame, TBoard, TMove, TPlayer>(depth - 1)));
            }
            else
            {
                return Node.CreateTree(game);
            }
        }
    }
}
