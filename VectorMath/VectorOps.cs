using System.Numerics;

namespace VectorMath
{
    internal abstract class VectorOps<T> where T : struct
    {
        private static VectorOpsSerial<T> _serial;
        private static VectorOpsSimd<T> _simd;

        public static VectorOps<T> GetInstance()
        {
            if (Vector.IsHardwareAccelerated)
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

        public abstract MathVector<T> MultiplyScalar(MathVector<T> vector, T scalar);

        public abstract MathVector<T> Divide(MathVector<T> vector, T scalar);

        public MathVector<T> Normalize(MathVector<T> mathVector)
        {
            return Divide(mathVector, Length(mathVector));
        }

        public abstract T Length(MathVector<T> mathVector);
    }
}