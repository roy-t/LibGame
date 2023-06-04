using LibGame.Geometry;

namespace LibGame.Tests.Geometry;
public static class TrianglesTests
{
    [Fact(DisplayName = "`GetNormal` should give the normal of the given triangle")]
    public static void GetNormal()
    {
        var normal = Triangles.GetNormal(
            new Vector3(-1.0f, 0.0f, 0.0f),
            new Vector3(0.0f, 0.0f, -1.0f),
            new Vector3(1.0f, 0.0f, 0.0f)
        );

        EqualF(new Vector3(0.0f, 1.0f, 0.0f), normal);
    }

    [Fact(DisplayName = "`GetArea` should give the area of the given triangle")]
    public static void GetArea()
    {
        var area = Triangles.GetArea(
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(0.0f, 0.0f, -1.0f),
            new Vector3(1.0f, 0.0f, 0.0f)
        );

        EqualF(0.5f, area);
    }


    [Fact(DisplayName = "`IsTriangle` should determine if the given vertices from a triangle")]
    public static void IsTriangle()
    {
        True(Triangles.IsTriangle(
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(0.0f, 0.0f, -1.0f),
            new Vector3(1.0f, 0.0f, 0.0f)
        ));

        False(Triangles.IsTriangle(
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(0.5f, 0.0f, 0.0f),
            new Vector3(1.0f, 0.0f, 0.0f)
        ));

        False(Triangles.IsTriangle(
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(0.0f, 0.0f, 0.0f)
        ));
    }

    [Fact(DisplayName = "`IsTriangleClockwise` should determine if the given vertices are defined clockwise order")]
    public static void IsTriangleClockwise()
    {
        True(Triangles.IsTriangleClockwise(
            new Vector2(-1.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 0.0f)
        ));

        False(Triangles.IsTriangleClockwise(
            new Vector2(1.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(-1.0f, 0.0f)
        ));
    }

    [Fact(DisplayName = "`IsTriangleCounterClockwise` should determine if the given vertices are defined counter clockwise order")]
    public static void IsTriangleCounterClockwise()
    {
        False(Triangles.IsTriangleCounterClockwise(
            new Vector2(-1.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 0.0f)
        ));

        True(Triangles.IsTriangleCounterClockwise(
            new Vector2(1.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(-1.0f, 0.0f)
        ));
    }

    [Fact(DisplayName = "`IsVertexInsideTriangle` should determine if the given vertex is inside the given triangle")]
    public static void IsVertexInsideTriangle()
    {
        True(Triangles.IsVertexInsideTriangle(
            new Vector2(-1.0f, 0.0f),
            new Vector2(-1.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 0.0f)
        ));

        True(Triangles.IsVertexInsideTriangle(
            new Vector2(0.0f, 1.0f),
            new Vector2(-1.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 0.0f)
        ));

        True(Triangles.IsVertexInsideTriangle(
            new Vector2(1.0f, 0.0f),
            new Vector2(-1.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 0.0f)
        ));

        True(Triangles.IsVertexInsideTriangle(
            new Vector2(0.5f, 0.5f),
            new Vector2(-1.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 0.0f)
        ));

        False(Triangles.IsVertexInsideTriangle(
            new Vector2(0.5f, 0.6f),
            new Vector2(-1.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 0.0f)
        ));
    }


    [Fact(DisplayName = "`IsVertexInsideTriangle` should determine if the given projection of the vertex is inside the given triangle")]
    public static void IsVertexInsideTriangle3D()
    {
        True(Triangles.IsVertexInsideTriangle(
            new Vector3(-1.0f, 0.0f, 0.0f),
            new Vector3(-1.0f, 0.0f, 1.0f),
            new Vector3(0.0f, 1.0f, 1.0f),
            new Vector3(1.0f, 0.0f, 1.0f)
        ));

        True(Triangles.IsVertexInsideTriangle(
            new Vector3(0.0f, 1.0f, 0.0f),
            new Vector3(-1.0f, 0.0f, 1.0f),
            new Vector3(0.0f, 1.0f, 1.0f),
            new Vector3(1.0f, 0.0f, 1.0f)
        ));

        True(Triangles.IsVertexInsideTriangle(
            new Vector3(1.0f, 0.0f, 0.0f),
            new Vector3(-1.0f, 0.0f, 1.0f),
            new Vector3(0.0f, 1.0f, 1.0f),
            new Vector3(1.0f, 0.0f, 1.0f)
        ));

        True(Triangles.IsVertexInsideTriangle(
            new Vector3(0.5f, 0.5f, 0.0f),
            new Vector3(-1.0f, 0.0f, 1.0f),
            new Vector3(0.0f, 1.0f, 1.0f),
            new Vector3(1.0f, 0.0f, 1.0f)
        ));

        False(Triangles.IsVertexInsideTriangle(
            new Vector3(0.5f, 0.6f, 0.0f),
            new Vector3(-1.0f, 0.0f, 1.0f),
            new Vector3(0.0f, 1.0f, 1.0f),
            new Vector3(1.0f, 0.0f, 1.0f)
        ));
    }

    [Fact(DisplayName = "`Barycentric` should determine the Barycentric coordinates of the given vertex inside the given triangle")]
    public static void Barycentric()
    {
        var bl = Triangles.Barycentric(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 0, 0));
        EqualF(new Vector3(1.0f, 0.0f, 0.0f), bl);

        var tl = Triangles.Barycentric(new Vector3(0, 1, 0), new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 0, 0));
        EqualF(new Vector3(0.0f, 1.0f, 0.0f), tl);

        var br = Triangles.Barycentric(new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 0, 0));
        EqualF(new Vector3(0.0f, 0.0f, 1.0f), br);

        var center = Triangles.Barycentric(new Vector3(0.333f, 0.333f, 0), new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 0, 0));
        EqualF(new Vector3(0.333f, 0.333f, 0.333f), center);
    }
}
