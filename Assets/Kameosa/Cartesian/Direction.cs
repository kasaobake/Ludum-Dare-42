using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kameosa
{
    namespace Cartesian
    {
        public static class Direction
        {
            public static Coordinate Up
            {
                get { return new Coordinate(0, 1); }
            }

            public static Coordinate Right
            {
                get { return new Coordinate(1, 0); }
            }

            public static Coordinate Down
            {
                get { return new Coordinate(0, -1); }
            }

            public static Coordinate Left
            {
                get { return new Coordinate(-1, 0); }
            }

            public static List<Coordinate> All
            {
                get
                {
                    List<Coordinate> directions = new List<Coordinate>();

                    directions.Add(Up);
                    directions.Add(Down);
                    directions.Add(Left);
                    directions.Add(Right);

                    return directions;
                }
            }
        }
    }
}
