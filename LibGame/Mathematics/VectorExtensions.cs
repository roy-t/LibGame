using System.Numerics;

namespace LibGame.Mathematics;

public static class VectorExtensions
{
    public static Vector3 WithZ(this Vector2 vector, float z = 0.0f)
    {
        return new Vector3(vector, z);
    }
}
