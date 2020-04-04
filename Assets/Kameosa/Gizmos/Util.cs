using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kameosa
{
    namespace Gizmos
    {
        public sealed class Util
        {
            public static UnityEngine.Vector2 offset = UnityEngine.Vector2.zero;

            public static void DrawCross(UnityEngine.Vector2 position, UnityEngine.Color color, float size = 0.1f)
            {
                position += Util.offset;

                UnityEngine.Gizmos.color = color;
                UnityEngine.Vector2 offset1 = UnityEngine.Vector2.up * size;
                UnityEngine.Vector2 offset2 = UnityEngine.Vector2.right * size;
                UnityEngine.Gizmos.DrawLine(position + offset1 - offset2, position - offset1 + offset2);
                UnityEngine.Gizmos.DrawLine(position + offset1 + offset2, position - offset1 - offset2);
            }

            public static void DrawCircle(UnityEngine.Vector2 position, UnityEngine.Color color, float size = 0.1f)
            {
                position += Util.offset;

                UnityEngine.Gizmos.color = color;
                UnityEngine.Gizmos.DrawWireSphere(position, size);
            }

            public static void DrawConnectPoints(List<UnityEngine.Vector2> positions, UnityEngine.Color color, bool isDrawPoints = true)
            {
                if (positions == null || positions.Count == 0)
                {
                    return;
                }

                UnityEngine.Gizmos.color = color;

                if (isDrawPoints)
                {
                    DrawCircle(positions[0], color);
                }

                for (int i = 1; i < positions.Count; i++)
                {
                    if (isDrawPoints)
                    {
                        DrawCircle(positions[i], color);
                    }

                    UnityEngine.Gizmos.DrawLine(positions[i - 1] + Util.offset, positions[i] + Util.offset);
                }
            }
        }
    }
}