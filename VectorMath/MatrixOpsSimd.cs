using System.Collections.Generic;

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
                var result = new List<MathVector<T>>(left.Rows);
                for (var vectorIndex = 0; vectorIndex < left.Rows; vectorIndex++)
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
    }
}