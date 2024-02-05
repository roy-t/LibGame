using static LibGame.Graphics.Colors;

namespace LibGame.Graphics;

/// <summary>
/// Collection of colors that allows you to easily randomly pick one
/// </summary>
public sealed class ColorPalette
{
    private readonly ColorRGB[] ColorList;
    private readonly Random Random;

    public ColorPalette(params ColorRGB[] colors)
    {
        this.ColorList = colors;
        this.Random = new Random();
    }

    public IReadOnlyList<ColorRGB> Colors => this.ColorList;

    public ColorRGB Pick()
    {
        var index = this.Random.Next(this.ColorList.Length);
        return this.ColorList[index];
    }

    // Inspired by https://colorpalette.org/grass-green-lawn-color-palette/
    public static ColorPalette GrassLawn { get; } =
        new ColorPalette
        (
            FromHex("#c1dbbc"),
            FromHex("#558e1e"),
            FromHex("#b6d6a8"),
            FromHex("#2a5126"),

            FromHex("#98c680"),
            FromHex("#9dc59c"),
            FromHex("#3d593c"),
            FromHex("#467346"),

            FromHex("#619c3d"),
            FromHex("#83b287"),
            FromHex("#67a059"),
            FromHex("#85b870")
        );

    // Inspired by https://colorpalette.org/green-grass-water-color-palette-2/
    public static ColorPalette GrassWater { get; } =
    new ColorPalette
    (
        FromHex("#94925c"),
        FromHex("#c8cac0"),
        FromHex("#b2a070"),
        FromHex("#837d3e"),

        FromHex("#2a2717"),
        FromHex("#6e7630"),
        FromHex("#67682b"),
        FromHex("#2b341a"),

        FromHex("#36391b"),
        FromHex("#687a63"),
        FromHex("#586625"),
        FromHex("#8ba49d")
    );


    public static ColorPalette PrimariesAndSecondaries { get; } =
    new ColorPalette
    (
        new ColorRGB(0.0f, 0.0f, 0.0f),
        new ColorRGB(1.0f, 0.0f, 0.0f),
        new ColorRGB(0.0f, 1.0f, 0.0f),
        new ColorRGB(0.0f, 0.0f, 1.0f),
        new ColorRGB(0.0f, 1.0f, 1.0f),
        new ColorRGB(1.0f, 0.0f, 1.0f),
        new ColorRGB(1.0f, 1.0f, 0.0f),
        new ColorRGB(1.0f, 1.0f, 1.0f)
    );
}
