namespace LinearRegressionForOneVariable
{
    public static class Generator
    {
        public static DataPoint[] Generate(int count)
        {
            var result = new DataPoint[count];

            for (var i = 0; i < count; i++)
            {
                result[i] = new DataPoint {X = i, Y = i };
            }

            return result;
        }
    }
}