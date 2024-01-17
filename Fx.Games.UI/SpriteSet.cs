using Raylib_cs;

namespace Games;

public record SpriteSet<TPiece>(Texture2D Texture, FrozenDictionary<TPiece, Rectangle> Map)
    where TPiece : notnull
{
    public SpriteSet(Texture2D Texture, IEnumerable<(TPiece, Rectangle)> map) : this(
        Texture,
        map.ToDictionary().ToFrozenDictionary())
    { }

    internal void Draw(TPiece piece, Rectangle rect)
    {
        var (w, h) = (Texture.Height, Texture.Width);
        var source = this.Map[piece];
        Ray.DrawTexturePro(this.Texture, source, rect, Vector2.Zero, 0.0f, Color.WHITE);
    }
}
