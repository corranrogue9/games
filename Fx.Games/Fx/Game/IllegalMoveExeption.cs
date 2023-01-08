namespace Fx.Game
{
    using System;

    public sealed class IllegalMoveExeption : Exception //// TODO implement correctly
    {
        public IllegalMoveExeption()
            : base()
        {
        }

        public IllegalMoveExeption(string message)
            : base(message)
        {
        }
    }
}
