



namespace Games;

public record struct Coordinate(int Rank, int File)
{
    public static implicit operator (int Rank, int File)(Coordinate value)
    {
        return (value.Rank, value.File);
    }

    public static implicit operator Coordinate((int Rank, int File) value)
    {
        return new Coordinate(value.Rank, value.File);
    }

    public static Coordinate operator +(Coordinate a, Coordinate b)
    {
        return new Coordinate(a.Rank + b.Rank, a.File + b.File);
    }

    public static Coordinate operator *(Coordinate a, int f)
    {
        return new Coordinate(a.Rank * f, a.File * f);
    }
}
