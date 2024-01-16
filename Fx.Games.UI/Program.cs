
namespace Games;


public enum Game { Chess, Backgammon, TicTacToe };

public class CommandLineArgs(Game chess)
{
    public static CommandLineArgs FromArgs(string[] args)
    {
        var result = new CommandLineArgs(Game.TicTacToe);

        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "-c":
                    result.Game = Game.Chess;
                    break;
                case "-b":
                    result.Game = Game.Backgammon;
                    break;
            }
        }
        return result;
    }

    public Game Game { get; private set; } = chess;
}


public static class Program
{
    public static void Main(string[] args)
    {
        //// TODO install the virtual GPU on host cld05 - https://social.technet.microsoft.com/wiki/contents/articles/31771.server-2016-experience-guide-enabling-opengl-support-for-vgpu.aspx
        //// TODO then get the driver installed on the guest os odat04

        var cmd = CommandLineArgs.FromArgs(args);
        switch (cmd.Game)
        {
            case Game.Chess:
                new Chess.ChessApp().Run();
                break;

            case Game.TicTacToe:
                new TicTacToe.TicTacToeApp().Run();
                break;
        }
    }
}
