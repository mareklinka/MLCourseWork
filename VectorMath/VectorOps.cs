using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace VectorMath
{
    internal abstract class VectorOps<T> where T : struct
    {
        private static VectorOpsSerial<T> _serial;
        private static VectorOpsSimd<T> _simd;

        public static VectorOps<T> GetInstance()
        {
#if SIMD
            if (true)
#else
            if (Vector.IsHardwareAccelerated)
#endif

            {
                return _simd ?? (_simd = new VectorOpsSimd<T>());
            }
            else
            {
                return _serial ?? (_serial = new VectorOpsSerial<T>());
            }
        }

        public abstract MathVector<T> Add(MathVector<T> left, MathVector<T> right);

        public abstract MathVector<T> Subtract(MathVector<T> left, MathVector<T> right);

        public abstract T Dot(MathVector<T> left, MathVector<T> right);

        public abstract MathVector<T> Multiply(MathVector<T> left, MathVector<T> right);

        public abstract MathVector<T> Multiply(MathVector<T> left, MathMatrix<T> right);

        public abstract MathVector<T> MultiplyScalar(MathVector<T> vector, T scalar);

        public abstract MathVector<T> Divide(MathVector<T> vector, T scalar);

        public MathVector<T> Normalize(MathVector<T> mathVector)
        {
            return Divide(mathVector, Length(mathVector));
        }

        public abstract T Length(MathVector<T> mathVector);

        public abstract MathVector<T> Abs(MathVector<T> mathVector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected int AsInt(T value)
        {
            return (int)(ValueType)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected float AsFloat(T value)
        {
            return (float)(ValueType)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected double AsDouble(T value)
        {
            return (double)(ValueType)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected T AsT(int value)
        {
            return (T)(ValueType)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected T AsT(float value)
        {
            return (T)(ValueType)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected T AsT(double value)
        {
            return (T)(ValueType)value;
        }
    }
}