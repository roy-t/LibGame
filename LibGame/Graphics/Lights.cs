namespace LibGame.Graphics;
public static class Lights
{
    public static float LightVolumeRadius(float strength, float minimumLightInfluence = 0.001f)
    {
        return MathF.Sqrt(strength / minimumLightInfluence);
    }

    public static float Attenuation(float strength, float distance)
    {
        return strength / (distance * distance);
    }
}
