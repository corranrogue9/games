namespace Fx.Game
{
    using Fx.Tree;

    public static class GameExtensions
    {
        public static ITree<IGame<TGame, TBoard, TMove, TPlayer>> ToTree<TGame, TBoard, TMove, TPlayer>(this IGame<TGame, TBoard, TMove, TPlayer> game) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            return game.ToTree(-1);
        }

        internal static ITree<IGame<TGame, TBoard, TMove, TPlayer>> ToTree<TGame, TBoard, TMove, TPlayer>(this IGame<TGame, TBoard, TMove, TPlayer> game, int depth) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            if (!game.IsGameOver && depth != 0)
            {
                return Node.CreateTree(game, game.Moves.Select(move => game.CommitMove(move).ToTree(depth - 1)));
            }
            else
            {
                return Node.CreateTree(game);
            }
        }

        public static ITree<TGame> ToTree<TGame, TBoard, TMove, TPlayer>(this TGame game) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            return game.ToTree<TGame, TBoard, TMove, TPlayer>(-1);
        }

        internal static ITree<TGame> ToTree<TGame, TBoard, TMove, TPlayer>(this TGame game, int depth) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            if (!game.IsGameOver && depth != 0)
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
