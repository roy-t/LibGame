using System.Diagnostics;
using System.Numerics;

namespace LibGame.Mathematics;

public static class Radians
{
    public static float Lerp(float from, float to, float t)
    {
        var retval = from + (Repeat(to - from, 2 * MathF.PI) * t);
        return WrapRadians(retval);
    }

    private static float Repeat(float t, float length)
    {
        return t - (MathF.Floor(t / length) * length);
    }

    /* wrap x -> [-pi, pi) */
    public static float WrapRadians(float radians)
    {
        return WrapRadians(radians, -MathF.PI, MathF.PI);
    }
    private static float FMod(float x, float y)
    {
        return x % y;
    }

    /* wrap x -> [0,max) */
    public static float WrapRadians(float radians, float max)
    {
        return FMod(max + FMod(radians, max), max);
    }

    /* wrap x -> [min,max) */
    public static float WrapRadians(float radians, float min, float max)
    {
        return min + WrapRadians(radians - min, max - min);
    }

    public static float DistanceRadians(float sourceAngle, float targetAngle)
    {
        sourceAngle = WrapRadians(sourceAngle);
        targetAngle = WrapRadians(targetAngle);

        var angle = targetAngle - sourceAngle;
        if (angle > MathF.PI)
        {
            angle -= MathF.Tau;
        }

        if (angle < -MathF.PI)
        {
            angle += MathF.Tau;
        }

        return angle;
    }

    public static float YawFromVector(Vector3 direction)
    {
        Debug.Assert(Vector3.Normalize(direction) != Vector3.UnitY);
        Debug.Assert(Vector3.Normalize(direction) != -Vector3.UnitY);

        // project on XZ plane
        var projected = Vector3.Normalize(new Vector3(direction.X, 0.0f, direction.Z));
        return WrapRadians(MathF.Atan2(projected.X, projected.Z) + MathF.PI);
    }
    
    public static float PitchFromVector(Vector3 direction)
    {
        Debug.Assert(Vector3.Normalize(direction) != Vector3.UnitX);
        Debug.Assert(Vector3.Normalize(direction) != -Vector3.UnitX);

        // project on ZY plane
        var projected = Vector3.Normalize(new Vector3(0.0f, direction.Y, direction.Z));
        return WrapRadians(MathF.Atan2(projected.Y, -projected.Z)); // minus because we use -Z is forward
    }
}
