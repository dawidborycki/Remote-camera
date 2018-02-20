using System;
using System.Collections.Generic;

namespace RemoteCamera.AnomalyDetection
{
    public class AnomalyDetectedEventArgs : EventArgs
    {
        #region Properties

        public IList<BrightnessDataPoint> AnomalousValues { get; private set; }

        #endregion

        #region Constructor

        public AnomalyDetectedEventArgs(IList<BrightnessDataPoint> anomalousValues)
        {
            AnomalousValues = anomalousValues;
        }

        #endregion
    }
}