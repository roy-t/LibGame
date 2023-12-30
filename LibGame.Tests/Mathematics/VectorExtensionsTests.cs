using LibGame.Mathematics;

namespace LibGame.Tests.Mathematics;
public static class VectorExtensionsTests
{
    [Fact(DisplayName ="`Expand should return a new Vector3 with the given Z component value")]
    public static void Expand()
    {
        EqualF(new Vector3(0.0f, 0.0f, 0.0f), new Vector2(0.0f, 0.0f).Expand(0.0f));
        EqualF(new Vector3(0.0f, 0.0f, 1.0f), new Vector2(0.0f, 0.0f).Expand(1.0f));
    }
}
