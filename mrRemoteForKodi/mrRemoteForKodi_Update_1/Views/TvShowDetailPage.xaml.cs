using mrRemoteForKodi_Update_1.Helpers;
using mrRemoteForKodi_Update_1.Models;
using System;
using System.Linq;
using Template10.Common;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace mrRemoteForKodi_Update_1.Views
{
    public sealed partial class TvShowDetailPage : Page
    {
        private Remote _remote;
        private TvShow _tvShow;

        public TvShowDetailPage()
        {
            InitializeComponent();
        }

        // read all the information from the page navigation
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var param = e.Parameter?.ToString();
            var service = Template10.Services.SerializationService.SerializationService.Json;
            _tvShow = service.Deserialize<TvShow>(param);

            if (_tvShow != null)
            {
                loadRemote();

                if (_remote != null)
                {
                    var response = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandVideoLibraryGetSeasons(Convert.ToInt32(_tvShow.Tvshowid)));

                    if (response != "statusError" && response != "connectionError")
                    {
                        var seasonLibrary = GetInformationHelper.getSeasonLibrary(response);

                        if (seasonLibrary != null)
                            GridViewSeason.ItemsSource = seasonLibrary;
                    }
                }
            }
        }

        private async void GridViewSeason_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedSeason = (Season)e.ClickedItem;

            if (_tvShow != null)
            {
                loadRemote();

                if (_remote != null)
                {
                    var response = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandVideoLibraryGetEpisodes(Convert.ToInt32(_tvShow.Tvshowid), Convert.ToInt32(clickedSeason.SeasonNumber)));

                    if (response != "statusError" && response != "connectionError")
                    {
                        var episodeLibrary = GetInformationHelper.getEpisodeLibrary(response);

                        if (episodeLibrary != null)
                        {
                            listViewEpisode.ItemsSource = episodeLibrary;
                        }
                    }
                }
            }
        }

        private async void listViewEpisode_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedEpisode = (Episode)e.ClickedItem;

            var requestDialog = new ContentDialog()
            {
                Title = "Where do you want to play this episode?",
                Content = Environment.NewLine + clickedEpisode.Label,
                PrimaryButtonText = "Kodi",
                SecondaryButtonText = "mrRemote for Kodi"
            };

            var result = await requestDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                loadRemote();

                if (_remote != null)
                {
                    var response = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandPlayerOpen("episodeid", Convert.ToInt32(clickedEpisode.EpisodeId)));

                    if (response != "statusError" && response != "connectionError")
                        WindowWrapper.Current().NavigationServices.FirstOrDefault().Navigate(typeof(MainPage));
                }
            }
            else if (result == ContentDialogResult.Secondary)
            {
                loadRemote();

                if (_remote != null && clickedEpisode.File != null)
                {
                    var response = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandFilesDownload(clickedEpisode.File));

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
        }

        // load remote from database
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