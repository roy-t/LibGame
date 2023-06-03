﻿using Xunit.Sdk;

namespace LibGame.Tests;
public class AssertFloat
{
    private const float DEFAULT_TOLERANCE = 1.0f / 1000.0f;

    public static void EqualF(float expected, float actual, float tolerance = DEFAULT_TOLERANCE)
    {
        if (MathF.Abs(expected - actual) > tolerance)
        {
            throw new AssertActualExpectedException(expected, actual, string.Empty);
        }
    }

    public static void EqualF(Vector2 expected, Vector2 actual, float tolerance = DEFAULT_TOLERANCE)
    {
        if (Vector2.Distance(expected, actual) > tolerance)
        {
            throw new AssertActualExpectedException(expected, actual, string.Empty);
        }
    }

    public static void EqualF(Vector3 expected, Vector3 actual, float tolerance = DEFAULT_TOLERANCE)
    {
        if (Vector3.Distance(expected, actual) > tolerance)
        {
            throw new AssertActualExpectedException(expected, actual, string.Empty);
        }
    }

    public static void EqualF(Vector4 expected, Vector4 actual, float tolerance = DEFAULT_TOLERANCE)
    {
        if (Vector4.Distance(expected, actual) > tolerance)
        {
            throw new AssertActualExpectedException(expected, actual, string.Empty);
        }
    }
}
