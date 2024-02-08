global using static src.Globals;

using System;

namespace src;

public static partial class Globals
{
    public static partial class UtilFuncs
    {
        public static bool RoughlyEqual(float a, float b, float eps = 1e-3f)
            => MathF.Abs(a - b) <= eps;

        public static float Lerp(float t, float min, float max)
            => t * (max - min) + min;

        public static float Clamp(float x, float min, float max)
        {
            if(x < min) return min;
            if(x > max) return max;
            return x;
        }
        public static int Clamp(int x, int min, int max)
        {
            if(x < min) return min;
            if(x > max) return max;
            return x;
        }

        private static string GetSuffix(string str)
        {
            if(str.Length > 1 && str[^2] == '1')
                return "th";

            return str[^1] switch {
                '1' => "st",
                '2' => "nd",
                '3' => "rd",
                _ => "th"
            };
        }
        public static string GetSuffix(int n) => GetSuffix(Math.Abs(n).ToString());
        public static string GetSuffix(ulong n) => GetSuffix(n.ToString());

        public static string AddSuffix(int n)
            => $"{n}{GetSuffix(n)}";
        public static string AddSuffix(int n, string format)
            => $"{n.ToString(format)}{GetSuffix(n)}";
        public static string AddSuffix(ulong n)
            => $"{n}{GetSuffix(n)}";
        public static string AddSuffix(ulong n, string format)
            => $"{n.ToString(format)}{GetSuffix(n)}";
    }


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