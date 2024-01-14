
using System.Diagnostics.CodeAnalysis;

namespace Games;

public record struct SquareGrid((int X, int Y) Size, int SquareSize, (int X, int Y) Margin, bool IsCheckered)
{
    public readonly void DrawGrid(Font fnt)
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

        #region  vertical Labels
        const float F = 16f;
        for (int i = 0; i < h; i++)
        {
            var label = ((char)('1' + h - 1 - i)).ToString();
            // DrawText(label, o.x + 0 * d.x + 2, o.y + i * d.y, F, i % 2 != 0 ? LIGHTGRAY : DARKGRAY);
            RAY.DrawTextEx(fnt, label, new Vector2(ox + 0 * sz + 2, oy + i * sz), F, 1, i % 2 != 0 ? RAY.LIGHTGRAY : RAY.DARKGRAY);
            // DrawTextEx(fnt, "Raylib is easy!!!", new Vector2(20.0f, 100.0f), 32f, 2, MAROON);
        }
        #endregion

        #region horizontal Labels
        for (int i = 0; i < w; i++)
        {
            var label = ((char)('a' + i)).ToString();
            RAY.DrawTextEx(fnt, label, new Vector2(ox + (i + 1) * sz - F, oy + h * sz - F), F, 1, i % 2 == 0 ? RAY.LIGHTGRAY : RAY.DARKGRAY);//
        }
        #endregion
    }

    public readonly void DrawPiece(ChessPieceTextures pieces, Piece piece, (int X, int Y) dest)
    {
        var rect = new Rectangle(
            this.Margin.X + this.SquareSize * dest.X,
            this.Margin.X + this.SquareSize * dest.Y,
            this.SquareSize,
            this.SquareSize);
        pieces.Draw(piece, rect);
    }

    public readonly bool TryGetSquareUnderMouse([MaybeNullWhen(false)] out Coordinate coordinate)
    {
        var pos = RAY.GetMousePosition();
        int x = (int)((pos.X - this.Margin.X) / this.SquareSize);
        int y = (int)((pos.Y - this.Margin.Y) / this.SquareSize);
        if (x >= 0 && x < 8 && y >= 0 && y < 8)
        {
            coordinate = new Coordinate(x, y);
            return true;
        }
        coordinate = default;
        return false;
    }
}
