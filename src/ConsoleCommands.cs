using System;

namespace src;

public static class ConsoleCommands
{
    public static void Receive(string input)
    {
        string[] split = (input ?? "").Split();

        try
        {
            switch(split[0])
            {
                case "w": ConsoleWindow.windowMode = split[1] switch {
                    "hide" or "h" => ConsoleWindow.WindowMode.Hide,
                    "restore" or "r" or "rest" or "res" => ConsoleWindow.WindowMode.Restore,
                    "maximize" or "maximise" or "x" or "mx" or "max" => ConsoleWindow.WindowMode.Maximize,
                    "minimize" or "minimise" or "n" or "mn" or "min" => ConsoleWindow.WindowMode.Minimize,
                    _ => ConsoleWindow.windowMode
                }; break;

                default: Output($"Unknown command '{split[0]}'"); break;
            }
        }
        catch(Exception exc)
        {
            Output(exc.ToString());
        }
    }

    public static void BeginLoop()
    {
        while(Window.running)
            Receive(Console.ReadLine()!);
    }

    public static void Output(string s) => ConsoleWindow.Output(s);
}