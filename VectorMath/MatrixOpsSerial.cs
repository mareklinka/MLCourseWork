using System;

namespace VectorMath
{
    internal class MatrixOpsSerial<T> : MatrixOps<T> where T : struct 
    {
        public override MathMatrix<T> Add(MathMatrix<T> left, MathMatrix<T> right)
        {
            var data = new T[left.Rows, left.Columns];

            if (typeof(T) == typeof(int))
            {
                for (var row = 0; row < left.Rows; row++)
                {
                    for (var col = 0; col < left.Columns; col++)
                    {
                        data[row, col] =
                            (T) (ValueType) ((int) (ValueType) left[row, col] + (int) (ValueType) right[row, col]);
                    }
                }
            }
            else if (typeof(T) == typeof(float))
            {
                for (var row = 0; row < left.Rows; row++)
                {
                    for (var col = 0; col < left.Columns; col++)
                    {
                        data[row, col] =
                            (T)(ValueType)((float)(ValueType)left[row, col] + (float)(ValueType)right[row, col]);
                    }
                }
            }
            else
            {
                throw new NotSupportedException();
            }

            return new MathMatrix<T>(data, MatrixVectorizationType.ByRow);
        }

        public override MathMatrix<T> Subtract(MathMatrix<T> left, MathMatrix<T> right)
        {
            var data = new T[left.Rows, left.Columns];

            if (typeof(T) == typeof(int))
            {
                for (var row = 0; row < left.Rows; row++)
                {
                    for (var col = 0; col < left.Columns; col++)
                    {
                        data[row, col] =
                            (T)(ValueType)((int)(ValueType)left[row, col] - (int)(ValueType)right[row, col]);
                    }
                }
            }
            else if (typeof(T) == typeof(float))
            {
                for (var row = 0; row < left.Rows; row++)
                {
                    for (var col = 0; col < left.Columns; col++)
                    {
                        data[row, col] =
                            (T)(ValueType)((float)(ValueType)left[row, col] - (float)(ValueType)right[row, col]);
                    }
                }
            }
            else
            {
                throw new NotSupportedException();
            }

            return new MathMatrix<T>(data, MatrixVectorizationType.ByRow);
        }
    }
}