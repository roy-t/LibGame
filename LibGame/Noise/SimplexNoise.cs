﻿using System.Numerics;

namespace LibGame.Noise;

// C# port of Java Simplex Noise, original file header:

/*
 * A speed-improved simplex noise algorithm for 2D, 3D and 4D in Java.
 *
 * Based on example code by Stefan Gustavson (stegu@itn.liu.se).
 * Optimisations by Peter Eastman (peastman@drizzle.stanford.edu).
 * Better rank ordering method by Stefan Gustavson in 2012.
 *
 * This could be speeded up even further, but it's useful as it is.
 *
 * Version 2012-03-09
 *
 * This code was placed in the public domain by its original author,
 * Stefan Gustavson. You may use it as you see fit, but
 * attribution is appreciated.
 *
 */

public static class SimplexNoise
{    
    // 2D simplex noise
    public static float Noise(float x, float y)
    {
        float n0, n1, n2; // Noise contributions from the three corners
                          // Skew the input space to determine which simplex cell we're in
        var s = (x + y) * F2; // Hairy factor for 2D
        var i = Fastfloor(x + s);
        var j = Fastfloor(y + s);
        var t = (i + j) * G2;
        var X0 = i - t; // Unskew the cell origin back to (x,y) space
        var Y0 = j - t;
        var x0 = x - X0; // The x,y distances from the cell origin
        var y0 = y - Y0;
        // For the 2D case, the simplex shape is an equilateral triangle.
        // Determine which simplex we are in.
        int i1, j1; // Offsets for second (middle) corner of simplex in (i,j) coords
        if (x0 > y0) { i1 = 1; j1 = 0; } // lower triangle, XY order: (0,0)->(1,0)->(1,1)
        else { i1 = 0; j1 = 1; }      // upper triangle, YX order: (0,0)->(0,1)->(1,1)
                                      // A step of (1,0) in (i,j) means a step of (1-c,-c) in (x,y), and
                                      // a step of (0,1) in (i,j) means a step of (-c,1-c) in (x,y), where
                                      // c = (3-sqrt(3))/6
        var x1 = x0 - i1 + G2; // Offsets for middle corner in (x,y) unskewed coords
        var y1 = y0 - j1 + G2;
        var x2 = x0 - 1.0f + (2.0f * G2); // Offsets for last corner in (x,y) unskewed coords
        var y2 = y0 - 1.0f + (2.0f * G2);
        // Work out the hashed gradient indices of the three simplex corners
        var ii = i & 255;
        var jj = j & 255;
        int gi0 = PermMod12[ii + Perm[jj]];
        int gi1 = PermMod12[ii + i1 + Perm[jj + j1]];
        int gi2 = PermMod12[ii + 1 + Perm[jj + 1]];
        // Calculate the contribution from the three corners
        var t0 = 0.5f - (x0 * x0) - (y0 * y0);
        if (t0 < 0.0f)
        {
            n0 = 0.0f;
        }
        else
        {
            t0 *= t0;
            n0 = t0 * t0 * Dot(Grad3[gi0], x0, y0);  // (x,y) of grad3 used for 2D gradient
        }
        var t1 = 0.5f - (x1 * x1) - (y1 * y1);
        if (t1 < 0.0f)
        {
            n1 = 0.0f;
        }
        else
        {
            t1 *= t1;
            n1 = t1 * t1 * Dot(Grad3[gi1], x1, y1);
        }
        var t2 = 0.5f - (x2 * x2) - (y2 * y2);
        if (t2 < 0.0f)
        {
            n2 = 0.0f;
        }
        else
        {
            t2 *= t2;
            n2 = t2 * t2 * Dot(Grad3[gi2], x2, y2);
        }
        // Add contributions from each corner to get the final noise value.
        // The result is scaled to return values in the interval [-1,1].
        return 70.0f * (n0 + n1 + n2);
    }


    // 3D simplex noise
    public static float Noise(float x, float y, float z)
    {
        float n0, n1, n2, n3; // Noise contributions from the four corners
                              // Skew the input space to determine which simplex cell we're in
        var s = (x + y + z) * F3; // Very nice and simple skew factor for 3D
        var i = Fastfloor(x + s);
        var j = Fastfloor(y + s);
        var k = Fastfloor(z + s);
        var t = (i + j + k) * G3;
        var X0 = i - t; // Unskew the cell origin back to (x,y,z) space
        var Y0 = j - t;
        var Z0 = k - t;
        var x0 = x - X0; // The x,y,z distances from the cell origin
        var y0 = y - Y0;
        var z0 = z - Z0;
        // For the 3D case, the simplex shape is a slightly irregular tetrahedron.
        // Determine which simplex we are in.
        int i1, j1, k1; // Offsets for second corner of simplex in (i,j,k) coords
        int i2, j2, k2; // Offsets for third corner of simplex in (i,j,k) coords
        if (x0 >= y0)
        {
            if (y0 >= z0)
            { i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 1; k2 = 0; } // X Y Z order
            else if (x0 >= z0) { i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 0; k2 = 1; } // X Z Y order
            else { i1 = 0; j1 = 0; k1 = 1; i2 = 1; j2 = 0; k2 = 1; } // Z X Y order
        }
        else
        { // x0<y0
            if (y0 < z0) { i1 = 0; j1 = 0; k1 = 1; i2 = 0; j2 = 1; k2 = 1; } // Z Y X order
            else if (x0 < z0) { i1 = 0; j1 = 1; k1 = 0; i2 = 0; j2 = 1; k2 = 1; } // Y Z X order
            else { i1 = 0; j1 = 1; k1 = 0; i2 = 1; j2 = 1; k2 = 0; } // Y X Z order
        }
        // A step of (1,0,0) in (i,j,k) means a step of (1-c,-c,-c) in (x,y,z),
        // a step of (0,1,0) in (i,j,k) means a step of (-c,1-c,-c) in (x,y,z), and
        // a step of (0,0,1) in (i,j,k) means a step of (-c,-c,1-c) in (x,y,z), where
        // c = 1/6.
        var x1 = x0 - i1 + G3; // Offsets for second corner in (x,y,z) coords
        var y1 = y0 - j1 + G3;
        var z1 = z0 - k1 + G3;
        var x2 = x0 - i2 + (2.0f * G3); // Offsets for third corner in (x,y,z) coords
        var y2 = y0 - j2 + (2.0f * G3);
        var z2 = z0 - k2 + (2.0f * G3);
        var x3 = x0 - 1.0f + (3.0f * G3); // Offsets for last corner in (x,y,z) coords
        var y3 = y0 - 1.0f + (3.0f * G3);
        var z3 = z0 - 1.0f + (3.0f * G3);
        // Work out the hashed gradient indices of the four simplex corners
        var ii = i & 255;
        var jj = j & 255;
        var kk = k & 255;
        int gi0 = PermMod12[ii + Perm[jj + Perm[kk]]];
        int gi1 = PermMod12[ii + i1 + Perm[jj + j1 + Perm[kk + k1]]];
        int gi2 = PermMod12[ii + i2 + Perm[jj + j2 + Perm[kk + k2]]];
        int gi3 = PermMod12[ii + 1 + Perm[jj + 1 + Perm[kk + 1]]];
        // Calculate the contribution from the four corners
        var t0 = 0.6f - (x0 * x0) - (y0 * y0) - (z0 * z0);
        if (t0 < 0.0f)
        {
            n0 = 0.0f;
        }
        else
        {
            t0 *= t0;
            n0 = t0 * t0 * Dot(Grad3[gi0], x0, y0, z0);
        }
        var t1 = 0.6f - (x1 * x1) - (y1 * y1) - (z1 * z1);
        if (t1 < 0.0f)
        {
            n1 = 0.0f;
        }
        else
        {
            t1 *= t1;
            n1 = t1 * t1 * Dot(Grad3[gi1], x1, y1, z1);
        }
        var t2 = 0.6f - (x2 * x2) - (y2 * y2) - (z2 * z2);
        if (t2 < 0.0f)
        {
            n2 = 0.0f;
        }
        else
        {
            t2 *= t2;
            n2 = t2 * t2 * Dot(Grad3[gi2], x2, y2, z2);
        }
        var t3 = 0.6f - (x3 * x3) - (y3 * y3) - (z3 * z3);
        if (t3 < 0.0f)
        {
            n3 = 0.0f;
        }
        else
        {
            t3 *= t3;
            n3 = t3 * t3 * Dot(Grad3[gi3], x3, y3, z3);
        }
        // Add contributions from each corner to get the final noise value.
        // The result is scaled to stay just inside [-1,1]
        return 32.0f * (n0 + n1 + n2 + n3);
    }


    // 4D simplex noise, better simplex rank ordering method 2012-03-09
    public static float Noise(float x, float y, float z, float w)
    {

        float n0, n1, n2, n3, n4; // Noise contributions from the five corners
                                  // Skew the (x,y,z,w) space to determine which cell of 24 simplices we're in
        var s = (x + y + z + w) * F4; // Factor for 4D skewing
        var i = Fastfloor(x + s);
        var j = Fastfloor(y + s);
        var k = Fastfloor(z + s);
        var l = Fastfloor(w + s);
        var t = (i + j + k + l) * G4; // Factor for 4D unskewing
        var X0 = i - t; // Unskew the cell origin back to (x,y,z,w) space
        var Y0 = j - t;
        var Z0 = k - t;
        var W0 = l - t;
        var x0 = x - X0;  // The x,y,z,w distances from the cell origin
        var y0 = y - Y0;
        var z0 = z - Z0;
        var w0 = w - W0;
        // For the 4D case, the simplex is a 4D shape I won't even try to describe.
        // To find out which of the 24 possible simplices we're in, we need to
        // determine the magnitude ordering of x0, y0, z0 and w0.
        // Six pair-wise comparisons are performed between each possible pair
        // of the four coordinates, and the results are used to rank the numbers.
        var rankx = 0;
        var ranky = 0;
        var rankz = 0;
        var rankw = 0;
        if (x0 > y0)
        {
            rankx++;
        }
        else
        {
            ranky++;
        }

        if (x0 > z0)
        {
            rankx++;
        }
        else
        {
            rankz++;
        }

        if (x0 > w0)
        {
            rankx++;
        }
        else
        {
            rankw++;
        }

        if (y0 > z0)
        {
            ranky++;
        }
        else
        {
            rankz++;
        }

        if (y0 > w0)
        {
            ranky++;
        }
        else
        {
            rankw++;
        }

        if (z0 > w0)
        {
            rankz++;
        }
        else
        {
            rankw++;
        }

        int i1, j1, k1, l1; // The integer offsets for the second simplex corner
        int i2, j2, k2, l2; // The integer offsets for the third simplex corner
        int i3, j3, k3, l3; // The integer offsets for the fourth simplex corner
                            // simplex[c] is a 4-vector with the numbers 0, 1, 2 and 3 in some order.
                            // Many values of c will never occur, since e.g. x>y>z>w makes x<z, y<w and x<w
                            // impossible. Only the 24 indices which have non-zero entries make any sense.
                            // We use a thresholding to set the coordinates in turn from the largest magnitude.
                            // Rank 3 denotes the largest coordinate.
        i1 = rankx >= 3 ? 1 : 0;
        j1 = ranky >= 3 ? 1 : 0;
        k1 = rankz >= 3 ? 1 : 0;
        l1 = rankw >= 3 ? 1 : 0;
        // Rank 2 denotes the second largest coordinate.
        i2 = rankx >= 2 ? 1 : 0;
        j2 = ranky >= 2 ? 1 : 0;
        k2 = rankz >= 2 ? 1 : 0;
        l2 = rankw >= 2 ? 1 : 0;
        // Rank 1 denotes the second smallest coordinate.
        i3 = rankx >= 1 ? 1 : 0;
        j3 = ranky >= 1 ? 1 : 0;
        k3 = rankz >= 1 ? 1 : 0;
        l3 = rankw >= 1 ? 1 : 0;
        // The fifth corner has all coordinate offsets = 1, so no need to compute that.
        var x1 = x0 - i1 + G4; // Offsets for second corner in (x,y,z,w) coords
        var y1 = y0 - j1 + G4;
        var z1 = z0 - k1 + G4;
        var w1 = w0 - l1 + G4;
        var x2 = x0 - i2 + (2.0f * G4); // Offsets for third corner in (x,y,z,w) coords
        var y2 = y0 - j2 + (2.0f * G4);
        var z2 = z0 - k2 + (2.0f * G4);
        var w2 = w0 - l2 + (2.0f * G4);
        var x3 = x0 - i3 + (3.0f * G4); // Offsets for fourth corner in (x,y,z,w) coords
        var y3 = y0 - j3 + (3.0f * G4);
        var z3 = z0 - k3 + (3.0f * G4);
        var w3 = w0 - l3 + (3.0f * G4);
        var x4 = x0 - 1.0f + (4.0f * G4); // Offsets for last corner in (x,y,z,w) coords
        var y4 = y0 - 1.0f + (4.0f * G4);
        var z4 = z0 - 1.0f + (4.0f * G4);
        var w4 = w0 - 1.0f + (4.0f * G4);
        // Work out the hashed gradient indices of the five simplex corners
        var ii = i & 255;
        var jj = j & 255;
        var kk = k & 255;
        var ll = l & 255;
        var gi0 = Perm[ii + Perm[jj + Perm[kk + Perm[ll]]]] % 32;
        var gi1 = Perm[ii + i1 + Perm[jj + j1 + Perm[kk + k1 + Perm[ll + l1]]]] % 32;
        var gi2 = Perm[ii + i2 + Perm[jj + j2 + Perm[kk + k2 + Perm[ll + l2]]]] % 32;
        var gi3 = Perm[ii + i3 + Perm[jj + j3 + Perm[kk + k3 + Perm[ll + l3]]]] % 32;
        var gi4 = Perm[ii + 1 + Perm[jj + 1 + Perm[kk + 1 + Perm[ll + 1]]]] % 32;
        // Calculate the contribution from the five corners
        var t0 = 0.6f - (x0 * x0) - (y0 * y0) - (z0 * z0) - (w0 * w0);
        if (t0 < 0.0f)
        {
            n0 = 0.0f;
        }
        else
        {
            t0 *= t0;
            n0 = t0 * t0 * Dot(Grad4[gi0], x0, y0, z0, w0);
        }
        var t1 = 0.6f - (x1 * x1) - (y1 * y1) - (z1 * z1) - (w1 * w1);
        if (t1 < 0.0f)
        {
            n1 = 0.0f;
        }
        else
        {
            t1 *= t1;
            n1 = t1 * t1 * Dot(Grad4[gi1], x1, y1, z1, w1);
        }
        var t2 = 0.6f - (x2 * x2) - (y2 * y2) - (z2 * z2) - (w2 * w2);
        if (t2 < 0.0f)
        {
            n2 = 0.0f;
        }
        else
        {
            t2 *= t2;
            n2 = t2 * t2 * Dot(Grad4[gi2], x2, y2, z2, w2);
        }
        var t3 = 0.6f - (x3 * x3) - (y3 * y3) - (z3 * z3) - (w3 * w3);
        if (t3 < 0.0f)
        {
            n3 = 0.0f;
        }
        else
        {
            t3 *= t3;
            n3 = t3 * t3 * Dot(Grad4[gi3], x3, y3, z3, w3);
        }
        var t4 = 0.6f - (x4 * x4) - (y4 * y4) - (z4 * z4) - (w4 * w4);
        if (t4 < 0.0f)
        {
            n4 = 0.0f;
        }
        else
        {
            t4 *= t4;
            n4 = t4 * t4 * Dot(Grad4[gi4], x4, y4, z4, w4);
        }
        // Sum up and scale the result to cover the range [-1,1]
        return 27.0f * (n0 + n1 + n2 + n3 + n4);
    }

    // To remove the need for index wrapping, float the permutation table length
    private static readonly short[] Perm = new short[512];
    private static readonly short[] PermMod12 = new short[512];

    static SimplexNoise()
    {
        for (var i = 0; i < 512; i++)
        {
            Perm[i] = P[i & 255];
            PermMod12[i] = (short)(Perm[i] % 12);
        }
    }

    // Skewing and unskewing factors for 2, 3, and 4 dimensions
    private static readonly float F2 = 0.5f * (MathF.Sqrt(3.0f) - 1.0f);
    private static readonly float G2 = (3.0f - MathF.Sqrt(3.0f)) / 6.0f;
    private static readonly float F3 = 1.0f / 3.0f;
    private static readonly float G3 = 1.0f / 6.0f;
    private static readonly float F4 = (MathF.Sqrt(5.0f) - 1.0f) / 4.0f;
    private static readonly float G4 = (5.0f - MathF.Sqrt(5.0f)) / 20.0f;

    // This method is a *lot* faster than using (int)Math.floor(x)
    private static int Fastfloor(float x)
    {
        var xi = (int)x;
        return x < xi ? xi - 1 : xi;
    }

    private static float Dot(Vector3 g, float x, float y)
    {
        return (g.X * x) + (g.Y * y);
    }

    private static float Dot(Vector3 g, float x, float y, float z)
    {
        return (g.X * x) + (g.Y * y) + (g.Z * z);
    }

    private static float Dot(Vector4 g, float x, float y)
    {
        return (g.X * x) + (g.Y * y);
    }

    private static float Dot(Vector4 g, float x, float y, float z)
    {
        return (g.X * x) + (g.Y * y) + (g.Z * z);
    }

    private static float Dot(Vector4 g, float x, float y, float z, float w)
    {
        return (g.X * x) + (g.Y * y) + (g.Z * z) + (g.W * w);
    }

    private static readonly Vector3[] Grad3 =
    [
        new Vector3(1.0f, 1.0f, 0.0f),
        new Vector3(-1.0f, 1.0f, 0.0f),
        new Vector3(1.0f, -1.0f, 0.0f),
        new Vector3(-1.0f, -1.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 1.0f),
        new Vector3(-1.0f, 0.0f, 1.0f),
        new Vector3(1.0f, 0.0f, -1.0f),
        new Vector3(-1.0f, 0.0f, -1.0f),
        new Vector3(0.0f, 1.0f, 1.0f),
        new Vector3(0.0f, -1.0f, 1.0f),
        new Vector3(0.0f, 1.0f, -1.0f),
        new Vector3(0.0f, -1.0f, -1.0f)
    ];

    private static readonly Vector4[] Grad4 =
    [
        new Vector4(0.0f, 1.0f, 1.0f, 1.0f),
        new Vector4(0.0f, 1.0f, 1.0f, -1.0f),
        new Vector4(0.0f, 1.0f, -1.0f, 1.0f),
        new Vector4(0.0f, 1.0f, -1.0f, -1.0f),
        new Vector4(0.0f, -1.0f, 1.0f, 1.0f),
        new Vector4(0.0f, -1.0f, 1.0f, -1.0f),
        new Vector4(0.0f, -1.0f, -1.0f, 1.0f),
        new Vector4(0.0f, -1.0f, -1.0f, -1.0f),
        new Vector4(1.0f, 0.0f, 1.0f, 1.0f),
        new Vector4(1.0f, 0.0f, 1.0f, -1.0f),
        new Vector4(1.0f, 0.0f, -1.0f, 1.0f),
        new Vector4(1.0f, 0.0f, -1.0f, -1.0f),
        new Vector4(-1.0f, 0.0f, 1.0f, 1.0f),
        new Vector4(-1.0f, 0.0f, 1.0f, -1.0f),
        new Vector4(-1.0f, 0.0f, -1.0f, 1.0f),
        new Vector4(-1.0f, 0.0f, -1.0f, -1.0f),
        new Vector4(1.0f, 1.0f, 0.0f, 1.0f),
        new Vector4(1.0f, 1.0f, 0.0f, -1.0f),
        new Vector4(1.0f, -1.0f, 0.0f, 1.0f),
        new Vector4(1.0f, -1.0f, 0.0f, -1.0f),
        new Vector4(-1.0f, 1.0f, 0.0f, 1.0f),
        new Vector4(-1.0f, 1.0f, 0.0f, -1.0f),
        new Vector4(-1.0f, -1.0f, 0.0f, 1.0f),
        new Vector4(-1.0f, -1.0f, 0.0f, -1.0f),
        new Vector4(1.0f, 1.0f, 1.0f, 0.0f),
        new Vector4(1.0f, 1.0f, -1.0f, 0.0f),
        new Vector4(1.0f, -1.0f, 1.0f, 0.0f),
        new Vector4(1.0f, -1.0f, -1.0f, 0.0f),
        new Vector4(-1.0f, 1.0f, 1.0f, 0.0f),
        new Vector4(-1.0f, 1.0f, -1.0f, 0.0f),
        new Vector4(-1.0f, -1.0f, 1.0f, 0.0f),
        new Vector4(-1.0f, -1.0f, -1.0f, 0.0f)
    ];

    private static readonly short[] P =
    [
        151,
        160,
        137,
        91,
        90,
        15,
        131,
        13,
        201,
        95,
        96,
        53,
        194,
        233,
        7,
        225,
        140,
        36,
        103,
        30,
        69,
        142,
        8,
        99,
        37,
        240,
        21,
        10,
        23,
        190,
        6,
        148,
        247,
        120,
        234,
        75,
        0,
        26,
        197,
        62,
        94,
        252,
        219,
        203,
        117,
        35,
        11,
        32,
        57,
        177,
        33,
        88,
        237,
        149,
        56,
        87,
        174,
        20,
        125,
        136,
        171,
        168,
        68,
        175,
        74,
        165,
        71,
        134,
        139,
        48,
        27,
        166,
        77,
        146,
        158,
        231,
        83,
        111,
        229,
        122,
        60,
        211,
        133,
        230,
        220,
        105,
        92,
        41,
        55,
        46,
        245,
        40,
        244,
        102,
        143,
        54,
        65,
        25,
        63,
        161,
        1,
        216,
        80,
        73,
        209,
        76,
        132,
        187,
        208,
        89,
        18,
        169,
        200,
        196,
        135,
        130,
        116,
        188,
        159,
        86,
        164,
        100,
        109,
        198,
        173,
        186,
        3,
        64,
        52,
        217,
        226,
        250,
        124,
        123,
        5,
        202,
        38,
        147,
        118,
        126,
        255,
        82,
        85,
        212,
        207,
        206,
        59,
        227,
        47,
        16,
        58,
        17,
        182,
        189,
        28,
        42,
        223,
        183,
        170,
        213,
        119,
        248,
        152,
        2,
        44,
        154,
        163,
        70,
        221,
        153,
        101,
        155,
        167,
        43,
        172,
        9,
        129,
        22,
        39,
        253,
        19,
        98,
        108,
        110,
        79,
        113,
        224,
        232,
        178,
        185,
        112,
        104,
        218,
        246,
        97,
        228,
        251,
        34,
        242,
        193,
        238,
        210,
        144,
        12,
        191,
        179,
        162,
        241,
        81,
        51,
        145,
        235,
        249,
        14,
        239,
        107,
        49,
        192,
        214,
        31,
        181,
        199,
        106,
        157,
        184,
        84,
        204,
        176,
        115,
        121,
        50,
        45,
        127,
        4,
        150,
        254,
        138,
        236,
        205,
        93,
        222,
        114,
        67,
        29,
        24,
        72,
        243,
        141,
        128,
        195,
        78,
        66,
        215,
        61,
        156,
        180
    ];
}