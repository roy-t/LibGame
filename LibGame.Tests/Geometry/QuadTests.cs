using LibGame.Geometry;

namespace LibGame.Tests.Geometry;

public static class QuadTests
{
    [Fact(DisplayName ="`GetArea` should calculate the area of the given quad")]
    public static void GetArea()
    {
        var area = Quads.GetArea(new Vector3(1, 1, 0), new Vector3(1, -1, 0), new Vector3(-1, -1, 0), new Vector3(-1, 1, 0));
        EqualF(4.0f, area);
    }

    [Fact(DisplayName = "`GetNormal` should calculate the normal of the given quad")]
    public static void GetNormal()
    {
        var normal = Quads.GetNormal(new Vector3(1, 1, 0), new Vector3(1, -1, 0), new Vector3(-1, -1, 0), new Vector3(-1, 1, 0));
        EqualF(new Vector3(0, 0, 1), normal);
    }
}
