namespace LibGame.Mathematics;

public static class Dimensions
{
    public static int MipSlices(int width, int height)
    {
        return MipSlices(Math.Max(width, height));
    }

    public static int MipSlices(int resolution)
    {
        return Math.Max(0, 1 + (int)MathF.Floor(MathF.Log2(resolution)));
    }
}
