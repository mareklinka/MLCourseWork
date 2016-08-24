using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace LinearRegressionForOneVariable
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($".NET 4.6 Vector SIMD acceleration enabled: {Vector.IsHardwareAccelerated}");
            Console.WriteLine();

            while (true)
            {
                LinearRegression();
            }            
        }

        private static void BenchmarkSum()
        {
            var count = 100000000;
            var data = new int[count]; // 10m floats

            var r = new Random();
            for (var i = 0; i < count; i++)
            {
                data[i] = r.Next(0, 2);
            }

            var sw = new Stopwatch();
            sw.Start();
            var sum = data.Sum();
            sw.Stop();

            Console.WriteLine($"Normal sum: {sum:N} in {sw.Elapsed.TotalMilliseconds} ms");

            sw.Reset();
            sw.Start();

            sum = data.AsParallel().Sum();

            sw.Stop();
            Console.WriteLine($"AsParallel normal sum: {sum:N} in {sw.Elapsed.TotalMilliseconds} ms");

            sw.Reset();
            var lockObject = new object();
            sum = 0;
            sw.Start();

            Parallel.ForEach(
                // The values to be aggregated 
                data,

                // The local initial partial result
                () => 0,

                // The loop body
                (v, loopState, partialResult) => v + partialResult,

                // The final step of each local context            
                (localPartialSum) =>
                {
                    // Enforce serial access to single, shared result
                    lock (lockObject)
                    {
                        sum += localPartialSum;
                    }
                });

            sw.Stop();
            Console.WriteLine($"Parallel.ForEach normal sum: {sum:N} in {sw.Elapsed.TotalMilliseconds} ms");
            Console.WriteLine();
            sw.Reset();

            var vectorized = Vectorize(data);
            sw.Start();
            var sumVector = Vector<int>.Zero;
            foreach (var vector in vectorized)
            {
                sumVector += vector;
            }

            sum = SumVector(sumVector);
            sw.Stop();

            Console.WriteLine($"Simple vector sum: {sum:N} in {sw.Elapsed.TotalMilliseconds} ms");

            sw.Reset();
            sw.Start();

            sum = vectorized.AsParallel().Aggregate(Vector<int>.Zero, (v1, v2) => v1 + v2, SumVector);

            sw.Stop();
            Console.WriteLine($"AsParallel vector sum: {sum:N} in {sw.Elapsed.TotalMilliseconds} ms");

            sw.Reset();
            lockObject = new object();
            sum = 0;
            sw.Start();

            Parallel.ForEach(
                // The values to be aggregated 
                vectorized,

                // The local initial partial result
                () => Vector<int>.Zero,

                // The loop body
                (v, loopState, partialResult) => v + partialResult,

                // The final step of each local context            
                (localPartialSum) =>
                {
                    // Enforce serial access to single, shared result
                    lock (lockObject)
                    {
                        sum += SumVector(localPartialSum);
                    }
                });

            sw.Stop();
            Console.WriteLine($"Parallel.ForEach vector sum: {sum:N} in {sw.Elapsed.TotalMilliseconds} ms");
        }


        private static int SumVector(Vector<int> input)
        {
            var result = 0;
            for (var i = 0; i < Vector<int>.Count; ++i)
            {
                result += input[i];
            }
            return result;
        }

        private static List<Vector<int>> Vectorize(int[] data)
        {
            var vectorsX = new List<Vector<int>>();
            for (var i = 0; i < Math.Ceiling(data.Length / (float)Vector<int>.Count); i = ++i)
            {
                vectorsX.Add(new Vector<int>(data, i * Vector<int>.Count));
            }
            return vectorsX;
        }

        private static void LinearRegression()
        { 
            // need a function in the form of h(x) = k0 + k1*x
            // must minimize k0, k1 such that they are close to training data
            Console.Write("Number of data points to generate: ");
            var count = int.Parse(Console.ReadLine());
            var trainingData = Generator.Generate(count);

            var result =
                Regression.Linear(Normalization.Rescaling(new FeatureDefinition("X", _ => _.X, (p, f) => p.X = f),
                        new FeatureDefinition("Y", _ => _.Y, (p, f) => p.Y = f)))
                    .Compute(trainingData);
            
            Console.WriteLine($"Linear regression complete: {result.FunctionString}");
            Console.WriteLine(
                $"Result reached in {result.Iterations} gradient iterations and {result.Duration.TotalMilliseconds} ms");
            
            Console.WriteLine("Predicting random values:");

            var r = new Random();
            for (var i = 0; i < 10; i++)
            {
                var x = (float) r.Next(100, 200);
                var y = result.Result(new[] {x});

                Console.WriteLine($"X = {x}; Y = {y:F4}");
            }
        }
    }
}
