using UnityEngine;

namespace Kameosa
{
    namespace Math
    {
        public static class Util
        {
            public static int Roll(int numberOfSides)
            {
                return Random.Range(1, numberOfSides);
            }

            public static int GetRandomOddIntegerFromRange(int min, int max)
            {
                return (Mathf.FloorToInt(Random.Range(min / 2, max / 2)) * 2) + 1;
            }

            public static int CeilingToOddInteger(int number)
            {
                if (number % 2 == 1)
                {
                    return number;
                }

                return number + 1;
            }

            public static bool IsEqual(float a, float b, float delta = 0.001f)
            {
                return Mathf.Abs(a - b) < delta;
            }

            // Keep easingValue between 1-3 for best result.
            // https://youtu.be/3D0PeJh6GY8
            public static float Ease01(float k, float easingValue = 2)
            {
                k = Mathf.Clamp01(k);
                return Mathf.Pow(k, easingValue) / (Mathf.Pow(k, easingValue) + Mathf.Pow(1 - k, easingValue));
            }
        }
    }
}
