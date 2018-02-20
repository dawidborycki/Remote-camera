#region Using

using Microsoft.Azure.Devices.Client;

#endregion

namespace RemoteCamera.AzureHelpers
{
    public static class Configuration
    {
        #region Properties

        public static string Hostname { get; } = "<TYPE_YOUR_HOSTNAME_HERE>";

        public static string DeviceId { get; } = "RemoteCamera";
        
        public static string DeviceKey { get; } = "<TYPE_YOUR_KEY_HERE>";

        #endregion

        #region Methods

        public static DeviceAuthenticationWithRegistrySymmetricKey AuthenticationKey()
        {
            return new DeviceAuthenticationWithRegistrySymmetricKey(DeviceId, DeviceKey);
        }

        #endregion
    }
}
