using System;
using Windows.UI.Xaml.Data;

namespace mrRemoteForKodi_Update_1.Converters
{
    public class RatingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var ratingValue = (string)value;

                if (ratingValue != null)
                {
                    if (ratingValue.Length > 3)
                    {
                        return ratingValue.Remove(3);
                    }
                    else
                    {
                        return ratingValue;
                    }
                }
                else return "0.0";
            }
            catch (Exception)
            {
                return "0.0";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}