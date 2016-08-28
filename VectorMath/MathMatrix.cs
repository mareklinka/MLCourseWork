using System;
using System.Collections.Generic;
using System.Numerics;

namespace VectorMath
{
    public sealed class MathMatrix<T> where T : struct
    {
        private readonly T[,] _data;

        public List<MathVector<T>> Vectors { get; }

        public MathMatrix(T[,] data, MatrixVectorizationType vectorization = MatrixVectorizationType.ByRow)
        {
            if (data == null || data.GetLength(0) == 0 || data.GetLength(1) == 0)
            {
                throw new ArgumentNullException(nameof(data));
            }

            VectorizationMode = vectorization;
            Rows = data.GetLength(0);
            Columns = data.GetLength(1);

            if (Vector.IsHardwareAccelerated)
            {
                switch (vectorization)
                {
                    case MatrixVectorizationType.ByRow:
                        var rows = new List<MathVector<T>>(Rows);
                        var size = System.Runtime.InteropServices.Marshal.SizeOf<T>();
                        for (var row = 0; row < Rows; row++)
                        {
                            var rowData = new T[Columns];
                            
                            Buffer.BlockCopy(data, row * Columns * size, rowData, 0, Columns*size);

                            rows.Add(new MathVector<T>(rowData));
                        }

                        Vectors = rows;
                        break;
                    case MatrixVectorizationType.ByColumn:
                        var columns = new List<MathVector<T>>(Columns);
                        for (var col = 0; col < Columns; col++)
                        {
                            var colData = new T[Rows];
                            for (var row = 0; row < Rows; row++)
                            {
                                colData[row] = data[row, col];
                            }

                            columns.Add(new MathVector<T>(colData));
                        }

                        Vectors = columns;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(vectorization), vectorization, null);
                }
            }
            else
            {
                _data = data;
                
            }
        }

        public MathMatrix(List<MathVector<T>> vectors, MatrixVectorizationType vectorization)
        {
            if (!Vector.IsHardwareAccelerated)
            {
                throw new NotSupportedException();
            }

            VectorizationMode = vectorization;
            switch (vectorization)
            {
                case MatrixVectorizationType.ByRow:
                    Rows = vectors.Count;
                    Columns = vectors[0].Length;
                    break;
                case MatrixVectorizationType.ByColumn:
                    Columns = vectors.Count;
                    Rows = vectors[0].Length;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(vectorization), vectorization, null);
            }
            Vectors = vectors;
        }

        public MatrixVectorizationType VectorizationMode { get; }

        public int Rows { get; }

        public int Columns { get; }

        public T[,] ToArray()
        {
            if (Vector.IsHardwareAccelerated)
            {
                var result = new T[Rows, Columns];

                if (VectorizationMode == MatrixVectorizationType.ByRow)
                {
                    var size = System.Runtime.InteropServices.Marshal.SizeOf<T>();
                    for (var row = 0; row < Vectors.Count; row++)
                    {
                        Buffer.BlockCopy(Vectors[row].ToArray(), 0, result, row * Columns * size, Columns * size);
                    }
                }
                else
                {
                    for (var col = 0; col < Vectors.Count; col++)
                    {
                        var columnData = Vectors[col].ToArray();
                        for (var row = 0; row < Rows; row++)
                        {
                            result[row, col] = columnData[row];
                        }
                    }
                }

                return result;
            }
            else
            {
                return _data;
            }
        }

        public T this[int row, int col]
        {
            get
            {
                if (Vector.IsHardwareAccelerated)
                {
                    switch (VectorizationMode)
                    {
                        case MatrixVectorizationType.ByRow:
                            return Vectors[row][col];
                        case MatrixVectorizationType.ByColumn:
                            return Vectors[col][row];
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else
                {
                    return _data[row, col];
                }
            }
        }

        public MathMatrix<T> Add(MathMatrix<T> right)
        {
            if (right == null) throw new ArgumentNullException(nameof(right));

            if (Rows != right.Rows || Columns != right.Columns)
            {
                throw new InvalidOperationException("Unable to add two matrices of different sizes.");
            }

            return MatrixOps<T>.GetInstance().Add(this, right);
        }

        public MathMatrix<T> Subtract(MathMatrix<T> right)
        {
            if (right == null) throw new ArgumentNullException(nameof(right));

            if (Rows != right.Rows || Columns != right.Columns)
            {
                throw new InvalidOperationException("Unable to subtract two matrices of different sizes.");
            }

            return MatrixOps<T>.GetInstance().Subtract(this, right);
        }

        public MathMatrix<T> Multiply(MathMatrix<T> right)
        {
            if (right == null) throw new ArgumentNullException(nameof(right));

            if (Rows != right.Columns || Columns != right.Rows)
            {
                throw new InvalidOperationException($"Unable to multiply matrix {Rows}x{Columns} by {right.Rows}x{right.Columns}. Incompatible dimensions.");
            }

            return MatrixOps<T>.GetInstance().Multiply(this, right);
        }
    }
}