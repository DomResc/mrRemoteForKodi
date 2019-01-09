using mrRemoteForKodi_Update_1.Helpers;
using System;
using System.Net;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace mrRemoteForKodi_Update_1.Converters
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var imageValue = (string)value;
            var image = new BitmapImage();

            try
            {
                if (string.IsNullOrWhiteSpace(imageValue) != true)
                {
                    if (imageValue.StartsWith("image://http") && imageValue.EndsWith("/"))
                    {
                        imageValue = imageValue.Remove(0, 8);
                        imageValue = imageValue.Remove(imageValue.Length - 1);

                        return new BitmapImage(new Uri(WebUtility.UrlDecode(imageValue)));
                    }
                    else if (imageValue.StartsWith("image://"))
                    {
                        image.StreamToBitmapConverter(imageValue);

                        return image;
                    }
                    else
                    {
                        return new BitmapImage(new Uri("ms-appx:///Assets/Image_Placeholder.png"));
                    }
                }
                else
                {
                    return new BitmapImage(new Uri("ms-appx:///Assets/Image_Placeholder.png"));
                }
            }
            catch
            {
                return new BitmapImage(new Uri("ms-appx:///Assets/Image_Placeholder.png"));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}