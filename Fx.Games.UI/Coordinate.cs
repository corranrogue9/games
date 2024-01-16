namespace Games;

public record struct Coordinate(
    /// <summary>
    /// X coordinate
    /// </summary
    int File,
    /// <summary>
    /// Y coordinate
    /// </summary
    int Rank) : IFormattable
{
    public static implicit operator (int File, int Rank)(Coordinate value)
    {
        return (value.File, value.Rank);
    }

    public static implicit operator Coordinate((int File, int Rank) value)
    {
        return new Coordinate(value.File, value.Rank);
    }

    public static Coordinate operator +(Coordinate a, Coordinate b)
    {
        return new Coordinate(a.File + b.File, a.Rank + b.Rank);
    }

    public static Coordinate operator *(Coordinate a, int f)
    {
        return new Coordinate(a.File * f, a.Rank * f);
    }

    public readonly string ToString(string? format, IFormatProvider? formatProvider)
    {
        return format switch
        {
            "A" => $"{FILE[this.File]}{RANK[this.Rank]}",
            _ => this.ToString(),
        };
    }

    // https://en.wikipedia.org/wiki/Algebraic_notation_(chess)
    private static readonly char[] FILE = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H'];

    private static readonly char[] RANK = ['8', '7', '6', '5', '4', '3', '2', '1'];
}
