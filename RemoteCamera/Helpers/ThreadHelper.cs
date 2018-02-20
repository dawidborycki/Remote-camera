using System;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace RemoteCamera.Helpers
{
    public static class ThreadHelper
    {
        #region Properties

        public static CoreDispatcher Dispatcher { get; set; }

        #endregion

        #region Methods (Public)

        public static async Task InvokeOnMainThread(Action action)
        {        
            if(Dispatcher == null)
            {
                throw new Exception("Cannot access UI thread");
            }

            if(Dispatcher.HasThreadAccess)
            {
                action?.Invoke();
            }
            else
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    action?.Invoke();
                });
            }
        }

        #endregion
    }
}
