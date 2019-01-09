using mrRemoteForKodi_Update_1.Helpers;
using mrRemoteForKodi_Update_1.Models;
using System;
using System.Linq;
using Template10.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace mrRemoteForKodi_Update_1.Views
{
    public sealed partial class MovieDetailPage : Page
    {
        private Movie _movie;
        private Remote _remote;
        private Uri _imdbLink;

        public MovieDetailPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var param = e.Parameter?.ToString();
            var service = Template10.Services.SerializationService.SerializationService.Json;

            _movie = service.Deserialize<Movie>(param);
            _imdbLink = new Uri("https://www.imdb.com/title/" + _movie.ImdbNumber);
        }

        private async void buttonStartMovie_Click(object sender, RoutedEventArgs e)
        {
            loadRemote();

            if (_remote != null)
            {
                var response = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandPlayerOpen("movieid", Convert.ToInt32(_movie.Movieid)));

                if (response != "statusError" && response != "connectionError")
                    WindowWrapper.Current().NavigationServices.FirstOrDefault().Navigate(typeof(MainPage));
            }
        }

        private async void buttonPlayMovie_Click(object sender, RoutedEventArgs e)
        {
            loadRemote();

            if (_remote != null && _movie.File != null)
            {
                var response = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandFilesDownload(_movie.File));

                if (response != "statusError" && response != "connectionError")
                {
                    var file = new FileVideo();

                    var filePos = GetInformationHelper.getVideoFile(response);

                    if (string.IsNullOrWhiteSpace(filePos) != true)
                    {
                        file.File = filePos;
                        file.Host = _remote.Host;
                        file.Port = _remote.Port;
                        file.User = _remote.User;
                        file.Pass = _remote.Pass;

                        WindowWrapper.Current().NavigationServices.FirstOrDefault().Navigate(typeof(PlayerPage), file);
                    }
                }
            }
        }

        private void loadRemote()
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
                else
                {
                    _remote = null;
                }
            }
        }
    }
}