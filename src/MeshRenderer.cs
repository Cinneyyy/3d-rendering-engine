using System.Drawing;

namespace src;

public class MeshRenderer(Mesh? mesh = null) : WorldObject, IRenderableObject
{
    private Mesh? _mesh = mesh;
    private Point[] projBuff = new Point[mesh?.verts?.Length ?? 0];


    public Mesh? mesh
    {
        get => _mesh;
        set {
            if(_mesh != null)
                _mesh.onVertCountChanged -= c => projBuff = new Point[c];

            _mesh = value;

            if(_mesh != null)
            {
                _mesh.onVertCountChanged += c => projBuff = new Point[c];
                projBuff = new Point[_mesh.verts.Length];
            }
        }
    }
    public override Vec3f anchor => mesh?.anchor ?? Vec3f.zero;
    public int renderLayer
    {
        get => (this as IRenderableObject).renderLayer;
        set => (this as IRenderableObject).ChangeLayer(value);
    }

    int IRenderableObject.renderLayer { get; set; } = 0;
    bool IRenderableObject.renderEnabled { get; set; }


    void IRenderableObject.RenderToScreen(Graphics canvas, Camera cam)
    {
        Out($"(Frame #{Renderer.frameCount}) Called RenderToScreen");

        if(mesh == null)
            return;

        mesh.CalculateAnchor();
        Font font = new Font(FontFamily.GenericMonospace, 7.5f);
        for(int i = 0; i < projBuff.Length; i++)
        {
            Vec2f uv;
            projBuff[i] = (Point)Renderer.WorldToScreen(uv = WPP.Project3dPoint(mesh.verts[i], this, Renderer.cam), out var _);
            canvas.DrawString(uv.ToString("0.00"), font, Brushes.White, projBuff[i].X, projBuff[i].Y);
        }

        Pen pen = new(Brushes.White);
        foreach(var t in mesh.tris)
        {
            canvas.DrawLine(pen, projBuff[t.a], projBuff[t.b]);
            canvas.DrawLine(pen, projBuff[t.b], projBuff[t.c]);
            canvas.DrawLine(pen, projBuff[t.c], projBuff[t.a]);
        }
    }
}