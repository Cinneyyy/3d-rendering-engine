using System;
using System.Drawing;
using System.Linq;

namespace src;

public class QuadMesh(Vec3f[] vertices, QuadMesh.Quad[] quads) : Mesh(vertices)
{
    /// <summary> 
    /// A is top left <br />
    /// B is top right <br />
    /// C is bottom right <br />
    /// D is bottom left <br />
    /// </summary>
    public record struct Quad(int a, int b, int c, int d, Brush brush)
    {
        public static float Dist(Quad q, Mesh mesh)
        {
            Vec3f[] points = (from p in new Vec3f[] {
                mesh[q.a],
                mesh[q.b],
                mesh[q.c],
                mesh[q.d]
            } select Renderer.camPos - p).ToArray();;

            float avg = 0f;
            foreach(var v2 in points)
                avg += v2.length;
            return avg / 4f;
        }
    }


    public Quad[] quads = quads;


    public QuadMesh(Mesh mesh, Quad[] quads) : this(mesh.CopyVertices(), quads) { }


    internal override void DrawToScreen(Graphics canvas)
    {
        PointF[] points = new PointF[4];
        Array.Sort(quads, (a, b) => (int)((Quad.Dist(b, this) - Quad.Dist(a, this))) * 100);

        foreach(Quad q in quads)
        {
            points[0] = projectionBuffer[q.a];
            points[1] = projectionBuffer[q.b];
            points[2] = projectionBuffer[q.c];
            points[3] = projectionBuffer[q.d];

            canvas.FillPolygon(q.brush, points);
        }
    }
}