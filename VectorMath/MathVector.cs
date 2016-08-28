using System;
using System.Collections.Generic;
using System.Numerics;

namespace VectorMath
{
    public sealed class MathVector<T> where T : struct
    {
        private readonly T[] _tail;

        public MathVector(T[] data)
        {
            if (data == null || data.Length == 0)
            {
                throw new ArgumentNullException(nameof(data));
            }

            Length = data.Length;

            if (Vector.IsHardwareAccelerated)
            {
                Vectors = Vectorize(data, out _tail);
            }
            else
            {
                Data = data;
            }
        }

        internal MathVector(List<Vector<T>> vectors, T[] tail)
        {
            if (!Vector.IsHardwareAccelerated)
            {
                throw new InvalidOperationException("Unable to construct a vector-based MathVector without HW support.");
            }

            Length = vectors.Count*Vector<T>.Count + tail.Length;

            Vectors = vectors;
            _tail = tail;
        }

        private static List<Vector<T>> Vectorize(T[] data, out T[] remainder)
        {
            var vectors = new List<Vector<T>>(data.Length/Vector<T>.Count);
            var i = 0;
            for (; i < data.Length/Vector<T>.Count; ++i)
            {
                vectors.Add(new Vector<T>(data, i*Vector<T>.Count));
            }

            var j = i = i*Vector<T>.Count;
            var tail = new T[data.Length - i];
            for (; i < data.Length; ++i)
            {
                tail[i - j] = data[i];
            }
            remainder = tail;

            return vectors;
        }

        public int Length { get; }

        internal T[] Data { get; }

        internal List<Vector<T>> Vectors { get; }

        internal T[] Tail => _tail;

        public static MathVector<T> operator +(MathVector<T> left, MathVector<T> right)
        {
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));

            if (left.Length != right.Length)
            {
                throw new InvalidOperationException("Unable to add two vectors of different sizes.");
            }

            return VectorOps<T>.GetInstance().Add(left, right);
        }

        public static MathVector<T> operator -(MathVector<T> left, MathVector<T> right)
        {
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));

            if (left.Length != right.Length)
            {
                throw new InvalidOperationException("Unable to add two vectors of different sizes.");
            }

            return VectorOps<T>.GetInstance().Subtract(left, right);
        }

        public MathVector<T> MultiplyScalar(T scalar)
        {
            return VectorOps<T>.GetInstance().MultiplyScalar(this, scalar);
        }

        public MathVector<T> Multiply(MathVector<T> right)
        {
            if (Length != right.Length)
            {
                throw new NotSupportedException();
            }

            return VectorOps<T>.GetInstance().Multiply(this, right);
        }

        public T this[int i]
        {
            get
            {
                if (Vector.IsHardwareAccelerated)
                {
                    var vectorizedCount = Vector<T>.Count*Vectors.Count;
                    if (i >= vectorizedCount)
                    {
                        return Tail[i - vectorizedCount];
                    }
                    else
                    {
                        var vectorIndex = (i/Vector<T>.Count);
                        return Vectors[vectorIndex][i%Vector<T>.Count];
                    }
                }
                else
                {
                    return Data[i];
                }
            }
        }

        public T Dot(MathVector<T> right)
        {
            if (right == null) throw new ArgumentNullException(nameof(right));

            if (Length != right.Length)
            {
                throw new InvalidOperationException("Unable to multiply two vectors of different sizes.");
            }

            return VectorOps<T>.GetInstance().Dot(this, right);
        }

        public MathVector<T> DivideScalar(T scalar)
        {
            return VectorOps<T>.GetInstance().Divide(this, scalar);
        }

        public MathVector<T> Normalize()
        {
            return VectorOps<T>.GetInstance().Normalize(this);
        }

        public T VectorLength()
        {
            return VectorOps<T>.GetInstance().Length(this);
        }

        public T[] ToArray()
        {
            if (Vector.IsHardwareAccelerated)
            {
                var data = new T[Length];
                var i = 0;
                for (; i < Vectors.Count; i++)
                {
                    Vectors[i].CopyTo(data, i * Vector<T>.Count);
                }

                if (Tail.Length > 0)
                {
                    Tail.CopyTo(data, i*Vector<T>.Count);
                }

                return data;
            }
            else
            {
                return Data;
            }
        }
    }
}
