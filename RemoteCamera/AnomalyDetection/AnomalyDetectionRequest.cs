using RemoteCamera.Helpers;
using System.Collections.Generic;

namespace RemoteCamera.AnomalyDetection
{
    public class AnomalyDetectionRequest
    {
        public Inputs Inputs { get; set; }
        public GlobalParameters GlobalParameters { get; set; }

        public AnomalyDetectionRequest(IList<BrightnessDataPoint> brightnessData)
        {
            Inputs = new Inputs()
            {
                Data = new Data()
                {
                    ColumnNames = new string[]
                    {
                    "Time",
                    "Brightness"
                    },

                    Values = ConversionHelper.BrightnessDataToStringTable(brightnessData)
                }
            };
        }
    }

    public class Inputs
    {
        public Data Data { get; set; }
    }

    public class Data
    {
        public string[] ColumnNames { get; set; }
        public string[,] Values { get; set; }
    }
    
    public class GlobalParameters { }          
}
