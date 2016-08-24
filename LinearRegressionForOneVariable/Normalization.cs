using System;
using System.Collections.Generic;
using System.Linq;

namespace LinearRegressionForOneVariable
{
    public abstract class Normalization
    {
        public static Normalization Rescaling(params FeatureDefinition[] features)
        {
            return new RescalingAlgorithm(features);
        }

        public abstract DataPoint[] Normalize(DataPoint[] dataPoints);

        public abstract DataPoint Normalize(DataPoint dataPoint);

        private sealed class RescalingAlgorithm : Normalization
        {
            private readonly FeatureDefinition[] _features;

            private readonly Dictionary<string, Tuple<float, float>> _normalizationParameters = new Dictionary<string, Tuple<float, float>>();

            public RescalingAlgorithm(FeatureDefinition[] features)
            {
                _features = features;
            }

            public override DataPoint[] Normalize(DataPoint[] data)
            {
                _normalizationParameters.Clear();
                var result = new DataPoint[data.Length];

                foreach (var feature in _features)
                {
                    var min = data.Min(feature.Getter);
                    var max = data.Max(feature.Getter);
                    
                    _normalizationParameters.Add(feature.Name, new Tuple<float, float>(min, max));
                }

                for (var index = 0; index < data.Length; index++)
                {
                    var dataPoint = data[index];
                    result[index] = Normalize(dataPoint);
                }

                return result;
            }

            public override DataPoint Normalize(DataPoint dataPoint)
            {
                var newDataPoint = new DataPoint();

                foreach (var feature in _features)
                {
                    var min = _normalizationParameters[feature.Name].Item1;
                    var max = _normalizationParameters[feature.Name].Item2;

                    feature.Setter(newDataPoint, Rescale(feature.Getter(dataPoint), min, max));
                }

                return newDataPoint;
            }

            private static float Rescale(float value, float min, float max)
            {
                return (value - min) / (max - min);
            }
        }
    }
}