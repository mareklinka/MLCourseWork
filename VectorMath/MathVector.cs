using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;

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

#if SIMD
            if (true)
#else
             if (Vector.IsHardwareAccelerated)
#endif
            {
                Vectors = Vectorize(data, out _tail);
            }
            else
            {
                Data = data;
            }
        }

        public MathVector(T value, int count)
        {

            Length = count;

#if SIMD
            if (true)
#else
            if (Vector.IsHardwareAccelerated)
#endif
            {
                var vectorCount = count/Vector<T>.Count;
                Vectors = new List<Vector<T>>(vectorCount);

                for (var i = 0; i < vectorCount; i++)
                {
                    Vectors.Add(new Vector<T>(value));
                }

                var remainder = count%Vector<T>.Count;

                _tail = new T[remainder];
                for (int i = 0; i < remainder; i++)
                {
                    _tail[i] = value;
                }
            }
            else
            {
                Data = new T[count];
                for (var i = 0; i < count; i++)
                {
                    Data[i] = value;
                }
            }
        }

        internal MathVector(List<Vector<T>> vectors, T[] tail)
        {
#if SIMD
            if (false)
#else
            if (!Vector.IsHardwareAccelerated)
#endif
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

        public static MathVector<T> One(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            var data = new T[length];

            if (typeof(T) == typeof(int))
            {
                for (var i = 0; i < length; i++)
                {
                    data[i] = (T) (ValueType) 1;
                }
            }
            else if (typeof(T) == typeof(float))
            {
                for (var i = 0; i < length; i++)
                {
                    data[i] = (T)(ValueType)1F;
                }
            }
            else
            {
                throw new NotSupportedException();
            }

            return new MathVector<T>(data);
        }

        public static MathVector<T> Zero(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            var data = new T[length];

            return new MathVector<T>(data);
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
#if SIMD
                if (true)
#else
                if (Vector.IsHardwareAccelerated)
#endif

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

            internal set
            {
                if (Vector.IsHardwareAccelerated)
                {
                    var vectorizedCount = Vector<T>.Count * Vectors.Count;
                    if (i >= vectorizedCount)
                    {
                        Tail[i - vectorizedCount] = value;
                    }
                    else
                    {
                        var vectorIndex = (i / Vector<T>.Count);

                        var data = new T[Vector<T>.Count];
                        Vectors[vectorIndex].CopyTo(data);
                        data[i%Vector<T>.Count] = value;
                        Vectors[vectorIndex] = new Vector<T>(data);
                    }
                }
                else
                {
                    Data[i] = value;
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

        public MathVector<T> Abs()
        {
            return VectorOps<T>.GetInstance().Abs(this);
        }

        public T VectorLength()
        {
            return VectorOps<T>.GetInstance().Length(this);
        }

        public T[] ToArray()
        {
#if SIMD
            if (true)
#else
            if (Vector.IsHardwareAccelerated)
#endif
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
                return (T[]) Data.Clone();
            }
        }

        public void ToArray(T[] target)
        {
#if SIMD
            if (true)
#else
            if (Vector.IsHardwareAccelerated)
#endif

            {
                var i = 0;
                for (; i < Vectors.Count; i++)
                {
                    Vectors[i].CopyTo(target, i * Vector<T>.Count);
                }

                if (Tail.Length > 0)
                {
                    Tail.CopyTo(target, i * Vector<T>.Count);
                }
            }
            else
            {
                var size = Marshal.SizeOf<T>();
                Buffer.BlockCopy(Data, 0, target, 0, size*Length);
            }
        }

        internal MathVector<T> Clone()
        {
#if SIMD
            if (true)
#else
            if (Vector.IsHardwareAccelerated)
#endif
            {
                var list = new List<Vector<T>>(Vectors.Count);
                for (var i = 0; i < Vectors.Count; i++)
                {
                    var data = new T[Vector<T>.Count];
                    Vectors[i].CopyTo(data);
                    list.Add(new Vector<T>(data));
                }

                return new MathVector<T>(list, (T[]) Tail.Clone());
            }
            else
            {
                return new MathVector<T>((T[]) Data.Clone());
            }
        }
    }
}
