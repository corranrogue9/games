namespace Fx.Game
{
    using Fx.Tree;

    public static class GameExtensions
    {
        internal static IGame<TGame, TBoard, TMove, TPlayer> AsGame<TGame, TBoard, TMove, TPlayer>(this IGame<TGame, TBoard, TMove, TPlayer> game) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            return game;
        }

        /*internal static ITree<IGame<TGame, TBoard, TMove, TPlayer>> ToTree<TGame, TBoard, TMove, TPlayer>(this IGame<TGame, TBoard, TMove, TPlayer> game) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            if (game.Moves.Any())
            {
                return Node.CreateTree(game, game.Moves.Select(move => game.CommitMove(move).ToTree()));
            }
            else
            {
                return Node.CreateTree(game);
            }
        }*/

        public static ITree<IGame<TGame, TBoard, TMove, TPlayer>> ToTree<TGame, TBoard, TMove, TPlayer>(this IGame<TGame, TBoard, TMove, TPlayer> game) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            if (game.Outcome == null)
            {
                return Node.CreateTree(game, game.Moves.Select(move => game.CommitMove(move).ToTree()));
            }
            else
            {
                return Node.CreateTree(game);
            }
        }

        internal static ITree<IGame<TGame, TBoard, TMove, TPlayer>> ToTree<TGame, TBoard, TMove, TPlayer>(this IGame<TGame, TBoard, TMove, TPlayer> game, int depth) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            if (game.Outcome == null && depth != 0)
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
            if (game.Outcome == null)
            {
                return Node.CreateTree(game, game.Moves.Select(move => game.CommitMove(move).ToTree<TGame, TBoard, TMove, TPlayer>()));
            }
            else
            {
                return Node.CreateTree(game);
            }
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
