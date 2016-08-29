using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VectorMath.Tests
{
    [TestClass]
    public class MathMatrixTests
    {
        private const float FloatEpsilon = 0.001F;

        [TestInitialize]
        public void Initialize()
        {
            Console.WriteLine($"Hardware acceleration: {Vector.IsHardwareAccelerated}");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullTest()
        {
            new MathMatrix<int>(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NoRowsTest()
        {
            new MathMatrix<int>(new int[0,1]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NoColsTest()
        {
            new MathMatrix<int>(new int[1, 0]);
        }

        [TestMethod]
        public void InitializeTest_ByRow()
        {
            var data = new [,] {{1, 2, 3}, {4, 5, 6}};
            var mathMatrix = new MathMatrix<int>(data, MatrixVectorizationType.ByRow);

            Assert.AreEqual(data.GetLength(0), mathMatrix.Rows);
            Assert.AreEqual(data.GetLength(1), mathMatrix.Columns);
        }

        [TestMethod]
        public void InitializeTest_ByColumn()
        {
            var data = new[,] { { 1, 2, 3 }, { 4, 5, 6 } };
            var mathMatrix = new MathMatrix<int>(data, MatrixVectorizationType.ByColumn);

            Assert.AreEqual(data.GetLength(0), mathMatrix.Rows);
            Assert.AreEqual(data.GetLength(1), mathMatrix.Columns);
        }

        [TestMethod]
        public void ToArray_ByRow()
        {
            var data = new[,] { { 1, 2, 3 }, { 4, 5, 6 } };
            var mathMatrix = new MathMatrix<int>(data, MatrixVectorizationType.ByRow);

            var result = mathMatrix.ToArray();

            for (var row = 0; row < data.GetLength(0); row++)
            {
                for (var col = 0; col < data.GetLength(1); col++)
                {
                    Assert.AreEqual(data[row, col], result[row, col]);
                }
            }
        }

        [TestMethod]
        public void ToArray_ByColumn()
        {
            var data = new[,] { { 1, 2, 3 }, { 4, 5, 6 } };
            var mathMatrix = new MathMatrix<int>(data, MatrixVectorizationType.ByColumn);

            var result = mathMatrix.ToArray();

            for (var row = 0; row < data.GetLength(0); row++)
            {
                for (var col = 0; col < data.GetLength(1); col++)
                {
                    Assert.AreEqual(data[row, col], result[row, col]);
                }
            }
        }

        [TestMethod]
        public void Indexer_ByRow()
        {
            var data = new[,] { { 1, 2, 3 }, { 4, 5, 6 } };
            var mathMatrix = new MathMatrix<int>(data, MatrixVectorizationType.ByRow);

            for (var row = 0; row < data.GetLength(0); row++)
            {
                for (var col = 0; col < data.GetLength(1); col++)
                {
                    Assert.AreEqual(data[row, col], mathMatrix[row, col]);
                }
            }
        }

        [TestMethod]
        public void Indexer_ByColumn()
        {
            var data = new[,] { { 1, 2, 3 }, { 4, 5, 6 } };
            var mathMatrix = new MathMatrix<int>(data, MatrixVectorizationType.ByColumn);

            for (var row = 0; row < data.GetLength(0); row++)
            {
                for (var col = 0; col < data.GetLength(1); col++)
                {
                    Assert.AreEqual(data[row, col], mathMatrix[row, col]);
                }
            }
        }

        [TestMethod]
        public void Add_Int_BothByRow()
        {
            var data1 = PrepareMatrixInt(10, 6);
            var data2 = PrepareMatrixInt(10, 6);
            var mathMatrix1 = new MathMatrix<int>(data1, MatrixVectorizationType.ByRow);
            var mathMatrix2 = new MathMatrix<int>(data2, MatrixVectorizationType.ByRow);

            var result = mathMatrix1.Add(mathMatrix2).ToArray();

            for (var row = 0; row < result.GetLength(0); row++)
            {
                for (var col = 0; col < result.GetLength(1); col++)
                {
                    Assert.AreEqual(data1[row, col] + data2[row, col], result[row, col]);
                }
            }
        }

        [TestMethod]
        public void Add_Int_BothByCol()
        {
            var data1 = PrepareMatrixInt(10, 6);
            var data2 = PrepareMatrixInt(10, 6);
            var mathMatrix1 = new MathMatrix<int>(data1, MatrixVectorizationType.ByColumn);
            var mathMatrix2 = new MathMatrix<int>(data2, MatrixVectorizationType.ByColumn);

            var result = mathMatrix1.Add(mathMatrix2).ToArray();

            for (var row = 0; row < result.GetLength(0); row++)
            {
                for (var col = 0; col < result.GetLength(1); col++)
                {
                    Assert.AreEqual(data1[row, col] + data2[row, col], result[row, col]);
                }
            }
        }

        [TestMethod]
        public void Add_Int_DifferentVect()
        {
            var data1 = PrepareMatrixInt(10, 6);
            var data2 = PrepareMatrixInt(10, 6);
            var mathMatrix1 = new MathMatrix<int>(data1, MatrixVectorizationType.ByRow);
            var mathMatrix2 = new MathMatrix<int>(data2, MatrixVectorizationType.ByColumn);

            var m = mathMatrix1.Add(mathMatrix2);
            var result = m.ToArray();

            Assert.AreEqual(MatrixVectorizationType.ByRow, m.VectorizationMode);

            for (var row = 0; row < result.GetLength(0); row++)
            {
                for (var col = 0; col < result.GetLength(1); col++)
                {
                    Assert.AreEqual(data1[row, col] + data2[row, col], result[row, col]);
                }
            }
        }

        [TestMethod]
        public void Add_Float_BothByRow()
        {
            var data1 = PrepareMatrixFloat(10, 6);
            var data2 = PrepareMatrixFloat(10, 6);
            var mathMatrix1 = new MathMatrix<float>(data1, MatrixVectorizationType.ByRow);
            var mathMatrix2 = new MathMatrix<float>(data2, MatrixVectorizationType.ByRow);

            var result = mathMatrix1.Add(mathMatrix2).ToArray();

            for (var row = 0; row < result.GetLength(0); row++)
            {
                for (var col = 0; col < result.GetLength(1); col++)
                {
                    Assert.AreEqual(data1[row, col] + data2[row, col], result[row, col]);
                }
            }
        }

        [TestMethod]
        public void Add_Float_BothByCol()
        {
            var data1 = PrepareMatrixFloat(10, 6);
            var data2 = PrepareMatrixFloat(10, 6);
            var mathMatrix1 = new MathMatrix<float>(data1, MatrixVectorizationType.ByColumn);
            var mathMatrix2 = new MathMatrix<float>(data2, MatrixVectorizationType.ByColumn);

            var result = mathMatrix1.Add(mathMatrix2).ToArray();

            for (var row = 0; row < result.GetLength(0); row++)
            {
                for (var col = 0; col < result.GetLength(1); col++)
                {
                    Assert.AreEqual(data1[row, col] + data2[row, col], result[row, col]);
                }
            }
        }

        [TestMethod]
        public void Add_Float_DifferentVect()
        {
            var data1 = PrepareMatrixFloat(10, 6);
            var data2 = PrepareMatrixFloat(10, 6);
            var mathMatrix1 = new MathMatrix<float>(data1, MatrixVectorizationType.ByRow);
            var mathMatrix2 = new MathMatrix<float>(data2, MatrixVectorizationType.ByColumn);

            var m = mathMatrix1.Add(mathMatrix2);
            var result = m.ToArray();

            Assert.AreEqual(MatrixVectorizationType.ByRow, m.VectorizationMode);

            for (var row = 0; row < result.GetLength(0); row++)
            {
                for (var col = 0; col < result.GetLength(1); col++)
                {
                    Assert.AreEqual(data1[row, col] + data2[row, col], result[row, col]);
                }
            }
        }

        [TestMethod]
        public void Subtract_Int_BothByRow()
        {
            var data1 = PrepareMatrixInt(10, 6);
            var data2 = PrepareMatrixInt(10, 6);
            var mathMatrix1 = new MathMatrix<int>(data1, MatrixVectorizationType.ByRow);
            var mathMatrix2 = new MathMatrix<int>(data2, MatrixVectorizationType.ByRow);

            var result = mathMatrix1.Subtract(mathMatrix2).ToArray();

            for (var row = 0; row < result.GetLength(0); row++)
            {
                for (var col = 0; col < result.GetLength(1); col++)
                {
                    Assert.AreEqual(data1[row, col] - data2[row, col], result[row, col]);
                }
            }
        }

        [TestMethod]
        public void Subtract_Int_BothByCol()
        {
            var data1 = PrepareMatrixInt(10, 6);
            var data2 = PrepareMatrixInt(10, 6);
            var mathMatrix1 = new MathMatrix<int>(data1, MatrixVectorizationType.ByColumn);
            var mathMatrix2 = new MathMatrix<int>(data2, MatrixVectorizationType.ByColumn);

            var result = mathMatrix1.Subtract(mathMatrix2).ToArray();

            for (var row = 0; row < result.GetLength(0); row++)
            {
                for (var col = 0; col < result.GetLength(1); col++)
                {
                    Assert.AreEqual(data1[row, col] - data2[row, col], result[row, col]);
                }
            }
        }

        [TestMethod]
        public void Subtract_Int_DifferentVect()
        {
            var data1 = PrepareMatrixInt(10, 6);
            var data2 = PrepareMatrixInt(10, 6);
            var mathMatrix1 = new MathMatrix<int>(data1, MatrixVectorizationType.ByRow);
            var mathMatrix2 = new MathMatrix<int>(data2, MatrixVectorizationType.ByColumn);

            var m = mathMatrix1.Subtract(mathMatrix2);
            var result = m.ToArray();

            Assert.AreEqual(MatrixVectorizationType.ByRow, m.VectorizationMode);

            for (var row = 0; row < result.GetLength(0); row++)
            {
                for (var col = 0; col < result.GetLength(1); col++)
                {
                    Assert.AreEqual(data1[row, col] - data2[row, col], result[row, col]);
                }
            }
        }

        [TestMethod]
        public void Subtract_Float_BothByRow()
        {
            var data1 = PrepareMatrixFloat(10, 6);
            var data2 = PrepareMatrixFloat(10, 6);
            var mathMatrix1 = new MathMatrix<float>(data1, MatrixVectorizationType.ByRow);
            var mathMatrix2 = new MathMatrix<float>(data2, MatrixVectorizationType.ByRow);

            var result = mathMatrix1.Subtract(mathMatrix2).ToArray();

            for (var row = 0; row < result.GetLength(0); row++)
            {
                for (var col = 0; col < result.GetLength(1); col++)
                {
                    Assert.AreEqual(data1[row, col] - data2[row, col], result[row, col]);
                }
            }
        }

        [TestMethod]
        public void Subtract_Float_BothByCol()
        {
            var data1 = PrepareMatrixFloat(10, 6);
            var data2 = PrepareMatrixFloat(10, 6);
            var mathMatrix1 = new MathMatrix<float>(data1, MatrixVectorizationType.ByColumn);
            var mathMatrix2 = new MathMatrix<float>(data2, MatrixVectorizationType.ByColumn);

            var result = mathMatrix1.Subtract(mathMatrix2).ToArray();

            for (var row = 0; row < result.GetLength(0); row++)
            {
                for (var col = 0; col < result.GetLength(1); col++)
                {
                    Assert.AreEqual(data1[row, col] - data2[row, col], result[row, col]);
                }
            }
        }

        [TestMethod]
        public void Subtract_Float_DifferentVect()
        {
            var data1 = PrepareMatrixFloat(10, 6);
            var data2 = PrepareMatrixFloat(10, 6);
            var mathMatrix1 = new MathMatrix<float>(data1, MatrixVectorizationType.ByRow);
            var mathMatrix2 = new MathMatrix<float>(data2, MatrixVectorizationType.ByColumn);

            var m = mathMatrix1.Subtract(mathMatrix2);
            var result = m.ToArray();

            Assert.AreEqual(MatrixVectorizationType.ByRow, m.VectorizationMode);

            for (var row = 0; row < result.GetLength(0); row++)
            {
                for (var col = 0; col < result.GetLength(1); col++)
                {
                    Assert.AreEqual(data1[row, col] - data2[row, col], result[row, col]);
                }
            }
        }

        [TestMethod]
        public void Multiply_Int_BothByRow()
        {
            var data1 = PrepareMatrixInt(10, 6);
            var data2 = PrepareMatrixInt(6, 10);
            var mathMatrix1 = new MathMatrix<int>(data1, MatrixVectorizationType.ByRow);
            var mathMatrix2 = new MathMatrix<int>(data2, MatrixVectorizationType.ByRow);

            var result = mathMatrix1.Multiply(mathMatrix2).ToArray();

            for (var row = 0; row < result.GetLength(0); row++)
            {
                for (var col = 0; col < result.GetLength(1); col++)
                {
                    var rowData = new int[data1.GetLength(1)];
                    Buffer.BlockCopy(data1, sizeof(int)*row*data1.GetLength(1), rowData, 0, sizeof(int)*data1.GetLength(1));
                    var colData = new int[data2.GetLength(0)];
                    for (var i = 0; i < data2.GetLength(0); i++)
                    {
                        colData[i] = data2[i, col];
                    }

                    var sumProduct = rowData.Zip(colData, (x, y) => new {x, y}).Sum(_ => _.x*_.y);

                    Assert.AreEqual(sumProduct, result[row, col], $"Assertion failure at {row}x{col}");
                }
            }
        }

        [TestMethod]
        public void Multiply_Int_BothByCol()
        {
            var data1 = PrepareMatrixInt(10, 6);
            var data2 = PrepareMatrixInt(6, 10);
            var mathMatrix1 = new MathMatrix<int>(data1, MatrixVectorizationType.ByColumn);
            var mathMatrix2 = new MathMatrix<int>(data2, MatrixVectorizationType.ByColumn);

            var result = mathMatrix1.Multiply(mathMatrix2).ToArray();

            for (var row = 0; row < result.GetLength(0); row++)
            {
                for (var col = 0; col < result.GetLength(1); col++)
                {
                    var rowData = new int[data1.GetLength(1)];
                    Buffer.BlockCopy(data1, sizeof(int) * row * data1.GetLength(1), rowData, 0, sizeof(int) * data1.GetLength(1));
                    var colData = new int[data2.GetLength(0)];
                    for (var i = 0; i < data2.GetLength(0); i++)
                    {
                        colData[i] = data2[i, col];
                    }

                    var sumProduct = rowData.Zip(colData, (x, y) => new { x, y }).Sum(_ => _.x * _.y);

                    Assert.AreEqual(sumProduct, result[row, col], $"Assertion failure at {row}x{col}");
                }
            }
        }

        [TestMethod]
        public void Multiply_Int_DifferentVect()
        {
            var data1 = PrepareMatrixInt(10, 6);
            var data2 = PrepareMatrixInt(6, 10);
            var mathMatrix1 = new MathMatrix<int>(data1, MatrixVectorizationType.ByRow);
            var mathMatrix2 = new MathMatrix<int>(data2, MatrixVectorizationType.ByColumn);

            var result = mathMatrix1.Multiply(mathMatrix2).ToArray();

            for (var row = 0; row < result.GetLength(0); row++)
            {
                for (var col = 0; col < result.GetLength(1); col++)
                {
                    var rowData = new int[data1.GetLength(1)];
                    Buffer.BlockCopy(data1, sizeof(int) * row * data1.GetLength(1), rowData, 0, sizeof(int) * data1.GetLength(1));
                    var colData = new int[data2.GetLength(0)];
                    for (var i = 0; i < data2.GetLength(0); i++)
                    {
                        colData[i] = data2[i, col];
                    }

                    var sumProduct = rowData.Zip(colData, (x, y) => new { x, y }).Sum(_ => _.x * _.y);

                    Assert.AreEqual(sumProduct, result[row, col], $"Assertion failure at {row}x{col}");
                }
            }
        }

        [TestMethod]
        public void Multiply_Int_DifferentVect2()
        {
            var data1 = PrepareMatrixInt(10, 6);
            var data2 = PrepareMatrixInt(6, 10);
            var mathMatrix1 = new MathMatrix<int>(data1, MatrixVectorizationType.ByRow);
            var mathMatrix2 = new MathMatrix<int>(data2, MatrixVectorizationType.ByColumn);

            var result = mathMatrix1.Multiply(mathMatrix2).ToArray();

            for (var row = 0; row < result.GetLength(0); row++)
            {
                for (var col = 0; col < result.GetLength(1); col++)
                {
                    var rowData = new int[data1.GetLength(1)];
                    Buffer.BlockCopy(data1, sizeof(int) * row * data1.GetLength(1), rowData, 0, sizeof(int) * data1.GetLength(1));
                    var colData = new int[data2.GetLength(0)];
                    for (var i = 0; i < data2.GetLength(0); i++)
                    {
                        colData[i] = data2[i, col];
                    }

                    var sumProduct = rowData.Zip(colData, (x, y) => new { x, y }).Sum(_ => _.x * _.y);

                    Assert.AreEqual(sumProduct, result[row, col], $"Assertion failure at {row}x{col}");
                }
            }
        }

        [TestMethod]
        public void Multiply_Float_BothByRow()
        {
            var data1 = PrepareMatrixFloat(4, 3, 50);
            var data2 = PrepareMatrixFloat(3, 4, 50);
            var mathMatrix1 = new MathMatrix<float>(data1, MatrixVectorizationType.ByRow);
            var mathMatrix2 = new MathMatrix<float>(data2, MatrixVectorizationType.ByRow);

            var result = mathMatrix1.Multiply(mathMatrix2).ToArray();

            for (var row = 0; row < result.GetLength(0); row++)
            {
                for (var col = 0; col < result.GetLength(1); col++)
                {
                    var rowData = new float[data1.GetLength(1)];
                    Buffer.BlockCopy(data1, sizeof(float) * row * data1.GetLength(1), rowData, 0, sizeof(float) * data1.GetLength(1));
                    var colData = new float[data2.GetLength(0)];
                    for (var i = 0; i < data2.GetLength(0); i++)
                    {
                        colData[i] = data2[i, col];
                    }

                    var sumProduct = rowData.Zip(colData, (x, y) => new { x, y }).Sum(_ => _.x * _.y);

                    Assert.AreEqual(sumProduct, result[row, col], FloatEpsilon, $"Assertion failure at {row}x{col}");
                }
            }
        }

        [TestMethod]
        public void Multiply_Float_BothByCol()
        {
            var data1 = PrepareMatrixFloat(4, 3, 50);
            var data2 = PrepareMatrixFloat(3, 4, 50);
            var mathMatrix1 = new MathMatrix<float>(data1, MatrixVectorizationType.ByColumn);
            var mathMatrix2 = new MathMatrix<float>(data2, MatrixVectorizationType.ByColumn);

            var result = mathMatrix1.Multiply(mathMatrix2).ToArray();

            for (var row = 0; row < result.GetLength(0); row++)
            {
                for (var col = 0; col < result.GetLength(1); col++)
                {
                    var rowData = new float[data1.GetLength(1)];
                    Buffer.BlockCopy(data1, sizeof(float) * row * data1.GetLength(1), rowData, 0, sizeof(float) * data1.GetLength(1));
                    var colData = new float[data2.GetLength(0)];
                    for (var i = 0; i < data2.GetLength(0); i++)
                    {
                        colData[i] = data2[i, col];
                    }

                    var sumProduct = rowData.Zip(colData, (x, y) => new { x, y }).Sum(_ => _.x * _.y);

                    Assert.AreEqual(sumProduct, result[row, col], FloatEpsilon, $"Assertion failure at {row}x{col}");
                }
            }
        }

        [TestMethod]
        public void Multiply_Float_DifferentVect()
        {
            var data1 = PrepareMatrixFloat(4, 3, 50);
            var data2 = PrepareMatrixFloat(3, 4, 50);
            var mathMatrix1 = new MathMatrix<float>(data1, MatrixVectorizationType.ByRow);
            var mathMatrix2 = new MathMatrix<float>(data2, MatrixVectorizationType.ByColumn);

            var result = mathMatrix1.Multiply(mathMatrix2).ToArray();

            for (var row = 0; row < result.GetLength(0); row++)
            {
                for (var col = 0; col < result.GetLength(1); col++)
                {
                    var rowData = new float[data1.GetLength(1)];
                    Buffer.BlockCopy(data1, sizeof(float) * row * data1.GetLength(1), rowData, 0, sizeof(float) * data1.GetLength(1));
                    var colData = new float[data2.GetLength(0)];
                    for (var i = 0; i < data2.GetLength(0); i++)
                    {
                        colData[i] = data2[i, col];
                    }

                    var sumProduct = rowData.Zip(colData, (x, y) => new { x, y }).Sum(_ => _.x * _.y);

                    Assert.AreEqual(sumProduct, result[row, col], FloatEpsilon, $"Assertion failure at {row}x{col}");
                }
            }
        }

        [TestMethod]
        public void Multiply_Float_DifferentVect2()
        {
            var data1 = PrepareMatrixFloat(4, 3, 50);
            var data2 = PrepareMatrixFloat(3, 4, 50);
            var mathMatrix1 = new MathMatrix<float>(data1, MatrixVectorizationType.ByColumn);
            var mathMatrix2 = new MathMatrix<float>(data2, MatrixVectorizationType.ByRow);

            var result = mathMatrix1.Multiply(mathMatrix2).ToArray();

            for (var row = 0; row < result.GetLength(0); row++)
            {
                for (var col = 0; col < result.GetLength(1); col++)
                {
                    var rowData = new float[data1.GetLength(1)];
                    Buffer.BlockCopy(data1, sizeof(float) * row * data1.GetLength(1), rowData, 0, sizeof(float) * data1.GetLength(1));
                    var colData = new float[data2.GetLength(0)];
                    for (var i = 0; i < data2.GetLength(0); i++)
                    {
                        colData[i] = data2[i, col];
                    }

                    var sumProduct = rowData.Zip(colData, (x, y) => new { x, y }).Sum(_ => _.x * _.y);

                    Assert.AreEqual(sumProduct, result[row, col], FloatEpsilon, $"Assertion failure at {row}x{col}");
                }
            }
        }

        [TestMethod]
        public void TransposeInPlace_Int()
        {
            var rows = 10;
            var columns = 6;
            var data1 = PrepareMatrixInt(rows, columns);
            var mathMatrix1 = new MathMatrix<int>(data1, MatrixVectorizationType.ByRow);

            var result = mathMatrix1.TransposeInPlace().ToArray();

            Assert.AreEqual(rows, result.GetLength(1));
            Assert.AreEqual(columns, result.GetLength(0));

            for (var row = 0; row < result.GetLength(0); row++)
            {
                for (var col = 0; col < result.GetLength(1); col++)
                {
                    Assert.AreEqual(data1[col, row], result[row, col]);
                }
            }
        }

        [TestMethod]
        public void TransposeInPlace_Float()
        {
            var rows = 10;
            var columns = 6;
            var data1 = PrepareMatrixFloat(rows, columns);
            var mathMatrix1 = new MathMatrix<float>(data1, MatrixVectorizationType.ByRow);

            var result = mathMatrix1.TransposeInPlace().ToArray();

            Assert.AreEqual(rows, result.GetLength(1));
            Assert.AreEqual(columns, result.GetLength(0));

            for (var row = 0; row < result.GetLength(0); row++)
            {
                for (var col = 0; col < result.GetLength(1); col++)
                {
                    Assert.AreEqual(data1[col, row], result[row, col]);
                }
            }
        }

        [TestMethod]
        public void Transpose_Int()
        {
            var rows = 10;
            var columns = 6;
            var data1 = PrepareMatrixInt(rows, columns);
            var mathMatrix1 = new MathMatrix<int>(data1, MatrixVectorizationType.ByRow);

            var result = mathMatrix1.Transpose().ToArray();

            Assert.AreEqual(rows, result.GetLength(1));
            Assert.AreEqual(columns, result.GetLength(0));

            for (var row = 0; row < result.GetLength(0); row++)
            {
                for (var col = 0; col < result.GetLength(1); col++)
                {
                    Assert.AreEqual(data1[col, row], result[row, col]);
                }
            }
        }

        [TestMethod]
        public void Determinant_2x2_Float()
        {
            var rows = 2;
            //var data1 = PrepareMatrixFloat(rows, rows);
            var data1 = new [,] {{1F, 2}, {3, 4}};
            var mathMatrix1 = new MathMatrix<float>(data1, MatrixVectorizationType.ByRow);

            var result = mathMatrix1.Determinant();

            Assert.AreEqual(data1[0, 0] * data1[1, 1] - data1[0, 1] * data1[1, 0], result, FloatEpsilon);
        }

        [TestMethod]
        public void Determinant_3x3_Float()
        {
            // example computed by Wolfram Alpha
            var data1 = new[,] { { 1F, 2, 3 }, { 3, 2, 1 }, { 2, 1, 3 } };
            var mathMatrix1 = new MathMatrix<float>(data1, MatrixVectorizationType.ByRow);

            var result = mathMatrix1.Determinant();

            Assert.AreEqual(-12, result, FloatEpsilon);
        }

        [TestMethod]
        public void Determinant_4x4_Float()
        {
            // example computed by Wolfram Alpha
            var data1 = new[,] { { 1F, 2, 3, 4 }, { 3, 4, 2, 1 }, { 1, 1, 1, 3 }, { 3, 2, 1, 4 } };
            var mathMatrix1 = new MathMatrix<float>(data1, MatrixVectorizationType.ByRow);

            var result = mathMatrix1.Determinant();

            Assert.AreEqual(12, result, FloatEpsilon);
        }

        [TestMethod]
        public void Determinant_5x5_Float()
        {
            // example computed by Wolfram Alpha
            var data1 = new[,]
            {
                {0.541865F, 0.616302F, 0.678667F, 0.701059F, 0.65555F},
                {0.669392F, 0.299938F, 0.653423F, 0.984241F, 0.426578F},
                {0.344834F, 0.0316045F, 0.928016F, 0.258747F, 0.58845F},
                {0.0867786F, 0.640929F, 0.192049F, 0.726113F, 0.667954F},
                {0.612612F, 0.939742F, 0.695467F, 0.697182F, 0.159928F}
            };
            var mathMatrix1 = new MathMatrix<float>(data1, MatrixVectorizationType.ByRow);

            var result = mathMatrix1.Determinant();

            Assert.AreEqual(0.0696283, result, FloatEpsilon);
        }

        [TestMethod]
        public void Determinant_6x6_Float()
        {
            // example computed by Wolfram Alpha
            var data1 = new float[,]
            {
                {0.552924F, 0.234921F, 0.792491F, 0.331809F, 0.566531F, 0.410049F},
                {0.658599F, 0.582204F, 0.95111F, 0.717428F, 0.331445F, 0.785975F},
                {0.991842F, 0.0453762F, 0.874894F, 0.376171F, 0.0568837F, 0.652004F},
                {0.349564F, 0.161621F, 0.668324F, 0.545462F, 0.840894F, 0.481515F},
                {0.710822F, 0.192863F, 0.784584F, 0.937499F, 0.571135F, 0.291105F},
                {0.815712F, 0.202527F, 0.000147915F, 0.137108F, 0.493101F, 0.162635F}
            };
            var mathMatrix1 = new MathMatrix<float>(data1, MatrixVectorizationType.ByRow);

            var result = mathMatrix1.Determinant();

            Assert.AreEqual(-0.0572824F, result, FloatEpsilon);
        }

        [TestMethod]
        public void Determinant_10x10_Float()
        {
            // example computed by Wolfram Alpha
            var data1 = new float[,]
            {
                {
                    0.947608F, 0.0188665F, 0.311291F, 0.425392F, 0.802979F, 0.132695F, 0.0301334F, 0.359189F, 0.593238F,
                    0.953465F
                },
                {
                    0.784727F, 0.217887F, 0.393151F, 0.605163F, 0.7522F, 0.586127F, 0.0508186F, 0.141586F, 0.847686F,
                    0.695732F
                },
                {
                    0.987599F, 0.274374F, 0.428349F, 0.335581F, 0.841285F, 0.769568F, 0.0418207F, 0.999481F, 0.0516447F,
                    0.783739F
                },
                {
                    0.670933F, 0.00458136F, 0.907957F, 0.50934F, 0.64579F, 0.503563F, 0.697977F, 0.887498F, 0.797104F,
                    0.44686F
                },
                {
                    0.0113925F, 0.572658F, 0.288216F, 0.0524767F, 0.553339F, 0.956658F, 0.038212F, 0.42527F, 0.237809F,
                    0.388843F
                },
                {
                    0.960779F, 0.611897F, 0.259834F, 0.618542F, 0.116246F, 0.137611F, 0.0942259F, 0.513293F, 0.647765F,
                    0.559395F
                },
                {
                    0.210231F, 0.885404F, 0.948014F, 0.217033F, 0.00980245F, 0.723663F, 0.455302F, 0.86287F, 0.547611F,
                    0.476013F
                },
                {
                    0.0304889F, 0.695998F, 0.736968F, 0.935107F, 0.656312F, 0.760413F, 0.0868986F, 0.608241F, 0.609165F,
                    0.663919F
                },
                {
                    0.05538F, 0.421821F, 0.933948F, 0.372348F, 0.000887508F, 0.198492F, 0.961668F, 0.388987F, 0.446955F,
                    0.410674F
                },
                {
                    0.388189F, 0.426757F, 0.535275F, 0.423885F, 0.705795F, 0.319361F, 0.995437F, 0.788767F, 0.120898F,
                    0.722064F
                }
            };
            var mathMatrix1 = new MathMatrix<float>(data1, MatrixVectorizationType.ByRow);

            var result = mathMatrix1.Determinant();

            Assert.AreEqual(0.00351536F, result, FloatEpsilon);
        }

        [TestMethod]
        public void Transpose_Float()
        {
            var rows = 10;
            var columns = 6;
            var data1 = PrepareMatrixFloat(rows, columns);
            var mathMatrix1 = new MathMatrix<float>(data1, MatrixVectorizationType.ByRow);

            var result = mathMatrix1.Transpose().ToArray();

            Assert.AreEqual(rows, result.GetLength(1));
            Assert.AreEqual(columns, result.GetLength(0));

            for (var row = 0; row < result.GetLength(0); row++)
            {
                for (var col = 0; col < result.GetLength(1); col++)
                {
                    Assert.AreEqual(data1[col, row], result[row, col]);
                }
            }
        }

        [TestCategory("Performance")]
        [TestMethod]
        public void MultiplicationPerformance_Int()
        {
            // not really a test
            // serves as a performance-testing workbench
            var n = 300;
            var data1 = PrepareMatrixInt(n, n, 2);
            var data2 = PrepareMatrixInt(n, n, 2);

            var mathMatrix1 = new MathMatrix<int>(data1, MatrixVectorizationType.ByRow);
            var mathMatrix2 = new MathMatrix<int>(data2, MatrixVectorizationType.ByColumn);

            var sw = new Stopwatch();

            for (var i = 0; i < 100; i++)
            {
                sw.Start();
                var m = mathMatrix1.Multiply(mathMatrix2);
                sw.Stop();
            }

            Console.WriteLine("Total time: " + sw.Elapsed.TotalMilliseconds);
        }

        [TestCategory("Performance")]
        [TestMethod]
        public void MultiplicationPerformance_Float()
        {
            var n = 300;
            var data1 = PrepareMatrixFloat(n, n, 1);
            var data2 = PrepareMatrixFloat(n, n, 1);

            var mathMatrix1 = new MathMatrix<float>(data1, MatrixVectorizationType.ByRow);
            var mathMatrix2 = new MathMatrix<float>(data2, MatrixVectorizationType.ByColumn);

            var sw = new Stopwatch();

            for (var i = 0; i < 100; i++)
            {
                sw.Start();
                var m = mathMatrix1.Multiply(mathMatrix2);
                sw.Stop();
            }

            Console.WriteLine("Total time: " + sw.Elapsed.TotalMilliseconds);
        }

        [TestCategory("Performance")]
        [TestMethod]
        public void TransposePerformance_Int()
        {
            // not really a test
            // serves as a performance-testing workbench
            var n = 300;
            var data1 = PrepareMatrixInt(n, n, 2);

            var mathMatrix1 = new MathMatrix<int>(data1, MatrixVectorizationType.ByRow);

            var sw = new Stopwatch();

            for (var i = 0; i < 100; i++)
            {
                sw.Start();
                var m = mathMatrix1.Transpose();
                sw.Stop();
            }

            Console.WriteLine("Total time: " + sw.Elapsed.TotalMilliseconds);
        }

        [TestCategory("Performance")]
        [TestMethod]
        public void TransposePerformance_Float()
        {
            var n = 300;
            var data1 = PrepareMatrixFloat(n, n, 1);

            var mathMatrix1 = new MathMatrix<float>(data1, MatrixVectorizationType.ByRow);

            var sw = new Stopwatch();

            for (var i = 0; i < 100; i++)
            {
                sw.Start();
                var m = mathMatrix1.Transpose();
                sw.Stop();
            }

            Console.WriteLine("Total time: " + sw.Elapsed.TotalMilliseconds);
        }

        [TestCategory("Performance")]
        [TestMethod]
        public void TransposeInPlacePerformance_Int()
        {
            // not really a test
            // serves as a performance-testing workbench
            var n = 300;
            var data1 = PrepareMatrixInt(n, n, 2);

            var mathMatrix1 = new MathMatrix<int>(data1, MatrixVectorizationType.ByRow);

            var sw = new Stopwatch();

            for (var i = 0; i < 100; i++)
            {
                sw.Start();
                var m = mathMatrix1.TransposeInPlace();
                sw.Stop();
            }

            Console.WriteLine("Total time: " + sw.Elapsed.TotalMilliseconds);
        }

        [TestCategory("Performance")]
        [TestMethod]
        public void TransposeInPlacePerformance_Float()
        {
            var n = 300;
            var data1 = PrepareMatrixFloat(n, n, 1);

            var mathMatrix1 = new MathMatrix<float>(data1, MatrixVectorizationType.ByRow);

            var sw = new Stopwatch();

            for (var i = 0; i < 100; i++)
            {
                sw.Start();
                var m = mathMatrix1.TransposeInPlace();
                sw.Stop();
            }

            Console.WriteLine("Total time: " + sw.Elapsed.TotalMilliseconds);
        }

        [TestCategory("Performance")]
        [TestMethod]
        public void DeterminantPerformance_Float()
        {
            var n = 300;
            var data1 = PrepareMatrixFloat(n, n, 1);

            var mathMatrix1 = new MathMatrix<float>(data1, MatrixVectorizationType.ByRow);

            var sw = new Stopwatch();

            for (var i = 0; i < 100; i++)
            {
                sw.Start();
                var m = mathMatrix1.Determinant();
                sw.Stop();
            }

            Console.WriteLine("Total time: " + sw.Elapsed.TotalMilliseconds);
        }

        private static int[,] PrepareMatrixInt(int rows, int cols, int max = 500)
        {
            var data = new int[rows, cols];
            var r = new Random();

            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < cols; col++)
                {
                    data[row, col] = r.Next(0, max);
                }
            }

            return data;
        }

        private static float[,] PrepareMatrixFloat(int rows, int cols, int max = 500)
        {
            var data = new float[rows, cols];
            var r = new Random();

            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < cols; col++)
                {
                    data[row, col] = (float)r.NextDouble() * max;
                }
            }

            return data;
        }
    }
}