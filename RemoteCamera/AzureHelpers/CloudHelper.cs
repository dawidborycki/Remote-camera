#region Using

using Microsoft.Azure.Devices.Client;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

#endregion

namespace RemoteCamera.AzureHelpers
{
    public class CloudHelper
    {
        #region Properties

        public bool IsInitialized { get; private set; } = false;

        #endregion

        #region Events

        public event EventHandler<UpdateCameraPreviewCommandEventArgs> UpdateCameraPreviewCommandReceived;

        #endregion

        #region Fields

        private DeviceClient deviceClient;

        #endregion

        #region Methods (Public)

        public async Task Initialize()
        {
            if (!IsInitialized)
            {
                deviceClient = DeviceClient.Create(Configuration.Hostname,
                    Configuration.AuthenticationKey());

                await deviceClient.OpenAsync();

                IsInitialized = true;

                BeginRemoteCommandHandling();
            }
        }

        public async void SendBrightness(byte brightness)
        {
            if (IsInitialized)
            {
                // Construct TelemetryData
                var telemetryData = new TelemetryData()
                {
                    Brightness = brightness
                };

                // Serialize TelemetryData and send it to the cloud
                await SendMessage(telemetryData);
            }
        }

        public async Task SendDeviceInfo()
        {            
            var deviceInfo = new DeviceInfo()
            {
                IsSimulatedDevice = false,
                ObjectType = "DeviceInfo",
                Version = "1.0",
                DeviceProperties = new DeviceProperties(Configuration.DeviceId),                
                
                // Commands collection
                Commands = new Command[]
                {
                    CommandHelper.CreateCameraPreviewStatusCommand()
                }
            };

            await SendMessage(deviceInfo);
        }

        #endregion

        #region Methods (Private)

        private async Task SendMessage(Object message)
        {
            var serializedMessage = MessageHelper.Serialize(message);

            await deviceClient.SendEventAsync(serializedMessage);
        }

        private void BeginRemoteCommandHandling()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    var message = await deviceClient.ReceiveAsync();

                    if (message != null)
                    {
                        await HandleIncomingMessage(message);
                    }
                }
            });
        }

        private async Task HandleIncomingMessage(Message message)
        {
            try
            {
                // Deserialize message to remote command
                var remoteCommand = MessageHelper.Deserialize(message);

                // Parse command
                ParseCommand(remoteCommand);

                // Send confirmation to the cloud
                await deviceClient.CompleteAsync(message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                // Reject message, if it was not parsed correctly
                await deviceClient.RejectAsync(message);
            }
        }

        private void ParseCommand(RemoteCommand remoteCommand)
        {
            // Verify remote command name
            if (string.Compare(remoteCommand.Name, CommandHelper.CameraPreviewCommandName) == 0)
            {
                // Raise an event, when the valid command was received
                UpdateCameraPreviewCommandReceived(this,
                    new UpdateCameraPreviewCommandEventArgs(remoteCommand.Parameters.IsPreviewActive));
            }
        }

        #endregion
    }
}