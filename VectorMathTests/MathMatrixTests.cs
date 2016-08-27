using System;
using System.Diagnostics;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VectorMath.Tests
{
    [TestClass]
    public class MathMatrixTests
    {
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
        public void Perfo()
        {
            // not really a test
            // serves as a performance-testing workbench
            var n = 300;
            var data1 = PrepareMatrixFloat(n, n);
            var data2 = PrepareMatrixFloat(n, n);

            var mathMatrix1 = new MathMatrix<float>(data1, MatrixVectorizationType.ByRow);
            var mathMatrix2 = new MathMatrix<float>(data2, MatrixVectorizationType.ByRow);

            var sw = new Stopwatch();

            for (int i = 0; i < 10000; i++)
            {
                sw.Start();
                var m = mathMatrix1.Subtract(mathMatrix2);
                sw.Stop();
            }

            Console.WriteLine("Total time: " + sw.Elapsed.TotalMilliseconds);
        }

        private int[,] PrepareMatrixInt(int rows, int cols)
        {
            var data = new int[rows, cols];
            var r = new Random();

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    data[row, col] = r.Next(0, 500);
                }
            }

            return data;
        }

        private float[,] PrepareMatrixFloat(int rows, int cols)
        {
            var data = new float[rows, cols];
            var r = new Random();

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    data[row, col] = (float)r.NextDouble() * 500;
                }
            }

            return data;
        }
    }
}