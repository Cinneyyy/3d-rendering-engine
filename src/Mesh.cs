using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace src;

public class Mesh(params Vec3f[] vertices)
{
    public PointF[] projectionBuffer = [];
    public Vec3f[] vertices = vertices;


    public Vec3f center
    {
        get {
            Vec3f sum = Vec3f.zero;
            foreach(Vec3f v in vertices)
                sum += v;
            return sum / vertices.Length;
        }
    }


    public Mesh(IEnumerable<Vec3f> vertices) : this(vertices.ToArray()) { }
    public Mesh(params float[][] vertices) : this(from v in vertices select new Vec3f(v[0], v[1], v[2])) { }
    public Mesh(Mesh mesh) : this((Vec3f[])mesh.vertices.Clone()) { }


    internal virtual void DrawToScreen(Graphics canvas) 
        => throw new NotImplementedException("DrawToScreen was not overriden");
}