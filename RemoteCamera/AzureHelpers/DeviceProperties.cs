namespace RemoteCamera.AzureHelpers
{
    public class DeviceProperties
    {
        #region Properties

        public string DeviceID { get; set; }

        public bool HubEnabledState { get; set; }

        public string DeviceState { get; set; }

        public string Manufacturer { get; set; }

        public string ModelNumber { get; set; }

        public string SerialNumber { get; set; }

        public string FirmwareVersion { get; set; }

        public string AvailablePowerSources { get; set; }

        public string PowerSourceVoltage { get; set; }

        public string BatteryLevel { get; set; }

        public string MemoryFree { get; set; }

        public string Platform { get; set; }

        public string Processor { get; set; }

        public string InstalledRAM { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        #endregion

        #region Constructor

        public DeviceProperties(string deviceId)
        {
            DeviceID = deviceId;
            SetDefaultValues();
        }

        #endregion

        #region Methods

        private void SetDefaultValues()
        {
            HubEnabledState = true;
            DeviceState = "normal";
            Manufacturer = "Dawid Borycki";
            ModelNumber = "Remote Camera";
            SerialNumber = "HD-3000";
            FirmwareVersion = "1.0";
            AvailablePowerSources = "1";
            PowerSourceVoltage = "5 V";
            BatteryLevel = "N/A";
            MemoryFree = "N/A";
            Platform = "Windows 10 IoT Core";
            Processor = "ARM";
            InstalledRAM = "1 GB";
            Latitude = 47.6063889;
            Longitude = -122.3308333;
        }

        #endregion
    }
}
