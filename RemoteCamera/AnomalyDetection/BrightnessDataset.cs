using System.Collections.Generic;

namespace RemoteCamera.AnomalyDetection
{
    public class BrightnessDataset
    {
        #region Fields

        private uint length;

        #endregion

        #region Properties

        public List<BrightnessDataPoint> Data { get; private set; }

        public bool IsFull
        {
            get { return Data.Count == length; }
        }

        #endregion

        #region Constructor

        public BrightnessDataset(uint length = 30)
        {
            this.length = length;
            Data = new List<BrightnessDataPoint>();
        }

        #endregion

        #region Methods (Public)

        public void Add(BrightnessDataPoint dataPoint)
        {
            Data.Add(dataPoint);

            if(Data.Count > length)
            {
                Data.RemoveAt(0);
            }
        }
        
        #endregion
    }
}
