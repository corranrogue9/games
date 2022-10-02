using System;

namespace Fx.Games.Gobble
{
    public struct GobblePiece
    {
        public GobblePiece(GobbleSize size, GobbleColor color) { Size = size; Color = color; }
        public GobbleSize Size { get; set; }
        public GobbleColor Color { get; set; }

    }
}
