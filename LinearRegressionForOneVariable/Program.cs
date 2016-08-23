using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace LinearRegressionForOneVariable
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($".NET 4.6 Vector SIMD acceleration enabled: {Vector.IsHardwareAccelerated}");
            Console.WriteLine();
            // need a function in the form of h(x) = k0 + k1*x
            // must minimize k0, k1 such that they are close to training data
            while (true)
            {
                LinearRegression();
            }           
        }

        private static void LinearRegression()
        {
            Console.Write("Number of data points to generate: ");
            var count = int.Parse(Console.ReadLine());
            var trainingData = Generator.Generate(count);
            // normalization of both axes to prevent overflowing the double type
            // normalization on Y will cause the returned k0 to be scaled as well
            // normalization will have no effect on the X axis and/or k1
            var normalized = Normalization.Rescale(trainingData);
            double k0;
            double k1;
            var result = LinearRegressionForOneVariable.LinearRegression.CalculateVector(normalized, out k0, out k1);

            Console.WriteLine("Vector implementation:");
            Console.WriteLine($"Linear regression complete: h(x) = {k0:F4} + {k1:F4}*x");
            Console.WriteLine(
                $"Result reached in {result.Iterations} gradient iterations and {result.Duration.TotalMilliseconds} ms");

            var result2 = LinearRegressionForOneVariable.LinearRegression.Calculate(normalized, out k0, out k1);
            Console.WriteLine("Normal implementation:");
            Console.WriteLine($"Linear regression complete: h(x) = {k0:F4} + {k1:F4}*x");
            Console.WriteLine(
                $"Result reached in {result2.Iterations} gradient iterations and {result2.Duration.TotalMilliseconds} ms");
        }
    }

    public static class Normalization
    {
        public static DataPoint[] Rescale(DataPoint[] data)
        {
            var result = new DataPoint[data.Length];
            var maxX = data.Max(_ => _.X);
            var minX = data.Min(_ => _.X);
            var maxY = data.Max(_ => _.Y);
            var minY = data.Min(_ => _.Y);

            for (var i = 0; i < data.Length; i++)
            {
                var point = data[i];
                result[i] = new DataPoint {X = Rescale(point.X, minX, maxX), Y = Rescale(point.Y, minY, maxY)};
            }

            return result;
        }

        private static double Rescale(double value, double min, double max)
        {
            return (value - min)/(max - min);
        }
    }

    public static class LinearRegression
    {
        public static CalculationResult Calculate(DataPoint[] data, out double k0, out double k1)
        {
            // must find local minimum of mean square error function mse(x) = 1/2m * sum((k0 + k1*x - y)^2)
            // using batch gradient descent:
            var ik0 = 0.0; // initial state
            var ik1 = 0.0; // initial state
            const double alpha = 0.75; // learning speed
            var m = (double)data.Length;
            var count = 0;
            const double epsilon = 0.000000001; // sensitivity
            var sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                ++count;
                var derivativeInK0 = 1/m*(data.Sum(d =>(ik0 + d.X*ik1 - d.Y))); // partial derivation in k0
                var derivativeInK1 = 1/m*(data.Sum(d =>(ik0 + d.X * ik1 - d.Y) * d.X)); // partial derivation in k1

                var newK0 = ik0 - alpha*(derivativeInK0);
                var newK1 = ik1 - alpha*(derivativeInK1);

                if (Math.Abs(newK0 - ik0) < epsilon && Math.Abs(newK1 - ik1) < epsilon)
                {
                    k0 = ik0;
                    k1 = ik1;
                    sw.Stop();
                    return new CalculationResult(count, sw.Elapsed);
                }

                ik0 = newK0;
                ik1 = newK1;
            }
        }

        public static CalculationResult CalculateVector(DataPoint[] data, out double k0, out double k1)
        {
            // must find local minimum of mean square error function mse(x) = 1/2m * sum((k0 + k1*x - y)^2)
            // using batch gradient descent:
            var ik0 = 0.0; // initial state
            var ik1 = 0.0; // initial state
            const double alpha = 0.75; // learning speed
            var m = (double)data.Length;
            var count = 0;
            var epsilon = 0.000000001; // sensitivity
            var sw = new Stopwatch();

            // vectorize features
            var doubles = data.Select(_ => _.X).ToArray();
            var vectorsX = Vectorize(doubles);

            // vectorize values
            doubles = data.Select(_ => _.Y).ToArray();
            var vectorsY = Vectorize(doubles);

            var fractionVector = new Vector<double>(1/m);
            sw.Start();

            while (true)
            {
                ++count;
                var k0vector = new Vector<double>(ik0);
                var k1vector = new Vector<double>(ik1);
                var derivativeInK0Vector = Vector<double>.Zero;
                var derivativeInK1Vector = Vector<double>.Zero;

                for (var index = 0; index < vectorsX.Count; index++)
                {
                    var x = vectorsX[index];
                    var y = vectorsY[index];
                    derivativeInK0Vector += k0vector + x*k1vector - y;
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
                    k0 = ik0;
                    k1 = ik1;
                    sw.Stop();
                    return new CalculationResult(count, sw.Elapsed);
                }

                ik0 = newK0;
                ik1 = newK1;
            }
        }

        private static List<Vector<double>> Vectorize(double[] doubles)
        {
            var vectorsX = new List<Vector<double>>();
            for (var i = 0; i < Math.Ceiling(doubles.Length/(double) Vector<double>.Count); i = ++i)
            {
                vectorsX.Add(new Vector<double>(doubles, i * Vector<double>.Count));
            }
            return vectorsX;
        }

        private static double SumVector(Vector<double> derivativeInK1Vector)
        {
            var result = 0.0;
            for (var i = 0; i < Vector<double>.Count; ++i)
            {
                result += derivativeInK1Vector[i];
            }
            return result;
        }

        public sealed class CalculationResult
        {
            public CalculationResult(int iterations, TimeSpan duration)
            {
                Duration = duration;
                Iterations = iterations;
            }

            public TimeSpan Duration { get; private set; }

            public int Iterations { get; private set; }
        }
    }

    public struct DataPoint
    {
        /// <summary>
        /// Feature.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Target.
        /// </summary>
        public double Y { get; set; }
    }

    public static class Generator
    {
        public static DataPoint[] Generate(int count)
        {
            var r = new Random();
            var result = new DataPoint[count];

            for (var i = 0; i < count; i++)
            {
                result[i] = new DataPoint {X = i, Y = i+Math.Sin(i) * i };
            }

            return result;
        }
    }
}
