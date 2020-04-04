using UnityEngine;

namespace Kameosa
{
    namespace Vector2
    {
        public static class Util
        {
            public static UnityEngine.Vector2 AngleToVector2(float angle)
            {
                angle *= Mathf.Deg2Rad;

                return new UnityEngine.Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            }

            /// <summary>
            /// Return true if 2 vectors by distance is lesser than delta.
            /// Delta default to 0.001f.
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <param name="delta"></param>
            /// <returns></returns>
            public static bool IsEqual(UnityEngine.Vector2 a, UnityEngine.Vector2 b, float delta = 0.001f)
            {
                return UnityEngine.Vector2.Distance(a, b) < delta;
            }
        }
    }
}
