using System.Runtime.CompilerServices;

namespace Fx.Game.Chess
{
    using System.Collections.Generic;

    /// <summary>
    /// coordinates (a–h for files, 1–8 for ranks) to uniquely identify each square.
    /// </summary>
    public sealed class Square : IEquatable<Square>
    {
        public Square(int x, int y) { this.x = x; this.y = y; }

        public readonly int x;
        public readonly int y;

        public static Square operator +(Square coordinate, Direction dir)
        {
            return new Square(coordinate.x + dir.dx, coordinate.y + dir.dy);
        }

        public override string ToString() => $"{"abcdefgh"[x]}{y + 1}";

        public bool Equals(Square? other)
        {
            return other != null && this.x == other.x && this.y == other.y;
        }


        public static IEnumerable<Square> All
        {
            get
            {
                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        yield return new Square(x, y);
                    }
                }
            }
        }

        public bool IsOnBoard { get => this.x >= 0 && this.y >= 0 && this.x < 8 && this.y < 8; }



        public override bool Equals(object? obj)
        {
            return obj is Square other && Equals(this, other);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(this.x, this.y);
        }
    }
}