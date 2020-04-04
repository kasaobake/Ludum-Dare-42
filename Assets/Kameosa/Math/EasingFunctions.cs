using UnityEngine;

/*
 * TERMS OF USE - EASING EQUATIONS
 * Open source under the BSD License.
 * Copyright (c)2001 Robert Penner
 * All rights reserved.
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
 * Neither the name of the author nor the names of contributors may be used to endorse or promote products derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
 * THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE 
 * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; 
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT 
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

namespace Kameosa
{
    namespace Math
    {
        public class Easing
        {
            public enum Functions
            {
                InQuad,
                OutQuad,
                InOutQuad,
                InCubic,
                OutCubic,
                InOutCubic,
                InQuart,
                OutQuart,
                InOutQuart,
                InQuint,
                OutQuint,
                InOutQuint,
                InSine,
                OutSine,
                InOutSine,
                InExpo,
                OutExpo,
                InOutExpo,
                InCirc,
                OutCirc,
                InOutCirc,
                Linear,
                Spring,
                InBounce,
                OutBounce,
                InOutBounce,
                InBack,
                OutBack,
                InOutBack,
                InElastic,
                OutElastic,
                InOutElastic,
            }

            private const float NATURAL_LOG_OF_2 = 0.693147181f;

            //
            // Easing functions
            //

            public static float Linear(float start, float end, float value)
            {
                return Mathf.Lerp(start, end, value);
            }

            public static float Spring(float start, float end, float value)
            {
                value = Mathf.Clamp01(value);
                value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
                return start + (end - start) * value;
            }

            public static float InQuad(float start, float end, float value)
            {
                end -= start;
                return end * value * value + start;
            }

            public static float OutQuad(float start, float end, float value)
            {
                end -= start;
                return -end * value * (value - 2) + start;
            }

            public static float InOutQuad(float start, float end, float value)
            {
                value /= .5f;
                end -= start;
                if (value < 1)
                    return end * 0.5f * value * value + start;
                value--;
                return -end * 0.5f * (value * (value - 2) - 1) + start;
            }

            public static float InCubic(float start, float end, float value)
            {
                end -= start;
                return end * value * value * value + start;
            }

            public static float OutCubic(float start, float end, float value)
            {
                value--;
                end -= start;
                return end * (value * value * value + 1) + start;
            }

            public static float InOutCubic(float start, float end, float value)
            {
                value /= .5f;
                end -= start;
                if (value < 1)
                    return end * 0.5f * value * value * value + start;
                value -= 2;
                return end * 0.5f * (value * value * value + 2) + start;
            }

            public static float InQuart(float start, float end, float value)
            {
                end -= start;
                return end * value * value * value * value + start;
            }

            public static float OutQuart(float start, float end, float value)
            {
                value--;
                end -= start;
                return -end * (value * value * value * value - 1) + start;
            }

            public static float InOutQuart(float start, float end, float value)
            {
                value /= .5f;
                end -= start;
                if (value < 1)
                    return end * 0.5f * value * value * value * value + start;
                value -= 2;
                return -end * 0.5f * (value * value * value * value - 2) + start;
            }

            public static float InQuint(float start, float end, float value)
            {
                end -= start;
                return end * value * value * value * value * value + start;
            }

            public static float OutQuint(float start, float end, float value)
            {
                value--;
                end -= start;
                return end * (value * value * value * value * value + 1) + start;
            }

            public static float InOutQuint(float start, float end, float value)
            {
                value /= .5f;
                end -= start;
                if (value < 1)
                    return end * 0.5f * value * value * value * value * value + start;
                value -= 2;
                return end * 0.5f * (value * value * value * value * value + 2) + start;
            }

            public static float InSine(float start, float end, float value)
            {
                end -= start;
                return -end * Mathf.Cos(value * (Mathf.PI * 0.5f)) + end + start;
            }

            public static float OutSine(float start, float end, float value)
            {
                end -= start;
                return end * Mathf.Sin(value * (Mathf.PI * 0.5f)) + start;
            }

            public static float InOutSine(float start, float end, float value)
            {
                end -= start;
                return -end * 0.5f * (Mathf.Cos(Mathf.PI * value) - 1) + start;
            }

            public static float InExpo(float start, float end, float value)
            {
                end -= start;
                return end * Mathf.Pow(2, 10 * (value - 1)) + start;
            }

            public static float OutExpo(float start, float end, float value)
            {
                end -= start;
                return end * (-Mathf.Pow(2, -10 * value) + 1) + start;
            }

            public static float InOutExpo(float start, float end, float value)
            {
                value /= .5f;
                end -= start;
                if (value < 1)
                    return end * 0.5f * Mathf.Pow(2, 10 * (value - 1)) + start;
                value--;
                return end * 0.5f * (-Mathf.Pow(2, -10 * value) + 2) + start;
            }

            public static float InCirc(float start, float end, float value)
            {
                end -= start;
                return -end * (Mathf.Sqrt(1 - value * value) - 1) + start;
            }

            public static float OutCirc(float start, float end, float value)
            {
                value--;
                end -= start;
                return end * Mathf.Sqrt(1 - value * value) + start;
            }

            public static float InOutCirc(float start, float end, float value)
            {
                value /= .5f;
                end -= start;
                if (value < 1)
                    return -end * 0.5f * (Mathf.Sqrt(1 - value * value) - 1) + start;
                value -= 2;
                return end * 0.5f * (Mathf.Sqrt(1 - value * value) + 1) + start;
            }

            public static float InBounce(float start, float end, float value)
            {
                end -= start;
                float d = 1f;
                return end - OutBounce(0, end, d - value) + start;
            }

            public static float OutBounce(float start, float end, float value)
            {
                value /= 1f;
                end -= start;
                if (value < (1 / 2.75f))
                {
                    return end * (7.5625f * value * value) + start;
                }
                else if (value < (2 / 2.75f))
                {
                    value -= (1.5f / 2.75f);
                    return end * (7.5625f * (value) * value + .75f) + start;
                }
                else if (value < (2.5 / 2.75))
                {
                    value -= (2.25f / 2.75f);
                    return end * (7.5625f * (value) * value + .9375f) + start;
                }
                else
                {
                    value -= (2.625f / 2.75f);
                    return end * (7.5625f * (value) * value + .984375f) + start;
                }
            }

            public static float InOutBounce(float start, float end, float value)
            {
                end -= start;
                float d = 1f;
                if (value < d * 0.5f)
                    return InBounce(0, end, value * 2) * 0.5f + start;
                else
                    return OutBounce(0, end, value * 2 - d) * 0.5f + end * 0.5f + start;
            }

            public static float InBack(float start, float end, float value)
            {
                end -= start;
                value /= 1;
                float s = 1.70158f;
                return end * (value) * value * ((s + 1) * value - s) + start;
            }

            public static float OutBack(float start, float end, float value)
            {
                float s = 1.70158f;
                end -= start;
                value = (value) - 1;
                return end * ((value) * value * ((s + 1) * value + s) + 1) + start;
            }

            public static float InOutBack(float start, float end, float value)
            {
                float s = 1.70158f;
                end -= start;
                value /= .5f;
                if ((value) < 1)
                {
                    s *= (1.525f);
                    return end * 0.5f * (value * value * (((s) + 1) * value - s)) + start;
                }
                value -= 2;
                s *= (1.525f);
                return end * 0.5f * ((value) * value * (((s) + 1) * value + s) + 2) + start;
            }

            public static float InElastic(float start, float end, float value)
            {
                end -= start;

                float d = 1f;
                float p = d * .3f;
                float s;
                float a = 0;

                if (value == 0)
                    return start;

                if ((value /= d) == 1)
                    return start + end;

                if (a == 0f || a < Mathf.Abs(end))
                {
                    a = end;
                    s = p / 4;
                }
                else
                {
                    s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
                }

                return -(a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
            }

            public static float OutElastic(float start, float end, float value)
            {
                end -= start;

                float d = 1f;
                float p = d * .3f;
                float s;
                float a = 0;

                if (value == 0)
                    return start;

                if ((value /= d) == 1)
                    return start + end;

                if (a == 0f || a < Mathf.Abs(end))
                {
                    a = end;
                    s = p * 0.25f;
                }
                else
                {
                    s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
                }

                return (a * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) + end + start);
            }

            public static float InOutElastic(float start, float end, float value)
            {
                end -= start;

                float d = 1f;
                float p = d * .3f;
                float s;
                float a = 0;

                if (value == 0)
                    return start;

                if ((value /= d * 0.5f) == 2)
                    return start + end;

                if (a == 0f || a < Mathf.Abs(end))
                {
                    a = end;
                    s = p / 4;
                }
                else
                {
                    s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
                }

                if (value < 1)
                    return -0.5f * (a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
                return a * Mathf.Pow(2, -10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) * 0.5f + end + start;
            }

            //
            // These are derived functions that the motor can use to get the speed at a specific time.
            //
            // The easing functions all work with a normalized time (0 to 1) and the returned value here
            // reflects that. Values returned here should be divided by the actual time.
            //
            // TODO: These functions have not had the testing they deserve. If there is odd behavior around
            //       dash speeds then this would be the first place I'd look.

            public static float LinearD(float start, float end, float value)
            {
                return end - start;
            }

            public static float InQuadD(float start, float end, float value)
            {
                return 2f * (end - start) * value;
            }

            public static float OutQuadD(float start, float end, float value)
            {
                end -= start;
                return -end * value - end * (value - 2);
            }

            public static float InOutQuadD(float start, float end, float value)
            {
                value /= .5f;
                end -= start;

                if (value < 1)
                {
                    return end * value;
                }

                value--;

                return end * (1 - value);
            }

            public static float InCubicD(float start, float end, float value)
            {
                return 3f * (end - start) * value * value;
            }

            public static float OutCubicD(float start, float end, float value)
            {
                value--;
                end -= start;
                return 3f * end * value * value;
            }

            public static float InOutCubicD(float start, float end, float value)
            {
                value /= .5f;
                end -= start;

                if (value < 1)
                {
                    return (3f / 2f) * end * value * value;
                }

                value -= 2;

                return (3f / 2f) * end * value * value;
            }

            public static float InQuartD(float start, float end, float value)
            {
                return 4f * (end - start) * value * value * value;
            }

            public static float OutQuartD(float start, float end, float value)
            {
                value--;
                end -= start;
                return -4f * end * value * value * value;
            }

            public static float InOutQuartD(float start, float end, float value)
            {
                value /= .5f;
                end -= start;

                if (value < 1)
                {
                    return 2f * end * value * value * value;
                }

                value -= 2;

                return -2f * end * value * value * value;
            }

            public static float InQuintD(float start, float end, float value)
            {
                return 5f * (end - start) * value * value * value * value;
            }

            public static float OutQuintD(float start, float end, float value)
            {
                value--;
                end -= start;
                return 5f * end * value * value * value * value;
            }

            public static float InOutQuintD(float start, float end, float value)
            {
                value /= .5f;
                end -= start;

                if (value < 1)
                {
                    return (5f / 2f) * end * value * value * value * value;
                }

                value -= 2;

                return (5f / 2f) * end * value * value * value * value;
            }

            public static float InSineD(float start, float end, float value)
            {
                return (end - start) * 0.5f * Mathf.PI * Mathf.Sin(0.5f * Mathf.PI * value);
            }

            public static float OutSineD(float start, float end, float value)
            {
                end -= start;
                return (Mathf.PI * 0.5f) * end * Mathf.Cos(value * (Mathf.PI * 0.5f));
            }

            public static float InOutSineD(float start, float end, float value)
            {
                end -= start;
                return end * 0.5f * Mathf.PI * Mathf.Cos(Mathf.PI * value);
            }
            public static float InExpoD(float start, float end, float value)
            {
                return (10f * NATURAL_LOG_OF_2 * (end - start) * Mathf.Pow(2f, 10f * (value - 1)));
            }

            public static float OutExpoD(float start, float end, float value)
            {
                end -= start;
                return 5f * NATURAL_LOG_OF_2 * end * Mathf.Pow(2f, 1f - 10f * value);
            }

            public static float InOutExpoD(float start, float end, float value)
            {
                value /= .5f;
                end -= start;

                if (value < 1)
                {
                    return 5f * NATURAL_LOG_OF_2 * end * Mathf.Pow(2f, 10f * (value - 1));
                }

                value--;

                return (5f * NATURAL_LOG_OF_2 * end) / (Mathf.Pow(2f, 10f * value));
            }

            public static float InCircD(float start, float end, float value)
            {
                return ((end - start) * value) / Mathf.Sqrt(1f - value * value);
            }

            public static float OutCircD(float start, float end, float value)
            {
                value--;
                end -= start;
                return (-end * value) / Mathf.Sqrt(1f - value * value);
            }

            public static float InOutCircD(float start, float end, float value)
            {
                value /= .5f;
                end -= start;

                if (value < 1)
                {
                    return (end * value) / (2f * Mathf.Sqrt(1f - value * value));
                }

                value -= 2;

                return (-end * value) / (2f * Mathf.Sqrt(1f - value * value));
            }

            public static float InBounceD(float start, float end, float value)
            {
                end -= start;
                float d = 1f;

                return OutBounceD(0, end, d - value);
            }

            public static float OutBounceD(float start, float end, float value)
            {
                value /= 1f;
                end -= start;

                if (value < (1 / 2.75f))
                {
                    return 2f * end * 7.5625f * value;
                }
                else if (value < (2 / 2.75f))
                {
                    value -= (1.5f / 2.75f);
                    return 2f * end * 7.5625f * value;
                }
                else if (value < (2.5 / 2.75))
                {
                    value -= (2.25f / 2.75f);
                    return 2f * end * 7.5625f * value;
                }
                else
                {
                    value -= (2.625f / 2.75f);
                    return 2f * end * 7.5625f * value;
                }
            }

            public static float InOutBounceD(float start, float end, float value)
            {
                end -= start;
                float d = 1f;

                if (value < d * 0.5f)
                {
                    return InBounceD(0, end, value * 2) * 0.5f;
                }
                else
                {
                    return OutBounceD(0, end, value * 2 - d) * 0.5f;
                }
            }

            public static float InBackD(float start, float end, float value)
            {
                // Since the motor only cares about final speed, we only consider that part of the bounce function.
                float s = 1.70158f;

                return 3f * (s + 1f) * (end - start) * value * value - 2f * s * (end - start) * value;
            }

            public static float OutBackD(float start, float end, float value)
            {
                float s = 1.70158f;
                end -= start;
                value = (value) - 1;

                return end * ((s + 1f) * value * value + 2f * value * ((s + 1f) * value + s));
            }

            public static float InOutBackD(float start, float end, float value)
            {
                float s = 1.70158f;
                end -= start;
                value /= .5f;

                if ((value) < 1)
                {
                    s *= (1.525f);
                    return 0.5f * end * (s + 1) * value * value + end * value * ((s + 1f) * value - s);
                }

                value -= 2;
                s *= (1.525f);
                return 0.5f * end * ((s + 1) * value * value + 2f * value * ((s + 1f) * value + s));
            }

            public static float InElasticD(float start, float end, float value)
            {
                end -= start;

                float d = 1f;
                float p = d * .3f;
                float s;
                float a = 0;

                if (a == 0f || a < Mathf.Abs(end))
                {
                    a = end;
                    s = p / 4;
                }
                else
                {
                    s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
                }

                float c = 2 * Mathf.PI;

                // From an online derivative calculator, kinda hoping it is right.
                return ((-a) * d * c * Mathf.Cos((c * (d * (value - 1f) - s)) / p)) / p -
                    5f * NATURAL_LOG_OF_2 * a * Mathf.Sin((c * (d * (value - 1f) - s)) / p) *
                    Mathf.Pow(2f, 10f * (value - 1f) + 1f);
            }

            public static float OutElasticD(float start, float end, float value)
            {
                end -= start;

                float d = 1f;
                float p = d * .3f;
                float s;
                float a = 0;

                if (a == 0f || a < Mathf.Abs(end))
                {
                    a = end;
                    s = p * 0.25f;
                }
                else
                {
                    s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
                }

                return (a * Mathf.PI * d * Mathf.Pow(2f, 1f - 10f * value) *
                    Mathf.Cos((2f * Mathf.PI * (d * value - s)) / p)) / p - 5f * NATURAL_LOG_OF_2 * a *
                    Mathf.Pow(2f, 1f - 10f * value) * Mathf.Sin((2f * Mathf.PI * (d * value - s)) / p);
            }

            public static float InOutElasticD(float start, float end, float value)
            {
                end -= start;

                float d = 1f;
                float p = d * .3f;
                float s;
                float a = 0;

                if (a == 0f || a < Mathf.Abs(end))
                {
                    a = end;
                    s = p / 4;
                }
                else
                {
                    s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
                }

                if (value < 1)
                {
                    value -= 1;

                    return -5f * NATURAL_LOG_OF_2 * a * Mathf.Pow(2f, 10f * value) * Mathf.Sin(2 * Mathf.PI * (d * value - 2f) / p) -
                        a * Mathf.PI * d * Mathf.Pow(2f, 10f * value) * Mathf.Cos(2 * Mathf.PI * (d * value - s) / p) / p;
                }

                value -= 1;

                return a * Mathf.PI * d * Mathf.Cos(2f * Mathf.PI * (d * value - s) / p) / (p * Mathf.Pow(2f, 10f * value)) -
                    5f * NATURAL_LOG_OF_2 * a * Mathf.Sin(2f * Mathf.PI * (d * value - s) / p) / (Mathf.Pow(2f, 10f * value));
            }

            public static float SpringD(float start, float end, float value)
            {
                value = Mathf.Clamp01(value);
                end -= start;

                // Damn... Thanks http://www.derivative-calculator.net/
                return end * (6f * (1f - value) / 5f + 1f) * (-2.2f * Mathf.Pow(1f - value, 1.2f) *
                    Mathf.Sin(Mathf.PI * value * (2.5f * value * value * value + 0.2f)) + Mathf.Pow(1f - value, 2.2f) *
                    (Mathf.PI * (2.5f * value * value * value + 0.2f) + 7.5f * Mathf.PI * value * value * value) *
                    Mathf.Cos(Mathf.PI * value * (2.5f * value * value * value + 0.2f)) + 1f) -
                    6f * end * (Mathf.Pow(1 - value, 2.2f) * Mathf.Sin(Mathf.PI * value * (2.5f * value * value * value + 0.2f)) + value
                    / 5f);

            }

            public delegate float EasingFunc(float s, float e, float v);

            /// <summary>
            /// Returns the function associated to the easingFunction enum. This value returned should be cached as it allocates memory
            /// to return.
            /// </summary>
            /// <param name="easingFunction">The enum associated with the easing function.</param>
            /// <returns>The easing function</returns>
            public static EasingFunc GetEasingFunction(Functions easingFunction)
            {
                if (easingFunction == Functions.InQuad)
                {
                    return InQuad;
                }

                if (easingFunction == Functions.OutQuad)
                {
                    return OutQuad;
                }

                if (easingFunction == Functions.InOutQuad)
                {
                    return InOutQuad;
                }

                if (easingFunction == Functions.InCubic)
                {
                    return InCubic;
                }

                if (easingFunction == Functions.OutCubic)
                {
                    return OutCubic;
                }

                if (easingFunction == Functions.InOutCubic)
                {
                    return InOutCubic;
                }

                if (easingFunction == Functions.InQuart)
                {
                    return InQuart;
                }

                if (easingFunction == Functions.OutQuart)
                {
                    return OutQuart;
                }

                if (easingFunction == Functions.InOutQuart)
                {
                    return InOutQuart;
                }

                if (easingFunction == Functions.InQuint)
                {
                    return InQuint;
                }

                if (easingFunction == Functions.OutQuint)
                {
                    return OutQuint;
                }

                if (easingFunction == Functions.InOutQuint)
                {
                    return InOutQuint;
                }

                if (easingFunction == Functions.InSine)
                {
                    return InSine;
                }

                if (easingFunction == Functions.OutSine)
                {
                    return OutSine;
                }

                if (easingFunction == Functions.InOutSine)
                {
                    return InOutSine;
                }

                if (easingFunction == Functions.InExpo)
                {
                    return InExpo;
                }

                if (easingFunction == Functions.OutExpo)
                {
                    return OutExpo;
                }

                if (easingFunction == Functions.InOutExpo)
                {
                    return InOutExpo;
                }

                if (easingFunction == Functions.InCirc)
                {
                    return InCirc;
                }

                if (easingFunction == Functions.OutCirc)
                {
                    return OutCirc;
                }

                if (easingFunction == Functions.InOutCirc)
                {
                    return InOutCirc;
                }

                if (easingFunction == Functions.Linear)
                {
                    return Linear;
                }

                if (easingFunction == Functions.Spring)
                {
                    return Spring;
                }

                if (easingFunction == Functions.InBounce)
                {
                    return InBounce;
                }

                if (easingFunction == Functions.OutBounce)
                {
                    return OutBounce;
                }

                if (easingFunction == Functions.InOutBounce)
                {
                    return InOutBounce;
                }

                if (easingFunction == Functions.InBack)
                {
                    return InBack;
                }

                if (easingFunction == Functions.OutBack)
                {
                    return OutBack;
                }

                if (easingFunction == Functions.InOutBack)
                {
                    return InOutBack;
                }

                if (easingFunction == Functions.InElastic)
                {
                    return InElastic;
                }

                if (easingFunction == Functions.OutElastic)
                {
                    return OutElastic;
                }

                if (easingFunction == Functions.InOutElastic)
                {
                    return InOutElastic;
                }

                return null;
            }

            /// <summary>
            /// Gets the derivative function of the appropriate easing function. If you use an easing function for position then this
            /// function can get you the speed at a given time (normalized).
            /// </summary>
            /// <param name="easingFunction"></param>
            /// <returns>The derivative function</returns>
            public static EasingFunc GetEasingFunctionDerivative(Functions easingFunction)
            {
                if (easingFunction == Functions.InQuad)
                {
                    return InQuadD;
                }

                if (easingFunction == Functions.OutQuad)
                {
                    return OutQuadD;
                }

                if (easingFunction == Functions.InOutQuad)
                {
                    return InOutQuadD;
                }

                if (easingFunction == Functions.InCubic)
                {
                    return InCubicD;
                }

                if (easingFunction == Functions.OutCubic)
                {
                    return OutCubicD;
                }

                if (easingFunction == Functions.InOutCubic)
                {
                    return InOutCubicD;
                }

                if (easingFunction == Functions.InQuart)
                {
                    return InQuartD;
                }

                if (easingFunction == Functions.OutQuart)
                {
                    return OutQuartD;
                }

                if (easingFunction == Functions.InOutQuart)
                {
                    return InOutQuartD;
                }

                if (easingFunction == Functions.InQuint)
                {
                    return InQuintD;
                }

                if (easingFunction == Functions.OutQuint)
                {
                    return OutQuintD;
                }

                if (easingFunction == Functions.InOutQuint)
                {
                    return InOutQuintD;
                }

                if (easingFunction == Functions.InSine)
                {
                    return InSineD;
                }

                if (easingFunction == Functions.OutSine)
                {
                    return OutSineD;
                }

                if (easingFunction == Functions.InOutSine)
                {
                    return InOutSineD;
                }

                if (easingFunction == Functions.InExpo)
                {
                    return InExpoD;
                }

                if (easingFunction == Functions.OutExpo)
                {
                    return OutExpoD;
                }

                if (easingFunction == Functions.InOutExpo)
                {
                    return InOutExpoD;
                }

                if (easingFunction == Functions.InCirc)
                {
                    return InCircD;
                }

                if (easingFunction == Functions.OutCirc)
                {
                    return OutCircD;
                }

                if (easingFunction == Functions.InOutCirc)
                {
                    return InOutCircD;
                }

                if (easingFunction == Functions.Linear)
                {
                    return LinearD;
                }

                if (easingFunction == Functions.Spring)
                {
                    return SpringD;
                }

                if (easingFunction == Functions.InBounce)
                {
                    return InBounceD;
                }

                if (easingFunction == Functions.OutBounce)
                {
                    return OutBounceD;
                }

                if (easingFunction == Functions.InOutBounce)
                {
                    return InOutBounceD;
                }

                if (easingFunction == Functions.InBack)
                {
                    return InBackD;
                }

                if (easingFunction == Functions.OutBack)
                {
                    return OutBackD;
                }

                if (easingFunction == Functions.InOutBack)
                {
                    return InOutBackD;
                }

                if (easingFunction == Functions.InElastic)
                {
                    return InElasticD;
                }

                if (easingFunction == Functions.OutElastic)
                {
                    return OutElasticD;
                }

                if (easingFunction == Functions.InOutElastic)
                {
                    return InOutElasticD;
                }

                return null;
            }
        }
    }
}
