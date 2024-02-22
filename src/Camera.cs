using System;

namespace src;

public class Camera(Vec3f pos, Vec3f rot, float fovDeg) : WorldObject(pos, rot, Vec3f.zero)
{
    private float _fov = fovDeg * MathF.PI/180f;

    public float fov
    {
        get => _fov;
        set {
            _fov = value;
            focalPlane = new(0f, 0f, fov);
        }
    }
    public Vec3f focalPlane { get; private set; } = new(0f, 0f, fovDeg * MathF.PI/180f);


    public Vec3f forward => Vec3f.FromEulerToForward(rot);
    public Vec3f right => Vec3f.FromEulerToRight(rot);
    public Vec3f up => Vec3f.FromEulerToUp(rot);
    public override Vec3f anchor => Vec3f.zero;


    public Camera() : this(Vec3f.zero, Vec3f.zero, 90f) { }
}