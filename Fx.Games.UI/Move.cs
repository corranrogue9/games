namespace Games;

public record struct Move(Coordinate Origin, Coordinate Destination, bool Capture) : IFormattable
{

    public readonly string ToString(string? format, IFormatProvider? formatProvider)
    {
        return format switch
        {
            "A" => $"{this.Origin:A}-{this.Destination:A}{(Capture ? "x" : "")}",
            _ => this.ToString(),
        };
    }
}

