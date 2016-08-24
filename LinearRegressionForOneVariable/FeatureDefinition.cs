using System;

namespace LinearRegressionForOneVariable
{
    public sealed class FeatureDefinition
    {
        public FeatureDefinition(string name, Func<DataPoint, float> getter, Action<DataPoint, float> setter)
        {
            Name = name;
            Getter = getter;
            Setter = setter;
        }

        public string Name { get; }

        public Func<DataPoint, float> Getter { get; }

        public Action<DataPoint, float> Setter { get; }
    }
}