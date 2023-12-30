using System.Drawing;
using System.Numerics;

namespace LibGame.Graphics;

/// <summary>
/// A color in HSV space
/// </summary>
/// <param name="H">Hue, in [0..1]</param>
/// <param name="S">Saturation, in [0..1]</param>
/// <param name="V">Lightness, in [0..1]</param>
public readonly record struct ColorHSV(float H, float S, float V)
{
    public static implicit operator ColorHSV(Vector3 vector) => new(vector.X, vector.Y, vector.Z);
    public static implicit operator Vector3(ColorHSV color) => new(color.H, color.S, color.V);    
}


/// <summary>
/// Standard color representation, the red, green, and blue values are gamma corrected
/// </summary>
/// <param name="R">Red, in [0..1]</param>
/// <param name="G">Green, in [0..1]</param>
/// <param name="B">Blue, in [0..1]</param>
public readonly record struct ColorRGB(float R, float G, float B)
{
    public static implicit operator ColorRGB(Vector3 vector) => new(vector.X, vector.Y, vector.Z);
    public static implicit operator Vector3(ColorRGB color) => new(color.R, color.G, color.B);
    public static implicit operator Vector4(ColorRGB color) => new(color.R, color.G, color.B, 1.0f);
}

/// <summary>
/// A Color represented using red, green, and blue components in linear (not-gamma corrected) space
/// </summary>
/// <param name="R">Red, in [0..1]</param>
/// <param name="G">Green, in [0..1]</param>
/// <param name="B">Blue, in [0..1]</param>
public readonly record struct ColorLinear(float R, float G, float B)
{
    public static implicit operator ColorLinear(Vector3 vector) => new(vector.X, vector.Y, vector.Z);
    public static implicit operator Vector3(ColorLinear color) => new(color.R, color.G, color.B);
    public static implicit operator Vector4(ColorLinear color) => new(color.R, color.G, color.B, 1.0f);
}

/// <summary>
/// Utility methods for colors
/// </summary>
public static class Colors
{
    private const float Gamma = 2.2f;
    private const float InverseGamma = 0.45454545455f;

    /// <summary>
    /// Linearly interpolate (lerp) between two RGB colors.
    /// </summary>
    /// <remarks>
    /// The interpolation itself is done in HSV space to improve color accuracy
    /// </remarks>
    public static ColorRGB Interpolate(ColorRGB a, ColorRGB b, float amount)
    {
        var hsvA = RGBToHSV(a);
        var hsvB = RGBToHSV(b);

        var interolated = Vector3.Lerp(hsvA, hsvB, amount);
        return HSVToRGB(interolated);        
    }

    /// <summary>
    /// Computes a color from a hex string
    /// </summary>
    public static ColorRGB FromHex(string hex)
    {
        var c = ColorTranslator.FromHtml(hex);
        return new ColorRGB(c.R / 255.0f, c.G / 255.0f, c.B / 255.0f);
    }

    // Conversion methods adapted from: http://www.easyrgb.com/en/math.php

    /// <summary>
    /// Converts an RGB color to a HSV color
    /// </summary>
    public static ColorHSV RGBToHSV(ColorRGB color)
    {
        var r = color.R;
        var g = color.G;
        var b = color.B;

        var min = Math.Min(r, Math.Min(g, b));
        var max = Math.Max(r, Math.Max(g, b));
        var deltaMax = max - min;

        var h = 0.0f;
        var s = 0.0f;
        var v = max;

        if (max > 0.0f)
        {
            s = deltaMax / max;
            var deltaR = (((max - r) / 6.0f) + (deltaMax / 2.0f)) / deltaMax;
            var deltaG = (((max - g) / 6.0f) + (deltaMax / 2.0f)) / deltaMax;
            var deltaB = (((max - b) / 6.0f) + (deltaMax / 2.0f)) / deltaMax;

            if (r >= g && r >= b)
            {
                h = deltaB - deltaG;
            }
            else if (g >= b && g >= r)
            {
                h = (1.0f / 3.0f) + deltaR - deltaB;
            }
            else
            {
                h = (2.0f / 3.0f) + deltaG - deltaR;
            }

            if (h < 0.0f) { h += 1.0f; }
            if (h > 1.0f) { h -= 1.0f; }
        }

        return new ColorHSV(h, s, v);
    }

    /// <summary>
    /// Converts an HSV color to an RGB color
    /// </summary>
    public static ColorRGB HSVToRGB(ColorHSV color)
    {
        var h = color.H;
        var s = color.S;
        var v = color.V;

        if (s > 0.0f)
        {
            float r;
            float g;
            float b;

            h *= 6.0f;
            if (h >= 6.0f)
            {
                h = 0.0f;
            }
            var i = (int)h;
            var c1 = v * (1.0f - s);
            var c2 = v * (1.0f - (s * (h - i)));
            var c3 = v * (1.0f - (s * (1.0f - (h - i))));

            if      (i == 0) { r = v;  g = c3; b = c1; }
            else if (i == 1) { r = c2; g = v;  b = c1; }
            else if (i == 2) { r = c1; g = v;  b = c3; }
            else if (i == 3) { r = c1; g = c2; b = v;  }
            else if (i == 4) { r = c3; g = c1; b = v;  }
            else             { r = v;  g = c1; b = c2; }

            return new ColorRGB(r, g, b);
        }
        else
        {
            return new ColorRGB(v, v, v);
        }
    }

    /// <summary>
    /// Converts an RGB color to a linear color
    /// </summary>
    public static ColorLinear RGBToLinear(ColorRGB color)
    {
        var r = MathF.Pow(color.R, Gamma);
        var g = MathF.Pow(color.G, Gamma);
        var b = MathF.Pow(color.B, Gamma);

        return new ColorLinear(r, g, b);
    }

    /// <summary>
    /// Converts a linear color to an RGB color
    /// </summary>
    public static ColorRGB LinearToRGB(ColorLinear color)
    {
        var r = MathF.Pow(color.R, InverseGamma);
        var g = MathF.Pow(color.G, InverseGamma);
        var b = MathF.Pow(color.B, InverseGamma);

        return new ColorRGB(r, g, b);
    }
}
