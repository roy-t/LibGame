namespace LibGame.Graphics;
public static class Lights
{
    /// <summary>
    /// Radius of sphere in which the light source is affecting surfaces
    /// </summary>
    public static float LightVolumeRadius(float strength, float minimumLightInfluence = 0.001f)
    {
        return MathF.Sqrt(strength / minimumLightInfluence);
    }

    /// <summary>
    /// Quadratic attunuation of light over distance
    /// </summary>
    public static float Attenuation(float strength, float distance)
    {
        return strength / (distance * distance);
    }
}
