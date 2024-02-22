using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using WPP = src.WeakPerspectiveProjector;

namespace src;

public abstract class Mesh(params Vec3f[] vertices) : WorldObject(), IRenderableObject
{
    private protected static readonly PointF culled = new(float.PositiveInfinity, float.PositiveInfinity);

    private protected PointF[] projectionBuffer = [];

    private Vec3f[] vertices = vertices;


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
           select v * scl + pos;

    public Vec3f[] CopyVertices()
        => (Vec3f[])vertices.Clone();

    void IRenderableObject.RenderToScreen(Graphics canvas)
    {
        Vec3f anchor = this.anchor;

        projectionBuffer =
            (from v in GetVertices()
             let cull = WPP.Cull(v, Renderer.cam.pos, Renderer.cam.rot, Renderer.cam.fov)
             let proj = cull ? culled : (PointF)Renderer.WorldToScreen(WPP.Project3dPoint(v, rot, anchor, pos, Renderer.cam.pos, Renderer.cam.rot, Renderer.cam.fov))
             select cull ? proj : (proj with { Y = Renderer.ScreenH - proj.Y }))
            .ToArray();

        DrawToScreen(canvas);
    }


    private protected override WorldObject CopyWorldObjectChild()
    {
        Mesh clone = CopyMeshChild();
        clone.vertices = CopyVertices();
        clone.projectionBuffer = new PointF[projectionBuffer.Length];
        return clone;
    }

    private protected abstract Mesh CopyMeshChild();

    private protected abstract void DrawToScreen(Graphics canvas);
}