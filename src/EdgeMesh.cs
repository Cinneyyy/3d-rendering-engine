using System.Drawing;

namespace src;

public class EdgeMesh(Vec3f[] vertices, EdgeMesh.Edge[] edges) : Mesh(vertices)
{
    public record struct Edge(int a, int b);


    public Edge[] edges = edges;


    public EdgeMesh(Mesh mesh, Edge[] edges) : this((Vec3f[])mesh.vertices.Clone(), edges) { }


    internal override void DrawToScreen(Graphics canvas)
    {
        foreach(Edge e in edges)
            canvas.DrawLine(new(Brushes.White), projectionBuffer[e.a], projectionBuffer[e.b]);
    }
}