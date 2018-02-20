namespace RemoteCamera.AzureHelpers
{
    public class CommandHelper
    {
        #region Properties

        public static string CameraPreviewCommandName { get; } = "Update camera preview";
        private static string cameraPreviewCommandParameterName = "IsPreviewActive";

        #endregion

        #region Methods

        public static Command CreateCameraPreviewStatusCommand()
        {
            return new Command()
            {
                Name = CameraPreviewCommandName,
                Parameters = new CommandParameter[] {
                    new CommandParameter()
                    {
                        Name = cameraPreviewCommandParameterName,
                        Type = "Boolean"
                    }
                }
            };
        }

        #endregion
    }
}
