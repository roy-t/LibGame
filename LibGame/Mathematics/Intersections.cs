using System.Numerics;

namespace LibGame.Mathematics;
public static class Intersections
{
    private const float NearZeroEpsilon = 4.7683716E-07f; // 2^-21: 0x35000000

    /// <summary>
    /// This does a ray cast on a triangle to see if there is an intersection.
    /// This ONLY works on CW wound triangles. For this it assumes Z- is forward!
    /// </summary>
    /// <param name="position">Ray position</param>
    /// <param name="direction">Ray direction</param>
    /// <param name="v0">Triangle Corner 1</param>
    /// <param name="v1">Triangle Corner 2</param>
    /// <param name="v2">Triangle Corner 3</param>    
    /// <returns>Distance along the ray where the intersection happens, or null</returns>
    public static float? RayTriangle(in Vector3 position, in Vector3 direction, in Vector3 v0, in Vector3 v1, in Vector3 v2)
    {
        // Code origin can no longer be determined.
        // was adapted from C++ code.

        // compute normal
        var edgeA = v1 - v2;
        var edgeB = v0 - v2;

        var normal = Vector3.Cross(direction, edgeB);

        // find determinant
        var det = Vector3.Dot(edgeA, normal);

        // if perpendicular, exit
        if (det < NearZeroEpsilon)
        {
            return null;
        }
        det = 1.0f / det;

        // calculate distance from vertex0 to ray origin
        var s = position - v2;
        var u = det * Vector3.Dot(s, normal);

        if (u < -NearZeroEpsilon || u > 1.0f + NearZeroEpsilon)
        {
            return null;
        }

        var r = Vector3.Cross(s, edgeA);
        var v = det * Vector3.Dot(direction, r);
        if (v < -NearZeroEpsilon || u + v > 1.0f + NearZeroEpsilon)
        {
            return null;
        }

        // distance from ray to triangle
        det *= Vector3.Dot(edgeB, r);

        // Vector3 endPosition;
        // we dont want the point that is behind the ray cast.
        if (det < 0.0f)
        {
            return null;
        }

        return det;
    }
}
