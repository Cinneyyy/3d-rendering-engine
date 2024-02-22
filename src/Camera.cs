using System;

namespace src;

public class Camera(Vec3f pos, Vec3f rot, float fovDeg, float near, float far) : WorldObject(pos, rot, Vec3f.zero)
{
    private float _fov = fovDeg * MathF.PI/180f;
    private Mat4x4 _projectionMatrix = new();
    private float _near = near, _far = far;


    public float fov
    {
        get => _fov;
        set {
            _fov = value;
            focalPlane = new(0f, 0f, fov);
            CalculateProjectionMatrix();
        }
    }
    public Vec3f focalPlane { get; private set; } = new(0f, 0f, fovDeg * MathF.PI/180f);
    public Mat4x4 projectionMatrix => _projectionMatrix;
    public float near
    {
        get => _near;
        set {
            _near = value;
            CalculateProjectionMatrix();
        }
    }
    public float far
    {
        get => _far;
        set {
            _far = value;
            CalculateProjectionMatrix();
        }
    }


    public Vec3f forward => Vec3f.FromEulerToForward(rot);
    public Vec3f right => Vec3f.FromEulerToRight(rot);
    public Vec3f up => Vec3f.FromEulerToUp(rot);
    public override Vec3f anchor => Vec3f.zero;


    public Camera() : this(Vec3f.zero, Vec3f.zero, 90f, .1f, 100f) { }


    public void CalculateProjectionMatrix()
    {
        float invTan = 1f / MathF.Tan(fov/2f);

        _projectionMatrix[0, 0] = Renderer.RATIO_HW * invTan;
        _projectionMatrix[1, 1] = invTan;
        _projectionMatrix[2, 2] = far / (far - near);
        _projectionMatrix[2, 3] = 1f;
        _projectionMatrix[3, 2] = (-far * near) / (far - near);
    }
}