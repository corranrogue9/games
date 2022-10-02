using System;

namespace Fx.Games.Bobble
{
    public struct BobblePiece 
    {
        public BobblePiece(BobbleSize size, BobbleColor color) { Size = size; Color = color; }
        public BobbleSize Size { get; set; }
        public BobbleColor Color { get; set;  }
        
    }
}
