using mrRemoteForKodi_Update_1.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace mrRemoteForKodi_Update_1.Helpers
{
    static class BitmapHelper
    {
        private static Remote _remote = null;
        private static Stream stream = null;

        public static async void StreamToBitmapConverter(this BitmapImage image, string imagePath)
        {
            loadRemote();

            if (_remote != null)
            {
                stream = await ConnectionHelper.ExecuteRequestImage(_remote.Host, _remote.Port, _remote.User, _remote.Pass, imagePath);
            }

            if (stream != null)
            {
                MemoryStream ms = new MemoryStream();
                stream.CopyTo(ms);
                using (var randomAccessStream = await ConvertToRandomAccessStream(ms))
                    await image.SetSourceAsync(randomAccessStream);
            }
            else
            {
                image.UriSource = new Uri("ms-appx:///Assets/Image_Placeholder.png");
            }
        }

        private static async Task<IRandomAccessStream> ConvertToRandomAccessStream(MemoryStream memoryStream)
        {
            var randomAccessStream = new InMemoryRandomAccessStream();
            var outputStream = randomAccessStream.GetOutputStreamAt(0);
            var dw = new DataWriter(outputStream);

            var task = Task.Factory.StartNew(() => dw.WriteBytes(memoryStream.ToArray()));
            await task;

            await dw.StoreAsync();
            await outputStream.FlushAsync();

            return randomAccessStream;
        }

        private static void loadRemote()
        {
            using (var db = new RemoteContext())
            {
                var remotes = db.Remotes
                    .Where(b => b.Fav == true)
                    .ToList();

                // check the remotes is fav
                if (remotes.Count() != 0)
                {
                    foreach (Remote remote in remotes)
                    {
                        // set _remote
                        _remote = remote;
                    }
                }
            }
        }
    }
}
