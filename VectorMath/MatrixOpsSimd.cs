using System;
using System.Collections.Generic;
using System.Numerics;

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
    }
}   