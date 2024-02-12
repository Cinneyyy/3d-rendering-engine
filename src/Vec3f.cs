using System;
using System.Collections;
using System.Collections.Generic;

namespace src;

public record struct Vec3f(float x, float y, float z) : IEnumerable<float>
{
    public float x = x, y = y, z = z;

    public static readonly Vec3f zero = new(0f), nan = new(float.NaN);
    public static readonly Vec3f one = new(1f), negOne = -one;
    public static readonly Vec3f inf = new(float.PositiveInfinity), negInf = new(float.NegativeInfinity);
    public static readonly Vec3f min = new(float.MinValue), max = new(float.MaxValue);
    public static readonly Vec3f right = new(1f, 0f, 0f), left = -right;
    public static readonly Vec3f up = new(0f, 1f, 0f), down = -up;
    public static readonly Vec3f forward = new(0f, 0f, 1f), backward = -forward;
    public static readonly Vec3f[] directions = [ up, down, left, right, forward, backward ];


    public readonly float sqrLength => x*x + y*y + z*z;
    public readonly float length => MathF.Sqrt(sqrLength);
    public readonly Vec3f normalized => this / length;


    public Vec3f(float x, float y) : this(x, y, 0f) { }
    public Vec3f(float xyz) : this(xyz, xyz, xyz) { }
    public Vec3f(Vec2f v2) : this(v2.x, v2.y) { }
    public Vec3f(Vec2i v2) : this(v2.x, v2.y) { }
    public Vec3f(Vec2f v2, float z) : this(v2.x, v2.y, z) { }
    public Vec3f(Vec2i v2, float z) : this(v2.x, v2.y, z) { }
    public Vec3f(Vec2i v2, int z) : this(v2.x, v2.y, z) { }
    public Vec3f(Vec3i v3) : this(v3.x, v3.y, v3.z) { }


    public readonly override string ToString() => $"({x}; {y}; {z})";

    public readonly void Deconstruct(out float x, out float y, out float z)
    {
        x = this.x;
        y = this.y;
        z = this.z;
    }

    public readonly IEnumerator<float> GetEnumerator()
    {
        yield return x;
        yield return y;
        yield return z;
    }
    readonly IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    public readonly Vec3i Round() => new((int)x, (int)y, (int)z);
    public readonly Vec3i Ceil() => new((int)MathF.Ceiling(x), (int)MathF.Ceiling(y), (int)MathF.Ceiling(z));
    public readonly Vec3i Floor() => new((int)MathF.Floor(x), (int)MathF.Floor(y), (int)MathF.Floor(z));


    public static Vec3f Op(Vec3f a, Vec3f b, Func<float, float, float> op)
        => new(op(a.x, b.x), op(a.y, b.y), op(a.z, b.z));

    public static float Dot(Vec3f a, Vec3f b)
        => a.x*b.x + a.y*b.y + a.z*b.z;

    public static Vec3f FromEulerRotation(Vec3f rot)
        => new(MathF.Sin(rot.x), MathF.Sin(rot.y), MathF.Sin(rot.z));

    public static Vec3f RotateX(Vec3f v, float rot)
    => new(v.x,
           MathF.Cos(rot) * v.y - MathF.Sin(rot) * v.z,
           MathF.Sin(rot) * v.y + MathF.Cos(rot) * v.z);

    public static Vec3f RotateY(Vec3f v, float rot)
        => new(MathF.Cos(rot) * v.x - MathF.Sin(rot) * v.z,
               v.y,
               MathF.Sin(rot) * v.x + MathF.Cos(rot) * v.z);

    public static Vec3f RotateZ(Vec3f v, float rot)
        => new(MathF.Cos(rot) * v.x - MathF.Sin(rot) * v.y,
               MathF.Sin(rot) * v.x + MathF.Cos(rot) * v.y,
               v.z);

    public static Vec3f RotateYXZ(Vec3f v, Vec3f rot)
    {
        /*
        Vec3f y = new(
            MathF.Cos(rot.y) * v.x - MathF.Sin(rot.y) * v.z,
            v.y,
            MathF.Sin(rot.y) * v.x + MathF.Cos(rot.y) * v.z);

        Vec3f x =  new(
            y.x,
            MathF.Cos(rot.x) * y.y - MathF.Sin(rot.x) * y.z,
            MathF.Sin(rot.x) * y.y + MathF.Cos(rot.x) * y.z);

        Vec3f z = new(
            MathF.Cos(rot.z) * x.x - MathF.Sin(rot.z) * x.y,
            MathF.Sin(rot.z) * x.x + MathF.Cos(rot.z) * x.y,
            x.z);

        Vec3f xyz = new(
            MathF.Cos(rot.z) * (MathF.Cos(rot.y) * v.x - MathF.Sin(rot.y) * v.z) - MathF.Sin(rot.z) * (MathF.Cos(rot.x) * v.y - MathF.Sin(rot.x) * (MathF.Sin(rot.y) * v.x + MathF.Cos(rot.y) * v.z)),
            MathF.Sin(rot.z) * (MathF.Cos(rot.y) * v.x - MathF.Sin(rot.y) * v.z) + MathF.Cos(rot.z) * (MathF.Cos(rot.x) * v.y - MathF.Sin(rot.x) * (MathF.Sin(rot.y) * v.x + MathF.Cos(rot.y) * v.z)),
            MathF.Sin(rot.x) * v.y + MathF.Cos(rot.x) * (MathF.Sin(rot.y) * v.x + MathF.Cos(rot.y) * v.z));
        */

        return RotateZ(RotateX(RotateY(v, rot.y), rot.x), rot.z).normalized;
    }

    #region Do not touch !!
    public static Vec3f EulerToVector(float xRotation, float yRotation, float zRotation)
    {
        // Convert Euler angles to rotation matrix
        float[,] rotationMatrix = EulerToRotationMatrix(-xRotation, yRotation, zRotation);

        // Apply rotation matrix to unit vector
        Vec3f rotatedVector = new Vec3f(
            rotationMatrix[0, 2],  // x component of the rotated vector
            rotationMatrix[1, 2],  // y component of the rotated vector
            rotationMatrix[2, 2]); // z component of the rotated vector

        return rotatedVector.normalized;
    }

    private static float[,] EulerToRotationMatrix(float xRotation, float yRotation, float zRotation)
    {
        // Convert Euler angles to rotation matrix
        float[,] xRotationMatrix = {
            {1, 0, 0},
            {0, (float)Math.Cos(xRotation), -(float)Math.Sin(xRotation)},
            {0, (float)Math.Sin(xRotation), (float)Math.Cos(xRotation)}
        };

        float[,] yRotationMatrix = {
            {(float)Math.Cos(yRotation), 0, (float)Math.Sin(yRotation)},
            {0, 1, 0},
            {-(float)Math.Sin(yRotation), 0, (float)Math.Cos(yRotation)}
        };

        float[,] zRotationMatrix = {
            {(float)Math.Cos(zRotation), -(float)Math.Sin(zRotation), 0},
            {(float)Math.Sin(zRotation), (float)Math.Cos(zRotation), 0},
            {0, 0, 1}
        };

        // Combine rotation matrices
        return MultiplyMatrices(zRotationMatrix, MultiplyMatrices(yRotationMatrix, xRotationMatrix));
    }

    private static float[,] MultiplyMatrices(float[,] matrix1, float[,] matrix2)
    {
        int rows1 = matrix1.GetLength(0);
        int cols1 = matrix1.GetLength(1);
        int cols2 = matrix2.GetLength(1);

        float[,] result = new float[rows1, cols2];

        for(int i = 0; i < rows1; i++)
        {
            for(int j = 0; j < cols2; j++)
            {
                float sum = 0;
                for(int k = 0; k < cols1; k++)
                {
                    sum += matrix1[i, k] * matrix2[k, j];
                }
                result[i, j] = sum;
            }
        }

        return result;
    }
    #endregion


    public static Vec3f operator +(Vec3f a, Vec3f b) => new(a.x + b.x, a.y + b.y, a.z + b.z);
    public static Vec3f operator -(Vec3f a, Vec3f b) => new(a.x - b.x, a.y - b.y, a.z - b.z);
    public static Vec3f operator *(Vec3f a, Vec3f b) => new(a.x * b.x, a.y * b.y, a.z * b.z);
    public static Vec3f operator /(Vec3f a, Vec3f b) => new(a.x / b.x, a.y / b.y, a.z / b.z);
    public static Vec3f operator %(Vec3f a, Vec3f b) => new(a.x % b.x, a.y % b.y, a.z % b.z);

    public static Vec3f operator *(float a, Vec3f b) => new(a * b.x, a * b.y, a * b.z);
    public static Vec3f operator *(Vec3f a, float b) => new(a.x * b, a.y * b, a.z * b);
    public static Vec3f operator /(Vec3f a, float b) => new(a.x / b, a.y / b, a.z / b);
    public static Vec3f operator %(Vec3f a, float b) => new(a.x % b, a.y % b, a.z % b);

    public static Vec3f operator +(Vec3f v) => new(+v.x, +v.y, +v.z);
    public static Vec3f operator -(Vec3f v) => new(-v.x, -v.y, -v.z);


    public static explicit operator Vec2f(Vec3f v) => new(v.x, v.y);
    public static explicit operator Vec3f(Vec2f v) => new(v);

    public static explicit operator Vec2i(Vec3f v) => new((int)v.x, (int)v.y);
    public static explicit operator Vec3f(Vec2i v) => new(v);
}