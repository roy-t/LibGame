using LibGame.Mathematics;

namespace LibGame.Tests.Mathematics;

public static class RadiansTests
{
    [Fact(DisplayName = "`Lerp` should interpolate between to angles")]
    public static void Lerp()
    {
        EqualF(0, Radians.Lerp(-MathF.PI + 0.00001f, MathF.PI, 0.5f));
        EqualF(MathF.PI, Radians.Lerp(0.00001f, MathF.PI * 1.99999f, 0.5f), 0.1f);
    }

    [Fact(DisplayName = "`WrapRadians` should wrap any angle in [-pi, pi)")]
    public static void WrapRadians()
    {
        EqualF(-MathF.PI, Radians.WrapRadians(MathF.PI));

        EqualF(MathF.PI - 1.0f, Radians.WrapRadians(-MathF.PI - 1.0f));

        EqualF(1.0f, Radians.WrapRadians(1.0f + (MathF.PI * 6)));
    }
}