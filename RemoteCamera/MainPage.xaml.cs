#region Using

using RemoteCamera.AnomalyDetection;
using RemoteCamera.AzureHelpers;
using RemoteCamera.Helpers;
using RemoteCamera.Processing;
using RemoteCamera.ViewModels;
using Windows.UI.Xaml.Controls;

#endregion

namespace RemoteCamera
{
    public sealed partial class MainPage : Page
    {
        #region Fields

        private RemoteCameraViewModel remoteCameraViewModel;

        #endregion

        #region Constructor

        public MainPage()
        {
            InitializeComponent();

            remoteCameraViewModel = new RemoteCameraViewModel(CaptureElementPreview);

            remoteCameraViewModel.ImageProcessor.ProcessingDone += ImageProcessor_ProcessingDone;
            remoteCameraViewModel.CloudHelper.UpdateCameraPreviewCommandReceived += CloudHelper_UpdateCameraPreviewCommandReceived;

            remoteCameraViewModel.AnomalyDetector.TrainingDataReady += AnomalyDetector_TrainingDataReady;
            remoteCameraViewModel.AnomalyDetector.AnomalyDetected += AnomalyDetector_AnomalyDetected;

            ThreadHelper.Dispatcher = Dispatcher;
        }

        #endregion

        #region Methods

        private async void ImageProcessor_ProcessingDone(object sender, ImageProcessorEventArgs e)
        {
            // Update display through dispatcher
            DisplayBrightness(e.Brightness);

            // Send telemetry
            if (remoteCameraViewModel.IsTelemetryActive)
            {
                remoteCameraViewModel.CloudHelper.SendBrightness(e.Brightness);
            }

            // Anomaly detection (training)
            if (remoteCameraViewModel.IsTrainingActive)
            {
                await remoteCameraViewModel.AnomalyDetector.AddTrainingValue(e.Brightness);
            }

            // Anomaly detection
            if (remoteCameraViewModel.IsAnomalyDetectionActive)
            {
                await remoteCameraViewModel.AnomalyDetector.AddTestValue(e.Brightness);
            }
        }

        private async void DisplayBrightness(byte brightness)
        {
            await ThreadHelper.InvokeOnMainThread(() =>
            {
                remoteCameraViewModel.Brightness = brightness;
            });
        }

        private async void CloudHelper_UpdateCameraPreviewCommandReceived(object sender, UpdateCameraPreviewCommandEventArgs e)
        {
            await ThreadHelper.InvokeOnMainThread(async () =>
            {
                if (e.IsPreviewActive)
                {
                    await remoteCameraViewModel.PreviewStart();
                }
                else
                {
                    await remoteCameraViewModel.PreviewStop();
                }
            });
        }

        private async void AnomalyDetector_TrainingDataReady(object sender, TrainingDataReadyEventArgs e)
        {
            await ThreadHelper.InvokeOnMainThread(() =>
            {
                remoteCameraViewModel.IsTrainingActive = false;
                remoteCameraViewModel.TrainingDataSetFilePath = e.FilePath;
            });
        }

        private async void AnomalyDetector_AnomalyDetected(object sender, AnomalyDetectedEventArgs e)
        {
            await ThreadHelper.InvokeOnMainThread(() =>
            {
                foreach (var anomalousValue in e.AnomalousValues)
                {
                    if (!remoteCameraViewModel.AnomalousValues.Contains(anomalousValue))
                    {
                        remoteCameraViewModel.AnomalousValues.Add(anomalousValue);
                    }
                }
            });
        }

        #endregion
    }
}