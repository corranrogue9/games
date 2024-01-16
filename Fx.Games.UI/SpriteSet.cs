namespace Games;

public record SpriteSet<TPiece>(Texture Texture, FrozenDictionary<TPiece, Rectangle> Map)
    where TPiece : notnull
{
    public SpriteSet(Texture Texture, IEnumerable<(TPiece, Rectangle)> map) : this(
        Texture,
        map.ToDictionary().ToFrozenDictionary())
    { }

    internal void Draw(TPiece piece, Rectangle rect)
    {
        var (w, h) = (Texture.height, Texture.height);
        var source = this.Map[piece];
        RAY.DrawTexturePro(this.Texture, source, rect, Vector2.Zero, 0.0f, RAY.WHITE);
    }
}
