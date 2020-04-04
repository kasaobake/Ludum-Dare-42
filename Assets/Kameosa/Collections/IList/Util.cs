using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kameosa
{
    namespace Collections
    {
        namespace IList
        {
            public static class Util
            {
                public static IList<T> Randomize<T>(this System.Collections.Generic.IList<T> list)
                {
                    return Randomize(list, (int) System.DateTime.Now.Ticks);
                }

                public static IList<T> Randomize<T>(this System.Collections.Generic.IList<T> list, int seed)
                {
                    System.Random random = new System.Random(seed);

                    for (int i = 0; i < list.Count - 1; i++)
                    {
                        T temp = list[i];
                        //int randomIndex = Random.Range(i, list.Count);
                        int randomIndex = random.Next(i, list.Count);
                        list[i] = list[randomIndex];
                        list[randomIndex] = temp;
                    }

                    return list;
                }

                public static void Fill<T>(this System.Collections.Generic.IList<T> list, int size, T value)
                {
                    list.Clear();

                    for (int i = 0; i < size; i++)
                    {
                        list.Add(value);
                    }
                }

                public static void Fill<T>(this System.Collections.Generic.IList<T> list, T value)
                {
                    list.Fill(list.Count, value);
                }

                public static T GetRandom<T>(this System.Collections.Generic.IList<T> list)
                {
                    if (list.Count == 0)
                    {
                        return default(T);
                    }

                    return list[Random.Range(0, list.Count)];
                }

                public static T GetMedian<T>(this IList<T> list)
                {
                    if (list.Count == 0)
                    {
                        return default(T);
                    }

                    return list[Mathf.FloorToInt(list.Count / 2)];
                }
            }
        }
    }
}
