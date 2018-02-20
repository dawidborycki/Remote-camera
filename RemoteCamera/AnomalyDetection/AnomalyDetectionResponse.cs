namespace RemoteCamera.AnomalyDetection
{
    public class AnomalyDetectionResponse
    {
        public Results Results { get; set; }
    }

    public class Results
    {
        public AnomalyDetectionResult AnomalyDetectionResult { get; set; }
    }

    public class AnomalyDetectionResult
    {
        public string Type { get; set; }
        public Value Value { get; set; }
    }

    public class Value
    {
        public string[] ColumnNames { get; set; }
        public string[] ColumnTypes { get; set; }
        public string[,] Values { get; set; }
    }
}
