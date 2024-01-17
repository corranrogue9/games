
using System.Security.Cryptography;

namespace Games.Gobble;


public class GobbleApp(float sz, int off)
{
    public void Run()
    {
        const int screenWidth = 1200;
        const int screenHeight = 700;

        Ray.InitWindow(screenWidth, screenHeight, "Gobble");


        Ray.SetTargetFPS(24);
        // Set our game to run at 60 frames-per-second

        // var board = GobbleBoard.Random();
        var board = new GobbleBoard();
        var currentPlayer = Player.Blue;
        var selectedSize = 2;//new Square([null, null, currentPlayer]);

        //--------------------------------------------------------------------------------------
        // Main game loop. Detect window close button or ESC key
        while (!Ray.WindowShouldClose())
        {
            var sz = MathF.Min(Ray.GetScreenWidth() * .8f, Ray.GetScreenHeight() * .8f);
            var off = 30;

            // Draw
            // ----------------------------------------------------------------------------------
            Ray.BeginDrawing();
            Ray.ClearBackground(Color.WHITE);

            if (TryGetNumberKey(out var num))
            {
                selectedSize = num;
                Console.WriteLine("selected size = {0}", selectedSize);
            };

            // draw selected size
            var selection = new Square();
            if (selectedSize != 0) { selection.Ring[selectedSize - 1] = currentPlayer; }
            selection.Draw(new Vector2(off + sz * 1.2f, off + sz * 1 / 6));

            // place ring
            if (selectedSize > 0 && TryGetMouseClick(out var sq))
            {
                Console.WriteLine("trying to add {0} to {1}", selectedSize, sq);
                if (board[sq.X, sq.Y].Ring[selectedSize - 1] == null) // TODO && no larger size present
                {
                    board[sq.X, sq.Y].Ring[selectedSize - 1] = currentPlayer;

                    // change player and reset size selection
                    currentPlayer = currentPlayer == Player.Blue ? Player.Orange : Player.Blue;
                    selectedSize = 0;
                }
            }

            DrawBoard(board);

            Ray.EndDrawing();
        }

        //--------------------------------------------------------------------------------------
        Ray.CloseWindow();
    }

    private void DrawBoard(GobbleBoard board)
    {
        Ray.DrawLineEx(new Vector2(off + sz * 1 / 3, off + 0), new Vector2(off + sz * 1 / 3, off + sz), 8f, Color.BLACK);
        Ray.DrawLineEx(new Vector2(off + sz * 2 / 3, off + 0), new Vector2(off + sz * 2 / 3, off + sz), 8f, Color.BLACK);
        Ray.DrawLineEx(new Vector2(off + 0, off + sz * 1 / 3), new Vector2(off + sz, off + sz * 1 / 3), 8f, Color.BLACK);
        Ray.DrawLineEx(new Vector2(off + 0, off + sz * 2 / 3), new Vector2(off + sz, off + sz * 2 / 3), 8f, Color.BLACK);

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                var center = new Vector2(off + sz * (1 + x * 2) / 6, off + sz * (1 + y * 2) / 6);
                var sq = board[x, y];
                sq.Draw(center);
            }
        }
    }

    public bool TryGetMouseClick(out (int X, int Y) sq)
    {
        if (Ray.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
        {
            var mouse = Ray.GetMousePosition();
            var pos = (X: (int)((mouse.X - off) / (sz / 3)), Y: (int)((mouse.Y - off) / (sz / 3)));
            // System.Console.WriteLine("clicked {0} {1}", mouse, pos);

            if (pos.X >= 0 && pos.X < 3 && pos.Y >= 0 && pos.Y < 3)
            {
                sq = pos;
                return true;
            }
        }
        sq = default;
        return false; ;
    }

    private static bool TryGetNumberKey([MaybeNullWhen(false)] out int key)
    {
        switch ((KeyboardKey)Ray.GetKeyPressed())
        {
            case KeyboardKey.KEY_ONE:
                key = 1; return true;
            case KeyboardKey.KEY_TWO:
                key = 2; return true;
            case KeyboardKey.KEY_THREE:
                key = 3; return true;
            default:
                key = 0; return false;
        }
    }
}
