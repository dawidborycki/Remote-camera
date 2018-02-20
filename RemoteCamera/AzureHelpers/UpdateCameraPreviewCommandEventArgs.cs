#region Using

using System;

#endregion

namespace RemoteCamera.AzureHelpers
{
    public class UpdateCameraPreviewCommandEventArgs : EventArgs
    {
        #region Properties

        public bool IsPreviewActive { get; private set; }

        #endregion

        #region Constructor

        public UpdateCameraPreviewCommandEventArgs(bool isPreviewActive)
        {
            IsPreviewActive = isPreviewActive;
        }

        #endregion
    }
}