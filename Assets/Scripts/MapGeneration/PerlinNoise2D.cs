using UnityEngine;
using Unity.Mathematics;

public static class PerlinNoise2D
{
   public static float PerlinNoiseValue(float x, float y, Vector2 Origin, int TextureSize, float NoiseScale, int NoiseOctaves, float IslandSize)
    {
        float a = 0, noisesize = NoiseScale, opacity = 1;

        for (int octaves = 0; octaves < NoiseOctaves; octaves++)
        {
            // Obliczenie warto�ci dla Perlin Noise
            float xVal = (x / (noisesize * TextureSize)) + Origin.x;
            float yVal = (y / (noisesize * TextureSize)) - Origin.y;
            float z = noise.snoise(new float2(xVal, yVal)); // Simplex noise
            a += Mathf.InverseLerp(0, 1, z) / opacity;

            // Zmniejszenie skali szumu i zwi�kszenie przezroczysto�ci
            noisesize /= 2f;
            opacity *= 2f;
        }

        // Zastosowanie mapy wygaszenia (FallOffMap), aby uzyska� efekt "wyspiarski"
        //return a;
        return a -= FallOffMap(x, y, TextureSize, IslandSize);
    }

    private static float FallOffMap(float x, float y, int size, float islandSize)
    {
        // Obliczanie wsp�czynnika gradientu na podstawie odleg�o�ci piksela od �rodka mapy
        float gradient = 1;
        gradient /= (x * y) / (size * size) * (1 - (x / size)) * (1 - (y / size));
        gradient -= 16;
        gradient /= islandSize;

        return gradient;
    }
}








