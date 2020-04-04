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
        public sealed class Tuple<T1, T2>
        {
            private readonly T1 first;
            private readonly T2 second;

            /// <summary>
            /// Returns the first element of the tuple
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
            /// Create a new tuple value
            /// </summary>
            /// <param name="first">First element of the tuple</param>
            /// <param name="second">Second element of the tuple</param>
            public Tuple(T1 first, T2 second)
            {
                this.first = first;
                this.second = second;
            }

            public override string ToString()
            {
                return string.Format("Tuple({0}, {1})", this.first, this.second);
            }

            public override int GetHashCode()
            {
                int hash = 17;

                hash = hash * 23 + (this.first == null ? 0 : this.first.GetHashCode());
                hash = hash * 23 + (this.second == null ? 0 : this.second.GetHashCode());

                return hash;
            }

            public override bool Equals(object o)
            {
                if (!(o is Tuple<T1, T2>))
                {
                    return false;
                }

                var other = (Tuple<T1, T2>) o;

                return this == other;
            }

            public bool Equals(Tuple<T1, T2> other)
            {
                return this == other;
            }

            public static bool operator ==(Tuple<T1, T2> a, Tuple<T1, T2> b)
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

                return a.first.Equals(b.first) && a.second.Equals(b.second);
            }

            public static bool operator !=(Tuple<T1, T2> a, Tuple<T1, T2> b)
            {
                return !(a == b);
            }

            public void Unpack(Action<T1, T2> unpackerDelegate)
            {
                unpackerDelegate(this.first, this.second);
            }
        }
    }
}