namespace RemoteCamera.AzureHelpers
{
    #region Command

    public class Command
    {
        #region Properties

        public string Name { get; set; }

        public CommandParameter[] Parameters { get; set; }

        #endregion
    }

    #endregion

    #region CommandParameter

    public class CommandParameter
    {
        #region Properties

        public string Name { get; set; }

        public string Type { get; set; }

        #endregion
    }

    #endregion
}
