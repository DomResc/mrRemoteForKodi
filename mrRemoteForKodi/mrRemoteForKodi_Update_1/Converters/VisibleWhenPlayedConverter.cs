using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace mrRemoteForKodi_Update_1.Converters
{
    public class VisibleWhenPlayedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                int playCount = Int32.Parse((string)value);

                if (playCount > 0)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            catch (Exception)
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}