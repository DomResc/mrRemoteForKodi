using mrRemoteForKodi_Update_1.Helpers;
using mrRemoteForKodi_Update_1.Models;
using Windows.System.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace mrRemoteForKodi_Update_1.Views
{
    public sealed partial class PlayerPage : Page
    {
        private DisplayRequest appDisplayRequest = null;

        public PlayerPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Disabled;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var param = e.Parameter?.ToString();
            var service = Template10.Services.SerializationService.SerializationService.Json;

            var file = service.Deserialize<FileVideo>(param);

            if (file != null)
            {
                var stream = await ConnectionHelper.ExecuteRequestVideo(file.Host, file.Port, file.User, file.Pass, file.File);

                if (stream != null)
                    mediaElementPlayer.SetSource(stream, stream.ContentType);
            }
        }

        private void mediaElementPlayer_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            MediaElement mediaElement = sender as MediaElement;
            if (mediaElement != null && mediaElement.IsAudioOnly == false)
            {
                if (mediaElement.CurrentState == MediaElementState.Playing)
                {
                    if (appDisplayRequest == null)
                    {
                        // This call creates an instance of the DisplayRequest object. 
                        appDisplayRequest = new DisplayRequest();
                        appDisplayRequest.RequestActive();
                    }
                }
                else // CurrentState is Buffering, Closed, Opening, Paused, or Stopped. 
                {
                    if (appDisplayRequest != null)
                    {
                        // Deactivate the display request and set the var to null.
                        appDisplayRequest.RequestRelease();
                        appDisplayRequest = null;
                    }
                }
            }
        }
    }
}
