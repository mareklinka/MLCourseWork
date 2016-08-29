using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace VectorMath
{
    internal abstract class MatrixOps<T> where T : struct
    {
        private static MatrixOpsSerial<T> _serial;
        private static MatrixOpsSimd<T> _simd;

        public static MatrixOps<T> GetInstance()
        {
#if SIMD
            if (true)
#else
            if (Vector.IsHardwareAccelerated)
#endif

            {
                return _simd ?? (_simd = new MatrixOpsSimd<T>());
            }
            else
            {
                return _serial ?? (_serial = new MatrixOpsSerial<T>());
            }
        }

        public abstract MathMatrix<T> Add(MathMatrix<T> left, MathMatrix<T> right);

        public abstract MathMatrix<T> Subtract(MathMatrix<T> left, MathMatrix<T> right);

        public abstract MathMatrix<T> Multiply(MathMatrix<T> left, MathMatrix<T> right);

        public abstract void TransposeInPlace(MathMatrix<T> matrix);

        public abstract MathMatrix<T> Transpose(MathMatrix<T> matrix);

        public abstract T Determinant(MathMatrix<T> matrix);

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

        protected void SwapRows(T[,] source, int firstRow, int secondRow)
        {
            var size = Marshal.SizeOf<T>();
            var rowLength = source.GetLength(1);

            var tempRow = new T[rowLength];

            Buffer.BlockCopy(source, rowLength * size * firstRow, tempRow, 0, rowLength * size);
            Buffer.BlockCopy(source, rowLength * size * secondRow, source, rowLength * size * firstRow, rowLength * size);
            Buffer.BlockCopy(tempRow, 0, source, rowLength * size * secondRow, rowLength * size);
        }
    }
}