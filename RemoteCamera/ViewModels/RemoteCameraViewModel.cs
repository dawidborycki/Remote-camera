#region Using

using RemoteCamera.AnomalyDetection;
using RemoteCamera.AzureHelpers;
using RemoteCamera.Helpers;
using RemoteCamera.Processing;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

#endregion

namespace RemoteCamera.ViewModels
{
    public class RemoteCameraViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Properties
        
        public byte Brightness
        {
            get { return brightness; }
            set
            {
                brightness = value;
                OnPropertyChanged();
            }
        }

        public bool IsPreviewActive
        {
            get { return isPreviewActive; }
            set
            {
                isPreviewActive = value;
                OnPropertyChanged();
                OnPropertyChanged("IsTrainingButtonEnabled");
            }
        }

        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                isConnected = value;
                OnPropertyChanged();
            }
        }

        public bool IsTelemetryActive
        {
            get { return isTelemetryActive; }
            set
            {
                isTelemetryActive = value;
                OnPropertyChanged();
            }
        }

        public bool IsTrainingActive
        {
            get { return isTrainingActive; }
            set
            {
                isTrainingActive = value;
                OnPropertyChanged();
                OnPropertyChanged("IsTrainingButtonEnabled");
            }
        }

        public bool IsTrainingButtonEnabled
        {
            get
            {
                return IsPreviewActive && !IsTrainingActive;
            }            
        }

        public bool IsAnomalyDetectionActive
        {
            get { return isAnomalyDetectionActive; }
            set
            {
                isAnomalyDetectionActive = value;
                OnPropertyChanged();
            }
        }

        public string TrainingDataSetFilePath
        {
            get { return trainingDataSetFilePath; }
            set
            {
                trainingDataSetFilePath = value;
                OnPropertyChanged();
            }
        }

        public ImageProcessor ImageProcessor { get; private set; }

        public CloudHelper CloudHelper { get; private set; }

        public AnomalyDetector AnomalyDetector { get; private set; }

        public ObservableCollection<BrightnessDataPoint> AnomalousValues { get; set; } = new ObservableCollection<BrightnessDataPoint>();

        #endregion

        #region Fields

        private CameraCapture cameraCapture = new CameraCapture();
        private CaptureElement captureElement;        

        private byte brightness;
        private bool isPreviewActive;
        private bool isConnected;
        private bool isTelemetryActive;
        private bool isTrainingActive;
        private bool isAnomalyDetectionActive;
        private string trainingDataSetFilePath;        

        #endregion

        #region Constructor

        public RemoteCameraViewModel(CaptureElement captureElement)
        {
            // Check if CaptureElement is not null and throw an exception accordingly
            ArgumentCheck.IsNull(captureElement, "captureElement");

            // Store reference to the CaptureElement
            this.captureElement = captureElement;

            // Instantiate CameraCapture and ImageProcessor
            cameraCapture = new CameraCapture();
            ImageProcessor = new ImageProcessor(cameraCapture);

            // Instantiate CloudHelper
            CloudHelper = new CloudHelper();

            // Instantiate AnomalyDetector
            AnomalyDetector = new AnomalyDetector();
        }

        #endregion

        #region Methods (Public)

        public async Task PreviewStart()
        {            
            if(!cameraCapture.IsInitialized)
            {
                await cameraCapture.Initialize(captureElement);
            }

            if (!cameraCapture.IsPreviewActive)
            {
                await cameraCapture.Start();
            }

            IsPreviewActive = cameraCapture.IsPreviewActive;

            UpdateProcessing();
        }

        public async Task PreviewStop()
        {
            if(cameraCapture.IsPreviewActive)
            {
                await cameraCapture.Stop();
            }

            IsPreviewActive = cameraCapture.IsPreviewActive;

            UpdateProcessing();
        }

        public async Task Connect()
        {
            await CloudHelper.Initialize();
            
            IsConnected = true;
        }        

        public void AcquireTrainingData()
        {
            IsTrainingActive = true;
        }

        #endregion

        #region Methods (Private)

        private void UpdateProcessing()
        {
            if(IsPreviewActive)
            {
                ImageProcessor.Start();                
            }
            else
            {
                ImageProcessor.Stop();
                Brightness = 0;
            }
        }

        #endregion        
    }
}
