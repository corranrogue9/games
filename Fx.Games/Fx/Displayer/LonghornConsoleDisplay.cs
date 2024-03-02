namespace Fx.Displayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fx.Game;
    using Fx.Game.Chess;
    using Fx.Tree;

    public sealed class LonghornConsoleDisplay<TPlayer> : IDisplayer<Longhorn<TPlayer>, LonghornBoard<TPlayer>, LonghornMove, TPlayer>
    {
        private readonly Func<TPlayer, string> transcriber;

        public LonghornConsoleDisplay(Func<TPlayer, string> transcriber)
        {
            this.transcriber = transcriber;
        }

        public void DisplayBoard(Longhorn<TPlayer> game)
        {
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine($"{this.transcriber(game.Board.Player1Status.Player)} status: {game.Board.Player1Status.Black} black cows, {game.Board.Player1Status.Green} green cows, {game.Board.Player1Status.Orange} orange cows, {game.Board.Player1Status.White} white cows; {string.Join(", ", game.Board.Player1Status.GoldNuggets)} gold nuggets");
            Console.WriteLine($"{this.transcriber(game.Board.Player2Status.Player)} status: {game.Board.Player2Status.Black} black cows, {game.Board.Player2Status.Green} green cows, {game.Board.Player2Status.Orange} orange cows, {game.Board.Player2Status.White} white cows; {string.Join(", ", game.Board.Player2Status.GoldNuggets)} gold nuggets");
            Console.WriteLine();

            var writer = new Writer();
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    var tile = game.Board.Tiles[i, j];
                    var playerLocation = game.Board.PlayerLocation;
                    var currentPlayerLocation = playerLocation != null && playerLocation.Row == i && playerLocation.Column == j;

                    if (tile.ActionToken is ActionToken.Ambush)
                    {
                        writer.Append(i * 8 + 0, $"action token: ambush");
                    }
                    else if (tile.ActionToken is ActionToken.BrandingIron)
                    {
                        writer.Append(i * 8 + 0, $"action token: branding iron");
                    }
                    else if (tile.ActionToken is ActionToken.Epidemic)
                    {
                        writer.Append(i * 8 + 0, $"action token: epidemice");
                    }
                    else if (tile.ActionToken is ActionToken.Gold gold)
                    {
                        writer.Append(i * 8 + 0, $"action token: gold ({gold.Amount})");
                    }
                    else if (tile.ActionToken is ActionToken.Rattlesnake)
                    {
                        writer.Append(i * 8 + 0, $"action token: rattlesnake");
                    }
                    else if (tile.ActionToken is ActionToken.Sheriff)
                    {
                        writer.Append(i * 8 + 0, $"action token: sheriff");
                    }
                    else if (tile.ActionToken is ActionToken.SnakeOil)
                    {
                        writer.Append(i * 8 + 0, $"action token: snake oil");
                    }
                    else
                    {
                        writer.Append(i * 8 + 0, $"");
                    }

                    writer.Append(i * 8 + 1, $"green cows: {tile.GreenCows}");
                    writer.Append(i * 8 + 2, $"black cows: {tile.BlackCows}");
                    writer.Append(i * 8 + 3, $"white cows: {tile.WhiteCows}");
                    writer.Append(i * 8 + 4, $"orange cows: {tile.OrangeCows}");

                    if (currentPlayerLocation)
                    {
                        writer.Append(i * 8 + 5, $"CURRENT PLAYER LOCATION!");
                    }
                    else
                    {
                        writer.Append(i * 8 + 5, string.Empty);
                    }
                }
            }

            Console.WriteLine(writer.Transcribe());
        }

        private sealed class Writer
        {
            private readonly int columnPadding;

            private readonly Dictionary<int, List<string>> lines;

            private readonly Dictionary<int, int> columnSizes;

            public Writer()
                : this(2)
            {
            }

            public Writer(int columnPadding)
            {
                this.columnPadding = columnPadding;

                this.lines = new Dictionary<int, List<string>>();
                this.columnSizes = new Dictionary<int, int>();
            }

            public void Append(int lineNumber, string value)
            {
                if (!this.lines.TryGetValue(lineNumber, out var columns))
                {
                    columns = new List<string>();
                    this.lines[lineNumber] = columns;
                }

                if (!this.columnSizes.TryGetValue(columns.Count, out var columnSize))
                {
                    columnSize = 0;
                }

                this.columnSizes[columns.Count] = Math.Max(value.Length, columnSize);
                columns.Add(value);
            }

            public string Transcribe()
            {
                var builders = new List<StringBuilder>();
                foreach (var (lineNumber, columns) in this.lines)
                {
                    while (lineNumber >= builders.Count)
                    {
                        builders.Add(new StringBuilder());
                    }

                    var builder = builders[lineNumber];
                    for (int i = 0; i < columns.Count; ++i)
                    {
                        var column = columns[i];
                        builder.Append(column);
                        var columnSize = this.columnSizes[i];
                        builder.Append(new string(' ', columnSize - column.Length + this.columnPadding));
                    }
                }

                var transcriber = new StringBuilder();
                foreach (var builder in builders)
                {
                    transcriber.AppendLine(builder.ToString());
                }

                return transcriber.ToString();
            }
        }

        public void DisplayMoves(Longhorn<TPlayer> game)
        {
            Console.WriteLine("Which move would you like to select?");
            int i = 0;
            foreach (var move in game.Moves)
            {
                Console.WriteLine($"{i}: {TranscribeMove(game, move)}");

                ++i;
            }
        }

        public string TranscribeMove(Longhorn<TPlayer> game, LonghornMove move)
        {
            if (move is LonghornMove.LocationChoice locationChoice)
            {
                return $"place the other player at ({locationChoice.Location.Row}, {locationChoice.Location.Column})";
            }
            else if (move is LonghornMove.LocationMove locationMove)
            {
                var newLocation = locationMove.NewLocation;
                string resultLocation;
                if (newLocation == null)
                {
                    resultLocation = "end the game";
                }
                else
                {
                    resultLocation = $"move to ({newLocation.Row}, {newLocation.Column})";
                }

                //// TODO actionmove is null for several types of action tokens; this is because there's no decision to be made for those tokens; however, 
                //// TODO displaying the move, we will likely want to show the user the action move that they will be committing; in this case, the displayer
                //// TODO will be responsible for knowing what the action move is that is being committed; this violates the idea that the game engine is the
                //// TODO only thing responsible for knowing how to play the game; on the other hand, if the game engine requires the action move to be
                //// TODO supplied, it's a pretty bad experience to commit a move that is legal except that the caller needed to know that the action move is
                //// TODO the remaining legal move; is there a way to improve the framework to account for such things? maybe the modeling of the move for this
                //// TODO game as having 3 components is the broken part?
                var action = string.Empty;
                if (locationMove.ActionMove is ActionMove.Ambush.StealGold ambushGold)
                {
                    action = ", steal a random gold nugget from your opponent,";
                }
                else if (locationMove.ActionMove is ActionMove.Ambush.StealCows ambushCows)
                {
                    action = $", steal the {ambushCows.Color} cows from your opponent,";
                }
                else if (locationMove.ActionMove is ActionMove.BrandingIron brandingIron)
                {
                    action = $", brand the {locationMove.TakeColor} cows at ({brandingIron.Location.Row}, {brandingIron.Location.Column}),";
                }
                else if (locationMove.ActionMove is ActionMove.Epidemic epidemic)
                {
                    action = $", cause an epidemic among the {epidemic.Color} cows,";
                }
                else if (locationMove.ActionMove is ActionMove.Gold)
                {
                    action = $", take the gold token,";
                }
                else if (locationMove.ActionMove is ActionMove.Rattlesnake rattlesnake)
                {
                    var locations = string.Join(", ", new[]
                        {
                                    (TakeColor.Black, rattlesnake.BlackLocation),
                                    (TakeColor.Green, rattlesnake.GreenLocation),
                                    (TakeColor.Orange, rattlesnake.OrangeLocation),
                                    (TakeColor.White, rattlesnake.WhiteLocation),
                                }
                        .Where(location => location.Item2 != null)
                        .Select(location => $"{location.Item1} cows to ({location.Item2.Row}, {location.Item2.Column})"));

                    action = $", guide your cows away from the rattlesnake ({locations}),";
                }
                else if (locationMove.ActionMove is ActionMove.Sheriff)
                {
                    action = $", get caught by the sheriff,";
                }
                else if (locationMove.ActionMove is ActionMove.SnakeOil)
                {
                    action = $", use the snake oil to take an extra turn,";
                }

                return $"take {CowCount(game, locationMove, game.Board.PlayerLocation)} {locationMove.TakeColor} cows{action} and {resultLocation}";
            }

            throw new InvalidOperationException($"invalid move type: {move.GetType()}");
        }

        private static int CowCount(Longhorn<TPlayer> game, LonghornMove.LocationMove locationMove, LonghornLocation location)
        {
            var tile = game.Board.Tiles[location.Row, location.Column];
            if (locationMove.TakeColor == TakeColor.Black)
            {
                return tile.BlackCows;
            }

            if (locationMove.TakeColor == TakeColor.Green)
            {
                return tile.GreenCows;
            }

            if (locationMove.TakeColor == TakeColor.Orange)
            {
                return tile.OrangeCows;
            }

            if (locationMove.TakeColor == TakeColor.White)
            {
                return tile.WhiteCows;
            }

            throw new InvalidOperationException($"invalid color specified for move: {(int)locationMove.TakeColor}");
        }

        public void DisplayOutcome(Longhorn<TPlayer> game)
        {
            Console.WriteLine($"{string.Join(",", game.Outcome.Winners)} won the game!");
        }

        public LonghornMove ReadMoveSelection(Longhorn<TPlayer> game)
        {
            var moves = game.Moves.ToList();
            while (true)
            {
                var input = Console.ReadLine();
                if (!int.TryParse(input, out var selectedMove) || selectedMove >= moves.Count)
                {
                    Console.WriteLine($"The input '{input}' was not the index of a legal move");
                    continue;
                }

                Console.WriteLine(selectedMove);
                return moves[selectedMove];
            }
        }
    }
}
