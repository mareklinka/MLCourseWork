using System;

namespace LinearRegressionForOneVariable
{
    public sealed class CalculationResult
    {
        public CalculationResult(Func<float[], float> function, string functionString, int iterations, TimeSpan duration)
        {
            Result = function;
            Duration = duration;
            Iterations = iterations;
            FunctionString = functionString;
        }

        public TimeSpan Duration { get; }

        public int Iterations { get; }

        public Func<float[], float> Result { get; } 

        public string FunctionString { get; }
    }
}