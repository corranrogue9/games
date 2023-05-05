namespace Fx.Game
{
    using Fx.Tree;

    public static class GameExtensions
    {
        internal static ITree<TGame> ToOtherTree<TGame, TMove, TPlayer>(this TGame game, int depth) where TGame : Fx.Game.IGame<TMove, TPlayer, TGame> //// TODO i'm surprised the compiler finds this...
        {
            if (game.Outcome == null && depth != 0)
            {
                return Node.CreateTree(game, game.LegalMoves.Select(move => game.CommitMove(move).ToOtherTree<TGame, TMove, TPlayer>(depth - 1)));
            }
            else
            {
                return Node.CreateTree(game);
            }
        }

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

        internal static ITree<Fx.Game.IGame<TMove, TPlayer, TGame>> ToOtherTree<TGame, TMove, TPlayer>(this Fx.Game.IGame<TMove, TPlayer, TGame> game) where TGame : Fx.Game.IGame<TMove, TPlayer, TGame>
        {
            if (game.Outcome == null)
            {
                return Node.CreateTree(game, game.LegalMoves.Select(move => game.CommitMove(move).ToOtherTree()));
            }
            else
            {
                return Node.CreateTree(game);
            }
        }

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
