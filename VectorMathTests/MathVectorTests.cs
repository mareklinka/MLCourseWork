using VectorMath;
using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VectorMath.Tests
{
    [TestClass]
    public class MathVectorTests
    {
        private const float FloatEpsilon = 0.001F;

        [TestInitialize]
        public void Initialize()
        {
            Console.WriteLine($"Hardware acceleration: {Vector.IsHardwareAccelerated}");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullData()
        {
            new MathVector<int>(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EmptyData()
        {
            new MathVector<int>(new int[0]);
        }

        [TestMethod]
        public void LengthTest()
        {
            var data = new[] { 1, 2, 3, 4, 5 };

            var v = new MathVector<int>(data);
            Assert.AreEqual(data.Length, v.Length);
        }

        [TestMethod]
        public void InitializationTest_WithTail()
        {
            var data = new[] { 1, 2, 3, 4, 5 };

            var v = new MathVector<int>(data);

            for (var i = 0; i < data.Length; i++)
            {
                Assert.AreEqual(data[i], v[i]);
            }
        }

        [TestMethod]
        public void InitializationTest_NoTail()
        {
            var data = new int[Vector<int>.Count];
            for (var i = 0; i < Vector<int>.Count; i++)
            {
                data[i] = i;
            }

            var v = new MathVector<int>(data);

            for (var i = 0; i < data.Length; i++)
            {
                Assert.AreEqual(data[i], v[i]);
            }
        }

        [TestMethod]
        public void ToArrayTest_WithTail()
        {
            var data1 = new[] { 1F, 2, 3, 4, 5 };

            var v1 = new MathVector<float>(data1);
            var result = v1.ToArray();

            for (var i = 0; i < data1.Length; i++)
            {
                Assert.AreEqual(data1[i], result[i]);
            }
        }

        [TestMethod]
        public void ToArrayTest_NoTail()
        {
            var data1 = new[] { 1F, 2, 3, 4 };

            var v1 = new MathVector<float>(data1);
            var result = v1.ToArray();

            for (var i = 0; i < data1.Length; i++)
            {
                Assert.AreEqual(data1[i], result[i]);
            }
        }

        [TestMethod]
        public void DotTest_Int_WithTail()
        {
            var data = new[] { 1, 2, 3, 4, 5 };

            var v1 = new MathVector<int>(data);
            var v2 = new MathVector<int>(data);

            var result = v1.Dot(v2);

            Assert.AreEqual(55, result);
        }

        [TestMethod]
        public void DotTest_Int_NoTail()
        {
            var data = new[] { 1, 2, 3, 4 };

            var v1 = new MathVector<int>(data);
            var v2 = new MathVector<int>(data);

            var result = v1.Dot(v2);

            Assert.AreEqual(30, result);
        }

        [TestMethod]
        public void PlusTest_Int_WithTail()
        {
            var data1 = new[] { 1, 2, 3, 4, 5 };
            var data2 = new[] { 2, 3, 4, 5, 6 };

            var v1 = new MathVector<int>(data1);
            var v2 = new MathVector<int>(data2);

            var result = v1 + v2;

            for (var i = 0; i < data1.Length; i++)
            {
                Assert.AreEqual(data1[i] + data2[i], result[i]);
            }
        }

        [TestMethod]
        public void PlusTest_Int_NoTail()
        {
            var data1 = new[] { 1, 2, 3, 4 };
            var data2 = new[] { 2, 3, 4, 5 };

            var v1 = new MathVector<int>(data1);
            var v2 = new MathVector<int>(data2);

            var result = v1 + v2;

            for (var i = 0; i < data1.Length; i++)
            {
                Assert.AreEqual(data1[i] + data2[i], result[i]);
            }
        }

        [TestMethod]
        public void MinusTest_Int_WithTail()
        {
            var data1 = new[] { 1, 2, 3, 4, 5 };
            var data2 = new[] { 2, 3, 4, 5, 6 };

            var v1 = new MathVector<int>(data1);
            var v2 = new MathVector<int>(data2);

            var result = v1 - v2;

            for (var i = 0; i < data1.Length; i++)
            {
                Assert.AreEqual(data1[i] - data2[i], result[i]);
            }
        }

        [TestMethod]
        public void MinusTest_Int_NoTail()
        {
            var data1 = new[] { 1, 2, 3, 4 };
            var data2 = new[] { 2, 3, 4, 5 };

            var v1 = new MathVector<int>(data1);
            var v2 = new MathVector<int>(data2);

            var result = v1 - v2;

            for (var i = 0; i < data1.Length; i++)
            {
                Assert.AreEqual(data1[i] - data2[i], result[i]);
            }
        }

        [TestMethod]
        public void MultiplyScalarTest_Int_WithTail()
        {
            var data1 = new[] { 1, 2, 3, 4, 5 };

            var v1 = new MathVector<int>(data1);

            var factor = 3;
            var result = v1.MultiplyScalar(factor);

            for (var i = 0; i < data1.Length; i++)
            {
                Assert.AreEqual(data1[i] * factor, result[i]);
            }
        }

        [TestMethod]
        public void MultiplyScalarTest_Int_NoTail()
        {
            var data1 = new[] { 1, 2, 3, 4 };

            var v1 = new MathVector<int>(data1);

            var factor = 3;
            var result = v1.MultiplyScalar(factor);

            for (var i = 0; i < data1.Length; i++)
            {
                Assert.AreEqual(data1[i] * factor, result[i]);
            }
        }

        [TestMethod]
        public void MultiplyTest_Int_WithTail()
        {
            var data1 = new[] { 1, 2, 3, 4, 5 };
            var data2 = new[] { 2, 3, 4, 5, 6 };

            var v1 = new MathVector<int>(data1);
            var v2 = new MathVector<int>(data2);
            
            var result = v1.Multiply(v2);

            for (var i = 0; i < data1.Length; i++)
            {
                Assert.AreEqual(data1[i] * data2[i], result[i]);
            }
        }

        [TestMethod]
        public void MultiplyTest_Int_NoTail()
        {
            var data1 = new[] { 1, 2, 3, 4 };
            var data2 = new[] { 2, 3, 4, 5 };

            var v1 = new MathVector<int>(data1);
            var v2 = new MathVector<int>(data2);

            var result = v1.Multiply(v2);

            for (var i = 0; i < data1.Length; i++)
            {
                Assert.AreEqual(data1[i] * data2[i], result[i]);
            }
        }

        [TestMethod]
        public void MultiplyMatrixTest_Int_WithTail()
        {
            var n = 10;
            var data1 = PrepareVectorInt(n, 2);
            var data2 = PrepareMatrixInt(n, 8, 2);

            var v1 = new MathVector<int>(data1);
            var v2 = new MathMatrix<int>(data2);

            var result = v1.Multiply(v2);

            for (var col = 0; col < data2.GetLength(1); col++)
            {
                var sum = 0;
                for (var row = 0; row < data2.GetLength(0); row++)
                {
                    sum += data1[row]*data2[row, col];
                }

                Assert.AreEqual(sum, result[col]);
            }
        }

        [TestMethod]
        public void MultiplyMatrixTest_Int_NoTail()
        {
            var n = 8;
            var data1 = PrepareVectorInt(n, 2);
            var data2 = PrepareMatrixInt(n, 7, 2);

            var v1 = new MathVector<int>(data1);
            var v2 = new MathMatrix<int>(data2);

            var result = v1.Multiply(v2);

            for (var col = 0; col < data2.GetLength(1); col++)
            {
                var sum = 0;
                for (var row = 0; row < data2.GetLength(0); row++)
                {
                    sum += data1[row] * data2[row, col];
                }

                Assert.AreEqual(sum, result[col]);
            }
        }

        [TestMethod]
        public void DivideScalarTest_Int_WithTail()
        {
            var data1 = new[] { 1, 2, 3, 4, 5 };

            var v1 = new MathVector<int>(data1);

            var factor = 3;
            var result = v1.DivideScalar(factor);

            for (var i = 0; i < data1.Length; i++)
            {
                Assert.AreEqual(data1[i] / factor, result[i]);
            }
        }

        [TestMethod]
        public void DivideScalarTest_Int_NoTail()
        {
            var data1 = new[] { 1, 2, 3, 4 };

            var v1 = new MathVector<int>(data1);

            var factor = 3;
            var result = v1.DivideScalar(factor);

            for (var i = 0; i < data1.Length; i++)
            {
                Assert.AreEqual(data1[i] / factor, result[i]);
            }
        }

        [TestMethod]
        public void LengthTest_Int_WithTail()
        {
            var data1 = new[] { 1, 2, 3, 4, 5 };

            var v1 = new MathVector<int>(data1);

            var result = v1.VectorLength();

            Assert.AreEqual((int)Math.Sqrt(data1.Sum(_ => _ * _)), result);
        }

        [TestMethod]
        public void LengthTest_Int_NoTail()
        {
            var data1 = new[] { 1, 2, 3, 4 };

            var v1 = new MathVector<int>(data1);

            var result = v1.VectorLength();

            Assert.AreEqual((int)Math.Sqrt(data1.Sum(_ => _ * _)), result);
        }

        [TestMethod]
        public void DotTest_Float_WithTail()
        {
            var data = new[] { 1F, 2, 3, 4, 5 };

            var v1 = new MathVector<float>(data);
            var v2 = new MathVector<float>(data);

            var result = v1.Dot(v2);

            Assert.AreEqual(55, result);
        }

        [TestMethod]
        public void DotTest_Float_NoTail()
        {
            var data = new[] { 1F, 2, 3, 4 };

            var v1 = new MathVector<float>(data);
            var v2 = new MathVector<float>(data);

            var result = v1.Dot(v2);

            Assert.AreEqual(30, result);
        }

        [TestMethod]
        public void PlusTest_Float_WithTail()
        {
            var data1 = new[] { 1F, 2, 3, 4, 5 };
            var data2 = new[] { 2F, 3, 4, 5, 6 };

            var v1 = new MathVector<float>(data1);
            var v2 = new MathVector<float>(data2);

            var result = v1 + v2;

            for (var i = 0; i < data1.Length; i++)
            {
                Assert.AreEqual(data1[i] + data2[i], result[i]);
            }
        }

        [TestMethod]
        public void PlusTest_Float_NoTail()
        {
            var data1 = new[] { 1F, 2, 3, 4 };
            var data2 = new[] { 2F, 3, 4, 5 };

            var v1 = new MathVector<float>(data1);
            var v2 = new MathVector<float>(data2);

            var result = v1 + v2;

            for (var i = 0; i < data1.Length; i++)
            {
                Assert.AreEqual(data1[i] + data2[i], result[i]);
            }
        }

        [TestMethod]
        public void MinusTest_Float_WithTail()
        {
            var data1 = new[] { 1F, 2, 3, 4, 5 };
            var data2 = new[] { 2F, 3, 4, 5, 6 };

            var v1 = new MathVector<float>(data1);
            var v2 = new MathVector<float>(data2);

            var result = v1 - v2;

            for (var i = 0; i < data1.Length; i++)
            {
                Assert.AreEqual(data1[i] - data2[i], result[i]);
            }
        }

        [TestMethod]
        public void MinusTest_Float_NoTail()
        {
            var data1 = new[] { 1F, 2, 3, 4 };
            var data2 = new[] { 2F, 3, 4, 5 };

            var v1 = new MathVector<float>(data1);
            var v2 = new MathVector<float>(data2);

            var result = v1 - v2;

            for (var i = 0; i < data1.Length; i++)
            {
                Assert.AreEqual(data1[i] - data2[i], result[i]);
            }
        }

        [TestMethod]
        public void MultiplyScalarTest_Float_WithTail()
        {
            var data1 = new[] { 1F, 2, 3, 4, 5 };

            var v1 = new MathVector<float>(data1);

            var factor = 3;
            var result = v1.MultiplyScalar(factor);

            for (var i = 0; i < data1.Length; i++)
            {
                Assert.AreEqual(data1[i] * factor, result[i]);
            }
        }

        [TestMethod]
        public void MultiplyScalarTest_Float_NoTail()
        {
            var data1 = new[] { 1F, 2, 3, 4 };

            var v1 = new MathVector<float>(data1);

            var factor = 3;
            var result = v1.MultiplyScalar(factor);

            for (var i = 0; i < data1.Length; i++)
            {
                Assert.AreEqual(data1[i] * factor, result[i]);
            }
        }

        [TestMethod]
        public void MultiplyTest_Float_WithTail()
        {
            var data1 = new[] { 1F, 2, 3, 4, 5 };
            var data2 = new[] { 2F, 3, 4, 5, 6 };

            var v1 = new MathVector<float>(data1);
            var v2 = new MathVector<float>(data2);

            var result = v1.Multiply(v2);

            for (var i = 0; i < data1.Length; i++)
            {
                Assert.AreEqual(data1[i] * data2[i], result[i]);
            }
        }

        [TestMethod]
        public void MultiplyTest_Float_NoTail()
        {
            var data1 = new[] { 1F, 2, 3, 4 };
            var data2 = new[] { 2F, 3, 4, 5 };

            var v1 = new MathVector<float>(data1);
            var v2 = new MathVector<float>(data2);

            var result = v1.Multiply(v2);

            for (var i = 0; i < data1.Length; i++)
            {
                Assert.AreEqual(data1[i] * data2[i], result[i]);
            }
        }

        [TestMethod]
        public void MultiplyMatrixTest_Float_WithTail()
        {
            var n = 10;
            var data1 = PrepareVectorFloat(n, 2);
            var data2 = PrepareMatrixFloat(n, 8, 2);

            var v1 = new MathVector<float>(data1);
            var v2 = new MathMatrix<float>(data2);

            var result = v1.Multiply(v2);

            for (var col = 0; col < data2.GetLength(1); col++)
            {
                var sum = 0F;
                for (var row = 0; row < data2.GetLength(0); row++)
                {
                    sum += data1[row] * data2[row, col];
                }

                Assert.AreEqual(sum, result[col], FloatEpsilon);
            }
        }

        [TestMethod]
        public void MultiplyMatrixTest_Float_NoTail()
        {
            var n = 8;
            var data1 = PrepareVectorFloat(n, 2);
            var data2 = PrepareMatrixFloat(n, 7, 2);

            var v1 = new MathVector<float>(data1);
            var v2 = new MathMatrix<float>(data2);

            var result = v1.Multiply(v2);

            for (var col = 0; col < data2.GetLength(1); col++)
            {
                var sum = 0F;
                for (var row = 0; row < data2.GetLength(0); row++)
                {
                    sum += data1[row] * data2[row, col];
                }

                Assert.AreEqual(sum, result[col], FloatEpsilon);
            }
        }

        [TestMethod]
        public void DivideScalarTest_Float_WithTail()
        {
            var data1 = new[] { 1F, 2, 3, 4, 5 };

            var v1 = new MathVector<float>(data1);

            var factor = 3;
            var result = v1.DivideScalar(factor);

            for (var i = 0; i < data1.Length; i++)
            {
                Assert.AreEqual(data1[i] / factor, result[i]);
            }
        }

        [TestMethod]
        public void DivideScalarTest_Float_NoTail()
        {
            var data1 = new[] { 1F, 2, 3, 4 };

            var v1 = new MathVector<float>(data1);

            var factor = 3;
            var result = v1.DivideScalar(factor);

            for (var i = 0; i < data1.Length; i++)
            {
                Assert.AreEqual(data1[i] / factor, result[i]);
            }
        }

        [TestMethod]
        public void LengthTest_Float_WithTail()
        {
            var data1 = new[] { 1F, 2, 3, 4, 5 };

            var v1 = new MathVector<float>(data1);

            var result = v1.VectorLength();

            Assert.AreEqual((float)Math.Sqrt(data1.Sum(_ => _ * _)), result);
        }

        [TestMethod]
        public void LengthTest_Float_NoTail()
        {
            var data1 = new[] { 1F, 2, 3, 4 };

            var v1 = new MathVector<float>(data1);

            var result = v1.VectorLength();

            Assert.AreEqual((float)Math.Sqrt(data1.Sum(_ => _ * _)), result);
        }

        [TestMethod]
        public void NormalizationTest_Float_WithTail()
        {
            var data1 = new[] { 1F, 2, 3, 4, 5 };

            var v1 = new MathVector<float>(data1);
            var length = v1.VectorLength();
            var result = v1.Normalize();

            for (var i = 0; i < data1.Length; i++)
            {
                Assert.AreEqual(data1[i] / length, result[i]);
            }
        }

        [TestMethod]
        public void NormalizationTest_Float_NoTail()
        {
            var data1 = new[] { 1F, 2, 3, 4 };

            var v1 = new MathVector<float>(data1);
            var length = v1.VectorLength();
            var result = v1.Normalize();

            for (var i = 0; i < data1.Length; i++)
            {
                Assert.AreEqual(data1[i] / length, result[i]);
            }
        }

        [TestCategory("Performance")]
        [TestMethod]
        public void MultiplyMatrixPerformance_Int()
        {
            var n = 300;
            var data1 = PrepareVectorInt(n, 2);
            var data2 = PrepareMatrixInt(n, n*2, 2);

            var mathVector1 = new MathVector<int>(data1);
            var mathMatrix2 = new MathMatrix<int>(data2, MatrixVectorizationType.ByColumn);

            var sw = new Stopwatch();

            for (var i = 0; i < 100; i++)
            {
                sw.Start();
                var m = mathVector1.Multiply(mathMatrix2);
                sw.Stop();
            }

            Console.WriteLine("Total time: " + sw.Elapsed.TotalMilliseconds);
        }

        [TestCategory("Performance")]
        [TestMethod]
        public void MultiplyMatrixPerformance_Float()
        {
            var n = 300;
            var data1 = PrepareVectorFloat(n, 2);
            var data2 = PrepareMatrixFloat(n, n*2, 2);

            var mathVector1 = new MathVector<float>(data1);
            var mathMatrix2 = new MathMatrix<float>(data2, MatrixVectorizationType.ByColumn);

            var sw = new Stopwatch();

            for (var i = 0; i < 100; i++)
            {
                sw.Start();
                var m = mathVector1.Multiply(mathMatrix2);
                sw.Stop();
            }

            Console.WriteLine("Total time: " + sw.Elapsed.TotalMilliseconds);
        }

        [TestCategory("Performance")]
        [TestMethod]
        public void MultiplyMatrixPerformance_Int_ByRow()
        {
            var n = 300;
            var data1 = PrepareVectorInt(n, 2);
            var data2 = PrepareMatrixInt(n, n*2, 2);

            var mathVector1 = new MathVector<int>(data1);
            var mathMatrix2 = new MathMatrix<int>(data2, MatrixVectorizationType.ByRow);

            var sw = new Stopwatch();

            for (var i = 0; i < 100; i++)
            {
                sw.Start();
                var m = mathVector1.Multiply(mathMatrix2);
                sw.Stop();
            }

            Console.WriteLine("Total time: " + sw.Elapsed.TotalMilliseconds);
        }

        [TestCategory("Performance")]
        [TestMethod]
        public void MultiplyMatrixPerformance_Float_ByRow()
        {
            var n = 300;
            var data1 = PrepareVectorFloat(n, 2);
            var data2 = PrepareMatrixFloat(n, n*2, 2);

            var mathVector1 = new MathVector<float>(data1);
            var mathMatrix2 = new MathMatrix<float>(data2, MatrixVectorizationType.ByRow);

            var sw = new Stopwatch();

            for (var i = 0; i < 100; i++)
            {
                sw.Start();
                var m = mathVector1.Multiply(mathMatrix2);
                sw.Stop();
            }

            Console.WriteLine("Total time: " + sw.Elapsed.TotalMilliseconds);
        }

        private static int[] PrepareVectorInt(int length, int max = 500)
        {
            var data = new int[length];
            var r = new Random();

            for (var row = 0; row < length; row++)
            {
                data[row] = r.Next(0, max);
            }

            return data;
        }

        private static float[] PrepareVectorFloat(int length, int max = 500)
        {
            var data = new float[length];
            var r = new Random();

            for (var row = 0; row < length; row++)
            {
                data[row] = (float)r.NextDouble() * max;
            }

            return data;
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