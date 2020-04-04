using System;
using UnityEngine;

namespace Kameosa
{
    namespace Vector3
    {
        public static class Util
        {
            /// <summary>
            /// Return true if 2 vectors by distance is lesser than delta.
            /// Delta default to 0.001f.
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <param name="delta"></param>
            /// <returns></returns>
            public static bool IsEqual(UnityEngine.Vector3 a, UnityEngine.Vector3 b, float delta = 0.001f)
            {
                return UnityEngine.Vector3.Distance(a, b) < delta;
            }

            public static UnityEngine.Vector3 Clamp(UnityEngine.Vector3 v, float minX = Int32.MinValue, float maxX = Int32.MaxValue, float minY = Int32.MinValue, float maxY = Int32.MaxValue, float minZ = Int32.MinValue, float maxZ = Int32.MaxValue)
            {
                v.x = Mathf.Clamp(v.x, minX, maxX);
                v.y = Mathf.Clamp(v.y, minY, maxY);
                v.z = Mathf.Clamp(v.z, minZ, maxZ);

                return v;
            }
        }
    }
}
