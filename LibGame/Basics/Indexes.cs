using System.Runtime.CompilerServices;

namespace LibGame.Basics;

public static class Indexes
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToOneDimensional(int x, int y, int stride)
    {
        return x + (stride * y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (int x, int y) ToTwoDimensional(int i, int stride)
    {
        var x = i % stride;
        var y = i / stride;

        return (x, y);
    }
}
