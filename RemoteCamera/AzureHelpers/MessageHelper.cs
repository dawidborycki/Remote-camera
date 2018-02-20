#region Using

using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using RemoteCamera.Helpers;
using System.Text;

#endregion

namespace RemoteCamera.AzureHelpers
{
    public static class MessageHelper
    {
        #region Methods 

        public static Message Serialize(object obj)
        {
            ArgumentCheck.IsNull(obj, "obj");

            var jsonData = JsonConvert.SerializeObject(obj);

            return new Message(Encoding.UTF8.GetBytes(jsonData));
        }

        public static RemoteCommand Deserialize(Message message)
        {
            ArgumentCheck.IsNull(message, "message");

            var jsonData = Encoding.UTF8.GetString(message.GetBytes());

            return JsonConvert.DeserializeObject<RemoteCommand>(jsonData);
        }

        #endregion
    }
}
