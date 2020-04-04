using System;

namespace Kameosa
{
    namespace Tuples
    {
        /// <summary>
        /// Represents a functional tuple that can be used to store
        /// two values of different types inside one object.
        /// </summary>
        /// <typeparam name="T1">The type of the first element</typeparam>
        /// <typeparam name="T2">The type of the second element</typeparam>
        /// <typeparam name="T3">The type of the third element</typeparam>
        /// <typeparam name="T4">The type of the fourth element</typeparam>
        public sealed class Tuple<T1, T2, T3, T4>
        {
            private readonly T1 first;
            private readonly T2 second;
            private readonly T3 third;
            private readonly T4 fourth;

            /// <summary>
            /// Retyurns the first element of the tuple
            /// </summary>
            public T1 First
            {
                get
                {
                    return this.first;
                }
            }

            /// <summary>
            /// Returns the second element of the tuple
            /// </summary>
            public T2 Second
            {
                get
                {
                    return this.second;
                }
            }

            /// <summary>
            /// Returns the second element of the tuple
            /// </summary>
            public T3 Third
            {
                get
                {
                    return this.third;
                }
            }

            /// <summary>
            /// Returns the second element of the tuple
            /// </summary>
            public T4 Fourth
            {
                get
                {
                    return this.fourth;
                }
            }

            /// <summary>
            /// Create a new tuple value
            /// </summary>
            /// <param name="first">First element of the tuple</param>
            /// <param name="second">Second element of the tuple</param>
            /// <param name="third">Third element of the tuple</param>
            /// <param name="fourth">Fourth element of the tuple</param>
            public Tuple(T1 first, T2 second, T3 third, T4 fourth)
            {
                this.first = first;
                this.second = second;
                this.third = third;
                this.fourth = fourth;
            }

            public override int GetHashCode()
            {
                int hash = 17;

                hash = hash * 23 + (this.first == null ? 0 : this.first.GetHashCode());
                hash = hash * 23 + (this.second == null ? 0 : this.second.GetHashCode());
                hash = hash * 23 + (this.third == null ? 0 : this.third.GetHashCode());
                hash = hash * 23 + (this.fourth == null ? 0 : this.fourth.GetHashCode());

                return hash;
            }

            public override bool Equals(object o)
            {
                if (o.GetType() != typeof(Tuple<T1, T2, T3, T4>))
                {
                    return false;
                }

                var other = (Tuple<T1, T2, T3, T4>)o;

                return this == other;
            }

            public static bool operator ==(Tuple<T1, T2, T3, T4> a, Tuple<T1, T2, T3, T4> b)
            {
                if (object.ReferenceEquals(a, null))
                {
                    return object.ReferenceEquals(b, null);
                }

                if (a.first == null && b.first != null)
                {
                    return false;
                }

                if (a.second == null && b.second != null)
                {
                    return false;
                }

                if (a.third == null && b.third != null)
                {
                    return false;
                }

                if (a.fourth == null && b.fourth != null)
                {
                    return false;
                }

                return a.first.Equals(b.first) && a.second.Equals(b.second) && a.third.Equals(b.third) && a.fourth.Equals(b.fourth);
            }

            public static bool operator !=(Tuple<T1, T2, T3, T4> a, Tuple<T1, T2, T3, T4> b)
            {
                return !(a == b);
            }

            public void Unpack(Action<T1, T2, T3, T4> unpackerDelegate)
            {
                unpackerDelegate(this.first, this.second, this.third, this.fourth);
            }
        }
    }
}