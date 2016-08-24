using System;
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
