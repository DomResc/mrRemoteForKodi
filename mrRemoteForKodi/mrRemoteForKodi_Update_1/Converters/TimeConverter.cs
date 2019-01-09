using System;
using Windows.UI.Xaml.Data;

namespace mrRemoteForKodi_Update_1.Converters
{
    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                int seconds = Int32.Parse((string)value);

                var timeSpan = TimeSpan.FromSeconds(seconds);

                return timeSpan.ToString(@"hh\:mm\:ss");
            }
            catch (Exception)
            {
                return "00:00:00";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}