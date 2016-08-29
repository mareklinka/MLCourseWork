using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;

namespace VectorMath
{
    internal class MatrixOpsSimd<T> : MatrixOps<T> where T : struct 
    {
        public override MathMatrix<T> Add(MathMatrix<T> left, MathMatrix<T> right)
        {
            if (left.VectorizationMode == right.VectorizationMode)
            {
                var vectorCount = left.VectorizationMode == MatrixVectorizationType.ByRow ? left.Rows : left.Columns;
                var result = new List<MathVector<T>>(vectorCount);
                for (var vectorIndex = 0; vectorIndex < vectorCount; vectorIndex++)
                {
                    result.Add(left.Vectors[vectorIndex] + right.Vectors[vectorIndex]);
                }

                return new MathMatrix<T>(result, left.VectorizationMode);
            }
            else
            {
                var leftByRow = left.VectorizationMode == MatrixVectorizationType.ByRow
                    ? left
                    : new MathMatrix<T>(left.ToArray(), MatrixVectorizationType.ByRow);
                var rightByRow = right.VectorizationMode == MatrixVectorizationType.ByRow
                    ? right
                    : new MathMatrix<T>(right.ToArray(), MatrixVectorizationType.ByRow);

                return Add(leftByRow, rightByRow);
            }
        }

        public override MathMatrix<T> Subtract(MathMatrix<T> left, MathMatrix<T> right)
        {
            if (left.VectorizationMode == right.VectorizationMode)
            {
                var vectorCount = left.VectorizationMode == MatrixVectorizationType.ByRow ? left.Rows : left.Columns;
                var result = new List<MathVector<T>>(left.Rows);
                for (var vectorIndex = 0; vectorIndex < vectorCount; vectorIndex++)
                {
                    result.Add(left.Vectors[vectorIndex] - right.Vectors[vectorIndex]);
                }

                return new MathMatrix<T>(result, left.VectorizationMode);
            }
            else
            {
                var leftByRow = left.VectorizationMode == MatrixVectorizationType.ByRow
                    ? left
                    : new MathMatrix<T>(left.ToArray(), MatrixVectorizationType.ByRow);
                var rightByRow = right.VectorizationMode == MatrixVectorizationType.ByRow
                    ? right
                    : new MathMatrix<T>(right.ToArray(), MatrixVectorizationType.ByRow);

                return Subtract(leftByRow, rightByRow);
            }
        }

        public override MathMatrix<T> Multiply(MathMatrix<T> left, MathMatrix<T> right)
        {
            if (left.VectorizationMode == MatrixVectorizationType.ByRow &&
                right.VectorizationMode == MatrixVectorizationType.ByColumn)
            {
                var result = new T[left.Rows, right.Columns];

                for (var row = 0; row < left.Rows; row++)
                {
                    for (var col = 0; col < right.Columns; col++)
                    {
                        result[row, col] = left.Vectors[row].Dot(right.Vectors[col]);
                    }
                }

                return new MathMatrix<T>(result, MatrixVectorizationType.ByColumn);
            }
            else
            {
                var leftByRow = left.VectorizationMode == MatrixVectorizationType.ByRow
                    ? left
                    : new MathMatrix<T>(left.ToArray(), MatrixVectorizationType.ByRow);
                var rightByCol = right.VectorizationMode == MatrixVectorizationType.ByColumn
                    ? right
                    : new MathMatrix<T>(right.ToArray(), MatrixVectorizationType.ByColumn);

                return Multiply(leftByRow, rightByCol);
            }
        }

        public override void TransposeInPlace(MathMatrix<T> matrix)
        {
            matrix.SwapVectorizationMode();
        }

        public override MathMatrix<T> Transpose(MathMatrix<T> matrix)
        {
            var list = new List<MathVector<T>>(matrix.Vectors.Count);

            for (var i = 0; i < matrix.Vectors.Count; i++)
            {
                list.Add(matrix.Vectors[i].Clone());
            }

            var result = new MathMatrix<T>(list, matrix.VectorizationMode);
            result.SwapVectorizationMode();

            return result;
        }

        public override T Determinant(MathMatrix<T> matrix)
        {
            T[,] lower;
            T[,] upper;
            int[] pi;
            int detPi;

            DecomposeLU(matrix, out lower, out upper, out pi, out detPi);

            var det = (float)detPi;
            for (var i = 0; i < matrix.Rows; i++)
            {
                det *= AsFloat(upper[i, i]);
            }

            return AsT(det);
        }

        private void DecomposeLU(MathMatrix<T> matrix, out T[,] lower, out T[,] upper, out int[] pi, out int detPi)
        {
            var size = Marshal.SizeOf<T>();
            lower = MathMatrix<T>.I(matrix.Rows).ToArray();
            upper = matrix.ToArray();

            if (typeof(T) == typeof(float))
            {
                pi = new int[matrix.Rows];

                for (var i = 0; i < matrix.Rows; i++)
                {
                    pi[i] = i;
                }

                var k0 = 0;
                detPi = 1;

                for (var k = 0; k < matrix.Columns - 1; k++)
                {
                    double p = 0;
                   
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
                        Console.WriteLine(p);
                        Console.WriteLine(k);
                        for (int row = 0; row < upper.GetLength(0); row++)
                        {
                            for (int col = 0; col < upper.GetLength(0); col++)
                            {
                                Console.Write(upper[row, col]);
                                Console.Write("|");
                            }

                            Console.WriteLine();
                        }

                        throw new NotSupportedException("The matrix is singular!");
                    }

                    if (k != k0)
                    {
                        detPi *= -1;

                        // switch two rows in permutation matrix
                        var temp = pi[k];
                        pi[k] = pi[k0];
                        pi[k0] = temp;

                        // switch rows in L
                        // optimized - only swapping the first K items, since we are in the LOWER matrix
                        SwapRows(lower, k0, k);
                        SwapRows(upper, k0, k);
                    }

                    for (var i = k + 1; i < matrix.Rows; i++)
                    {
                        // for vectorization see comments below

                       var ik = AsFloat(upper[i, k]) / AsFloat(upper[k, k]);
                        lower[i, k] = AsT(ik);
                        for (var j = k; j < matrix.Columns; j++)
                        {
                            upper[i, j] = AsT(AsFloat(upper[i, j]) - ik * AsFloat(upper[k, j]));
                        }
                    }
                }

                return;
            }

            throw new NotSupportedException();
        }
    }

    // unfortunate downside to the Vector<> struct - it must be initialized from an array
    // the following code performs vectorization of the final computation, but the repetitive copying of 
    // data into temp arrays is a huge overhead (2x slower than standard serial version)
    // the initialization and destruction of Vector<T> is also a pain here, as the init is slow and costs GC in the long run

    //var ik = AsFloat(upper[i, k]) / AsFloat(upper[k, k]);
    //lower[i, k] = AsT(ik);

    //var rowSize = matrix.Columns - k;
    //var ikVector = new MathVector<T>(AsT(ik), rowSize);

    //var upperRowDataI = new T[rowSize];
    //var upperRowDataK = new T[rowSize];

    //Buffer.BlockCopy(upper, size * matrix.Columns * i + k * size, upperRowDataI, 0, size * rowSize);
    //Buffer.BlockCopy(upper, size * matrix.Columns * k + k * size, upperRowDataK, 0, size * rowSize);

    //var upperRowIVector = new MathVector<T>(upperRowDataI);
    //var upperRowKVector = new MathVector<T>(upperRowDataK);
    //var resultData = (upperRowIVector - (ikVector.Multiply(upperRowKVector))).ToArray();

    //Buffer.BlockCopy(resultData, 0, upper, size * matrix.Columns * i + size * k, size * rowSize);
}