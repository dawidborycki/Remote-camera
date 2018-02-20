#region Using

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.UI.Xaml.Controls;

#endregion

namespace RemoteCamera.Helpers
{
    public class CameraCapture
    {
        #region Properties

        public MediaCapture MediaCapture { get; private set; } = new MediaCapture();

        public bool IsPreviewActive { get; private set; } = false;

        public bool IsInitialized { get; private set; } = false;

        public uint FrameWidth { get; private set; }

        public uint FrameHeight { get; private set; }

        public static BitmapPixelFormat BitmapPixelFormat { get; } = BitmapPixelFormat.Bgra8;

        #endregion

        #region Methods (Public)

        public async Task Initialize(CaptureElement captureElement)
        {
            if (!IsInitialized)
            {
                var settings = new MediaCaptureInitializationSettings()
                {
                    StreamingCaptureMode = StreamingCaptureMode.Video
                };

                try
                {
                    await MediaCapture.InitializeAsync(settings);

                    GetVideoProperties();

                    captureElement.Source = MediaCapture;
                    IsInitialized = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);

                    IsInitialized = false;
                }
            }
        }

        public async Task Start()
        {
            if (IsInitialized)
            {
                if (!IsPreviewActive)
                {
                    await MediaCapture.StartPreviewAsync();

                    IsPreviewActive = true;
                }
            }
        }

        public async Task Stop()
        {
            if (IsInitialized)
            {
                if (IsPreviewActive)
                {
                    await MediaCapture.StopPreviewAsync();

                    IsPreviewActive = false;
                }
            }
        }

        public async Task<SoftwareBitmap> GetPreviewBitmap()
        {
            using (VideoFrame videoFrame = new VideoFrame(BitmapPixelFormat,
                (int)FrameWidth, (int)FrameHeight))
            {
                await MediaCapture.GetPreviewFrameAsync(videoFrame);

                return videoFrame.SoftwareBitmap;
            }
        }

        #endregion

        #region Methods (Private)

        private void GetVideoProperties()
        {
            if (MediaCapture != null)
            {
                var videoEncodingProperties = MediaCapture.VideoDeviceController.GetMediaStreamProperties(
                    MediaStreamType.VideoPreview) as VideoEncodingProperties;

                FrameWidth = videoEncodingProperties.Width;
                FrameHeight = videoEncodingProperties.Height;
            }
        }

        #endregion
    }
}
