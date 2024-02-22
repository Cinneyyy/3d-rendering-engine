using System;
using System.Collections.Generic;
using System.Linq;

namespace src;

public class Mesh(Vec3f[]? verts = null, Tri[]? tris = null)
{
    private Vec3f[] _verts = verts ?? [];
    private Tri[] _tris = tris ?? [];


    public Vec3f[] verts
    {
        get => _verts;
        set {
            int oldLen = _verts.Length;
            _verts = value;
            onVertsChanged?.Invoke(_verts);
            if(oldLen != _verts.Length)
                onVertCountChanged?.Invoke(_verts.Length);
        }
    }
    public Tri[] tris
    {
        get => _tris;
        set {
            int oldLen = _tris.Length;
            _tris = value;
            onTrisChanged?.Invoke(_tris);
            if(oldLen != _tris.Length)
                onTriCountChanged?.Invoke(_tris.Length);
        }
    }
    public Vec3f anchor { get; set; } = Vec3f.zero;
    public event Action<Vec3f[]>? onVertsChanged;
    public event Action<int>? onVertCountChanged;
    public event Action<Tri[]>? onTrisChanged;
    public event Action<int>? onTriCountChanged;


    public Mesh(IEnumerable<Vec3f>? vertices = null, IEnumerable<Tri>? tris = null) : this(vertices?.ToArray(), tris?.ToArray()) { }
    public Mesh(Mesh mesh) : this(mesh.verts.CloneArr(), mesh.tris.CloneArr()) => anchor = mesh.anchor;


    public Vec3f this[int index]
    {
        get => verts[index];
        set => verts[index] = value;
    }


    public void CalculateAnchor()
    {
        Vec3f ptSum = Vec3f.zero;
        foreach(Vec3f v in verts)
            ptSum += v;
        anchor = ptSum / verts.Length;
    }
}