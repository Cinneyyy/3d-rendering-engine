using System.Drawing;

namespace src;

public class EdgeMesh(Vec3f[] vertices, EdgeMesh.Edge[] edges) : Mesh(vertices)
{
    public record struct Edge(int a, int b);


    public Edge[] edges = edges;


    public EdgeMesh(Mesh mesh, Edge[] edges) : this(mesh.CopyVertices(), edges) { }


    private protected override void DrawToScreen(Graphics canvas)
    {
        foreach(Edge e in edges)
            canvas.DrawLine(new(Brushes.White), projectionBuffer[e.a], projectionBuffer[e.b]);
    }

    private protected override Mesh CopyMeshChild()
    {
        EdgeMesh em = new([], edges.CloneArr());
        return em;
    }
}