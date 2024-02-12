using System;

namespace src;

public class Camera(Vec3f pos, Vec3f rot, float fovDeg) : WorldObject(pos, rot, Vec3f.zero)
{
    public float fov = fovDeg * MathF.PI/180f;


    public Vec3f forward => Vec3f.FromEulerToForward(rot);
    public Vec3f right => Vec3f.FromEulerToRight(rot);
    public Vec3f up => Vec3f.FromEulerToUp(rot);


    public Camera() : this(Vec3f.zero, Vec3f.zero, 90f) { }


    private protected override WorldObject CopyWorldObjectChild()
    {
        Camera cam = new();
        cam.fov = fov;
        return cam;
    }
}