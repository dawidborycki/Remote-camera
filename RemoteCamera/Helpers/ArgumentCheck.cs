#region Using

using System;

#endregion

namespace RemoteCamera.Helpers
{
    public static class ArgumentCheck
    {
        #region Methods

        public static void IsNull(object obj, string argumentName = "")
        {
            if(obj == null)
            {
                throw new ArgumentException($"The argument {argumentName} cannot be null");
            }
        }

        #endregion
    }
}
