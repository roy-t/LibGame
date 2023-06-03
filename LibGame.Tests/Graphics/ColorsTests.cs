using LibGame.Graphics;

namespace LibGame.Tests.Graphics;

public static class ColorsTests
{
    [Fact(DisplayName = "Converting a value with `RBGToHSV` and then back with `HSVToRGB` should result in the same value")]
    public static void RoundTripHSV()
    {
        var input = new ColorRGB(0.5f, 0.75f, 0.25f);
        var output = Colors.HSVToRGB(Colors.RGBToHSV(input));

        EqualF((Vector3)input, (Vector3)output);
    }

    [Fact(DisplayName = "Converting a value with `RGBToLinear` and then back with `LinearToRGB` should result in the same value")]
    public static void RoundTripLinear()
    {
        var input = new ColorRGB(0.5f, 0.75f, 0.25f);
        var output = Colors.LinearToRGB(Colors.RGBToLinear(input));

        EqualF((Vector3)input, (Vector3)output);
    }
}
