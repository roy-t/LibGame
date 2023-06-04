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

    [Fact(DisplayName = "`DistanceRadians` give the shortest distance along a circle between two radians")]
    public static void DistanceRadians()
    {
        EqualF(0.0f, Radians.DistanceRadians(0.0f, 0.0f));
        EqualF(MathF.PI, Radians.DistanceRadians(-MathF.PI, 0.0f));
        EqualF(MathF.PI, Radians.DistanceRadians(0.0f, MathF.PI - 0.0001f));
        EqualF(-MathF.PI, Radians.DistanceRadians(0.0f, MathF.PI + 0.0001f));
    }

    [Fact(DisplayName = "`YawFromVector` gives the yaw angle of the given vector")]
    public static void YawFromVector()
    {
        var forward = new Vector3(0.0f, 0.0f, -1.0f);
        var m = Matrix4x4.Identity;
        EqualF(0.0f, Radians.YawFromVector(Vector3.Transform(forward, m)));

        m = Matrix4x4.CreateFromYawPitchRoll(0.0f, 0.0f, 0.0f);        
        EqualF(0.0f, Radians.YawFromVector(Vector3.Transform(forward, m)));

        m = Matrix4x4.CreateFromYawPitchRoll(MathF.PI - 0.0001f, 0.0f, 0.0f);
        EqualF(MathF.PI - 0.0001f, Radians.YawFromVector(Vector3.Transform(forward, m)));

        m = Matrix4x4.CreateFromYawPitchRoll(MathF.PI + 0.0001f, 0.0f, 0.0f);
        EqualF(-MathF.PI - 0.0001f, Radians.YawFromVector(Vector3.Transform(forward, m)));
    }

    [Fact(DisplayName = "`PitchFromVector` gives the pitch angle of the given vector")]
    public static void PitchFromVector()
    {
        var forward = new Vector3(0.0f, 0.0f, -1.0f);
        var m = Matrix4x4.Identity;
        EqualF(0.0f, Radians.PitchFromVector(Vector3.Transform(forward, m)));

        m = Matrix4x4.CreateFromYawPitchRoll(0.0f, 0.0f, 0.0f);
        EqualF(0.0f, Radians.PitchFromVector(Vector3.Transform(forward, m)));

        m = Matrix4x4.CreateFromYawPitchRoll(0.0f, MathF.PI - 0.0001f, 0.0f);
        EqualF(MathF.PI - 0.0001f, Radians.PitchFromVector(Vector3.Transform(forward, m)));

        m = Matrix4x4.CreateFromYawPitchRoll(0.0f, MathF.PI + 0.0001f, 0.0f);
        EqualF(-MathF.PI - 0.0001f, Radians.PitchFromVector(Vector3.Transform(forward, m)));
    }
}

