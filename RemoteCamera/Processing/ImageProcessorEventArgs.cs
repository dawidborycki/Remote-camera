using System;

namespace RemoteCamera.Processing
{
    public class ImageProcessorEventArgs : EventArgs
    {
        public byte Brightness { get; private set; }

        public ImageProcessorEventArgs(byte brightness)
        {
            Brightness = brightness;
        }               
    }
}