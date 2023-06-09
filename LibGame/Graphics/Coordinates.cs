using System.Numerics;

namespace LibGame.Graphics;
/// <summary>
/// Handy extremes for the DirectX coordinate systems
/// See: https://learn.microsoft.com/en-us/windows/win32/direct3d10/d3d10-graphics-programming-guide-resources-coordinates?redirectedfrom=MSDN#pixel-coordinate-system
/// </summary>
public static class Coordinates
{
    public readonly record struct Extremes(Vector2 TopLeft, Vector2 TopRight, Vector2 BottomRight, Vector2 BottomLeft);

    public static readonly Extremes NormalizedDeviceCoordinates = new(new Vector2(-1.0f, 1.0f), new Vector2(1.0f, 1.0f), new Vector2(1.0f, -1.0f), new Vector2(-1.0f, -1.0f));
    public static readonly Extremes TexelCoordinates = new(new Vector2(0.0f, 0.0f), new Vector2(1.0f, 0.0f), new Vector2(1.0f, 1.0f), new Vector2(0.0f, 1.0f));
}
