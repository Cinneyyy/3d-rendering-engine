using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace src;

public record struct Vec2f(float x, float y) : IEnumerable<float>
{
    public float x = x, y = y;

    public static readonly Vec2f zero = new(0f), nan = new(float.NaN);
    public static readonly Vec2f one = new(1f), negOne = -one;
    public static readonly Vec2f inf = new(float.PositiveInfinity), negInf = new(float.NegativeInfinity);
    public static readonly Vec2f min = new(float.MinValue), max = new(float.MaxValue);
    public static readonly Vec2f right = new(1f, 0f), left = -right;
    public static readonly Vec2f up = new(0f, 1f), down = -up;
    public static readonly Vec2f[] directions = [ up, down, left, right ];


    public readonly float sqrLength => x*x + y*y;
    public readonly float length => MathF.Sqrt(sqrLength);
    public readonly Vec2f normalized => this / length;
    public readonly float toAngle => (MathF.Atan2(y, x) + MathF.Tau) % MathF.Tau;


    public Vec2f(float xy) : this(xy, xy) { }
    public Vec2f(Size sz) : this(sz.Width, sz.Height) { }
    public Vec2f(SizeF sz) : this(sz.Width, sz.Height) { }
    public Vec2f(Point pt) : this(pt.X, pt.Y) { }
    public Vec2f(PointF pt) : this(pt.X, pt.Y) { }
    public Vec2f(Vec2i v2) : this(v2.x, v2.y) { }
    public Vec2f(Vec3f v3) : this(v3.x, v3.y) { }
    public Vec2f(Vec3i v3) : this(v3.x, v3.y) { }


    public readonly override string ToString() => $"({x}; {y})";
    public readonly string ToString(string? fFormat) => $"({x.ToString(fFormat)}; {y.ToString(fFormat)})";

    public readonly void Deconstruct(out float x, out float y)
    {
        x = this.x;
        y = this.y;
    }

    public readonly Vec2i Round() => new((int)x, (int)y);
    public readonly Vec2i Ceil() => new((int)MathF.Ceiling(x), (int)MathF.Ceiling(y));
    public readonly Vec2i Floor() => new((int)MathF.Floor(x), (int)MathF.Floor(y));

    public readonly IEnumerator<float> GetEnumerator()
    {
        yield return x;
        yield return y;
    }
    readonly IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();


    public static Vec2f Op(Vec2f a, Vec2f b, Func<float, float, float> op)
        => new(op(a.x, b.x), op(a.y, b.y));

    public static Vec2f Rotate(Vec2f vec, float radians)
    {
        float sin = MathF.Sin(radians), cos = MathF.Cos(radians);
        return new(
            vec.x * cos - vec.y * sin,
            vec.x * sin + vec.y * cos);
    }

    public static Vec2f FromAngle(float radians)
        => new(MathF.Cos(radians), MathF.Sin(radians));

    public static float Dot(Vec2f a, Vec2f b)
        => a.x*b.x + a.y*b.y;


    public static Vec2f operator +(Vec2f a, Vec2f b) => new(a.x + b.x, a.y + b.y);
    public static Vec2f operator -(Vec2f a, Vec2f b) => new(a.x - b.x, a.y - b.y);
    public static Vec2f operator *(Vec2f a, Vec2f b) => new(a.x * b.x, a.y * b.y);
    public static Vec2f operator /(Vec2f a, Vec2f b) => new(a.x / b.x, a.y / b.y);
    public static Vec2f operator %(Vec2f a, Vec2f b) => new(a.x % b.x, a.y % b.y);

    public static Vec2f operator *(float a, Vec2f b) => new(a * b.x, a * b.y);
    public static Vec2f operator *(Vec2f a, float b) => new(a.x * b, a.y * b);
    public static Vec2f operator /(Vec2f a, float b) => new(a.x / b, a.y / b);
    public static Vec2f operator %(Vec2f a, float b) => new(a.x % b, a.y % b);

    public static Vec2f operator +(Vec2f v) => new(+v.x, +v.y);
    public static Vec2f operator -(Vec2f v) => new(-v.x, -v.y);


    public static explicit operator Size(Vec2f v) => new((int)v.x, (int)v.y);
    public static explicit operator Vec2f(Size s) => new(s);

    public static explicit operator SizeF(Vec2f v) => new(v.x, v.y);
    public static explicit operator Vec2f(SizeF s) => new(s);

    public static explicit operator Point(Vec2f v) => new((int)v.x, (int)v.y);
    public static explicit operator Vec2f(Point p) => new(p);

    public static explicit operator PointF(Vec2f v) => new(v.x, v.y);
    public static explicit operator Vec2f(PointF p) => new(p);

    public static explicit operator Vec2i(Vec2f v) => new((int)v.x, (int)v.y);
    public static explicit operator Vec2f(Vec2i v) => new(v.x, v.y);
}