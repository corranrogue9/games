namespace Games.Chess;

public class ChessApp()
{
    public void Run()
    {
        // SetTraceLogging();
        RAY.InitWindow(680, 680, "FX.Games");

        var texture = RAY.LoadTexture("resources/chess_spritesheet.png");
        var sprites = new SpriteSet<ChessPiece>(
            texture,
            from c in Enum.GetValues<PlayerColor>()
            from p in Enum.GetValues<PieceKind>()
            select (new ChessPiece(c, p), new Rectangle(texture.height * ((int)c * 6 + (int)p), 0, texture.height, texture.height))
        );

        var grid = new SquareGrid<ChessPiece>((8, 8), 80, (20, 20), true)
        {
            Font = RAY.LoadFontEx("resources/coolvetica rg.otf", 32, 256),
            SpriteSet = sprites
        };

        var minSize = (X: grid.Size.X * grid.SquareSize + 2 * grid.Margin.X, Y: grid.Size.Y * grid.SquareSize + 2 * grid.Margin.Y);
        RAY.InitWindow(680, 680, "FX.Games");
        RAY.SetWindowMinSize(minSize.X, minSize.Y);
        RAY.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);
        RAY.SetTargetFPS(60);

        // var fnt = RAY.LoadFontEx("resources/coolvetica rg.otf", 32, 256);
        // var pieces = ChessPieceTextures.FromFile("resources/chess_spritesheet.png");

        var board = new ChessBoard();
        Selection? selected = null;

        // Main game loop
        while (!RAY.WindowShouldClose()) // Detect window close button or ESC key
        {
            try
            {
                RAY.BeginDrawing();
                RAY.ClearBackground(RAY.DARKGRAY);
                // RAY.DrawFPS(10, 10);

                if (RAY.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
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
                    Highlight(grid, selected.Value.Origin, RAY.GREEN);
                    foreach (var move in selected.Value.Moves)
                    {
                        Highlight(grid, move.Destination, move.Capture ? RAY.RED : RAY.YELLOW);
                    }
                }
            }
            finally
            {
                RAY.EndDrawing();
            }
        }

        RAY.CloseWindow();
    }

    private static void Highlight(SquareGrid<ChessPiece> grid, Coordinate dest, Color color)
    {

        var center = (
            X: grid.Margin.X + grid.SquareSize * dest.File + grid.SquareSize / 2,
            Y: grid.Margin.X + grid.SquareSize * dest.Rank + grid.SquareSize / 2
        );
        var actual = new Color(color.r, color.g, color.b, (byte)127);
        // RAY.DrawRectangleRounded(rect, 0.2f, 0xFF, actual);
        RAY.DrawCircle(center.X, center.Y, grid.SquareSize / 4, actual);
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

    private static void SetTraceLogging()
    {
        // Unmanaged callback with Cdecl calling convention.
        [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
        unsafe static void CustomLogging(TraceLogLevel level, nint a, nint b)
        {
            // if (level <= LOG.LOG_INFO) return;
            switch (level)
            {
                case LOG_DEBUG: Console.ForegroundColor = ConsoleColor.White; break;
                case LOG_INFO: Console.ForegroundColor = ConsoleColor.Yellow; break;
                case LOG_WARNING: Console.ForegroundColor = ConsoleColor.DarkYellow; break;
                case LOG_ERROR: Console.ForegroundColor = ConsoleColor.Red; break;
                default: break;
            }

            var aa = Marshal.PtrToStringAnsi(a)!;
            Console.Error.WriteLine("{0}: {1}", level, aa);
        }

        unsafe
        {
            var callback = (delegate* unmanaged[Cdecl]<TraceLogLevel, nint, nint, void>)&CustomLogging;
            RAY.SetTraceLogCallback((delegate* unmanaged[Cdecl]<int, void*, void*, void>)callback);
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