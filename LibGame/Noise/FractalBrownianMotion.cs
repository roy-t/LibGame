namespace LibGame.Noise;

public static class FractalBrownianMotion
{
    public delegate float NoiseGenerator1D(float x);
    public delegate float NoiseGenerator2D(float x, float y);
    public delegate float NoiseGenerator3D(float x, float y, float z);
    public delegate float NoiseGenerator4D(float x, float y, float z, float w);

    private const float Lacunarity = 1.1f;
    private const float Persistance = 0.9f;
    private const int Octaves = 5;

    /// <summary>
    /// Generate more interesting noise by overlaying multiple octaves of noise with decreasing influence, but increasing frequency.
    /// </summary>
    /// <param name="generator">The noise generator</param>
    /// <param name="lacunarity">The desired increase in frequency per octave (1.0f..inf)</param>
    /// <param name="persistance">The desired decrease in amplitude per octave (0.0f..1.0f]</param>
    /// <param name="octaves">The number of layers to generate</param>
    public static float Generate(NoiseGenerator1D generator, float x, float lacunarity = Lacunarity, float persistance = Persistance, int octaves = Octaves)
    {
        var sum = 0.0f;
        var frequency = 1.0f;
        var amplitude = 1.0f;

        for (var i = 0; i < octaves; i++)
        {
            sum += generator(x * frequency) * amplitude;
            frequency *= lacunarity;
            amplitude *= persistance;

            x += 4643.0f;
        }

        return sum;
    }

    /// <summary>
    /// Generate more interesting noise by overlaying multiple octaves of noise with decreasing influence, but increasing frequency.
    /// Note that while the noise generator generates noise in the [-1..1] range, FBM can generate values outside of that range.
    /// </summary>
    /// <param name="generator">The noise generator</param>
    /// <param name="lacunarity">The desired increase in frequency per octave (1.0f..inf)</param>
    /// <param name="persistance">The desired decrease in amplitude per octave (0.0f..1.0f]</param>
    /// <param name="octaves">The number of layers to generate</param>
    public static float Generate(NoiseGenerator2D generator, float x, float y, float lacunarity = Lacunarity, float persistance = Persistance, int octaves = Octaves)
    {
        var sum = 0.0f;
        var frequency = 1.0f;
        var amplitude = 1.0f;

        for (var i = 0; i < octaves; i++)
        {
            sum += generator(x * frequency, y * frequency) * amplitude;
            frequency *= lacunarity;
            amplitude *= persistance;

            x += 4643.0f;
            y += 3121.0f;
        }

        return sum;
    }

    /// <summary>
    /// Generate more interesting noise by overlaying multiple octaves of noise with decreasing influence, but increasing frequency.
    /// Note that while the noise generator generates noise in the [-1..1] range, FBM can generate values outside of that range.
    /// </summary>
    /// <param name="generator">The noise generator</param>
    /// <param name="lacunarity">The desired increase in frequency per octave (1.0f..inf)</param>
    /// <param name="persistance">The desired decrease in amplitude per octave (0.0f..1.0f]</param>
    /// <param name="octaves">The number of layers to generate</param>
    public static float Generate(NoiseGenerator3D generator, float x, float y, float z, float lacunarity = Lacunarity, float persistance = Persistance, int octaves = Octaves)
    {
        var sum = 0.0f;
        var frequency = 1.0f;
        var amplitude = 1.0f;

        for (var i = 0; i < octaves; i++)
        {
            sum += generator(x * frequency, y * frequency, z * frequency) * amplitude;
            frequency *= lacunarity;
            amplitude *= persistance;

            x += 4643.0f;
            y += 3121.0f;
            z += 2467.0f;
        }

        return sum;
    }

    /// <summary>
    /// Generate more interesting noise by overlaying multiple octaves of noise with decreasing influence, but increasing frequency.
    /// Note that while the noise generator generates noise in the [-1..1] range, FBM can generate values outside of that range.
    /// </summary>
    /// <param name="generator">The noise generator</param>
    /// <param name="lacunarity">The desired increase in frequency per octave (1.0f..inf)</param>
    /// <param name="persistance">The desired decrease in amplitude per octave (0.0f..1.0f]</param>
    /// <param name="octaves">The number of layers to generate</param>
    public static float Generate(NoiseGenerator4D generator, float x, float y, float z, float w, float lacunarity = Lacunarity, float persistance = Persistance, int octaves = Octaves)
    {
        var sum = 0.0f;
        var frequency = 1.0f;
        var amplitude = 1.0f;

        for (var i = 0; i < octaves; i++)
        {
            sum += generator(x * frequency, y * frequency, z * frequency, w * frequency) * amplitude;
            frequency *= lacunarity;
            amplitude *= persistance;

            x += 4643.0f;
            y += 3121.0f;
            z += 2467.0f;
            w += 6911.0f;
        }

        return sum;
    }
}
