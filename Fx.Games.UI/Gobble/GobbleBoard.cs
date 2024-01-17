namespace Games.Gobble;


public enum Player { Blue, Orange }

public class Square(Player?[] pieces)
{
    public Square() : this(new Player?[3]) { }

    public Square(Player? l, Player? m, Player? s) : this([l, m, s]) { }

    public Player?[] Ring { get; } = pieces.Length != 3 ? throw new ArgumentOutOfRangeException() : pieces;

    private static readonly (float, float)[] Radii = [(10f, 30f), (35f, 55f), (60f, 80f)];

    public void Draw(Vector2 center)
    {
        foreach (var ((innerRadius, outerRadius), player) in Radii.Zip(Ring))
        {
            Ray.DrawRing(center, innerRadius, outerRadius, 0f, 360f, 0, GetColor(player));
        }
    }

    private Color GetColor(Player? player) => player switch
    {
        Player.Orange => Color.ORANGE,
        Player.Blue => Color.BLUE,
        _ => Ray.Fade(Color.BEIGE, 0.1f),
    };
}


public class GobbleBoard
{
    public GobbleBoard()
    {
        squares = new Square[3, 3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                squares[i, j] = new Square();
            }
        }
    }

    private GobbleBoard(Square[,] squares)
    {
        Debug.Assert(squares.Rank == 2 && squares.GetUpperBound(0) == 2 && squares.GetUpperBound(1) == 2);
        this.squares = squares;
    }

    private readonly Square[,] squares;


    public Square this[int x, int y] => squares[x, y];

    internal static GobbleBoard Random()
    {
        var rng = new Random();
        var board = new Square[3, 3];
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                board[x, y] = new Square();
                var n = rng.Next(4);
                for (int i = 0; i < n; i++)
                {
                    board[x, y].Ring[i] = rng.NextDouble() < 0.5 ? Player.Blue : Player.Orange;
                }
            }
        }
        return new GobbleBoard(board);
    }


}

