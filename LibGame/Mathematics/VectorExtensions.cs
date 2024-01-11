using System.Numerics;
using System.Runtime.CompilerServices;
using static LibGame.Mathematics.MathUtils;

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

    /// <summary>
    /// Apply the given function to every component of the vector
    /// </summary>    
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

    /// <summary>
    /// Apply the given function to every component of the vector
    /// </summary>
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

    /// <summary>
    /// Apply the given function to every component of the vector
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Apply(this Vector2 v, Func<float, float> function)
    {
        return new Vector2
        (
            function(v.X),
            function(v.Y)
        );
    }

    /// <summary>
    /// Copy the given vector components to a new vector
    /// </summary>
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

    /// <summary>
    /// Copy the given vector components to a new vector
    /// </summary>
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

    /// <summary>
    /// Copy the given vector components to a new vector
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Swizzle(this Vector4 v, VectorComponent a, VectorComponent b)
    {
        return new Vector2
        (
            v[(int)a],
            v[(int)b]
        );
    }

    /// <summary>
    /// Copy the given vector components to a new vector
    /// </summary>
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

    /// <summary>
    /// Copy the given vector components to a new vector
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Swizzle(this Vector3 v, VectorComponent a, VectorComponent b)
    {
        return new Vector2
        (
            v[(int)a],
            v[(int)b]
        );
    }

    /// <summary>
    /// Copy the given vector components to a new vector
    /// </summary>
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

    /// <summary>
    /// Copy the given vector components to a new vector
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Swizzle(this Vector2 v, VectorComponent a, VectorComponent b)
    {
        return new Vector2
        (
            v[(int)a],
            v[(int)b]
        );
    }

    /// <summary>
    /// Copy the given vector components to a new vector
    /// </summary>
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

    /// <summary>
    /// Copy the given vector components to a new vector
    /// </summary>
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

    /// <summary>
    /// Add more dimensions to the given vector
    /// </summary>    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Expand(this Vector2 vector, float z = 0.0f)
    {
        return new Vector3(vector, z);
    }

    /// <summary>
    /// Add more dimensions to the given vector
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Expand(this Vector2 vector, float z, float w)
    {
        return new Vector4(vector, z, w);
    }

    /// <summary>
    /// Add more dimensions to the given vector
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Expand(this Vector3 vector, float w = 0.0f)
    {
        return new Vector4(vector, w);
    }


    /// <summary>
    /// Treat the vector as a circular array and move each element 'amount' positions to the left
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 RotateLeft(this Vector2 vector, int amount = 1)
    {
        return new Vector2(
            vector[(amount + 0) % 4],
            vector[(amount + 1) % 4]);
    }

    /// <summary>
    /// Treat the vector as a circular array and move each element 'amount' positions to the left
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 RotateLeft(this Vector3 vector, int amount = 1)
    {
        return new Vector3(
            vector[(amount + 0) % 4],
            vector[(amount + 1) % 4],
            vector[(amount + 2) % 4]);
    }

    /// <summary>
    /// Treat the vector as a circular array and move each element 'amount' positions to the left
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 RotateLeft(this Vector4 vector, int amount = 1)
    {
        return new Vector4(
            vector[CMod(amount + 0, 4)],
            vector[CMod(amount + 1, 4)],
            vector[CMod(amount + 2, 4)],
            vector[CMod(amount + 3, 4)]);
    }

    /// <summary>
    /// Treat the vector as a circular array and move each element 'amount' positions to the right
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 RotateRight(this Vector4 vector, int amount = 1)
    {
        return new Vector4(
            vector[CMod(0 - amount, 4)],
            vector[CMod(1 - amount, 4)],
            vector[CMod(2 - amount, 4)],
            vector[CMod(3 - amount, 4)]);
    }
}
