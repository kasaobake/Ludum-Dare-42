using System.Collections.Generic;
using Kameosa.Cartesian;

namespace Kameosa
{
    namespace Constants
    {
        public static class Resolution 
        {
            public static Dictionary<string, Coordinate> Low = new Dictionary<string, Coordinate>()
            {
                {  "3:2", new Coordinate(480, 320) },
                {  "4:3", new Coordinate(640, 480) },
                {  "16:9", new Coordinate(960, 540) },
            };

            public static Dictionary<string, Coordinate> Mid = new Dictionary<string, Coordinate>()
            {
                {  "3:2", new Coordinate(960, 640) },
                {  "4:3", new Coordinate(960, 720) },
                {  "16:9", new Coordinate(1280, 720) },
            };

            public static Dictionary<string, Coordinate> High = new Dictionary<string, Coordinate>()
            {
                {  "3:2", new Coordinate(1920, 1280) },
                {  "4:3", new Coordinate(1280, 960) },
                {  "16:9", new Coordinate(1920, 1080) },
            };
        }
    }
}
