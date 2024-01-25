using Fx.Displayer;
using Fx.Game;

namespace Games.Chess;

public class ChessDisplayer : IDisplayer<Fx.Game.TicTacToe<string>, Fx.Game.TicTacToeBoard, Fx.Game.TicTacToeMove, string>, IDisposable
{
    private bool disposed;

    public ChessDisplayer()
    {
        Ray.InitWindow(680, 680, "FX.Games");

        var texture = Ray.LoadTexture("resources/chess_spritesheet.png");
        var sprites = new SpriteSet<ChessPiece>(
            texture,
            from c in Enum.GetValues<PlayerColor>()
            from p in Enum.GetValues<PieceKind>()
            select (new ChessPiece(c, p), new Rectangle(texture.Height * ((int)c * 6 + (int)p), 0, texture.Width, texture.Height))
        );

        var grid = new SquareGrid<ChessPiece>((8, 8), 80, (20, 20), true)
        {
            Font = Raylib.LoadFontEx("resources/coolvetica rg.otf", 32, null, 256),
            SpriteSet = sprites
        };

        var minSize = (X: grid.Size.X * grid.SquareSize + 2 * grid.Margin.X, Y: grid.Size.Y * grid.SquareSize + 2 * grid.Margin.Y);
        Ray.InitWindow(680, 680, "FX.Games");
        Ray.SetWindowMinSize(minSize.X, minSize.Y);
        Ray.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);
        Ray.SetTargetFPS(24);

        // var fnt = Ray.LoadFontEx("resources/coolvetica rg.otf", 32, 256);
        // var pieces = ChessPieceTextures.FromFile("resources/chess_spritesheet.png");

        var board = new ChessBoard();

        this.disposed = false;
    }

    public void DisplayBoard(TicTacToe<string> game)
    {
        new TicTacToeConsoleDisplayer<string>(_ => _).DisplayBoard(game);
    }

    public void DisplayMoves(TicTacToe<string> game)
    {
        new TicTacToeConsoleDisplayer<string>(_ => _).DisplayMoves(game);
    }

    public void DisplayOutcome(TicTacToe<string> game)
    {

        new TicTacToeConsoleDisplayer<string>(_ => _).DisplayOutcome(game);
    }

    public TicTacToeMove ReadMoveSelection(TicTacToe<string> game)
    {
        return new TicTacToeConsoleDisplayer<string>(_ => _).ReadMoveSelection(game);
    }

    public void Dispose()
    {
        if (this.disposed)
        {
            return;
        }

        Ray.CloseWindow();
    }
}

public class ChessApp()
{
    public void Run()
    {
        Ray.InitWindow(680, 680, "FX.Games");

        var texture = Ray.LoadTexture("resources/chess_spritesheet.png");
        var sprites = new SpriteSet<ChessPiece>(
            texture,
            from c in Enum.GetValues<PlayerColor>()
            from p in Enum.GetValues<PieceKind>()
            select (new ChessPiece(c, p), new Rectangle(texture.Height * ((int)c * 6 + (int)p), 0, texture.Width, texture.Height))
        );

        var grid = new SquareGrid<ChessPiece>((8, 8), 80, (20, 20), true)
        {
            Font = Raylib.LoadFontEx("resources/coolvetica rg.otf", 32, null, 256),
            SpriteSet = sprites
        };

        var minSize = (X: grid.Size.X * grid.SquareSize + 2 * grid.Margin.X, Y: grid.Size.Y * grid.SquareSize + 2 * grid.Margin.Y);
        Ray.InitWindow(680, 680, "FX.Games");
        Ray.SetWindowMinSize(minSize.X, minSize.Y);
        Ray.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);
        Ray.SetTargetFPS(24);

        // var fnt = Ray.LoadFontEx("resources/coolvetica rg.otf", 32, 256);
        // var pieces = ChessPieceTextures.FromFile("resources/chess_spritesheet.png");

        var board = new ChessBoard();
        Selection? selected = null;

        // Main game loop
        while (!Ray.WindowShouldClose()) // Detect window close button or ESC key
        {
            try
            {
                Ray.BeginDrawing();
                Ray.ClearBackground(Color.DARKGRAY);
                // Ray.DrawFPS(10, 10);

                if (Ray.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
                {
                    if (grid.TryGetSquareUnderMouse(out var selectedCoordinate))
                    {
                        // check if a move's destination is selected
                        if (selected != null && selected.Value.Moves.TryFind(move => move.Destination == selectedCoordinate, out var mv))
                        {
                            if (board.Commit(mv, out var capture))
                            {
                                System.Console.WriteLine("captured {0}", capture);
                            }
                            selected = null;
                        }
                        else
                        {
                            Console.WriteLine("selected {0}", selectedCoordinate);

                            selected = (selectedCoordinate, board.Moves(selectedCoordinate));
                            File.AppendAllText("log.txt", String.Format("{0:FEN}: {1:A} > [{2}]\r\n",
                                board,
                                selected.Value.Origin,
                                string.Join(", ", selected.Value.Moves.Select(m => $"{m.Destination:A}{(m.Capture ? "x" : "")}"))));
                        }
                    }
                    else
                    {
                        selected = null;
                    }
                }

                grid.DrawGrid();

                DrawPieces(grid, board);

                if (selected.HasValue)
                {
                    Highlight(grid, selected.Value.Origin, Color.GREEN);
                    foreach (var move in selected.Value.Moves)
                    {
                        Highlight(grid, move.Destination, move.Capture ? Color.RED : Color.YELLOW);
                    }
                }
            }
            finally
            {
                Ray.EndDrawing();
            }
        }

        Ray.CloseWindow();
    }

    private static void Highlight(SquareGrid<ChessPiece> grid, Coordinate dest, Color color)
    {

        var center = (
            X: grid.Margin.X + grid.SquareSize * dest.File + grid.SquareSize / 2,
            Y: grid.Margin.X + grid.SquareSize * dest.Rank + grid.SquareSize / 2
        );
        var actual = Raylib_cs.Raylib.Fade(color, 0.5f);
        // Ray.DrawRectangleRounded(rect, 0.2f, 0xFF, actual);
        Ray.DrawCircle(center.X, center.Y, grid.SquareSize / 4, actual);
    }

    private static void DrawPieces(SquareGrid<ChessPiece> grid, ChessBoard board)
    {
        for (int y = 0; y < grid.Size.Y; y++)
        {
            for (int x = 0; x < grid.Size.X; x++)
            {
                var piece = board[x, y];
                if (piece != null)
                {
                    grid.DrawPiece(piece.Value, (x, y));
                }
            }
        }
    }

}


internal struct Selection
{
    public Coordinate Origin;
    public IEnumerable<ChessMove> Moves;

    public Selection(Coordinate origin, IEnumerable<ChessMove> moves)
    {
        Origin = origin;
        Moves = moves;
    }

    public override bool Equals(object? obj)
    {
        return obj is Selection other &&
               Origin.Equals(other.Origin) &&
               EqualityComparer<IEnumerable<ChessMove>>.Default.Equals(Moves, other.Moves);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Origin, Moves);
    }

    public void Deconstruct(out Coordinate origin, out IEnumerable<ChessMove> moves)
    {
        origin = Origin;
        moves = Moves;
    }

    public static implicit operator (Coordinate Origin, IEnumerable<ChessMove> Moves)(Selection value)
    {
        return (value.Origin, value.Moves);
    }

    public static implicit operator Selection((Coordinate Origin, IEnumerable<ChessMove> Moves) value)
    {
        return new Selection(value.Origin, value.Moves);
    }
}