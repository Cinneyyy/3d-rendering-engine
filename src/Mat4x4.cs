using System;

namespace src;

public readonly record struct Mat4x4(float[,] matrix)
{
    public readonly float[,] matrix = matrix.GetLength(0) != 4 || matrix.GetLength(1) != 4 ? throw new() : matrix;


    public Mat4x4() : this(new float[4, 4]) { }


    public float this[int x, int y]
    {
        get => matrix[x, y];
        set => matrix[x, y] = value;
    }


    public Vec3f MultiplyWithVec3f(Vec3f v)
    {
        Vec3f o = new(
            v.x * this[0, 0] + v.y * this[1, 0] + v.z * this[2, 0] + this[3, 0],
            v.x * this[0, 1] + v.y * this[1, 1] + v.z * this[2, 1] + this[3, 1],
            v.x * this[0, 2] + v.y * this[1, 2] + v.z * this[2, 2] + this[3, 2]);
        float w = v.x * this[0, 3] + v.y * this[1, 3] + v.z * this[2, 3] + this[3, 3];

        if(w != 0f)
            o /= w;

        return o;
    }


    public static Mat4x4 EulerRotationMatrix(Vec3f rot)
        => EulerRotationMatrix(rot.x, rot.y, rot.z);
    public static Mat4x4 EulerRotationMatrix(float x, float y, float z)
    {
        float xs = MathF.Sin(x), xc = MathF.Cos(x),
              ys = MathF.Sin(y), yc = MathF.Cos(y),
              zs = MathF.Sin(z), zc = MathF.Cos(z);

        return new(new float[4, 4] {
            { yc * zc, xs * ys * zc - xc * zs, xc * ys * zc + xs * zs, 0f },
            { yc * zs, xs * ys * zs + xc * zc, xc * ys * zs - xs * zc, 0f },
            { -ys,     xs * yc,                xc * yc,                0f },
            { 0f,      0f,                     0f,                     0f }
        });
    }
}