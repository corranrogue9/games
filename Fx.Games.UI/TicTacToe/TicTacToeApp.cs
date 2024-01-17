namespace Games.TicTacToe;

enum Piece { X, O }

public class TicTacToeApp()
{
    internal void Run()
    {
        // SetTraceLogging();
        Ray.InitWindow(680, 680, "FX.Games");

        var texture = Ray.LoadTexture("resources/tictactoe_spritesheet.png");
        var sprites = new SpriteSet<Piece>(
            texture, [
                (Piece.O, new Rectangle(0, 0, 150, 150)),
                (Piece.X,  new Rectangle(150, 0, 150, 150))
            ]);
        Console.WriteLine("constructed sprites");
        var grid = new SquareGrid<Piece>((3, 3), 200, (20, 20), true)
        {
            Font = Ray.LoadFontEx("resources/coolvetica rg.otf", 32, [], 256),
            SpriteSet = sprites
        };

        var board = new Piece?[3, 3];


        // Main game loop
        while (!Ray.WindowShouldClose()) // Detect window close button or ESC key
        {
            try
            {
                Ray.BeginDrawing();
                Ray.ClearBackground(Color.DARKGRAY);

                if (Ray.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
                {
                    if (grid.TryGetSquareUnderMouse(out var selectedCoordinate))
                    {
                        Console.WriteLine("selected {0:A} {0}", selectedCoordinate);
                        var count = board.Cast<Piece?>().Sum(p => p switch { Piece.X => 1, Piece.O => -1, _ => 0 });
                        board[selectedCoordinate.File, selectedCoordinate.Rank] = count >= 0 ? Piece.O : Piece.X;

                    }
                }

                grid.DrawGrid();

                for (int x = 0; x < grid.Size.X; x++)
                {
                    for (int y = 0; y < grid.Size.Y; y++)
                    {
                        var piece = board[x, y];
                        if (piece != null)
                        {
                            grid.DrawPiece(piece.Value, (x, y));
                        }
                    }
                }


                // // if (selected.HasValue)
                // // {
                // //     Highlight(grid, selected.Value.Origin, Ray.GREEN);
                // //     foreach (var move in selected.Value.Moves)
                // //     {
                // //         Highlight(grid, move.Destination, move.Capture ? Ray.RED : Ray.YELLOW);
                // //     }
                // // }
            }
            finally
            {
                Ray.EndDrawing();
            }
        }

        Ray.CloseWindow();
    }
}