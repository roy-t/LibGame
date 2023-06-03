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


    [Fact(DisplayName = "`IsTriangle` should determine if the given area is a triangle or a three points on a line")]
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
    }

    // TODO: extend with tests for the rest of the methods in Triangles.cs
}
