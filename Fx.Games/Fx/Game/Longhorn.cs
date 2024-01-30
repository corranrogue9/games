
using System.Runtime.CompilerServices;

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
        private sealed class StartingTile
        {
            private StartingTile(int numberOfCows, bool isNuggetHill)
            {
                this.NumbrOfCows = numberOfCows;
                this.IsNuggetHill = isNuggetHill;
            }

            public int NumbrOfCows { get; }

            public bool IsNuggetHill { get; }

            public static IEnumerable<StartingTile> StartingTiles
            {
                get
                {
                    yield return new StartingTile(3, false);
                    yield return new StartingTile(4, false);
                    yield return new StartingTile(6, true);
                    yield return new StartingTile(4, false);
                    yield return new StartingTile(5, false);
                    yield return new StartingTile(4, false);
                    yield return new StartingTile(4, false);
                    yield return new StartingTile(4, false);
                    yield return new StartingTile(2, false);
                }
            }
        }

        private static IEnumerable<ActionToken> StartingActionTokens
        {
            get
            {
                yield return new ActionToken.Gold(200);
                yield return new ActionToken.Gold(200);
                yield return new ActionToken.Gold(300);
                yield return new ActionToken.Gold(300);
                yield return new ActionToken.Gold(400);
                yield return new ActionToken.Gold(500);
                //// TODO i don't know if the quanities of the above gold actions are correct, the picture doesn't seem to show it
                yield return new ActionToken.Ambush();
                yield return new ActionToken.Ambush();
                yield return new ActionToken.Ambush();
                yield return new ActionToken.Epidemic();
                yield return new ActionToken.BrandingIron();
                yield return new ActionToken.BrandingIron();
                yield return new ActionToken.BrandingIron();
                yield return new ActionToken.SnakeOil();
                yield return new ActionToken.SnakeOil();
                yield return new ActionToken.SnakeOil();
                yield return new ActionToken.Rattlesnake();
                yield return new ActionToken.Rattlesnake();
                yield return new ActionToken.Sheriff();
            }
        }

        private readonly (TPlayer player, int orange, int black, int green, int white, int gold) player1;
        private readonly (TPlayer player, int orange, int black, int green, int white, int gold) player2;
        private readonly Random random;

        public Longhorn(TPlayer player1, TPlayer player2, Random random)
            : this(random.Next() % 2 == 0, player1, player2, GenerateRandomBoard(random), random)
        {
        }

        private static LonghornBoard GenerateRandomBoard(Random random)
        {
            var startingActionTokens = StartingActionTokens.Shuffle(random).Take(9).ToList(); //// TODO use applyaggregation here to create the list and determine if there's a sheriff
            var sheriffIndex = startingActionTokens.FindIndex(token => token is ActionToken.Sheriff);
            if (sheriffIndex != -1)
            {
                //// TODO is this still properly random?
                var temp = startingActionTokens[startingActionTokens.Count];
                startingActionTokens[startingActionTokens.Count] = startingActionTokens[sheriffIndex];
                startingActionTokens[sheriffIndex] = temp;
            }

            var startingTiles = StartingTile.StartingTiles.Shuffle(random);
            var cows = Enumerable
                .Repeat(0, 9) //// TODO use enum?
                .Concat(Enumerable.Repeat(1, 9))
                .Concat(Enumerable.Repeat(2, 9))
                .Concat(Enumerable.Repeat(3, 9))
                .Shuffle(random);

            var tiles = new LonghornTile[3, 3];
            using (var startingActionTokensEnumerator = startingActionTokens.GetEnumerator())
            using (var startingTilesEnumerator = startingTiles.GetEnumerator())
            using (var cowsEnumerator = cows.GetEnumerator())
            {
                startingActionTokensEnumerator.MoveNext();
                startingTilesEnumerator.MoveNext();
                cowsEnumerator.MoveNext();

                for (int i = 0; i < 3; ++i)
                {
                    for (int j = 0; j < 3; ++j)
                    {
                        ActionToken startingActionToken;
                        if (sheriffIndex != -1 && startingTilesEnumerator.Current.IsNuggetHill)
                        {
                            startingActionToken = startingActionTokens[startingActionTokens.Count];
                        }
                        else
                        {
                            startingActionToken = startingActionTokensEnumerator.Current;
                            startingActionTokensEnumerator.MoveNext();
                        }

                        var orange = 0;
                        var black = 0;
                        var green = 0;
                        var white = 0;
                        for (int k = 0; k < startingTilesEnumerator.Current.NumbrOfCows; ++k)
                        {
                            if (cowsEnumerator.Current == 0)
                            {
                                ++orange;
                            }
                            else if (cowsEnumerator.Current == 1)
                            {
                                ++black;
                            }
                            else if (cowsEnumerator.Current == 2)
                            {
                                ++green;
                            }
                            else if (cowsEnumerator.Current == 3)
                            {
                                ++white;
                            }

                            cowsEnumerator.MoveNext();
                        }

                        tiles[i, j] = new LonghornTile(orange, black, green, white, startingActionToken);
                        startingTilesEnumerator.MoveNext();
                    }
                }
            }

            return new LonghornBoard(tiles, null);


            /*//// TODO one of the players is supposed to pick where the other one starts

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
            }*/
        }

        public Longhorn(bool firstPlayer, TPlayer player1, TPlayer player2, LonghornBoard board, Random random)
            : this((firstPlayer ? player1 : player2, 0, 0, 0, 0, 0), (firstPlayer ? player2 : player1, 0, 0, 0, 0, 0), board, random)
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
            this.random = random; //// TODO you don't need this after the initial board is created?
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
                if (playerLocation == null)
                {
                    for (int i = 0; i < 3; ++i)
                    {
                        for (int j = 0; j < 3; ++j) //// TODO remove the hardcoded 3's everywhere if possible
                        {
                            yield return new LonghornMove.LocationChoice(new LonghornLocation(i, j));
                        }
                    }
                }
                else
                {
                    var tile = this.Board.Tiles[playerLocation.Row, playerLocation.Column];
                    var takeColors = TakeColors(tile);
                    foreach (var takeColor in takeColors)
                    {
                        var newLocations = NewLocations(playerLocation, takeColor.count);
                        foreach (var newLocation in newLocations)
                        {
                            yield return new LonghornMove.LocationMove(takeColor.color, newLocation);
                        }
                    }
                }
            }
        }

        private IEnumerable<LonghornLocation> NewLocations(LonghornLocation currentLocation, int moveCount)
        {
            //// TODO i didn't quite understand how this works; can you go through the same tile twice? can you end at the tile you start at? what are the rules here...
            if (moveCount == 0)
            {
                yield return currentLocation;
                yield break;
            }

            if (currentLocation.Row + 1 < 3)
            {
                foreach (var location in NewLocations(new LonghornLocation(currentLocation.Row + 1, currentLocation.Column), moveCount - 1))
                {
                    yield return location;
                }
            }

            if (currentLocation.Row - 1 >= 0)
            {
                foreach (var location in NewLocations(new LonghornLocation(currentLocation.Row - 1, currentLocation.Column), moveCount - 1))
                {
                    yield return location;
                }
            }

            if (currentLocation.Column + 1 < 3)
            {
                foreach (var location in NewLocations(new LonghornLocation(currentLocation.Row, currentLocation.Column + 1), moveCount - 1))
                {
                    yield return location;
                }
            }

            if (currentLocation.Column - 1 >= 0)
            {
                foreach (var location in NewLocations(new LonghornLocation(currentLocation.Row, currentLocation.Column - 1), moveCount - 1))
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
            var playerLocation = this.Board.PlayerLocation;
            if (playerLocation == null && move is LonghornMove.LocationChoice locationChoice)
            {
                var newBoard = new LonghornBoard(this.Board.Tiles, locationChoice.Location);
                return new Longhorn<TPlayer>(this.player2, this.player1, newBoard, this.random);
            }
            else if (playerLocation != null && move is LonghornMove.LocationMove locationMove)
            {
                var newPlayer1 = this.player2;
                var newPlayer2 = this.player1;

                var newBoardTiles = this.Board.Tiles;
                if (locationMove.TakeColor == TakeColor.Black)
                {
                    var currentTile = newBoardTiles[playerLocation.Row, playerLocation.Column];
                    newPlayer2.black += currentTile.BlackCows;
                    newBoardTiles[playerLocation.Row, playerLocation.Column] = new LonghornTile(currentTile.OrangeCows, 0, currentTile.GreenCows, currentTile.WhiteCows, currentTile.ActionToken);
                }
                else if (locationMove.TakeColor == TakeColor.Green)
                {
                    var currentTile = newBoardTiles[playerLocation.Row, playerLocation.Column];
                    newPlayer2.green += currentTile.GreenCows;
                    newBoardTiles[playerLocation.Row, playerLocation.Column] = new LonghornTile(currentTile.OrangeCows, currentTile.BlackCows, 0, currentTile.WhiteCows, currentTile.ActionToken);
                }
                else if (locationMove.TakeColor == TakeColor.Orange)
                {
                    var currentTile = newBoardTiles[playerLocation.Row, playerLocation.Column];
                    newPlayer2.orange += currentTile.OrangeCows;
                    newBoardTiles[playerLocation.Row, playerLocation.Column] = new LonghornTile(0, currentTile.BlackCows, currentTile.GreenCows, currentTile.WhiteCows, currentTile.ActionToken);
                }
                else if (locationMove.TakeColor == TakeColor.White)
                {
                    var currentTile = newBoardTiles[playerLocation.Row, playerLocation.Column];
                    newPlayer2.white += currentTile.WhiteCows;
                    newBoardTiles[playerLocation.Row, playerLocation.Column] = new LonghornTile(currentTile.OrangeCows, currentTile.BlackCows, currentTile.GreenCows, 0, currentTile.ActionToken);
                }

                var newBoard = new LonghornBoard(newBoardTiles, locationMove.NewLocation);

                return new Longhorn<TPlayer>(newPlayer1, newPlayer2, newBoard, random);
            }
            else
            {
                throw new IllegalMoveExeption("TODO");
            }
        }
    }

    public sealed class LonghornBoard
    {
        private readonly LonghornTile[,] tiles;

        public LonghornBoard(LonghornTile[,] tiles, LonghornLocation? playerLocation)
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

        public LonghornLocation? PlayerLocation { get; }
    }

    public sealed class LonghornLocation
    {
        public LonghornLocation(int row, int column)
        {
            //// TODO assert

            this.Row = row;
            this.Column = column;
        }

        public int Row { get; }
        public int Column { get; }
    }

    public sealed class LonghornTile
    {
        public LonghornTile(int orangeCows, int blackCows, int greenCows, int whiteCows, ActionToken actionToken)
        {
            //// TODO assert

            this.OrangeCows = orangeCows;
            this.BlackCows = blackCows;
            this.GreenCows = greenCows;
            this.WhiteCows = whiteCows;
            this.ActionToken = actionToken;
        }

        public int OrangeCows { get; }

        public int BlackCows { get; }

        public int GreenCows { get; }

        public int WhiteCows { get; }

        public ActionToken ActionToken { get; }
    }

    public abstract class ActionToken
    {
        private ActionToken()
        {
        }

        public sealed class Gold : ActionToken
        {
            public Gold(int amount)
            {
                //// TODO assert 
                this.Amount = amount;
            }

            public int Amount { get; }
        }

        public sealed class SnakeOil : ActionToken
        {
        }

        public sealed class BrandingIron : ActionToken
        {
        }

        public sealed class Ambush : ActionToken
        {
        }

        public sealed class Epidemic : ActionToken
        {
        }

        public sealed class Rattlesnake : ActionToken
        {
        }

        public sealed class Sheriff : ActionToken
        {
        }
    }

    public abstract class LonghornMove
    {
        private LonghornMove()
        {
        }

        public sealed class LocationChoice : LonghornMove
        {
            public LocationChoice(LonghornLocation location)
            {
                this.Location = location;
            }

            public LonghornLocation Location { get; }
        }

        public sealed class LocationMove : LonghornMove
        {
            public LocationMove(TakeColor takeColor, LonghornLocation newLocation)
            {
                //// TODO assert

                this.TakeColor = takeColor;
                this.NewLocation = newLocation;
            }

            public TakeColor TakeColor { get; }

            public LonghornLocation NewLocation { get; }
        }
    }

    public enum TakeColor
    {
        Orange,
        Black,
        Green,
        White,
    }
}
