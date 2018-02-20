using Microsoft.Azure.Devices.Shared;

namespace RemoteCamera.AzureHelpers
{
    public class DeviceInfo
    {
        #region Properties

        public bool IsSimulatedDevice { get; set; }

        public string Version { get; set; }

        public string ObjectType { get; set; }

        public DeviceProperties DeviceProperties { get; set; }

        public Twin Twin { get; set; }

        public Command[] Commands { get; set; }

        #endregion
    }
}
