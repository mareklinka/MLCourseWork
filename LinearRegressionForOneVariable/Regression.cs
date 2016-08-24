using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace LinearRegressionForOneVariable
{
    public abstract class Regression
    {
        public static Regression Linear(Normalization normalization)
        {
            return new LinearRegression(normalization);
        }

        protected Regression(Normalization normalization)
        {
            Normalization = normalization;
        }

        protected Normalization Normalization { get; private set; }

        public abstract CalculationResult Compute(DataPoint[] trainingData);

        private sealed class LinearRegression : Regression
        {
            public override CalculationResult Compute(DataPoint[] data)
            {
                if (Vector.IsHardwareAccelerated)
                {
                    return CalculateVector(Normalization.Normalize(data));
                }
                else
                {
                    return CalculateBasic(Normalization.Normalize(data));
                }
            }

            private static CalculationResult CalculateBasic(DataPoint[] data)
            {
                // must find local minimum of mean square error function mse(x) = 1/2m * sum((k0 + k1*x - y)^2)
                // using batch gradient descent:
                var ik0 = 0.0F; // initial state
                var ik1 = 0.0F; // initial state
                const float alpha = 0.75F; // learning speed
                var m = (float) data.Length;
                var count = 0;
                const float epsilon = 0.000001F; // sensitivity
                var sw = new Stopwatch();
                sw.Start();
                while (true)
                {
                    ++count;
                    var derivativeInK0 = 1/m*(data.Sum(d => (ik0 + d.X*ik1 - d.Y))); // partial derivation in k0
                    var derivativeInK1 = 1/m*(data.Sum(d => (ik0 + d.X*ik1 - d.Y)*d.X)); // partial derivation in k1

                    var newK0 = ik0 - alpha*(derivativeInK0);
                    var newK1 = ik1 - alpha*(derivativeInK1);

                    if (Math.Abs(newK0 - ik0) < epsilon && Math.Abs(newK1 - ik1) < epsilon)
                    {
                        sw.Stop();
                        Func<float[], float> f = features => ik0 + (ik1*features[0]);
                        return new CalculationResult(f, $"h(x) = {ik0:F4} + {ik1:F4}*x", count, sw.Elapsed);
                    }

                    ik0 = newK0;
                    ik1 = newK1;
                }
            }

            private static CalculationResult CalculateVector(DataPoint[] data)
            {
                // must find local minimum of mean square error function mse(x) = 1/2m * sum((k0 + k1*x - y)^2)
                // using batch gradient descent:
                var ik0 = 0.0F; // initial state
                var ik1 = 0.0F; // initial state
                const float alpha = 0.75F; // learning speed
                var m = (float)data.Length;
                var count = 0;
                var epsilon = 0.000001F; // sensitivity
                var sw = new Stopwatch();

                // vectorize features
                var doubles = data.Select(_ => _.X).ToArray();
                var vectorsX = Vectorize(doubles);

                // vectorize values
                doubles = data.Select(_ => _.Y).ToArray();
                var vectorsY = Vectorize(doubles);

                var fractionVector = new Vector<float>(1 / m);
                sw.Start();

                while (true)
                {
                    ++count;
                    var k0vector = new Vector<float>(ik0);
                    var k1vector = new Vector<float>(ik1);
                    var derivativeInK0Vector = Vector<float>.Zero;
                    var derivativeInK1Vector = Vector<float>.Zero;

                    for (var index = 0; index < vectorsX.Count; index++)
                    {
                        var x = vectorsX[index];
                        var y = vectorsY[index];
                        derivativeInK0Vector += k0vector + x * k1vector - y;
                        derivativeInK1Vector += (k0vector + x * k1vector - y) * x;
                    }

                    derivativeInK0Vector *= fractionVector;
                    derivativeInK1Vector *= fractionVector;

                    // sum the result vector for k0
                    var derivativeInK0 = SumVector(derivativeInK0Vector);

                    // sum the result vector for k1
                    var derivativeInK1 = SumVector(derivativeInK1Vector);

                    var newK0 = ik0 - alpha * derivativeInK0;
                    var newK1 = ik1 - alpha * derivativeInK1;

                    // check tolerance - if we are close enough, return
                    if (Math.Abs(newK0 - ik0) < epsilon && Math.Abs(newK1 - ik1) < epsilon)
                    {
                        sw.Stop();
                        Func<float[], float> f = features => ik0 + (ik1 * features[0]);
                        return new CalculationResult(f, $"h(x) = {ik0:F4} + {ik1:F4}*x", count, sw.Elapsed);
                    }

                    ik0 = newK0;
                    ik1 = newK1;
                }
            }

            private static List<Vector<float>> Vectorize(float[] data)
            {
                var vectorsX = new List<Vector<float>>();
                for (var i = 0; i < Math.Ceiling(data.Length / (float)Vector<float>.Count); i = ++i)
                {
                    vectorsX.Add(new Vector<float>(data, i * Vector<float>.Count));
                }
                return vectorsX;
            }

            private static float SumVector(Vector<float> input)
            {
                var result = 0.0F;
                for (var i = 0; i < Vector<float>.Count; ++i)
                {
                    result += input[i];
                }
                return result;
            }

            public LinearRegression(Normalization normalization) : base(normalization)
            {
            }
        }
    }
}