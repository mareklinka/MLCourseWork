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