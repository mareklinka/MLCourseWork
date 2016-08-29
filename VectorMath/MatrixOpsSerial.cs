using System;
using System.Numerics;

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

        public override T Determinant(MathMatrix<T> matrix)
        {
            T[,] lower;
            T[,] upper;
            int[] pi;
            int detPi;

            DecomposeLU(matrix, out lower, out upper, out pi, out detPi);

            for (int row = 0; row < upper.GetLength(0); row++)
            {
                for (int col = 0; col < upper.GetLength(0); col++)
                {
                    Console.Write(upper[row, col]);
                    Console.Write("|");
                }

                Console.WriteLine();
            }

            var det = (float)detPi;
            for (var i = 0; i < matrix.Rows; i++)
            {
                det *= AsFloat(upper[i, i]);
            }

            return AsT(det);
        }

        private void DecomposeLU(MathMatrix<T> matrix, out T[,] lower, out T[,] upper, out int[] pi, out int detPi)
        {
            lower = MathMatrix<T>.I(matrix.Rows).ToArray();
            upper = matrix.ToArray();

            if (typeof(T) == typeof(float))
            {
                pi = new int[matrix.Rows];

                for (var i = 0; i < matrix.Rows; i++)
                {
                    pi[i] = i;
                }

                double p;
                var k0 = 0;
                int temp;
                detPi = 1;

                for (var k = 0; k < matrix.Columns - 1; k++)
                {
                    p = 0;

                    // find the row with the biggest pivot
                    for (var i = k; i < matrix.Rows; i++)      
                    {
                        var abs = Math.Abs(AsFloat(upper[i, k]));
                        if (abs > p)
                        {
                            p = abs;
                            k0 = i;
                        }
                    }

                    if (p == 0)
                    {
                        throw new NotSupportedException("The matrix is singular!");
                    }

                    if (k != k0)
                    {
                        detPi *= -1;

                        // switch two rows in permutation matrix
                        temp = pi[k];
                        pi[k] = pi[k0];
                        pi[k0] = temp;

                        SwapRows(lower, k, k0);
                        SwapRows(upper, k, k0);
                    }

                    for (var i = k + 1; i < matrix.Rows; i++)
                    {
                        var ik = AsFloat(upper[i, k]) / AsFloat(upper[k, k]);
                        lower[i, k] = AsT(ik);
                        for (var j = k; j < matrix.Columns; j++)
                        {
                            upper[i, j] = AsT(AsFloat(upper[i, j]) - ik*AsFloat(upper[k, j]));
                        }
                    }
                }
                
                return;
            }

            throw new NotSupportedException();
        }
    }
}