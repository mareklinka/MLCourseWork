using VectorMath;
using System;
using System.Linq;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VectorMath.Tests
{
    [TestClass]
    public class MathVectorTests
    {
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
    }
}