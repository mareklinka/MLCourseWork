using System.Numerics;

namespace VectorMath
{
    internal abstract class MatrixOps<T> where T : struct
    {
        private static MatrixOpsSerial<T> _serial;
        private static MatrixOpsSimd<T> _simd;

        public static MatrixOps<T> GetInstance()
        {
            if (Vector.IsHardwareAccelerated)
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
    }
}