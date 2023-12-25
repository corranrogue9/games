﻿
namespace Fx.Game
{
    public static class Extensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> self, Random random)
        {
            var array = self.ToArray();
            for (int i = 0; i < array.Length; ++i)
            {
                var next = random.Next(i, array.Length);
                var temp = array[i];
                array[i] = array[next];
                array[next] = temp;
                yield return array[i];
            }
        }
    }

    public sealed class Longhorn<TPlayer> : IGame<Longhorn<TPlayer>, LonghornBoard, LonghornMove, TPlayer>
    {
        private readonly (TPlayer player, int orange, int black, int green, int white, int gold) player1;
        private readonly (TPlayer player, int orange, int black, int green, int white, int gold) player2;
        private readonly Random random;

        public Longhorn(TPlayer player1, TPlayer player2, Random random)
            : this(player1, player2, GenerateRandomBoard(random), random)
        {
        }

        private static LonghornBoard GenerateRandomBoard(Random random)
        {
            //// TODO one of the players is supposed to pick where the other one starts

            var startingLocation = (random.Next(0, 3), random.Next(0, 3));

            //// TODO how many cows are there?
            var cows = Enumerable
                .Repeat(0, 15)
                .Concat(Enumerable.Repeat(1, 15))
                .Concat(Enumerable.Repeat(2, 15))
                .Concat(Enumerable.Repeat(3, 15))
                .Shuffle(random);
            using (var cowsEnumerator = cows.GetEnumerator())
            {
                var tiles = new LonghornTile[3, 3];
                for (int i = 0; i < 3; ++i)
                {
                    for (int j = 0; j < 3; ++j)
                    {
                        var numberOfCows = random.Next(3, 7); //// TODO is 6 the max? what is the distribution of tiles? there should probably be tile instances and random ones is chosen
                        var orange = 0;
                        var black = 0;
                        var green = 0;
                        var white = 0;
                        for (int k = 0; k < numberOfCows; ++k)
                        {
                            cowsEnumerator.MoveNext();
                            var current = cowsEnumerator.Current;
                            if (current == 0)
                            {
                                ++orange;
                            }
                            else if (current == 1)
                            {
                                ++black;
                            }
                            else if (current == 2)
                            {
                                ++green;
                            }
                            else
                            {
                                ++white;
                            }
                        }

                        var gold = random.Next(1, 5);
                        tiles[i, j] = new LonghornTile(orange, black, green, white, gold);
                    }
                }

                return new LonghornBoard(tiles, startingLocation);
            }
        }

        public Longhorn(TPlayer player1, TPlayer player2, LonghornBoard board, Random random)
            : this((player1, 0, 0, 0, 0, 0), (player2, 0, 0, 0, 0, 0), board, random)
        {
        }

        private Longhorn(
            (TPlayer player, int orange, int black, int green, int white, int gold) player1,
            (TPlayer player, int orange, int black, int green, int white, int gold) player2,
            LonghornBoard board,
            Random random)
        {
            this.player1 = player1;
            this.player2 = player2;
            this.Board = board;
            this.random = random;
        }

        public TPlayer CurrentPlayer
        {
            get
            {
                return this.player1.player;
            }
        }

        public IEnumerable<LonghornMove> Moves
        {
            get
            {
                var playerLocation = this.Board.PlayerLocation;
                var tile = this.Board.Tiles[playerLocation.row, playerLocation.column];
                var takeColors = TakeColors(tile);
                foreach (var takeColor in takeColors)
                {
                    var newLocations = NewLocations(playerLocation, takeColor.count);
                    foreach (var newLocation in newLocations)
                    {
                        yield return new LonghornMove(takeColor.color, newLocation);
                    }
                }
            }
        }

        private IEnumerable<(int row, int column)> NewLocations((int row, int column) currentLocation, int moveCount)
        {
            //// TODO i didn't quite understand how this works; can you go through the same tile twice? can you end at the tile you start at? what are the rules here...
            if (moveCount == 0)
            {
                yield return currentLocation;
                yield break;
            }

            if (currentLocation.row + 1 < 3)
            {
                foreach (var location in NewLocations((currentLocation.row + 1, currentLocation.column), moveCount - 1))
                {
                    yield return location;
                }
            }

            if (currentLocation.row - 1 >= 0)
            {
                foreach (var location in NewLocations((currentLocation.row - 1, currentLocation.column), moveCount - 1))
                {
                    yield return location;
                }
            }

            if (currentLocation.column + 1 < 3)
            {
                foreach (var location in NewLocations((currentLocation.row, currentLocation.column + 1), moveCount - 1))
                {
                    yield return location;
                }
            }

            if (currentLocation.column - 1 >= 0)
            {
                foreach (var location in NewLocations((currentLocation.row, currentLocation.column - 1), moveCount - 1))
                {
                    yield return location;
                }
            }
        }

        private IEnumerable<(TakeColor color, int count)> TakeColors(LonghornTile tile)
        {
            if (tile.OrangeCows > 0)
            {
                yield return (TakeColor.Orange, tile.OrangeCows);
            }
            
            if (tile.BlackCows > 0)
            {
                yield return (TakeColor.Black, tile.BlackCows);
            }
            
            if (tile.GreenCows > 0)
            {
                yield return (TakeColor.Green, tile.GreenCows);
            }
            
            if (tile.WhiteCows > 0)
            {
                yield return (TakeColor.White, tile.WhiteCows);
            }
        }

        public LonghornBoard Board { get; }

        public Outcome<TPlayer> Outcome
        {
            get
            {
                if (this.Moves.Any())
                {
                    return null;
                }

                var orangeValue = 0;
                var blackValue = 0;
                var greenValue = 0;
                var whiteValue = 0;
                foreach (var tile in this.Board.Tiles)
                {
                    orangeValue += tile.OrangeCows;
                    blackValue += tile.BlackCows;
                    greenValue += tile.GreenCows;
                    whiteValue += tile.WhiteCows;
                }

                var player1Score = this.player1.orange * orangeValue + this.player1.black * blackValue + this.player1.green * greenValue + this.player1.white * whiteValue + this.player1.gold;
                var player2Score = this.player2.orange * orangeValue + this.player2.black * blackValue + this.player2.green * greenValue + this.player2.white * whiteValue + this.player2.gold;

                var winners = new List<TPlayer>();
                if (player1Score >= player2Score)
                {
                    winners.Add(this.player1.player);
                }

                if (player2Score >= player1Score)
                {
                    winners.Add(this.player2.player);
                }

                return new Outcome<TPlayer>(winners);
            }
        }

        public Longhorn<TPlayer> CommitMove(LonghornMove move)
        {
            var newPlayer1 = this.player2;
            var newPlayer2 = this.player1;

            var newBoardTiles = this.Board.Tiles;
            if (move.TakeColor == TakeColor.Black)
            {
                var currentTile = newBoardTiles[this.Board.PlayerLocation.row, this.Board.PlayerLocation.column];
                newPlayer2.black += currentTile.BlackCows;
                newBoardTiles[this.Board.PlayerLocation.row, this.Board.PlayerLocation.column] = new LonghornTile(currentTile.OrangeCows, 0, currentTile.GreenCows, currentTile.WhiteCows, currentTile.Gold);
            }
            else if (move.TakeColor == TakeColor.Green)
            {
                var currentTile = newBoardTiles[this.Board.PlayerLocation.row, this.Board.PlayerLocation.column];
                newPlayer2.green += currentTile.GreenCows;
                newBoardTiles[this.Board.PlayerLocation.row, this.Board.PlayerLocation.column] = new LonghornTile(currentTile.OrangeCows, currentTile.BlackCows, 0, currentTile.WhiteCows, currentTile.Gold);
            }
            else if (move.TakeColor == TakeColor.Orange)
            {
                var currentTile = newBoardTiles[this.Board.PlayerLocation.row, this.Board.PlayerLocation.column];
                newPlayer2.orange += currentTile.OrangeCows;
                newBoardTiles[this.Board.PlayerLocation.row, this.Board.PlayerLocation.column] = new LonghornTile(0, currentTile.BlackCows, currentTile.GreenCows, currentTile.WhiteCows, currentTile.Gold);
            }
            else if (move.TakeColor == TakeColor.White)
            {
                var currentTile = newBoardTiles[this.Board.PlayerLocation.row, this.Board.PlayerLocation.column];
                newPlayer2.white += currentTile.WhiteCows;
                newBoardTiles[this.Board.PlayerLocation.row, this.Board.PlayerLocation.column] = new LonghornTile(currentTile.OrangeCows, currentTile.BlackCows, currentTile.GreenCows, 0, currentTile.Gold);
            }

            var newBoard = new LonghornBoard(newBoardTiles, move.NewLocation);

            return new Longhorn<TPlayer>(newPlayer1, newPlayer2, newBoard, random);
        }
    }

    public sealed class LonghornBoard
    {
        private readonly LonghornTile[,] tiles;

        public LonghornBoard(LonghornTile[,] tiles, (int row, int column) playerLocation)
        {
            this.tiles = tiles.Clone2();
            this.PlayerLocation = playerLocation;
        }

        public LonghornTile[,] Tiles
        {
            get
            {
                return this.tiles.Clone2();
            }
        }

        public (int row, int column) PlayerLocation { get; }
    }

    public sealed class LonghornTile
    {
        public LonghornTile(int orangeCows, int blackCows, int greenCows, int whiteCows, int gold)
        {
            this.OrangeCows = orangeCows;
            this.BlackCows = blackCows;
            this.GreenCows = greenCows;
            this.WhiteCows = whiteCows;

            //// TODO what about the special action thing?
            this.Gold = gold;
        }

        public int OrangeCows { get; }
        public int BlackCows { get; }
        public int GreenCows { get; }
        public int WhiteCows { get; }
        public int Gold { get; }
    }

    public sealed class LonghornMove
    {
        public LonghornMove(TakeColor takeColor, (int row, int column) newLocation)
        {
            this.TakeColor = takeColor;
            this.NewLocation = newLocation;
        }

        public TakeColor TakeColor { get; }

        public (int row, int column) NewLocation { get; }
    }

    public enum TakeColor
    {
        Orange,
        Black,
        Green,
        White,
    }
}