namespace src;

public abstract class WorldObject(Vec3f pos, Vec3f rot, Vec3f scale)
{
    public Vec3f pos = pos, _rot = rot, scale = scale;


    public abstract Vec3f anchor { get; }
    public Vec3f rot
    {
        get => _rot;
        set {
            _rot = value;
            rotMatrix = Mat4x4.EulerRotationMatrix(_rot);
        }
    }
    public Mat4x4 rotMatrix { get; private set; } = Mat4x4.EulerRotationMatrix(rot);


    public WorldObject() : this(Vec3f.zero, Vec3f.zero, Vec3f.one) { }


    public void TranslateX(float x) => pos.x += x;
    public void TranslateY(float y) => pos.y += y;
    public void TranslateZ(float z) => pos.z += z;
    public void Translate(Vec3f xyz) => pos += xyz;
    public void Translate(float x, float y, float z) => Translate(new(x, y, z));

    public void RotateX(float x) => Rotate(x, 0f, 0f);
    public void RotateY(float y) => Rotate(0f, y, 0f);
    public void RotateZ(float z) => Rotate(0f, 0f, z);
    public void Rotate(Vec3f xyz) => rot += xyz;
    public void Rotate(float x, float y, float z) => Rotate(new(x, y, z));
    public void SetRot(float? x = null, float? y = null, float? z = null) => rot = new(x ?? rot.x, y ?? rot.y, z ?? rot.z);
}