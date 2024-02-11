global using static src.Globals;

using System;

namespace src;

public static partial class Globals
{
    public static bool NotNull(in object? o, Action cb)
    {
        bool notNull = o != null;

        if(!notNull)
            cb();

        return notNull;
    }
    public static bool NotNull(in object? o, string? log = null) =>
        NotNull(in o, () => Out(log ?? string.Empty));

    public static void Out(object? msg) 
        => ConsoleWindow.Output(msg?.ToString());

    public static T? OutRet<T>(T? msg)
    {
        Out(msg);
        return msg;
    }
}