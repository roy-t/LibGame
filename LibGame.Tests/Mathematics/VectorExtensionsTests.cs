using LibGame.Mathematics;

namespace LibGame.Tests.Mathematics;
public static class VectorExtensionsTests
{
    [Fact(DisplayName ="`WithZ should return a new Vector3 with the given Z component value")]
    public static void WithZ()
    {
        EqualF(new Vector3(0.0f, 0.0f, 0.0f), new Vector2(0.0f, 0.0f).WithZ(0.0f));
        EqualF(new Vector3(0.0f, 0.0f, 1.0f), new Vector2(0.0f, 0.0f).WithZ(1.0f));
    }
}
