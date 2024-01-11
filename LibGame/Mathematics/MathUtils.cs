using System.Runtime.CompilerServices;

namespace LibGame.Mathematics;
public static class MathUtils
{
    /// <summary>
    /// Modulu function where the outcome is always a positive number
    /// less than or equal to length. For example, CMOD(-1, 5) == 4
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CMod(int index, int length)
    {
        index %= length;
        if (index < 0)
        {
            return index + length;
        }
        return index;
    }
}
