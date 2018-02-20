#region Using

using RemoteCamera.Helpers;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;

#endregion

namespace RemoteCamera.Processing
{
    public class ImageProcessor
    {
        #region Properties

        public bool IsActive { get; set; } = false;

        #endregion

        #region Events

        public event EventHandler<ImageProcessorEventArgs> ProcessingDone;

        #endregion

        #region Fields

        private Task processingTask;
        private CancellationTokenSource processingCancellationTokenSource;

        private TimeSpan delay = TimeSpan.FromMilliseconds(500);

        private CameraCapture cameraCapture;

        #endregion

        #region Constructor

        public ImageProcessor(CameraCapture cameraCapture)
        {
            ArgumentCheck.IsNull(cameraCapture, "cameraCapture");

            this.cameraCapture = cameraCapture;
        }

        #endregion

        #region Methods

        public void Start()
        {
            if (!IsActive)
            {
                InitializeProcessingTask();

                processingTask.Start();

                IsActive = true;
            }
        }

        public void Stop()
        {
            if (IsActive)
            {
                IsActive = false;

                processingCancellationTokenSource.Cancel();                

                processingTask.Wait();                
            }
        }

        private void InitializeProcessingTask()
        {
            processingCancellationTokenSource = new CancellationTokenSource();

            processingTask = new Task(async () =>
            {
                while (!processingCancellationTokenSource.IsCancellationRequested)
                {
                    if (IsActive)
                    {
                        var brightness = await GetBrightness();

                        ProcessingDone(this, new ImageProcessorEventArgs(brightness));

                        Task.Delay(delay).Wait();
                    }
                }
            }, processingCancellationTokenSource.Token);
        }

        private async Task<byte> GetBrightness()
        {
            var brightness = new byte();

            if (cameraCapture.IsPreviewActive)
            {
                // Get current preview bitmap
                var previewBitmap = await cameraCapture.GetPreviewBitmap();

                // Get underlying pixel data
                var pixelBuffer = GetPixelBuffer(previewBitmap);

                // Process buffer to determine mean gray value (brightness)
                brightness = CalculateMeanGrayValue(pixelBuffer);
            }

            return brightness;
        }

        private byte[] GetPixelBuffer(SoftwareBitmap softwareBitmap)
        {
            // Ensure bitmap pixel format is Bgra8
            if (softwareBitmap.BitmapPixelFormat != CameraCapture.BitmapPixelFormat)
            {
                SoftwareBitmap.Convert(softwareBitmap, CameraCapture.BitmapPixelFormat);
            }

            // Lock underlying bitmap buffer
            var bitmapBuffer = softwareBitmap.LockBuffer(BitmapBufferAccessMode.Read);

            // Use plane description to determine bitmap height and stride (the actual buffer width) 
            var planeDescription = bitmapBuffer.GetPlaneDescription(0);
            var pixelBuffer = new byte[planeDescription.Height * planeDescription.Stride];

            // Copy pixel data to a buffer
            softwareBitmap.CopyToBuffer(pixelBuffer.AsBuffer());

            return pixelBuffer;
        }

        private byte CalculateMeanGrayValue(byte[] pixelBuffer)
        {
            // Loop index increases by four since 
            // there are four channels (blue, green, red and alpha). 
            // Alpha is ignored for brightness calculation
            const int step = 4;
            double mean = 0.0;
            for (uint i = 0; i < pixelBuffer.Length; i += step)
            {
                mean += GetGrayscaleValue(pixelBuffer, i);
            }

            mean /= (pixelBuffer.Length / step);

            return Convert.ToByte(mean);
        }

        private static byte GetGrayscaleValue(byte[] pixelBuffer, uint startIndex)
        {
            var grayValue = (pixelBuffer[startIndex]
                + pixelBuffer[startIndex + 1]
                + pixelBuffer[startIndex + 2]) / 3.0;

            return Convert.ToByte(grayValue);
        }
    }

    #endregion
}
