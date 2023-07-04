namespace Fx.Game.Chess
{
    using System;
    using System.Collections.Generic;
    using static ChessPieceKind;
    using static ChessColor;
    using static ChessPiece;


    public enum ChessColor { White, Black }


    public enum ChessPieceKind { King, Queen, Knight, Bishop, Rook, Pawn }


    public struct ChessPiece : IEquatable<ChessPiece>
    {
        public ChessColor Color { get; }
        public ChessPieceKind Kind { get; }

        private ChessPiece(ChessColor color, ChessPieceKind kind) : this()
        {
            Color = color;
            Kind = kind;
        }

        public bool Equals(ChessPiece other)
        {
            return this.Color == other.Color && this.Kind == other.Kind;
        }

        public override bool Equals(object? obj)
        {
            return obj is ChessPiece piece && Equals(piece);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Color, Kind);
        }

        public static readonly ChessPiece WhiteKing = new(White, King);
        public static readonly ChessPiece WhiteQueen = new(White, Queen);
        public static readonly ChessPiece WhiteKnight = new(White, Knight);
        public static readonly ChessPiece WhiteBishop = new(White, Bishop);
        public static readonly ChessPiece WhiteRook = new(White, Rook);
        public static readonly ChessPiece WhitePawn = new(White, Pawn);

        public static readonly ChessPiece BlackKing = new(Black, King);
        public static readonly ChessPiece BlackQueen = new(Black, Queen);
        public static readonly ChessPiece BlackKnight = new(Black, Knight);
        public static readonly ChessPiece BlackBishop = new(Black, Bishop);
        public static readonly ChessPiece BlackRook = new(Black, Rook);
        public static readonly ChessPiece BlackPawn = new(Black, Pawn);

        internal char Symbol()
        {
            return (Color, Kind) switch
            {
                (White, King) => 'K',
                (White, Queen) => 'Q',
                (White, Knight) => 'N',
                (White, Bishop) => 'B',
                (White, Rook) => 'R',
                (White, Pawn) => 'P',

                (Black, King) => 'k',
                (Black, Queen) => 'q',
                (Black, Knight) => 'n',
                (Black, Bishop) => 'b',
                (Black, Rook) => 'r',
                (Black, Pawn) => 'p',

                _ => throw new NotImplementedException(),
            };
        }

        internal static ChessPiece? FromChar(char piece)
        {
            return piece switch
            {
                'K' => new ChessPiece(White, King),
                'Q' => new ChessPiece(White, Queen),
                'N' => new ChessPiece(White, Knight),
                'B' => new ChessPiece(White, Bishop),
                'R' => new ChessPiece(White, Rook),
                'P' => new ChessPiece(White, Pawn),

                'k' => new ChessPiece(Black, King),
                'q' => new ChessPiece(Black, Queen),
                'n' => new ChessPiece(Black, Knight),
                'b' => new ChessPiece(Black, Bishop),
                'r' => new ChessPiece(Black, Rook),
                'p' => new ChessPiece(Black, Pawn),

                ' ' => default(ChessPiece?),
                '_' => default(ChessPiece?),
                _ => throw new ArgumentOutOfRangeException(nameof(piece), $"not a chess piece symbol or space: `{piece}`"),
            };
        }
    }
}