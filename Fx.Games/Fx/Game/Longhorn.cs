
using System.Diagnostics.CodeAnalysis;
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

        public static bool TrySkip<T>(this IEnumerable<T> self, out T skipped, out IEnumerable<T> remaining)
        {
            using (var enumerator = self.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    skipped = default;
                    remaining = default;
                    return false;
                }

                skipped = enumerator.Current;
                remaining = TheRest(enumerator);
                return true;
            }
        }

        private static IEnumerable<T> TheRest<T>(IEnumerator<T> enumerator)
        {
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }
    }

    public sealed class Longhorn<TPlayer> : IGame<Longhorn<TPlayer>, LonghornBoard<TPlayer>, LonghornMove, TPlayer>
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

        private readonly (TPlayer player, int orange, int black, int green, int white, IEnumerable<int> gold, bool arrested) player1; //// TODO probably should use longhorplayerstatus
        private readonly (TPlayer player, int orange, int black, int green, int white, IEnumerable<int> gold, bool arrested) player2;
        private readonly Random random;
        private readonly bool previousMoveResultedInGameOver;

        public Longhorn(TPlayer player1, TPlayer player2, Random random)
            : this(random.Next() % 2 == 0, player1, player2, GenerateRandomBoard(random, player1, player2), random)
        {
        }

        private static LonghornBoard<TPlayer> GenerateRandomBoard(Random random, TPlayer player1, TPlayer player2)
        {
            var startingActionTokens = StartingActionTokens.Shuffle(random).Take(9).ToList(); //// TODO use applyaggregation here to create the list and determine if there's a sheriff
            var sheriffIndex = startingActionTokens.FindIndex(token => token is ActionToken.Sheriff);
            if (sheriffIndex != -1)
            {
                //// TODO is this still properly random?
                var temp = startingActionTokens[startingActionTokens.Count - 1];
                startingActionTokens[startingActionTokens.Count - 1] = startingActionTokens[sheriffIndex];
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
                            startingActionToken = startingActionTokens[startingActionTokens.Count - 1];
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

            return new LonghornBoard<TPlayer>(tiles, null, new LonghornPlayerStatus<TPlayer>(player1, 0, 0, 0, 0, Enumerable.Empty<int>()), new LonghornPlayerStatus<TPlayer>(player2, 0, 0, 0, 0, Enumerable.Empty<int>()));
        }

        public Longhorn(bool firstPlayer, TPlayer player1, TPlayer player2, LonghornBoard<TPlayer> board, Random random)
            : this((firstPlayer ? player1 : player2, 0, 0, 0, 0, Enumerable.Empty<int>(), false), (firstPlayer ? player2 : player1, 0, 0, 0, 0, Enumerable.Empty<int>(), false), board, random, false)
        {
        }

        private Longhorn(
            (TPlayer player, int orange, int black, int green, int white, IEnumerable<int> gold, bool arrested) player1,
            (TPlayer player, int orange, int black, int green, int white, IEnumerable<int> gold, bool arrested) player2,
            LonghornBoard<TPlayer> board,
            Random random,
            bool previousMoveResultedInGameOver)
        {
            this.player1 = player1;
            this.player2 = player2;
            this.Board = board;
            this.random = random; //// TODO you don't need this after the initial board is created?
            this.previousMoveResultedInGameOver = previousMoveResultedInGameOver;
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
                            var tile = this.Board.Tiles[i, j];
                            if (tile.BlackCows + tile.GreenCows + tile.OrangeCows + tile.WhiteCows == 4)
                            {
                                yield return new LonghornMove.LocationChoice(new LonghornLocation(i, j));
                            }
                        }
                    }
                }
                else
                {
                    //// TODO The special characteristics of dangerous locations: locations containing yellow Action tokens (Rattlesnake – Sheriff) are considered as dangerous. Consequently you cannot have the same player play twice in succession at the same dangerous location if other possibilities exist.
                    var tile = this.Board.Tiles[playerLocation.Row, playerLocation.Column];
                    var lastColor = LastColor(tile);
                    

                    var takeColors = TakeColors(tile);
                    foreach (var takeColor in takeColors)
                    {
                        var actionMoves = new List<ActionMove?>();
                        if (takeColor.color == lastColor)
                        {
                            //// TODO you shouldn't be able to move somewhere with a null action token; is this asserted anywhere?
                            if (tile.ActionToken is ActionToken.Ambush ambush)
                            {
                                actionMoves.Add(new ActionMove.Ambush(null));
                                var otherPlayer = this.Board.Player2Status;
                                if (otherPlayer.Black != 0)
                                {
                                    actionMoves.Add(new ActionMove.Ambush(TakeColor.Black));
                                }

                                if (otherPlayer.Green != 0)
                                {
                                    actionMoves.Add(new ActionMove.Ambush(TakeColor.Green));
                                }

                                if (otherPlayer.Orange != 0)
                                {
                                    actionMoves.Add(new ActionMove.Ambush(TakeColor.Orange));
                                }

                                if (otherPlayer.White != 0)
                                {
                                    actionMoves.Add(new ActionMove.Ambush(TakeColor.White));
                                }
                            }
                            else if (tile.ActionToken is ActionToken.BrandingIron)
                            {
                                var adjacentLocations = NewLocations(playerLocation, 1);
                                actionMoves.AddRange(adjacentLocations.Select(adjacentLocation => new ActionMove.BrandingIron(adjacentLocation)));
                                //// TODO the rules same "take all of the cattle of the same colour"; does this mean take the cattle of a color, or take the cattle of the color that you selected for your move?
                            }
                            else if (tile.ActionToken is ActionToken.Epidemic)
                            {
                                actionMoves.AddRange(Enumerable.Range(0, 4).Select(color => new ActionMove.Epidemic((TakeColor)color)));
                            }
                            else if (tile.ActionToken is ActionToken.Gold)
                            {
                                //// TODO is it useful to have an actionmove for this?
                                actionMoves.Add(null);
                            }
                            else if (tile.ActionToken is ActionToken.Rattlesnake)
                            {
                                var currentPlayer = this.Board.Player1Status;

                                IEnumerable<LonghornLocation?> blackLocations;
                                if (currentPlayer.Black == 0)
                                {
                                    blackLocations = new LonghornLocation?[] { null };
                                }
                                else
                                {
                                    blackLocations = NewLocations(playerLocation, 1);
                                }

                                IEnumerable<LonghornLocation?> greenLocations;
                                if (currentPlayer.Green == 0)
                                {
                                    greenLocations = new LonghornLocation?[] { null };
                                }
                                else
                                {
                                    greenLocations = NewLocations(playerLocation, 1);
                                }

                                IEnumerable<LonghornLocation?> orangeLocations;
                                if (currentPlayer.Orange == 0)
                                {
                                    orangeLocations = new LonghornLocation?[] { null };
                                }
                                else
                                {
                                    orangeLocations = NewLocations(playerLocation, 1);
                                }

                                IEnumerable<LonghornLocation?> whiteLocations;
                                if (currentPlayer.White == 0)
                                {
                                    whiteLocations = new LonghornLocation?[] { null };
                                }
                                else
                                {
                                    whiteLocations = NewLocations(playerLocation, 1);
                                }

                                actionMoves.AddRange(
                                    blackLocations.SelectMany(
                                        blackLocation => greenLocations.SelectMany(
                                            greenLocation => orangeLocations.SelectMany(
                                                orangeLocation => whiteLocations.Select(
                                                    whiteLocation => new ActionMove.Rattlesnake(blackLocation, greenLocation, orangeLocation, whiteLocation))))));
                            }
                            else if (tile.ActionToken is ActionToken.Sheriff)
                            {
                                //// TODO is it useful to have an actionmove for this?
                                actionMoves.Add(null);
                            }
                            else if (tile.ActionToken is ActionToken.SnakeOil)
                            {
                                //// TODO is it useful to have an actionmove for this?
                                actionMoves.Add(null);
                            }
                        }
                        else
                        {
                            actionMoves.Add(null);
                        }

                        var newLocations = NewLocations(playerLocation, takeColor.count).ToList();

                        //// TODO does applayaggregation help here?
                        if (newLocations.All(location =>
                            this.Board.Tiles[location.Row, location.Column].BlackCows == 0 &&
                            this.Board.Tiles[location.Row, location.Column].GreenCows == 0 &&
                            this.Board.Tiles[location.Row, location.Column].OrangeCows == 0 &&
                            this.Board.Tiles[location.Row, location.Column].WhiteCows == 0))
                        {
                            foreach (var actionMove in actionMoves)
                            {
                                yield return new LonghornMove.LocationMove(takeColor.color, null, actionMove);
                            }
                        }
                        else
                        {
                            foreach (var newLocation in newLocations)
                            {
                                foreach (var actionMove in actionMoves)
                                {
                                    yield return new LonghornMove.LocationMove(takeColor.color, newLocation, actionMove);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static TakeColor? LastColor(LonghornTile tile)
        {
            if (tile.BlackCows != 0 && tile.GreenCows == 0 && tile.OrangeCows == 0 && tile.WhiteCows == 0)
            {
                return TakeColor.Black;
            }

            if (tile.BlackCows == 0 && tile.GreenCows != 0 && tile.OrangeCows == 0 && tile.WhiteCows == 0)
            {
                return TakeColor.Green;
            }

            if (tile.BlackCows == 0 && tile.GreenCows == 0 && tile.OrangeCows != 0 && tile.WhiteCows == 0)
            {
                return TakeColor.Orange;
            }

            if (tile.BlackCows == 0 && tile.GreenCows == 0 && tile.OrangeCows == 0 && tile.WhiteCows != 0)
            {
                return TakeColor.White;
            }

            return null;
        }

        private sealed class LocationComparer : IEqualityComparer<LonghornLocation>
        {
            public bool Equals(LonghornLocation? x, LonghornLocation? y)
            {
                if (x == y)
                {
                    return true;
                }

                if (x == null)
                {
                    return false;
                }

                if (y == null)
                {
                    return false;
                }

                return x.Row == y.Row && x.Column == y.Column;
            }

            public int GetHashCode([DisallowNull] LonghornLocation obj)
            {
                return obj.Row ^ obj.Column;
            }
        }

        private IEnumerable<LonghornLocation> NewLocations(LonghornLocation currentLocation, int moveCount)
        {
            return NewLocations(currentLocation, moveCount, Enumerable.Empty<LonghornLocation>()).Distinct(new LocationComparer());
        }

        private IEnumerable<LonghornLocation> NewLocations(LonghornLocation currentLocation, int moveCount, IEnumerable<LonghornLocation> traversedLocations)
        {
            if (moveCount == 0)
            {
                if (!traversedLocations.Contains(currentLocation, new LocationComparer()))
                {
                    yield return currentLocation;
                }

                yield break;
            }

            if (currentLocation.Row + 1 < 3)
            {
                foreach (var location in NewLocations(new LonghornLocation(currentLocation.Row + 1, currentLocation.Column), moveCount - 1, traversedLocations.Append(currentLocation)))
                {
                    yield return location;
                }
            }

            if (currentLocation.Row - 1 >= 0)
            {
                foreach (var location in NewLocations(new LonghornLocation(currentLocation.Row - 1, currentLocation.Column), moveCount - 1, traversedLocations.Append(currentLocation)))
                {
                    yield return location;
                }
            }

            if (currentLocation.Column + 1 < 3)
            {
                foreach (var location in NewLocations(new LonghornLocation(currentLocation.Row, currentLocation.Column + 1), moveCount - 1, traversedLocations.Append(currentLocation)))
                {
                    yield return location;
                }
            }

            if (currentLocation.Column - 1 >= 0)
            {
                foreach (var location in NewLocations(new LonghornLocation(currentLocation.Row, currentLocation.Column - 1), moveCount - 1, traversedLocations.Append(currentLocation)))
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

        public LonghornBoard<TPlayer> Board { get; }

        public Outcome<TPlayer> Outcome
        {
            get
            {
                if (this.player1.arrested)
                {
                    return new Outcome<TPlayer>(new[] { this.player2.player });
                }
                
                if (this.player2.arrested)
                {
                    return new Outcome<TPlayer>(new[] { this.player1.player });
                }

                if (!this.previousMoveResultedInGameOver && this.Moves.Any())
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

                var player1Score = this.player1.orange * orangeValue + this.player1.black * blackValue + this.player1.green * greenValue + this.player1.white * whiteValue + this.player1.gold.Sum();
                var player2Score = this.player2.orange * orangeValue + this.player2.black * blackValue + this.player2.green * greenValue + this.player2.white * whiteValue + this.player2.gold.Sum();

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
            //// TODO check for legal moves

            var playerLocation = this.Board.PlayerLocation;
            if (playerLocation == null && move is LonghornMove.LocationChoice locationChoice)
            {
                var newBoard = new LonghornBoard<TPlayer>(
                    this.Board.Tiles, 
                    locationChoice.Location, 
                    new LonghornPlayerStatus<TPlayer>(this.player1.player, 0, 0, 0, 0, Enumerable.Empty<int>()), 
                    new LonghornPlayerStatus<TPlayer>(this.player1.player, 0, 0, 0, 0, Enumerable.Empty<int>()));
                return new Longhorn<TPlayer>(this.player2, this.player1, newBoard, this.random, false);
            }
            else if (playerLocation != null && move is LonghornMove.LocationMove locationMove)
            {
                var newPlayer1 = this.player2;
                var newPlayer2 = this.player1;

                var newBoardTiles = this.Board.Tiles;
                var currentTile = newBoardTiles[playerLocation.Row, playerLocation.Column];
                var lastCowColor = LastColor(currentTile);
                var takeActionToken = lastCowColor == locationMove.TakeColor;
                var actionToken = currentTile.ActionToken;

                if (locationMove.TakeColor == TakeColor.Black)
                {
                    newPlayer2.black += currentTile.BlackCows;
                    newBoardTiles[playerLocation.Row, playerLocation.Column] = new LonghornTile(currentTile.OrangeCows, 0, currentTile.GreenCows, currentTile.WhiteCows, takeActionToken ? null : currentTile.ActionToken);
                }
                else if (locationMove.TakeColor == TakeColor.Green)
                {
                    newPlayer2.green += currentTile.GreenCows;
                    newBoardTiles[playerLocation.Row, playerLocation.Column] = new LonghornTile(currentTile.OrangeCows, currentTile.BlackCows, 0, currentTile.WhiteCows, takeActionToken ? null : currentTile.ActionToken);
                }
                else if (locationMove.TakeColor == TakeColor.Orange)
                {
                    newPlayer2.orange += currentTile.OrangeCows;
                    newBoardTiles[playerLocation.Row, playerLocation.Column] = new LonghornTile(0, currentTile.BlackCows, currentTile.GreenCows, currentTile.WhiteCows, takeActionToken ? null : currentTile.ActionToken);
                }
                else if (locationMove.TakeColor == TakeColor.White)
                {
                    newPlayer2.white += currentTile.WhiteCows;
                    newBoardTiles[playerLocation.Row, playerLocation.Column] = new LonghornTile(currentTile.OrangeCows, currentTile.BlackCows, currentTile.GreenCows, 0, takeActionToken ? null : currentTile.ActionToken);
                }

                if (takeActionToken)
                {
                    if (actionToken is ActionToken.Ambush ambushToken && locationMove.ActionMove is ActionMove.Ambush ambushMove)
                    {
                        if (ambushMove.Color == null)
                        {
                            if (newPlayer1.gold.Shuffle(this.random).TrySkip(out var stolen, out var kept))
                            {
                                newPlayer1.gold = kept;
                                newPlayer2.gold = newPlayer2.gold.Append(stolen);
                            }
                        }
                        else if (ambushMove.Color == TakeColor.Black)
                        {
                            var stolen = Math.Min(newPlayer1.black, 2);
                            newPlayer1.black -= stolen;
                            newPlayer2.black += stolen;
                        }
                        else if (ambushMove.Color == TakeColor.Green)
                        {
                            var stolen = Math.Min(newPlayer1.green, 2);
                            newPlayer1.green -= stolen;
                            newPlayer2.green += stolen;
                        }
                        else if (ambushMove.Color == TakeColor.Orange)
                        {
                            var stolen = Math.Min(newPlayer1.orange, 2);
                            newPlayer1.orange -= stolen;
                            newPlayer2.orange += stolen;
                        }
                        else if (ambushMove.Color == TakeColor.White)
                        {
                            var stolen = Math.Min(newPlayer1.white, 2);
                            newPlayer1.white -= stolen;
                            newPlayer2.white += stolen;
                        }
                        else
                        {
                            throw new IllegalMoveExeption("TODO ambush color invalid");
                        }
                    }
                    else if (actionToken is ActionToken.BrandingIron brandingIronToken && locationMove.ActionMove is ActionMove.BrandingIron brandingIronMove)
                    {
                        var brandingLocation = brandingIronMove.Location;
                        var brandingTile = newBoardTiles[brandingLocation.Row, brandingLocation.Column];
                        if (locationMove.TakeColor == TakeColor.Black)
                        {
                            newPlayer2.black += brandingTile.BlackCows;
                            newBoardTiles[brandingLocation.Row, brandingLocation.Column] = new LonghornTile(brandingTile.OrangeCows, 0, brandingTile.GreenCows, brandingTile.WhiteCows, brandingTile.ActionToken);
                        }
                        else if (locationMove.TakeColor == TakeColor.Green)
                        {
                            newPlayer2.green += brandingTile.GreenCows;
                            newBoardTiles[brandingLocation.Row, brandingLocation.Column] = new LonghornTile(brandingTile.OrangeCows, brandingTile.BlackCows, 0, brandingTile.WhiteCows, brandingTile.ActionToken);
                        }
                        else if (locationMove.TakeColor == TakeColor.Orange)
                        {
                            newPlayer2.orange += brandingTile.OrangeCows;
                            newBoardTiles[brandingLocation.Row, brandingLocation.Column] = new LonghornTile(0, brandingTile.BlackCows, brandingTile.GreenCows, brandingTile.WhiteCows, brandingTile.ActionToken);
                        }
                        else if (locationMove.TakeColor == TakeColor.White)
                        {
                            newPlayer2.white += brandingTile.WhiteCows;
                            newBoardTiles[brandingLocation.Row, brandingLocation.Column] = new LonghornTile(brandingTile.OrangeCows, brandingTile.BlackCows, brandingTile.GreenCows, 0, brandingTile.ActionToken);
                        }
                    }
                    else if (actionToken is ActionToken.Epidemic epidemicToken && locationMove.ActionMove is ActionMove.Epidemic epidemicMove)
                    {
                        for (int i = 0; i < 3; ++i)
                        {
                            for (int j = 0; j < 3; ++j)
                            {
                                var originalTile = newBoardTiles[i, j];
                                if (epidemicMove.Color == TakeColor.Black)
                                {
                                    newBoardTiles[i, j] = new LonghornTile(originalTile.OrangeCows, 0, originalTile.GreenCows, originalTile.WhiteCows, originalTile.ActionToken);
                                }
                                else if (epidemicMove.Color == TakeColor.Green)
                                {
                                    newBoardTiles[i, j] = new LonghornTile(originalTile.OrangeCows, originalTile.BlackCows, 0, originalTile.WhiteCows, originalTile.ActionToken);
                                }
                                else if (epidemicMove.Color == TakeColor.Orange)
                                {
                                    newBoardTiles[i, j] = new LonghornTile(0, originalTile.BlackCows, originalTile.GreenCows, originalTile.WhiteCows, originalTile.ActionToken);
                                }
                                else if (epidemicMove.Color == TakeColor.White)
                                {
                                    newBoardTiles[i, j] = new LonghornTile(originalTile.OrangeCows, originalTile.BlackCows, originalTile.GreenCows, 0, originalTile.ActionToken);
                                }
                            }
                        }
                    }
                    else if (actionToken is ActionToken.Gold goldToken && locationMove.ActionMove == null)
                    {
                        newPlayer2.gold = newPlayer2.gold.Append(goldToken.Amount);
                    }
                    else if (actionToken is ActionToken.Rattlesnake rattlesnakeToken && locationMove.ActionMove is ActionMove.Rattlesnake rattlesnakeMove)
                    {
                        if (rattlesnakeMove.BlackLocation != null)
                        {
                            //// TODO assert that newplayer2.black is greater than 0?
                            newPlayer2.black -= 1;
                            var cowLocation = rattlesnakeMove.BlackLocation;
                            var originalTile = newBoardTiles[cowLocation.Row, cowLocation.Column];
                            newBoardTiles[cowLocation.Row, cowLocation.Column] = new LonghornTile(originalTile.OrangeCows, originalTile.BlackCows + 1, originalTile.GreenCows, originalTile.WhiteCows, originalTile.ActionToken);
                        }
                        
                        if (rattlesnakeMove.GreenLocation != null)
                        {
                            //// TODO assert that newplayer2.black is greater than 0?
                            newPlayer2.green -= 1;
                            var cowLocation = rattlesnakeMove.GreenLocation;
                            var originalTile = newBoardTiles[cowLocation.Row, cowLocation.Column];
                            newBoardTiles[cowLocation.Row, cowLocation.Column] = new LonghornTile(originalTile.OrangeCows, originalTile.BlackCows, originalTile.GreenCows + 1, originalTile.WhiteCows, originalTile.ActionToken);
                        }
                        
                        if (rattlesnakeMove.OrangeLocation != null)
                        {
                            //// TODO assert that newplayer2.black is greater than 0?
                            newPlayer2.orange -= 1;
                            var cowLocation = rattlesnakeMove.OrangeLocation;
                            var originalTile = newBoardTiles[cowLocation.Row, cowLocation.Column];
                            newBoardTiles[cowLocation.Row, cowLocation.Column] = new LonghornTile(originalTile.OrangeCows + 1, originalTile.BlackCows, originalTile.GreenCows, originalTile.WhiteCows, originalTile.ActionToken);
                        }
                        
                        if (rattlesnakeMove.WhiteLocation != null)
                        {
                            //// TODO assert that newplayer2.black is greater than 0?
                            newPlayer2.white -= 1;
                            var cowLocation = rattlesnakeMove.WhiteLocation;
                            var originalTile = newBoardTiles[cowLocation.Row, cowLocation.Column];
                            newBoardTiles[cowLocation.Row, cowLocation.Column] = new LonghornTile(originalTile.OrangeCows, originalTile.BlackCows, originalTile.GreenCows, originalTile.WhiteCows + 1, originalTile.ActionToken);
                        }
                    }
                    else if (actionToken is ActionToken.Sheriff sheriffToken && locationMove.ActionMove == null)
                    {
                        /*if (locationMove.NewLocation != null)
                        {
                            //// TODO this actually gets thrown erroneously
                            throw new IllegalMoveExeption("TODO sheriff token should end the game");
                        }*/

                        newPlayer2.arrested = true;
                    }
                    else if (actionToken is ActionToken.SnakeOil snakeOilToken && locationMove.ActionMove == null)
                    {
                        var temp = newPlayer1;
                        newPlayer1 = newPlayer2;
                        newPlayer2 = temp;
                    }
                    else
                    {
                        throw new IllegalMoveExeption("TODO action token and action move aren't aligned");
                    }
                }

                var newBoard = new LonghornBoard<TPlayer>(
                    newBoardTiles, 
                    locationMove.NewLocation,
                    new LonghornPlayerStatus<TPlayer>(newPlayer1.player, newPlayer1.orange, newPlayer1.black, newPlayer1.green, newPlayer1.white, newPlayer1.gold),
                    new LonghornPlayerStatus<TPlayer>(newPlayer2.player, newPlayer2.orange, newPlayer2.black, newPlayer2.green, newPlayer2.white, newPlayer2.gold));

                //// TODO what about if a player gets 9 cows of the same color?
                return new Longhorn<TPlayer>(newPlayer1, newPlayer2, newBoard, random, locationMove.NewLocation == null);
            }
            else
            {
                throw new IllegalMoveExeption("TODO");
            }
        }
    }

    public sealed class LonghornBoard<TPlayer>
    {
        private readonly LonghornTile[,] tiles;

        public LonghornBoard(LonghornTile[,] tiles, LonghornLocation? playerLocation, LonghornPlayerStatus<TPlayer> player1Status, LonghornPlayerStatus<TPlayer> player2Status)
        {
            this.tiles = tiles.Clone2();
            this.PlayerLocation = playerLocation;
            this.Player1Status = player1Status;
            this.Player2Status = player2Status;
        }

        public LonghornTile[,] Tiles
        {
            get
            {
                return this.tiles.Clone2();
            }
        }

        public LonghornLocation? PlayerLocation { get; }

        public LonghornPlayerStatus<TPlayer> Player1Status { get; }

        public LonghornPlayerStatus<TPlayer> Player2Status { get; }
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
        public LonghornTile(int orangeCows, int blackCows, int greenCows, int whiteCows, ActionToken? actionToken)
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

        public ActionToken? ActionToken { get; }
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
            public LocationMove(TakeColor takeColor, LonghornLocation? newLocation, ActionMove? actionMove)
            {
                //// TODO assert

                this.TakeColor = takeColor;
                this.NewLocation = newLocation;
                this.ActionMove = actionMove;
            }

            public TakeColor TakeColor { get; }

            public LonghornLocation? NewLocation { get; } //// TODO use inheritance; this is nullable because if all of the available tiles have no cows, the game is over

            public ActionMove? ActionMove { get; }
        }
    }

    public abstract class ActionMove
    {
        private ActionMove()
        {
        }

        public sealed class Ambush : ActionMove
        {
            public Ambush(TakeColor? color)
            {
                this.Color = color;
            }

            public TakeColor? Color { get; } //// TODO null means steal a random gold nugget
        }

        public sealed class BrandingIron : ActionMove
        {
            public BrandingIron(LonghornLocation location)
            {
                this.Location = location;
            }

            public LonghornLocation Location { get; }
        }

        public sealed class Epidemic : ActionMove
        {
            public Epidemic(TakeColor color)
            {
                this.Color = color;
            }

            public TakeColor Color { get; }
        }

        public sealed class Rattlesnake : ActionMove
        {
            public Rattlesnake(LonghornLocation? blackLocation, LonghornLocation? greenLocation, LonghornLocation? orangeLocation, LonghornLocation? whiteLocation)
            {
                this.BlackLocation = blackLocation;
                this.GreenLocation = greenLocation;
                this.OrangeLocation = orangeLocation;
                this.WhiteLocation = whiteLocation;
            }

            public LonghornLocation? BlackLocation { get; }
            public LonghornLocation? GreenLocation { get; }
            public LonghornLocation? OrangeLocation { get; }
            public LonghornLocation? WhiteLocation { get; }
        }
    }

    public sealed class LonghornPlayerStatus<TPlayer>
    {
        public LonghornPlayerStatus(TPlayer player, int orange, int black, int green, int white, IEnumerable<int> goldNuggets)
        {
            this.Player = player;
            this.Orange = orange;
            this.Black = black;
            this.Green = green;
            this.White = white;
            this.GoldNuggets = goldNuggets.ToList();
        }

        public TPlayer Player { get; }
        public int Orange { get; }
        public int Black { get; }
        public int Green { get; }
        public int White { get; }

        public IEnumerable<int> GoldNuggets { get; }
    }

    public enum TakeColor
    {
        Orange,
        Black,
        Green,
        White,
    }
}
