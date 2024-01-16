namespace Games;

public class SquareGrid<TPiece>
    where TPiece : notnull
{
    public SquareGrid((int X, int Y) size, int squareSize, (int X, int Y) margin, bool isCheckered)
    {
        this.Size = size;
        this.SquareSize = squareSize;
        this.Margin = margin;
        this.IsCheckered = isCheckered;

        var minSize = (X: this.Size.X * this.SquareSize + 2 * this.Margin.X, Y: this.Size.Y * this.SquareSize + 2 * this.Margin.Y);
        RAY.SetWindowMinSize(minSize.X, minSize.Y);
        RAY.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);
        RAY.SetTargetFPS(10);
    }

    public required Font Font { get; init; }
    public (int X, int Y) Size { get; }
    public int SquareSize { get; }
    public (int X, int Y) Margin { get; }
    public bool IsCheckered { get; }
    required public SpriteSet<TPiece> SpriteSet { get; init; }

    public void DrawGrid()
    {
        (int ox, int oy) = this.Margin;
        int sz = this.SquareSize;
        (int w, int h) = this.Size;

        #region checkered squares
        if (IsCheckered)
        {
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    RAY.DrawRectangle(ox + i * sz, oy + j * sz, sz, sz, ((i + j) % 2 == 0) ? RAY.LIGHTGRAY : RAY.DARKGRAY);
                }
            }
        }
        #endregion

        #region horizontal and vertical lines
        for (int i = 0; i <= w; i++)
        {
            RAY.DrawLineEx(
                new Vector2(ox + i * sz, oy + 0 * sz),
                new Vector2(ox + i * sz, oy + h * sz),
                2.0f, RAY.BLACK);
            RAY.DrawLineEx(
                new Vector2(ox + 0 * sz, oy + i * sz),
                new Vector2(ox + w * sz, oy + i * sz),
                2.0f, RAY.BLACK);
        }
        #endregion

        const float fontSize = 16f;
        #region  vertical Labels
        for (int i = 0; i < h; i++)
        {
            var label = ((char)('1' + h - 1 - i)).ToString();
            // DrawText(label, o.x + 0 * d.x + 2, o.y + i * d.y, F, i % 2 != 0 ? LIGHTGRAY : DARKGRAY);
            RAY.DrawTextEx(Font, label, new Vector2(ox + 0 * sz + 2, oy + i * sz), fontSize, 1, i % 2 != 0 ? RAY.LIGHTGRAY : RAY.DARKGRAY);
            // DrawTextEx(fnt, "Raylib is easy!!!", new Vector2(20.0f, 100.0f), 32f, 2, MAROON);
        }
        #endregion

        #region horizontal Labels
        for (int i = 0; i < w; i++)
        {
            var label = ((char)('a' + i)).ToString();
            var position = new Vector2(ox + (i + 1) * sz - fontSize, oy + h * sz - fontSize);
            RAY.DrawTextEx(Font, label, position, fontSize, 1, (i + h) % 2 == 0 ? RAY.LIGHTGRAY : RAY.DARKGRAY);//
        }
        #endregion
    }

    public void DrawPiece(TPiece piece, (int X, int Y) dest)
    {
        var rect = new Rectangle(
            this.Margin.X + this.SquareSize * dest.X,
            this.Margin.X + this.SquareSize * dest.Y,
            this.SquareSize,
            this.SquareSize);
        SpriteSet.Draw(piece, rect);
    }

    public bool TryGetSquareUnderMouse([MaybeNullWhen(false)] out Coordinate coordinate)
    {
        var pos = RAY.GetMousePosition();
        int x = (int)((pos.X - this.Margin.X) / this.SquareSize);
        int y = (int)((pos.Y - this.Margin.Y) / this.SquareSize);
        if (x >= 0 && x < this.Size.X && y >= 0 && y < this.Size.Y)
        {
            coordinate = new Coordinate(x, y);
            return true;
        }
        coordinate = default;
        return false;
    }
}
