using LibGame.Mathematics;

namespace LibGame.Tests.Mathematics;

public static class DimensionsTests
{
    [Fact(DisplayName = "`MipSlices` should return the number of mipmaps that fit the given resolution")]
    public static void MipSlices()
    {
        Equal(0, Dimensions.MipSlices(0));
        Equal(0, Dimensions.MipSlices(0, 0));
        
        Equal(9, Dimensions.MipSlices(256));
        Equal(9, Dimensions.MipSlices(1, 256));
        Equal(9, Dimensions.MipSlices(256, 1));
    }    
}