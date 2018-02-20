namespace RemoteCamera.AzureHelpers
{
    #region RemoteCommand

    public class RemoteCommand
    {
        #region Properties

        public string Name { get; set; }
        public string MessageId { get; set; }
        public string CreatedTime { get; set; }
        public string UpdatedTime { get; set; }
        public object Result { get; set; }
        public object ErrorMessage { get; set; }
        public Parameters Parameters { get; set; }

        #endregion
    }

    #endregion

    #region Parameters

    public class Parameters
    {
        #region Properties
        
        public bool IsPreviewActive { get; set; }

        #endregion
    }

    #endregion
}
