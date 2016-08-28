using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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

        public override MathMatrix<T> Multiply(MathMatrix<T> left, MathMatrix<T> right)
        {
            var data = new T[left.Rows, right.Columns];

            if (typeof(T) == typeof(int))
            {
                for (var row = 0; row < left.Rows; row++)
                {
                    for (var col = 0; col < right.Columns; col++)
                    {
                        for (var pointer = 0; pointer < left.Columns; pointer++)
                        {
                            data[row, col] =
                                (T)
                                (ValueType)
                                ((int) (ValueType) data[row, col] +
                                 (((int) (ValueType) left[row, pointer]*(int) (ValueType) right[pointer, col])));
                        }   
                    }
                }
            }
            else if (typeof(T) == typeof(float))
            {
                for (var row = 0; row < left.Rows; row++)
                {
                    for (var col = 0; col < right.Columns; col++)
                    {
                        for (var pointer = 0; pointer < left.Columns; pointer++)
                        {
                            data[row, col] =
                                (T)
                                (ValueType)
                                ((float) (ValueType) data[row, col] +
                                 (((float) (ValueType) left[row, pointer]*(float) (ValueType) right[pointer, col])));
                        }
                    }
                }
            }
            else
            {
                throw new NotSupportedException();
            }

            return new MathMatrix<T>(data, MatrixVectorizationType.ByRow);
        }

        public override void TransposeInPlace(MathMatrix<T> matrix)
        {
            var data = TransposeCore(matrix);

            matrix.Update(data);
        }

        private static T[,] TransposeCore(MathMatrix<T> matrix)
        {
            var data = new T[matrix.Columns, matrix.Rows];
            for (var row = 0; row < matrix.Rows; row++)
            {
                for (var col = 0; col < matrix.Columns; col++)
                {
                    data[col, row] = matrix[row, col];
                }
            }
            return data;
        }

        public override MathMatrix<T> Transpose(MathMatrix<T> matrix)
        {
            return new MathMatrix<T>(TransposeCore(matrix));
        }
    }
}