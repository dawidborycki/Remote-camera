using System;

namespace RemoteCamera.AnomalyDetection
{
    public class TrainingDataReadyEventArgs : EventArgs
    {
        public string FilePath { get; private set; }

        public TrainingDataReadyEventArgs(string filePath)
        {
            FilePath = filePath;
        }
    }
}
