using System.Security.Cryptography.X509Certificates;

namespace Games;

public static class Program
{
    public static void Main(string[] args)
    {
        // SetTraceLogging();

        var grid = new SquareGrid((8, 8), 80, (20, 20), true);

        var minSize = (X: grid.Size.X * grid.SquareSize + 2 * grid.Margin.X, Y: grid.Size.Y * grid.SquareSize + 2 * grid.Margin.Y);
        RAY.InitWindow(680, 680, "FX.Games");
        RAY.SetWindowMinSize(minSize.X, minSize.Y);
        RAY.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);
        RAY.SetTargetFPS(60);

        var fnt = RAY.LoadFontEx("resources/coolvetica rg.otf", 32, 256);
        var pieces = ChessPieceTextures.FromFile("resources/chess_spritesheet.png");

        var board = new ChessBoard();
        (Coordinate Origin, IEnumerable<Move> Moves)? selected = null;

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
                    if (grid.TryGetSquareUnderMouse(out var c))
                    {
                        // check if a move's destination is selected
                        if (selected != null && selected.Value.Moves.TryFind(move => move.Destination == c, out var mv))
                        {
                            if (board.Commit(mv, out var capture))
                            {
                                System.Console.WriteLine("captured {0}", capture);
                            }
                            selected = null;
                        }
                        else
                        {
                            selected = (c, board.Moves(c));
                        }
                    }
                    else
                    {
                        selected = null;
                    }
                }

                grid.DrawGrid(fnt);

                DrawPieces(grid, pieces, board);

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

    private static void Highlight(SquareGrid checkers, Coordinate dest, Color color)
    {
        // var rect = new Rectangle(
        //                         checkers.Margin.X + checkers.SquareSize * dest.Rank + checkers.SquareSize * .05f,
        //                         checkers.Margin.X + checkers.SquareSize * dest.File + checkers.SquareSize * .05f,
        //                         checkers.SquareSize * .9f,
        //                         checkers.SquareSize * .9f);
        var center = (
            X: checkers.Margin.X + checkers.SquareSize * dest.Rank + checkers.SquareSize / 2,
            Y: checkers.Margin.X + checkers.SquareSize * dest.File + checkers.SquareSize / 2
        );
        var actual = new Color(color.r, color.g, color.b, (byte)127);
        // RAY.DrawRectangleRounded(rect, 0.2f, 0xFF, actual);
        RAY.DrawCircle(center.X, center.Y, checkers.SquareSize / 4, actual);
    }

    private static void DrawPieces(SquareGrid checkers, ChessPieceTextures pieces, ChessBoard board)
    {
        for (int i = 0; i < checkers.Size.X; i++)
        {
            for (int j = 0; j < checkers.Size.Y; j++)
            {
                var piece = board[j, i];
                if (piece != null)
                {
                    checkers.DrawPiece(pieces, piece.Value, (i, j));
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
