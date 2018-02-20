#region Using

using System;
using Windows.UI.Xaml.Data;

#endregion

namespace RemoteCamera.Converters
{
    public class BrightnessStringConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, string language)
        {            
            return $"Brightness: {value}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
