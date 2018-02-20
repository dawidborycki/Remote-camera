using System;

namespace RemoteCamera.AnomalyDetection
{
    public struct BrightnessDataPoint
    {
        public DateTime Time { get; private set; }
        public byte Brightness { get; private set; }

        public BrightnessDataPoint(byte brightness)
        {
            Time = DateTime.Now;
            Brightness = brightness;
        }

        public BrightnessDataPoint(DateTime time, byte brightness)
            : this(brightness)
        {
            Time = time;
        }
    }
}
