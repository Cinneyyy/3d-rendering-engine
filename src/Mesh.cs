using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace src;

public class Mesh(params Vec3f[] vertices) : WorldObject()
{
    private Vec3f[] vertices = vertices;

    public PointF[] projectionBuffer = [];


    public Vec3f anchor
    {
        get {
            Vec3f sum = Vec3f.zero;
            foreach(Vec3f v in vertices)
                sum += v;
            return sum / vertices.Length + pos;
        }
    }


    public Mesh(IEnumerable<Vec3f> vertices) : this(vertices.ToArray()) { }
    public Mesh(params float[][] vertices) : this(from v in vertices select new Vec3f(v[0], v[1], v[2])) { }
    public Mesh(Mesh mesh) : this((Vec3f[])mesh.vertices.Clone()) { }


    public Vec3f this[int index] => GetVertex(index);


    public Vec3f GetVertex(int index)
        => vertices[index] + pos;

    public IEnumerable<Vec3f> GetVertices()
        => from v in vertices
           select v + pos;

    public Vec3f[] CopyVertices()
        => (Vec3f[])vertices.Clone();


    internal virtual void DrawToScreen(Graphics canvas) 
        => throw new NotImplementedException("DrawToScreen was not overriden");
}