using UnityEngine;

namespace Kameosa
{
    namespace Cartesian
    {
        public static class Util
        {
            #region Cartesian to Isometric projection
            public static UnityEngine.Vector2 CartesianCoordinateToIsometricVector2(float x, float y)
            {
                // return new UnityEngine.Vector2(x - y, (x + y) / 2);
                return new UnityEngine.Vector2(x + y, (y - x) / 2);
            }

            public static UnityEngine.Vector2 CartesianCoordinateToIsometricVector2(UnityEngine.Vector2 vector2)
            {
                return CartesianCoordinateToIsometricVector2(vector2.x, vector2.y);
            }

            public static UnityEngine.Vector2 CartesianCoordinateToIsometricVector2(UnityEngine.Vector3 vector3)
            {
                return CartesianCoordinateToIsometricVector2(vector3.x, vector3.y);
            }

            public static UnityEngine.Vector2 CartesianCoordinateToIsometricVector2(Coordinate coordinate)
            {
                return CartesianCoordinateToIsometricVector2(coordinate.X, coordinate.Y);
            }

            public static UnityEngine.Vector3 CartesianCoordinateToIsometricVector3(float x, float y)
            {
                UnityEngine.Vector2 vector2 = CartesianCoordinateToIsometricVector2(x, y);

                return new UnityEngine.Vector3(vector2.x, vector2.y);
            }

            public static UnityEngine.Vector3 CartesianCoordinateToIsometricVector3(UnityEngine.Vector2 vector2)
            {
                return CartesianCoordinateToIsometricVector3(vector2.x, vector2.y);
            }

            public static UnityEngine.Vector3 CartesianCoordinateToIsometricVector3(UnityEngine.Vector3 vector3)
            {
                return CartesianCoordinateToIsometricVector3(vector3.x, vector3.y);
            }

            public static UnityEngine.Vector3 CartesianCoordinateToIsometricVector3(Coordinate coordinate)
            {
                return CartesianCoordinateToIsometricVector3(coordinate.X, coordinate.Y);
            }
            #endregion

            #region Isometric to Cartesian projection
            public static UnityEngine.Vector2 IsometricToCartesianVector2(float x, float y)
            {
                // return new UnityEngine.Vector2(((2 * y) + x) / 2, ((2 * y) - x) / 2);
                return new UnityEngine.Vector2((x / 2) - y, y + (x / 2));
            }

            public static UnityEngine.Vector2 IsometricToCartesianVector2(UnityEngine.Vector2 vector2)
            {
                return IsometricToCartesianVector2(vector2.x, vector2.y);
            }

            public static UnityEngine.Vector2 IsometricToCartesianVector2(UnityEngine.Vector3 vector3)
            {
                return IsometricToCartesianVector2(vector3.x, vector3.y);
            }
            #endregion
        }
    }
}
