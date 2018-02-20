using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RemoteCamera.AnomalyDetection
{
    public class AnomalyDetector
    {
        #region Events

        public event EventHandler<TrainingDataReadyEventArgs> TrainingDataReady;

        public event EventHandler<AnomalyDetectedEventArgs> AnomalyDetected;

        #endregion

        #region Fields
        
        private const int trainingDataSetLength = 100;

        private List<BrightnessDataPoint> trainingDataSet = new List<BrightnessDataPoint>();

        private BrightnessDataset dataSet = new BrightnessDataset();

        private AnomalyDetectionClient anomalyDetectionClient = new AnomalyDetectionClient();

        #endregion

        #region Methods (Public)

        public async Task AddTrainingValue(byte brightness)
        {
            trainingDataSet.Add(new BrightnessDataPoint(brightness));

            // Check if all data points were acquired
            if (trainingDataSet.Count == trainingDataSetLength)
            {
                // If so, save them to csv file
                var brightnessFileStorage = await BrightnessFileStorage.CreateAsync();
                await brightnessFileStorage.WriteData(trainingDataSet);

                // ... and inform listeners that the training data set is ready
                TrainingDataReady?.Invoke(this,
                    new TrainingDataReadyEventArgs(brightnessFileStorage.FilePath));
            }
        }

        public async Task AddTestValue(byte brightness)
        {
            dataSet.Add(new BrightnessDataPoint(brightness));

            if (dataSet.IsFull)
            {
                try
                {
                    var anomalousValues = await anomalyDetectionClient.
                        DetectAnomalyAsync(dataSet.Data);

                    if (anomalousValues.Count > 0)
                    {
                        AnomalyDetected?.Invoke(this,
                            new AnomalyDetectedEventArgs(anomalousValues));
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }

        #endregion        
    }
}
