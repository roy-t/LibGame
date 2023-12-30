using System.Numerics;

namespace LibGame.Graphics;

public static class ProjectionMatrix
{
    /// <summary>
    /// Infinite reversed Z projection matrix: https://thxforthefish.com/posts/reverse_z/
    /// </summary>
    /// <param name="nearPlane">(0..inf]</param>
    /// <param name="fieldOfView">(0..PI]</param>
    /// <param name="aspectRatio">(0..inf)</param>    
    public static Matrix4x4 InfiniteReversedZ(float nearPlane, float fieldOfView, float aspectRatio)
    {
        var f = 1.0f / MathF.Tan(fieldOfView * 0.5f);
        return new Matrix4x4
        {
            M11 = f / aspectRatio,
            M22 = f,
            M34 = -1.0f,
            M43 = nearPlane
        };
    }

    /// <summary>
    /// Reversed Z projection matrix: https://thxforthefish.com/posts/reverse_z/
    /// </summary>
    /// <param name="nearPlane">(0..inf]</param>
    /// <param name="fieldOfView">(0..PI]</param>
    /// <param name="aspectRatio">(0..inf)</param>
    public static Matrix4x4 ReversedZ(float nearPlane, float farPlane, float fieldOfView, float aspectRatio)
    {
        var f = 1.0f / MathF.Tan(fieldOfView * 0.5f);
        return new Matrix4x4
        {
            M11 = f / aspectRatio,
            M22 = f,
            M33 = nearPlane / (farPlane - nearPlane),
            M34 = -1.0f,
            M43 = farPlane * nearPlane / (farPlane - nearPlane)
        };
    }

    /// <summary>
    /// Adds the given jitter to a projection matrix, useful when doing temporal anti aliasing: https://www.elopezr.com/temporal-aa-and-the-quest-for-the-holy-trail/
    /// </summary>
    public static Matrix4x4 Jitter(in Matrix4x4 projectionMatrix, Vector2 jitter)
    {        
        var offset = new Matrix4x4
        {
            M11 = 1,
            M22 = 1,
            M33 = 1,
            M44 = 1,
            M41 = jitter.X,
            M42 = jitter.Y,            
        };     

        return projectionMatrix * offset;
    }
}
