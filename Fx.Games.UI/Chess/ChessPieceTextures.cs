namespace Games;

// public class ChessPieceTextures(Texture texture, FrozenDictionary<Piece, int> map)
// {
//     private readonly Texture texture = texture;

//     private readonly FrozenDictionary<Piece, int> map = map;

//     public static ChessPieceTextures FromFile(string path)
//     {
//         // var textures =
//         //     from c in Enum.GetValues<Color>()
//         //     from p in Enum.GetValues<PieceKind>()
//         //     select (new Piece(c, p), RAY.LoadTexture($"{dir}/{c.ToString().ToLower()}_{p.ToString().ToLower()}.png"));
//         var texture = RAY.LoadTexture(path);
//         var map =
//             from c in Enum.GetValues<PlayerColor>()
//             from p in Enum.GetValues<PieceKind>()
//             select (new Piece(c, p), (int)c * 6 + (int)p);

//         return new ChessPieceTextures(texture, map.ToDictionary().ToFrozenDictionary());
//     }

//     public void Draw(Piece piece, Rectangle target)
//     {
//         var (w, h) = (texture.height, texture.height);
//         var ix = map[piece];
//         var source = new Rectangle(w * ix, 0, w, h);
//         RAY.DrawTexturePro(this.texture, source, target, Vector2.Zero, 0.0f, RAY.WHITE);
//     }
// }
