using System.Numerics;
using System.Runtime.CompilerServices;

namespace LibGame.Mathematics;

public static class VectorExtensions
{
    public enum VectorComponent : int
    {
        X = 0,
        Y = 1,
        Z = 2,
        W = 3,
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Apply(this Vector4 v, Func<float, float> function)
    {
        return new Vector4
        (
            function(v.X),
            function(v.Y),
            function(v.Z),
            function(v.W)
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Apply(this Vector3 v, Func<float, float> function)
    {
        return new Vector3
        (
            function(v.X),
            function(v.Y),
            function(v.Z)
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Apply(this Vector2 v, Func<float, float> function)
    {
        return new Vector2
        (
            function(v.X),
            function(v.Y)            
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Swizzle(this Vector4 v, VectorComponent a, VectorComponent b, VectorComponent c, VectorComponent d)
    {
        return new Vector4
        (
            v[(int)a],
            v[(int)b],
            v[(int)c],
            v[(int)d]
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Swizzle(this Vector4 v, VectorComponent a, VectorComponent b, VectorComponent c)
    {        
        return new Vector3
        (
            v[(int)a],
            v[(int)b],
            v[(int)c]
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Swizzle(this Vector4 v, VectorComponent a, VectorComponent b)
    {
        return new Vector2
        (
            v[(int)a],
            v[(int)b]
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Swizzle(this Vector3 v, VectorComponent a, VectorComponent b, VectorComponent c)
    {
        return new Vector3
        (
            v[(int)a],
            v[(int)b],
            v[(int)c]
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Swizzle(this Vector3 v, VectorComponent a, VectorComponent b)
    {
        return new Vector2
        (
            v[(int)a],
            v[(int)b]            
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Swizzle(this Vector3 v, VectorComponent a, VectorComponent b, VectorComponent c, VectorComponent d)
    {
        return new Vector4
        (
            v[(int)a],
            v[(int)b],
            v[(int)c],
            v[(int)d]
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Swizzle(this Vector2 v, VectorComponent a, VectorComponent b)
    {
        return new Vector2
        (
            v[(int)a],
            v[(int)b]            
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Swizzle(this Vector2 v, VectorComponent a, VectorComponent b, VectorComponent c)
    {
        return new Vector3
        (
            v[(int)a],
            v[(int)b],
            v[(int)c]
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Swizzle(this Vector2 v, VectorComponent a, VectorComponent b, VectorComponent c, VectorComponent d)
    {
        return new Vector4
        (
            v[(int)a],
            v[(int)b],
            v[(int)c],
            v[(int)d]
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Expand(this Vector2 vector, float z = 0.0f)
    {
        return new Vector3(vector, z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Expand(this Vector2 vector, float z, float w)
    {
        return new Vector4(vector, z, w);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Expand(this Vector3 vector, float w = 0.0f)
    {
        return new Vector4(vector, w);
    }
}
