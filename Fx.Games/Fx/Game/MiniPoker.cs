namespace Fx.Game
{
    using Fx.Todo;
    using System.Diagnostics;

    public enum MiniPokerMove
    {
        Fold,
        Hold,
    }

    public enum MiniPokerCard
    {
        Red,
        Black,
    }

    public abstract class MiniPokerBoard
    {
        private MiniPokerBoard()
        {
        }

        public sealed class BBoard : MiniPokerBoard
        {
        }

        public sealed class ABoard : MiniPokerBoard
        {
            public ABoard(MiniPokerCard card)
            {
                this.Card = card;
            }

            public MiniPokerCard Card { get; }
        }

        public sealed class PublicBoard : MiniPokerBoard
        {
            public PublicBoard(MiniPokerCard card, int asPurse)
            {
                this.Card = card;
                this.AsPurse = asPurse;
            }

            public int AsPurse { get; } //// TODO this probably makes more sense to be on the base type, but then our naming goes out the window; let's figure out better names and stuff and do it later

            public MiniPokerCard Card { get; }
        }
    }

    public sealed class MiniPoker<TPlayer> : IGameWithHiddenInformation<MiniPoker<TPlayer>, MiniPokerBoard, MiniPokerMove, TPlayer>
    {
        private readonly TPlayer a;

        private readonly TPlayer b;

        private readonly InternalBoardState internalBoardState;        

        public MiniPoker(TPlayer a, TPlayer b, Random random)
        {
            this.a = a;
            this.b = b;
            
            this.internalBoardState = new InternalBoardState(random.Next() % 2 == 0 ? MiniPokerCard.Black : MiniPokerCard.Red, null, null);
        }

        private MiniPoker(TPlayer a, TPlayer b, InternalBoardState internalBoardState)
        {
            this.a = a;
            this.b = b;
            this.internalBoardState = internalBoardState;
        }

        public TPlayer CurrentPlayer
        {
            get
            {
                return this.internalBoardState.AsMove == null ? this.a : this.b;
            }
        }

        public IEnumerable<MiniPokerMove> Moves
        {
            get
            {
                if (this.internalBoardState.AsMove == null)
                {
                    if (this.internalBoardState.Card == MiniPokerCard.Red)
                    {
                        yield return MiniPokerMove.Fold;
                    }

                    yield return MiniPokerMove.Hold;
                }
                else if (this.internalBoardState.BsMove == null)
                {
                    yield return MiniPokerMove.Fold;
                    yield return MiniPokerMove.Hold;
                }
                else
                {
                    yield break;
                }
            }
        }

        public MiniPokerBoard Board
        {
            get
            {
                if (this.internalBoardState.AsMove == null)
                {
                    return new MiniPokerBoard.ABoard(this.internalBoardState.Card);
                }
                else
                {
                    if (this.internalBoardState.AsMove == MiniPokerMove.Fold)
                    {
                        return new MiniPokerBoard.PublicBoard(this.internalBoardState.Card, -20);
                    }
                    else if (this.internalBoardState.BsMove == null)
                    {
                        return new MiniPokerBoard.BBoard();
                    }
                    else
                    {
                        var card = this.internalBoardState.Card;
                        var asMove = this.internalBoardState.AsMove;
                        var bsMove = this.internalBoardState.BsMove;
                        var aPurse = 0;
                        switch ((card, asMove, bsMove))
                        {
                            case (MiniPokerCard.Red, MiniPokerMove.Hold, MiniPokerMove.Fold):
                                aPurse += 10;
                                break;
                            case (MiniPokerCard.Red, MiniPokerMove.Hold, MiniPokerMove.Hold):
                                aPurse -= 40;
                                break;
                            case (MiniPokerCard.Red, MiniPokerMove.Fold, null):
                                aPurse -= 20;
                                break;
                            case (MiniPokerCard.Black, MiniPokerMove.Hold, MiniPokerMove.Fold):
                                aPurse += 10;
                                break;
                            case (MiniPokerCard.Black, MiniPokerMove.Hold, MiniPokerMove.Hold):
                                aPurse += 30;
                                break;
                        }

                        return new MiniPokerBoard.PublicBoard(this.internalBoardState.Card, aPurse);
                    }
                }
            }
        }

        public Outcome<TPlayer> Outcome
        {
            get
            {
                if (this.Board is MiniPokerBoard.PublicBoard publicBoard)
                {
                    if (publicBoard.AsPurse > 0)
                    {
                        return new Outcome<TPlayer>(new[] { this.a });
                    }
                    else
                    {
                        return new Outcome<TPlayer>(new[] { this.b });
                    }
                }

                return null;
            }
        }

        public MiniPoker<TPlayer> CommitMove(MiniPokerMove move)
        {
            Console.WriteLine($"{this.CurrentPlayer} selected {move}");

            if (!this.Moves.Contains(move)) //// TODO this will only check instance equality...
            {
                throw new IllegalMoveExeption();
            }

            if (this.internalBoardState.AsMove == null)
            {
                return new MiniPoker<TPlayer>(this.a, this.b, new InternalBoardState(this.internalBoardState.Card, move, null));
            }
            else
            {
                return new MiniPoker<TPlayer>(this.a, this.b, new InternalBoardState(this.internalBoardState.Card, this.internalBoardState.AsMove, move));
            }
        }

        public IEnumerable<IWeightedGame<MiniPoker<TPlayer>, MiniPokerBoard, MiniPokerMove, TPlayer>> ExploreMove(MiniPokerMove move)
        {
            throw new NotImplementedException();
        }

        private sealed class InternalBoardState
        {

            public InternalBoardState(MiniPokerCard card, MiniPokerMove? asMove, MiniPokerMove? bsMove)
            {
                this.Card = card;
                this.AsMove = asMove;
                this.BsMove = bsMove;
            }

            public MiniPokerCard Card { get; }

            public MiniPokerMove? AsMove { get; }

            public MiniPokerMove? BsMove { get; }
        }
    }
}
