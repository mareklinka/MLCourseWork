using System;

namespace VectorMath
{
    internal class VectorOpsSerial<T> : VectorOps<T> where T : struct 
    {
        public override MathVector<T> Add(MathVector<T> left, MathVector<T> right)
        {
            var sumResult = new T[left.Length];

            if (typeof(T) == typeof(int))
            {
                for (var i = 0; i < left.Length; ++i)
                {
                    sumResult[i] = (T)(ValueType)((int)(ValueType)left[i] + (int)(ValueType)right[i]);
                }
            }
            else if (typeof(T) == typeof(float))
            {
                for (var i = 0; i < left.Length; ++i)
                {
                    sumResult[i] = (T)(ValueType)((float)(ValueType)left[i] + (float)(ValueType)right[i]);
                }
            }
            else
            {
                throw new NotSupportedException($"Type {typeof(T).Name} is not supported.");
            }

            return new MathVector<T>(sumResult);
        }

        public override MathVector<T> Subtract(MathVector<T> left, MathVector<T> right)
        {
            var sumResult = new T[left.Length];

            if (typeof(T) == typeof(int))
            {
                for (var i = 0; i < left.Length; ++i)
                {
                    sumResult[i] = (T)(ValueType)((int)(ValueType)left[i] - (int)(ValueType)right[i]);
                }
            }
            else if (typeof(T) == typeof(float))
            {
                for (var i = 0; i < left.Length; ++i)
                {
                    sumResult[i] = (T)(ValueType)((float)(ValueType)left[i] - (float)(ValueType)right[i]);
                }
            }
            else
            {
                throw new NotSupportedException($"Type {typeof(T).Name} is not supported.");
            }

            return new MathVector<T>(sumResult);
        }

        public override T Dot(MathVector<T> left, MathVector<T> right)
        {
            if (typeof(T) == typeof(int))
            {
                var result = 0;
                for (var i = 0; i < left.Length; i++)
                {
                    var x = left[i];
                    var y = right[i];
                    result += (int)(ValueType)x*(int)(ValueType)y;
                }

                return (T)(ValueType)result;
            }
            if (typeof(T) == typeof(float))
            {
                var result = 0F;
                for (var i = 0; i < left.Length; i++)
                {
                    var x = left[i];
                    var y = right[i];
                    result += (float)(ValueType)x * (float)(ValueType)y;
                }

                return (T)(ValueType)result;
            }
            else
            {
                throw new NotSupportedException($"Type {typeof(T).Name} is not supported.");
            }
        }

        public override MathVector<T> Multiply(MathVector<T> left, MathVector<T> right)
        {
            var result = new T[left.Length];

            if (typeof(T) == typeof(int))
            {
                for (var i = 0; i < left.Length; i++)
                {
                    result[i] = (T)(ValueType)((int)(ValueType)left[i] * (int)(ValueType)right[i]);
                }

                return new MathVector<T>(result);
            }
            else if (typeof(T) == typeof(float))
            {
                for (var i = 0; i < left.Length; i++)
                {
                    result[i] = (T) (ValueType) ((float) (ValueType) left[i]*(float) (ValueType) right[i]);
                }

                return new MathVector<T>(result);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override MathVector<T> Multiply(MathVector<T> left, MathMatrix<T> right)
        {
            var result = new T[right.Columns];

            if (typeof(T) == typeof(int))
            {
                for (var col = 0; col < right.Columns; col++)
                {
                    for (var row = 0; row < left.Length; row++)
                    {
                        result[col] = AsT(AsInt(result[col]) + AsInt(left[row])*AsInt(right[row, col]));
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
                        result[col] = AsT(AsFloat(result[col]) + AsFloat(left[row]) * AsFloat(right[row, col]));
                    }
                }

                return new MathVector<T>(result);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override MathVector<T> MultiplyScalar(MathVector<T> vector, T scalar)
        {
            var result = new T[vector.Length];

            if (typeof(T) == typeof(int))
            {
                for (var i = 0; i < vector.Length; i++)
                {
                    result[i] = (T) (ValueType) ((int) (ValueType) vector[i]*(int) (ValueType) scalar);
                }

                return new MathVector<T>(result);
            }
            if (typeof(T) == typeof(float))
            {
                for (var i = 0; i < vector.Length; i++)
                {
                    result[i] = (T)(ValueType)((float)(ValueType)vector[i] * (float)(ValueType)scalar);
                }

                return new MathVector<T>(result);
            }
            else
            {
                throw new NotSupportedException($"Type {typeof(T).Name} is not supported.");
            }
        }

        public override MathVector<T> Divide(MathVector<T> vector, T scalar)
        {
            var result = new T[vector.Length];

            if (typeof(T) == typeof(int))
            {
                for (var i = 0; i < vector.Length; i++)
                {
                    result[i] = (T)(ValueType)((int)(ValueType)vector[i] / (int)(ValueType)scalar);
                }

                return new MathVector<T>(result);
            }
            if (typeof(T) == typeof(float))
            {
                for (var i = 0; i < vector.Length; i++)
                {
                    result[i] = (T)(ValueType)((float)(ValueType)vector[i] / (float)(ValueType)scalar);
                }

                return new MathVector<T>(result);
            }
            else
            {
                throw new NotSupportedException($"Type {typeof(T).Name} is not supported.");
            }
        }

        public override T Length(MathVector<T> mathVector)
        {
            checked
            {
                if (typeof(T) == typeof(int))
                {
                    var sum = 0;
                    for (var i = 0; i < mathVector.Length; i++)
                    {
                        var val = mathVector[i];
                        sum += (int)(ValueType)val * (int)(ValueType)val;
                    }

                    return (T) (ValueType) (int)Math.Sqrt(sum);
                }

                if (typeof(T) == typeof(float))
                {
                    var sum = 0F;
                    for (var i = 0; i < mathVector.Length; i++)
                    {
                        var val = mathVector[i];
                        sum += (float)(ValueType)val * (float)(ValueType)val;
                    }

                    return (T) (ValueType) (float) Math.Sqrt(sum);
                }

                throw new NotSupportedException();
            }
        }

        public override MathVector<T> Abs(MathVector<T> vector)
        {
            var result = new T[vector.Length];

            if (typeof(T) == typeof(int))
            {
                for (var i = 0; i < vector.Length; i++)
                {
                    result[i] = (T)(ValueType)(Math.Abs((int)(ValueType)vector[i]));
                }

                return new MathVector<T>(result);
            }
            else if (typeof(T) == typeof(float))
            {
                for (var i = 0; i < vector.Length; i++)
                {
                    result[i] = (T)(ValueType)(Math.Abs((float)(ValueType)vector[i]));
                }

                return new MathVector<T>(result);
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}