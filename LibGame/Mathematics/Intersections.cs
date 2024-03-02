using System.Numerics;

namespace LibGame.Mathematics;
public static class Intersections
{
    private const float RayEpsilon = 1e-20f;

    // TODO: remove  and replace with Vertices.Mathematics once the changes from commit 7a17c60 are in the Nuget package

    /// <summary>
    /// Determines if there is an intersection between the current object and a triangle.
    /// </summary>
    /// <param name="vertex1">Triangle Corner 1</param>
    /// <param name="vertex2">Triangle Corner 2</param>
    /// <param name="vertex3">Triangle Corner 3</param>
    /// <param name="distance">
    /// When the method completes, contains the distance of the intersection, or 0 if there was no intersection.</param>
    /// <returns>Whether the two objects intersected.</returns>
    public static bool RayTriangle(in Vector3 Position, in Vector3 Direction, in Vector3 vertex1, in Vector3 vertex2, in Vector3 vertex3, out float distance)
    {
        //-----------------------------------------------------------------------------
        // Compute the intersection of a ray (Origin, Direction) with a triangle
        // (V0, V1, V2).  Return true if there is an intersection and also set *pDist
        // to the distance along the ray to the intersection.
        //
        // The algorithm is based on Moller, Tomas and Trumbore, "Fast, Minimum Storage
        // Ray-Triangle Intersection", Journal of Graphics Tools, vol. 2, no. 1,
        // pp 21-28, 1997.
        //-----------------------------------------------------------------------------

        Vector3 edge1 = Vector3.Subtract(vertex2, vertex1);
        Vector3 edge2 = Vector3.Subtract(vertex3, vertex1);

        // p = Direction ^ e2;
        Vector3 directionCrossEdge2 = Vector3.Cross(Direction, edge2);

        // det = e1 * p;
        float determinant = Vector3.Dot(edge1, directionCrossEdge2);

        //If the ray is parallel to the triangle plane, there is no collision.
        //This also means that we are not culling, the ray may hit both the
        //back and the front of the triangle.
        if (determinant > -RayEpsilon && determinant < RayEpsilon)
        {
            distance = 0f;
            return false;
        }

        float inverseDeterminant = 1.0f / determinant;

        // Calculate the U parameter of the intersection point.
        Vector3 distanceVector;
        distanceVector.X = Position.X - vertex1.X;
        distanceVector.Y = Position.Y - vertex1.Y;
        distanceVector.Z = Position.Z - vertex1.Z;

        float triangleU;
        triangleU = (distanceVector.X * directionCrossEdge2.X) + (distanceVector.Y * directionCrossEdge2.Y) + (distanceVector.Z * directionCrossEdge2.Z);
        triangleU *= inverseDeterminant;

        // Make sure it is inside the triangle.
        if (triangleU < 0.0f || triangleU > 1.0f)
        {
            distance = 0.0f;
            return false;
        }

        // Calculate the V parameter of the intersection point.
        Vector3 distanceCrossEdge1;
        distanceCrossEdge1.X = (distanceVector.Y * edge1.Z) - (distanceVector.Z * edge1.Y);
        distanceCrossEdge1.Y = (distanceVector.Z * edge1.X) - (distanceVector.X * edge1.Z);
        distanceCrossEdge1.Z = (distanceVector.X * edge1.Y) - (distanceVector.Y * edge1.X);

        float triangleV;
        triangleV = ((Direction.X * distanceCrossEdge1.X) + (Direction.Y * distanceCrossEdge1.Y)) + (Direction.Z * distanceCrossEdge1.Z);
        triangleV *= inverseDeterminant;

        // Make sure it is inside the triangle.
        if (triangleV < 0.0f || triangleU + triangleV > 1.0f)
        {
            distance = 0.0f;
            return false;
        }

        // Compute the distance along the ray to the triangle.
        float rayDistance = (edge2.X * distanceCrossEdge1.X) + (edge2.Y * distanceCrossEdge1.Y) + (edge2.Z * distanceCrossEdge1.Z);
        rayDistance *= inverseDeterminant;

        //Is the triangle behind the ray origin?
        if (rayDistance < 0.0f)
        {
            distance = 0.0f;
            return false;
        }

        distance = rayDistance;
        return true;
    }
}
