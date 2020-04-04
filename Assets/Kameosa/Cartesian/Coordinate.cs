using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kameosa
{
    namespace Cartesian
    {
        [System.Serializable]
        public class Coordinate 
        {
            #region Private Variables
            [SerializeField]
            private int x;
            [SerializeField]
            private int y;
            #endregion

            #region Properties
            public int X
            {
                get
                {
                    return this.x;
                }

                set
                {
                    this.x = value;
                }
            }

            public int Y
            {
                get
                {
                    return this.y;
                }

                set
                {
                    this.y = value;
                }
            }

            public int SqrMagnitude
            {
                get
                {
                    return (this.x * this.x) + (this.y * this.y);
                }
            }

            public List<Coordinate> Neighbours
            {
                get
                {
                    List<Coordinate> neighbours = new List<Coordinate>();

                    foreach (Coordinate direction in Direction.All)
                    {
                        neighbours.Add(this + direction);
                    }

                    return neighbours;
                }
            }
            #endregion

            public Coordinate(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            #region Operators
            public static implicit operator UnityEngine.Vector2(Coordinate coordinate)
            {
                return new UnityEngine.Vector2(coordinate.X, coordinate.Y);
            }

            public static implicit operator UnityEngine.Vector3(Coordinate coordinate)
            {
                return new UnityEngine.Vector3(coordinate.X, coordinate.Y, 0);
            }

            public static implicit operator Coordinate(UnityEngine.Vector2 coordinate)
            {
                return new Coordinate((int)coordinate.x, (int)coordinate.y);
            }

            public static implicit operator string(Coordinate coordinate)
            {
                return "(" + coordinate.X + ", " + coordinate.Y + ")";
            }

            public static Coordinate operator +(Coordinate a, Coordinate b)
            {
                return new Coordinate(a.X + b.X, a.Y + b.Y);
            }

            public static Coordinate operator +(Coordinate a, UnityEngine.Vector2 b)
            {
                return new Coordinate(a.X + (int)b.x, a.Y + (int)b.y);
            }

            public static Coordinate operator -(Coordinate a, Coordinate b)
            {
                return new Coordinate(a.X - b.X, a.Y - b.Y);
            }

            public static Coordinate operator -(Coordinate a, UnityEngine.Vector2 b)
            {
                return new Coordinate(a.X - (int)b.x, a.Y - (int)b.y);
            }

            public static Coordinate operator *(Coordinate a, int b)
            {
                return new Coordinate(a.X * b, a.Y * b);
            }

            /*
            public static bool operator ==(Coordinate a, Coordinate b)
            {
                return (a.X == b.X && a.Y == b.Y);
            }

            public static bool operator !=(Coordinate a, Coordinate b)
            {
                return (a.X != b.X || a.Y != b.Y);
            }
            */
            #endregion

            public bool IsAbove(Coordinate coordinate)
            {
                return this.x > coordinate.X;
            }

            public bool IsBelow(Coordinate coordinate)
            {
                return this.x < coordinate.X;
            }

            public bool IsRightOf(Coordinate coordinate)
            {
                return this.y > coordinate.Y;
            }

            public bool IsLeftOf(Coordinate coordinate)
            {
                return this.y < coordinate.Y;
            }

            public bool Equals(Coordinate coordinate)
            {
                return (this.x == coordinate.X) && (this.y == coordinate.Y);
            }

            public bool NotEquals(Coordinate coordinate)
            {
                return !Equals(coordinate);
            }

            public override int GetHashCode()
            {
                return this.x ^ this.y;
            }

            public override string ToString()
            {
                return string.Format("({0}, {1})", this.x, this.y);
            }
        }
    }
}
