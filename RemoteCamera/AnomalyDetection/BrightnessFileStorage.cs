using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Streams;

namespace RemoteCamera.AnomalyDetection
{
    public class BrightnessFileStorage
    {
        #region Properties

        public string FilePath
        {
            get { return workingFile.Path; }
        }

        #endregion

        #region Fields

        private const string fileName = "BrightnessData.csv";
        
        private string folderName = Package.Current.DisplayName;

        private StorageFolder workingFolder;
        private StorageFile workingFile;

        #endregion

        #region Constructor
        
        private BrightnessFileStorage() { }

        #endregion

        #region Methods

        public static async Task<BrightnessFileStorage> CreateAsync()
        {
            var temperatureFileStorage = new BrightnessFileStorage();

            await temperatureFileStorage.PrepareFolder();
            await temperatureFileStorage.PrepareFile();

            return temperatureFileStorage;
        }

        public async Task WriteData(List<BrightnessDataPoint> brightnessDataSet)
        {
            var randomAccessStream = await workingFile.OpenAsync(FileAccessMode.ReadWrite);

            using (var dataWriter = new DataWriter(randomAccessStream))
            {
                // Write header
                WriteHeader(dataWriter);

                // Write data
                foreach (var dataPoint in brightnessDataSet)
                {
                    WriteLine(dataWriter, dataPoint);
                }

                await dataWriter.StoreAsync();
            }
        }        

        private async Task PrepareFolder()
        {
            var storageFolder = ApplicationData.Current.TemporaryFolder;

            // Check if folder already exists
            var storageItem = await storageFolder.TryGetItemAsync(folderName);

            if (storageItem == null)
            {
                // ... if not, create one
                storageFolder = await storageFolder.CreateFolderAsync(folderName);
            }
            else
            {
                storageFolder = (StorageFolder)storageItem;
            }

            workingFolder = storageFolder;
        }

        private async Task PrepareFile()
        {
            // Create file, overwriting the previous one
            workingFile = await workingFolder.CreateFileAsync(fileName,
                CreationCollisionOption.ReplaceExisting);
        }

        private void WriteHeader(DataWriter dataWriter)
        {
            dataWriter.WriteString("Time");
            dataWriter.WriteString(",");
            dataWriter.WriteString("Brightness\r\n");
        }

        private void WriteLine(DataWriter dataWriter, BrightnessDataPoint dataPoint)
        {            
            dataWriter.WriteString(dataPoint.Time.ToString("MM/dd/yyyy hh:mm:ss"));
            dataWriter.WriteString(",");
            dataWriter.WriteString(dataPoint.Brightness.ToString());
            dataWriter.WriteString("\r\n");
        }

        #endregion
    }
}
