using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;

namespace src;

public static class UtilFuncs
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

    public static void DrawImage(this Graphics graphics, Image image, PointF a, PointF b, PointF c, PointF d)
    {
        GraphicsPath path = new();
        path.AddPolygon([ a, b, c, d ]);
        graphics.SetClip(path);
        graphics.DrawImage(image, 0f, 0f);
    }

    public static T[] CloneArr<T>(this T[] src)
        => (T[])src.Clone();
}