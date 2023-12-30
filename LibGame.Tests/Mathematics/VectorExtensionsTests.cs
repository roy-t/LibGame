using LibGame.Mathematics;
using static LibGame.Mathematics.VectorExtensions.VectorComponent;

namespace LibGame.Tests.Mathematics;
public static class VectorExtensionsTests
{
    [Fact(DisplayName = "`Apply should apply the given function to every component of the vector")]
    public static void Apply()
    {
        EqualF(Vector4.One, Vector4.Zero.Apply(c => c + 1));
        EqualF(Vector3.One, Vector3.Zero.Apply(c => c + 1));
        EqualF(Vector2.One, Vector2.Zero.Apply(c => c + 1));
    }

    [Fact(DisplayName = "`Swizzle should copy the given components to a new vector")]
    public static void Swizzle()
    {
        var x = 0.0f;
        var y = 1.0f;
        var z = 2.0f;
        var w = 3.0f;

        var v2 = new Vector2(x, y);
        var v3 = new Vector3(x, y, z);
        var v4 = new Vector4(x, y, z, w);

        EqualF(new Vector2(y, x), v2.Swizzle(Y, X));
        EqualF(new Vector2(y, x), v3.Swizzle(Y, X));
        EqualF(new Vector2(y, x), v4.Swizzle(Y, X));

        EqualF(new Vector3(y, y, x), v2.Swizzle(Y, Y, X));
        EqualF(new Vector3(z, y, x), v3.Swizzle(Z, Y, X));
        EqualF(new Vector3(z, y, x), v4.Swizzle(Z, Y, X));

        EqualF(new Vector4(y, y, x, x), v2.Swizzle(Y, Y, X, X));
        EqualF(new Vector4(z, y, x, x), v3.Swizzle(Z, Y, X, X));
        EqualF(new Vector4(w, z, y, x), v4.Swizzle(W, Z, Y, X));
    }

    [Fact(DisplayName ="`Expand should return a new Vector3 with the given Z component value")]
    public static void Expand()
    {
        EqualF(new Vector4(0.0f, 0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f).Expand());
        EqualF(new Vector4(0.0f, 0.0f, 0.0f, 1.0f), new Vector3(0.0f, 0.0f, 0.0f).Expand(1.0f));

        EqualF(new Vector3(0.0f, 0.0f, 0.0f), new Vector2(0.0f, 0.0f).Expand());
        EqualF(new Vector3(0.0f, 0.0f, 0.0f), new Vector2(0.0f, 0.0f).Expand(0.0f));
        EqualF(new Vector3(0.0f, 0.0f, 1.0f), new Vector2(0.0f, 0.0f).Expand(1.0f));
    }
}
