using System;
using System.Collections.Generic;
using System.Numerics;

namespace VectorMath
{
    internal class VectorOpsSimd<T> : VectorOps<T> where T : struct
    {
        public override MathVector<T> Add(MathVector<T> left, MathVector<T> right)
        {
            var sumVectorized = new List<Vector<T>>(left.Vectors.Count);

            for (var i = 0; i < left.Vectors.Count; ++i)
            {
                var leftVector = left.Vectors[i];
                var rightVector = right.Vectors[i];
                var sumVector = leftVector + rightVector;

                sumVectorized.Add(sumVector);
            }

            if (left.Tail.Length > 0)
            {
                var remainderSum = new T[left.Tail.Length];
                if (typeof(T) == typeof(int))
                {
                    for (var i = 0; i < left.Tail.Length; ++i)
                    {
                        remainderSum[i] = (T)(ValueType)((int)(ValueType)left.Tail[i] + (int)(ValueType)right.Tail[i]);
                    }
                }
                if (typeof(T) == typeof(float))
                {
                    for (var i = 0; i < left.Tail.Length; ++i)
                    {
                        remainderSum[i] = (T)(ValueType)((float)(ValueType)left.Tail[i] + (float)(ValueType)right.Tail[i]);
                    }
                }
                return new MathVector<T>(sumVectorized, remainderSum);
            }

            return new MathVector<T>(sumVectorized, new T[0]);
        }

        public override MathVector<T> Subtract(MathVector<T> left, MathVector<T> right)
        {
            var sumVectorized = new List<Vector<T>>(left.Vectors.Count);

            for (var i = 0; i < left.Vectors.Count; ++i)
            {
                var leftVector = left.Vectors[i];
                var rightVector = right.Vectors[i];
                var sumVector = leftVector - rightVector;

                sumVectorized.Add(sumVector);
            }

            if (left.Tail.Length > 0)
            {
                var remainderSum = new T[left.Tail.Length];
                if (typeof(T) == typeof(int))
                {
                    for (var i = 0; i < left.Tail.Length; ++i)
                    {
                        remainderSum[i] = (T)(ValueType)((int)(ValueType)left.Tail[i] - (int)(ValueType)right.Tail[i]);
                    }
                    return new MathVector<T>(sumVectorized, remainderSum);
                }
                if (typeof(T) == typeof(float))
                {
                    for (var i = 0; i < left.Tail.Length; ++i)
                    {
                        remainderSum[i] = (T)(ValueType)((float)(ValueType)left.Tail[i] - (float)(ValueType)right.Tail[i]);
                    }
                    return new MathVector<T>(sumVectorized, remainderSum);
                }
                if (typeof(T) == typeof(double))
                {
                    for (var i = 0; i < left.Tail.Length; ++i)
                    {
                        remainderSum[i] = (T)(ValueType)((double)(ValueType)left.Tail[i] - (double)(ValueType)right.Tail[i]);
                    }
                    return new MathVector<T>(sumVectorized, remainderSum);
                }
            }

            return new MathVector<T>(sumVectorized, new T[0]);
        }

        public override T Dot(MathVector<T> left, MathVector<T> right)
        {
            var resultVector = Vector<T>.Zero;

            for (var i = 0; i < left.Vectors.Count; i++)
            {
                resultVector += Vector.Multiply(left.Vectors[i], right.Vectors[i]);
            }

            if (typeof(T) == typeof(int))
            {
                var result = 0;
                var asVectorInt32 = Vector.AsVectorInt32(resultVector);
                for (var i = 0; i < Vector<int>.Count; i++)
                {
                    result += asVectorInt32[i];
                }
                if (left.Tail.Length > 0)
                {
                    for (var i = 0; i < left.Tail.Length; ++i)
                    {
                        result += (int) (ValueType) left.Tail[i]*(int) (ValueType) right.Tail[i];
                    }
                }

                return (T)(ValueType)result;
            }
            else if (typeof(T) == typeof(float))
            {
                var result = 0F;
                var asVectorSingle = Vector.AsVectorSingle(resultVector);
                for (var i = 0; i < Vector<float>.Count; i++)
                {
                    result += asVectorSingle[i];
                }
                if (left.Tail.Length > 0)
                {
                    for (var i = 0; i < left.Tail.Length; ++i)
                    {
                        result += (float)(ValueType)left.Tail[i] * (float)(ValueType)right.Tail[i];
                    }
                }

                return (T)(ValueType)result;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override MathVector<T> Multiply(MathVector<T> left, MathVector<T> right)
        {
            var result = new List<Vector<T>>(left.Vectors.Count);

            for (var i = 0; i < left.Vectors.Count; i++)
            {
                result.Add(Vector.Multiply(left.Vectors[i], right.Vectors[i]));
            }
            
            if (typeof(T) == typeof(int))
            {
                if (left.Tail.Length > 0)
                {
                    var remainderMult = new T[left.Tail.Length];
                    for (var i = 0; i < left.Tail.Length; i++)
                    {
                        remainderMult[i] = (T)(ValueType)((int)(ValueType)left.Tail[i] * (int)(ValueType)right.Tail[i]);
                    }
                    return new MathVector<T>(result, remainderMult);
                }
            }
            else if (typeof(T) == typeof(float))
            {
                if (left.Tail.Length > 0)
                {
                    var remainderMult = new T[left.Tail.Length];
                    for (var i = 0; i < left.Tail.Length; i++)
                    {
                        remainderMult[i] = (T)(ValueType)((float)(ValueType)left.Tail[i] * (float)(ValueType)right.Tail[i]);
                    }
                    return new MathVector<T>(result, remainderMult);
                }
            }
            else if (typeof(T) == typeof(double))
            {
                if (left.Tail.Length > 0)
                {
                    var remainderMult = new T[left.Tail.Length];
                    for (var i = 0; i < left.Tail.Length; i++)
                    {
                        remainderMult[i] = (T)(ValueType)((double)(ValueType)left.Tail[i] * (double)(ValueType)right.Tail[i]);
                    }
                    return new MathVector<T>(result, remainderMult);
                }
            }

            return new MathVector<T>(result, new T[0]);
        }

        public override MathVector<T> Multiply(MathVector<T> left, MathMatrix<T> right)
        {
            // swapping vectorization mode on the fly would result in major performance loss (about 20% slower than serial version)
            if (right.VectorizationMode == MatrixVectorizationType.ByColumn)
            {
                var result = new T[right.Columns];

                for (var i = 0; i < right.Columns; i++)
                {
                    result[i] = left.Dot(right.Vectors[i]);
                }

                return new MathVector<T>(result);
            }
            else
            {
                // in case of row-major storage fall back to serial calculation to avoid performance hit
                var result = new T[right.Columns];
                var leftArray = left.ToArray();
                var rightArray = right.ToArray();

                if (typeof(T) == typeof(int))
                {
                    for (var col = 0; col < right.Columns; col++)
                    {
                        for (var row = 0; row < left.Length; row++)
                        {
                            result[col] = AsT(AsInt(result[col]) + AsInt(leftArray[row]) * AsInt(rightArray[row, col]));
                        }
                    }

                    return new MathVector<T>(result);
                }
                else if (typeof(T) == typeof(float))
                {
                    for (var col = 0; col < right.Columns; col++)
                    {
                        for (var row = 0; row < left.Length; row++)
                        {
                            result[col] = AsT(AsFloat(result[col]) + AsFloat(leftArray[row]) * AsFloat(rightArray[row, col]));
                        }
                    }

                    return new MathVector<T>(result);
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }

        public override MathVector<T> MultiplyScalar(MathVector<T> vector, T scalar)
        {
            var multVectorized = new List<Vector<T>>(vector.Vectors.Count);
            var factor = new Vector<T>(scalar);

            for (var i = 0; i < vector.Vectors.Count; ++i)
            {
                var leftVector = vector.Vectors[i];
                var multVector = leftVector*factor;

                multVectorized.Add(multVector);
            }

            if (typeof(T) == typeof(int))
            {
                if (vector.Tail.Length > 0)
                {
                    var remainderMult = new T[vector.Tail.Length];
                    for (var i = 0; i < vector.Tail.Length; i++)
                    {
                        remainderMult[i] = (T)(ValueType)((int)(ValueType)vector.Tail[i] * (int)(ValueType)scalar);
                    }
                    return new MathVector<T>(multVectorized, remainderMult);
                }
            }
            else if (typeof(T) == typeof(float))
            {
                if (vector.Tail.Length > 0)
                {
                    var remainderMult = new T[vector.Tail.Length];
                    for (var i = 0; i < vector.Tail.Length; i++)
                    {
                        remainderMult[i] = (T)(ValueType)((float)(ValueType)vector.Tail[i] * (float)(ValueType)scalar);
                    }
                    return new MathVector<T>(multVectorized, remainderMult);
                }
            }

            return new MathVector<T>(multVectorized, new T[0]);
        }

        public override MathVector<T> Divide(MathVector<T> vector, T scalar)
        {
            var multVectorized = new List<Vector<T>>(vector.Vectors.Count);
            var factor = new Vector<T>(scalar);

            for (var i = 0; i < vector.Vectors.Count; ++i)
            {
                var leftVector = vector.Vectors[i];
                var multVector =Vector.Divide(leftVector, factor);

                multVectorized.Add(multVector);
            }

            if (typeof(T) == typeof(int))
            {
                if (vector.Tail.Length > 0)
                {
                    var remainderMult = new T[vector.Tail.Length];
                    for (var i = 0; i < vector.Tail.Length; i++)
                    {
                        remainderMult[i] = (T)(ValueType)((int)(ValueType)vector.Tail[i] / (int)(ValueType)scalar);
                    }
                    return new MathVector<T>(multVectorized, remainderMult);
                }
            }
            if (typeof(T) == typeof(float))
            {
                if (vector.Tail.Length > 0)
                {
                    var remainderMult = new T[vector.Tail.Length];
                    for (var i = 0; i < vector.Tail.Length; i++)
                    {
                        remainderMult[i] = (T)(ValueType)((float)(ValueType)vector.Tail[i] / (float)(ValueType)scalar);
                    }
                    return new MathVector<T>(multVectorized, remainderMult);
                }
            }

            return new MathVector<T>(multVectorized, new T[0]);
        }

        public override T Length(MathVector<T> mathVector)
        {
            var sum = Vector<T>.Zero;

            for (var i = 0; i < mathVector.Vectors.Count; ++i)
            {
                var v = mathVector.Vectors[i];
                sum += Vector.Multiply(v, v);
            }

            if (typeof(T) == typeof(int))
            {
                var result = 0;

                for (var i = 0; i < Vector<T>.Count; i++)
                {
                    result += (int) (ValueType) sum[i];
                }

                if (mathVector.Tail.Length > 0)
                {
                    for (var i = 0; i < mathVector.Tail.Length; ++i)
                    {
                        var val = (int) (ValueType) mathVector.Tail[i];
                        result += val*val;
                    }
                }

                return (T)(ValueType)(int)Math.Sqrt(result);
            }

            if (typeof(T) == typeof(float))
            {
                var result = 0F;

                for (var i = 0; i < Vector<T>.Count; i++)
                {
                    result += (float) (ValueType) sum[i];
                }

                if (mathVector.Tail.Length > 0)
                {
                    for (var i = 0; i < mathVector.Tail.Length; ++i)
                    {
                        var val = (float)(ValueType)mathVector.Tail[i];
                        result +=  val*val;
                    }
                }

                return (T) (ValueType) (float) Math.Sqrt(result);
            }

            throw new NotSupportedException();
        }

        public override MathVector<T> Abs(MathVector<T> vector)
        {
            var result = new List<Vector<T>>(vector.Vectors.Count);

            for (var i = 0; i < vector.Vectors.Count; i++)
            {
                result.Add(Vector.Abs(vector.Vectors[i]));
            }

            if (typeof(T) == typeof(int))
            {
                if (vector.Tail.Length > 0)
                {
                    var remainderMult = new T[vector.Tail.Length];
                    for (var i = 0; i < vector.Tail.Length; i++)
                    {
                        remainderMult[i] = (T)(ValueType)(Math.Abs((int)(ValueType)vector.Tail[i]));
                    }
                    return new MathVector<T>(result, remainderMult);
                }
            }
            else if (typeof(T) == typeof(float))
            {
                if (vector.Tail.Length > 0)
                {
                    var remainderMult = new T[vector.Tail.Length];
                    for (var i = 0; i < vector.Tail.Length; i++)
                    {
                        remainderMult[i] = (T)(ValueType)(Math.Abs((float)(ValueType)vector.Tail[i]));
                    }
                    return new MathVector<T>(result, remainderMult);
                }
            }

            return new MathVector<T>(result, new T[0]);
        }
    }
}