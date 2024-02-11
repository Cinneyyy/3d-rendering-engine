using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace src;

public record struct Vec2i(int x, int y) : IEnumerable<int>
{
    public int x = x, y = y;


    public readonly int sqrLength => x*x + y*y;
    public readonly float length => MathF.Sqrt(sqrLength);


    public Vec2i(int xy) : this(xy, xy) { }
    public Vec2i(Point pt) : this(pt.X, pt.Y) { }
    public Vec2i(Size sz) : this(sz.Width, sz.Height) { }
    public Vec2i(Vec2f v2) : this((int)v2.x, (int)v2.y) { }
    public Vec2i(Vec3f v3) : this((int)v3.x, (int)v3.y) { }
    public Vec2i(Vec3i v3) : this(v3.x, v3.y) { }


    public readonly override string ToString() => $"({x}; {y})";

    public readonly void Deconstruct(out int x, out int y)
    {
        x = this.x;
        y = this.y;
    }

    public readonly IEnumerator<int> GetEnumerator()
    {
        yield return x;
        yield return y;
    }
    readonly IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();


    public static Vec2i Op(Vec2i a, Vec2i b, Func<int, int, int> op)
        => new(op(a.x, b.x), op(a.y, b.y));


    public static Vec2i operator +(Vec2i a, Vec2i b) => new(a.x + b.x, a.y + b.y);
    public static Vec2i operator -(Vec2i a, Vec2i b) => new(a.x - b.x, a.y - b.y);
    public static Vec2i operator *(Vec2i a, Vec2i b) => new(a.x * b.x, a.y * b.y);
    public static Vec2i operator /(Vec2i a, Vec2i b) => new(a.x / b.x, a.y / b.y);
    public static Vec2i operator %(Vec2i a, Vec2i b) => new(a.x % b.x, a.y % b.y);

    public static Vec2i operator *(int a, Vec2i b) => new(a * b.x, a * b.y);
    public static Vec2i operator *(Vec2i a, int b) => new(a.x * b, a.y * b);
    public static Vec2i operator /(Vec2i a, int b) => new(a.x / b, a.y / b);
    public static Vec2i operator %(Vec2i a, int b) => new(a.x % b, a.y % b);

    public static Vec2i operator +(Vec2i v) => new(+v.x, +v.y);
    public static Vec2i operator -(Vec2i v) => new(-v.x, -v.y);


    public static explicit operator Size(Vec2i v) => new(v.x, v.y);
    public static explicit operator Vec2i(Size s) => new(s);

    public static explicit operator Point(Vec2i v) => new(v.x, v.y);
    public static explicit operator Vec2i(Point p) => new(p);
}