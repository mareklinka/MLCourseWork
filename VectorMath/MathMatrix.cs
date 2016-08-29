using System;
using System.Collections.Generic;
using System.Numerics;

namespace VectorMath
{
    public sealed class MathMatrix<T> where T : struct
    {
        private T[,] _data;

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
#if SIMD
            if (true)
#else
            if (Vector.IsHardwareAccelerated)
#endif
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
#if SIMD
            if (false)
#else
            if (!Vector.IsHardwareAccelerated)
#endif

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

        public MatrixVectorizationType VectorizationMode { get; private set; }

        public int Rows { get; private set; }

        public int Columns { get; private set; }

        public T[,] ToArray()
        {
#if SIMD
            if (true)
#else
            if (Vector.IsHardwareAccelerated)
#endif

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
                return (T[,])_data.Clone();
            }
        }

        public T this[int row, int col]
        {
            get
            {
#if SIMD
                if (true)
#else
                if (Vector.IsHardwareAccelerated)
#endif


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

            internal set
            {
                if (Vector.IsHardwareAccelerated)
                {
                    switch (VectorizationMode)
                    {
                        case MatrixVectorizationType.ByRow:
                            Vectors[row][col] = value;
                            break;
                        case MatrixVectorizationType.ByColumn:
                            Vectors[col][row] = value;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else
                {
                    _data[row, col] = value;
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

        public MathMatrix<T> TransposeInPlace()
        {
            MatrixOps<T>.GetInstance().TransposeInPlace(this);
            return this;
        }

        public MathMatrix<T> Transpose()
        {
            return MatrixOps<T>.GetInstance().Transpose(this);
        }

        public T Determinant()
        {
            if (Rows != Columns)
            {
                throw new NotSupportedException();
            }

            return MatrixOps<T>.GetInstance().Determinant(this);
        }

        public static MathMatrix<T> Zero(int rows, int columns)
        {
            var data = new T[rows, columns];

            return new MathMatrix<T>(data);
        }

        public static MathMatrix<T> One(int rows, int columns)
        {
            var data = new T[rows, columns];

            if (typeof(T) == typeof(int))
            {
                for (var row = 0; row < rows; row++)
                {
                    for (var col = 0; col < columns; col++)
                    {
                        data[row, col] = (T)(ValueType)1;
                    }
                    
                }
            }
            else if (typeof(T) == typeof(float))
            {
                for (var row = 0; row < rows; row++)
                {
                    for (var col = 0; col < columns; col++)
                    {
                        data[row, col] = (T)(ValueType)1F;
                    }
                }
            }
            else
            {
                throw new NotSupportedException();
            }

            return new MathMatrix<T>(data);
        }

        public static MathMatrix<T> I(int dimension)
        {
            var data = new T[dimension, dimension];

            if (typeof(T) == typeof(int))
            {
                for (var i = 0; i < dimension; i++)
                {
                    data[i, i] = (T)(ValueType)1;
                }
            }
            else if (typeof(T) == typeof(float))
            {
                for (var i = 0; i < dimension; i++)
                {
                    data[i, i] = (T)(ValueType)1F;
                }
            }
            else if (typeof(T) == typeof(double))
            {
                for (var i = 0; i < dimension; i++)
                {
                    data[i, i] = (T)(ValueType)1D;
                }
            }
            else
            {
                throw new NotSupportedException();
            }

            return new MathMatrix<T>(data);
        }

        internal void Update(T[,] data)
        {
            if (Vector.IsHardwareAccelerated)
            {
                throw new NotSupportedException();
            }

            _data = data;
            Rows = data.GetLength(0);
            Columns = data.GetLength(1);
        }

        internal void SwapVectorizationMode()
        {
#if SIMD
            if (false)
#else
            if (!Vector.IsHardwareAccelerated)
#endif

            {
                throw new NotSupportedException();
            }

            switch (VectorizationMode)
            {
                case MatrixVectorizationType.ByRow:
                    VectorizationMode = MatrixVectorizationType.ByColumn;
                    break;
                case MatrixVectorizationType.ByColumn:
                    VectorizationMode = MatrixVectorizationType.ByRow;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var temp = Rows;
            Rows = Columns;
            Columns = temp;
        }

        internal MathMatrix<T> Clone()
        {
            if (Vector.IsHardwareAccelerated)
            {
                var list = new List<MathVector<T>>(Vectors.Count);
                for (var i = 0; i < Vectors.Count; i++)
                {
                    list.Add(Vectors[i].Clone());
                }

                return new MathMatrix<T>(list, VectorizationMode);
            }
            else
            {
                return new MathMatrix<T>((T[,]) _data.Clone());
            }
        }
    }
}