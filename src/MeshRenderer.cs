using System.Drawing;
using System;

namespace src;

public class MeshRenderer(Mesh? mesh = null) : WorldObject, IRenderableObject
{
    private Mesh? _mesh = mesh;
    private Vec3f[] translBuff = new Vec3f[mesh?.verts?.Length ?? 0];
    private Point[] projBuff = new Point[mesh?.verts?.Length ?? 0];
    private bool[] oobBuff = new bool[mesh?.tris?.Length ?? 0];


    public Mesh? mesh
    {
        get => _mesh;
        set {
            if(_mesh != null)
            {
                _mesh.onVertCountChanged -= c => {
                    translBuff = new Vec3f[c];
                    projBuff = new Point[c];
                };
                _mesh.onTriCountChanged -= c => {
                    oobBuff = new bool[c];
                };
            }

            _mesh = value;

            if(_mesh != null)
            {
                _mesh.onVertCountChanged += c => {
                    translBuff = new Vec3f[c];
                    projBuff = new Point[c];
                };
                _mesh.onTriCountChanged += c => {
                    oobBuff = new bool[c];
                };

                translBuff = new Vec3f[_mesh.verts.Length];
                projBuff = new Point[_mesh.verts.Length];
                oobBuff = new bool[_mesh.tris.Length];
            }
        }
    }
    public override Vec3f anchor => mesh?.anchor ?? Vec3f.zero;
    public int renderLayer
    {
        get => (this as IRenderableObject).renderLayer;
        set => (this as IRenderableObject).ChangeLayer(value);
    }
    public Vec3f[] verts
    {
        get => mesh?.verts ?? [];
        set {
            if(mesh != null)
                mesh.verts = value;
        }
    }
    public Tri[] tris
    {
        get => mesh?.tris ?? [];
        set {
            if(mesh != null)
                mesh.tris = value;
        }
    }

    int IRenderableObject.renderLayer { get; set; } = 0;
    bool IRenderableObject.renderEnabled { get; set; }


    void IRenderableObject.RenderToScreen(Graphics canvas, Camera cam)
    {
        if(mesh == null)
            return;

        mesh.CalculateAnchor();

        for(int i = 0; i < verts.Length; i++)
            translBuff[i] = WPP.RotatePtAroundSelf(mesh[i], this, cam);

        for(int i = 0; i < tris.Length; i++)
        {
            Vec3f normal = Vec3f.Cross(translBuff[tris[i].b] - translBuff[tris[i].a], translBuff[tris[i].c] - translBuff[tris[i].a]).normalized;
            float dot = Vec3f.Dot(normal, translBuff[tris[i].a] - cam.pos);
            oobBuff[i] = dot >= -1f;
        }

        for(int i = 0; i < verts.Length; i++)
            projBuff[i] = (Point)Renderer.UvToScreen((Vec2f)WPP.Project(WPP.RotatePtForCam(translBuff[i] + pos, cam), cam));

        Pen pen = new(Brushes.White);
        for(int i = 0; i < tris.Length; i++)
        {
            if(oobBuff[i])
                continue;

            canvas.DrawLine(pen, projBuff[tris[i].a], projBuff[tris[i].b]);
            canvas.DrawLine(pen, projBuff[tris[i].b], projBuff[tris[i].c]);
            canvas.DrawLine(pen, projBuff[tris[i].c], projBuff[tris[i].a]);
            
            //canvas.FillPolygon(tris[i].brush, new Point[] { projBuff[tris[i].a], projBuff[tris[i].b], projBuff[tris[i].c] });
        }
    }
}