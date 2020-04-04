using UnityEngine;

namespace Kameosa
{
    namespace Services
    {
        public static class FrameService
        {
            public static int GetFrameCount(float time)
            {
                float frames = time / Time.fixedDeltaTime;
                int roundedFrames = Mathf.RoundToInt(frames);

                if (Mathf.Approximately(frames, roundedFrames))
                {
                    return roundedFrames;
                }

                return Mathf.CeilToInt(frames);

            }
        }
    }
}
