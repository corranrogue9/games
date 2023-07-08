using System.Runtime.CompilerServices;

namespace Fx.Game.Chess
{
    using System.Collections.Generic;

    public sealed class Coordinate
    {
        public Coordinate(int x, int y) { this.x = x; this.y = y; }

        public readonly int x;
        public readonly int y;

        public static Coordinate operator +(Coordinate coordinate, Direction dir)
        {
            return new Coordinate(coordinate.x + dir.dx, coordinate.y + dir.dy);
        }

        public override string ToString() => $"{"abcdefgh"[x]}{y + 1}";

        public static IEnumerable<Coordinate> All
        {
            get
            {
                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        yield return new Coordinate(x, y);
                    }
                }
            }
        }

        public bool IsOnBoard { get => this.x >= 0 && this.y >= 0 && this.x < 8 && this.y < 8; }
    }
}