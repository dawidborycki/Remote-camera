using RemoteCamera.AnomalyDetection;
using System;
using System.Collections.Generic;

namespace RemoteCamera.Helpers
{
    public static class ConversionHelper
    {
        #region Methods (Public)

        public static string[,] BrightnessDataToStringTable(IList<BrightnessDataPoint> brightnessData)
        {
            ArgumentCheck.IsNull(brightnessData, "brightnessData");

            var dataLength = brightnessData.Count;
            var result = new string[dataLength, 2];

            for (var i = 0; i < dataLength; i++)
            {
                result[i, 0] = brightnessData[i].Time.ToString();
                result[i, 1] = brightnessData[i].Brightness.ToString();
            }

            return result;
        }

        public static IList<BrightnessDataPoint> AnomalyDetectionResponseToBrightnessData(AnomalyDetectionResponse response)
        {
            ArgumentCheck.IsNull(response, "response");

            var anomalousValues = response.Results.AnomalyDetectionResult.Value.Values;

            var result = new List<BrightnessDataPoint>();

            for(var i = 0; i < anomalousValues.GetLength(0); i++)
            {
                var brightnessDataPoint = new BrightnessDataPoint(
                    DateTime.Parse(anomalousValues[i, 0]), 
                    Byte.Parse(anomalousValues[i, 1]));

                result.Add(brightnessDataPoint);
            }

            return result;            
        }

        #endregion
    }
}
