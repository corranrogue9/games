using System.Runtime.CompilerServices;

namespace Fx.Game.Chess
{
    using System;
    using System.Collections.Generic;

    public sealed class Direction
    {
        public Direction(int dx, int dy) { this.dx = dx; this.dy = dy; }
        public readonly int dx;
        public readonly int dy;

        public static Direction operator *(Direction dir, int distance)
        {
            return new Direction(dir.dx * distance, dir.dy * distance);
        }


#pragma warning disable IDE0090
        public static readonly Direction N = new Direction(0, -1);
        public static readonly Direction S = new Direction(0, +1);
        public static readonly Direction W = new Direction(-1, 0);
        public static readonly Direction E = new Direction(+1, 0);

        public static readonly Direction NW = new Direction(-1, -1);
        public static readonly Direction SW = new Direction(-1, +1);
        public static readonly Direction NE = new Direction(+1, -1);
        public static readonly Direction SE = new Direction(+1, +1);
#pragma warning restore
    }
}