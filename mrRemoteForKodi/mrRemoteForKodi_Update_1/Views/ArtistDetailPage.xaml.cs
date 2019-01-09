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
    public sealed partial class ArtistDetailPage : Page
    {
        private Remote _remote;
        private Artist _artist;
        private Album _album;

        public ArtistDetailPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var param = e.Parameter?.ToString();
            var service = Template10.Services.SerializationService.SerializationService.Json;
            _artist = service.Deserialize<Artist>(param);

            if (_artist != null)
            {
                loadRemote();

                if (_remote != null)
                {
                    var response = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandAudioLibraryGetAlbum(Convert.ToInt32(_artist.ArtistId)));

                    if (response != "statusError" && response != "connectionError")
                    {
                        var albumLibrary = GetInformationHelper.getAudioLibraryAlbum(response);

                        if (albumLibrary != null)
                            GridViewAlbum.ItemsSource = albumLibrary;
                    }
                }
            }
        }

        private async void GridViewAlbum_ItemClick(object sender, ItemClickEventArgs e)
        {
            _album = (Album)e.ClickedItem;

            if (_artist != null)
            {
                loadRemote();

                if (_remote != null)
                {
                    var response = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandAudioLibraryGetSong(Convert.ToInt32(_album.AlbumId)));

                    if (response != "statusError" && response != "connectionError")
                    {
                        var songLibrary = GetInformationHelper.getAudioLibrarySong(response);

                        if (songLibrary != null)
                        {
                            listViewSong.ItemsSource = songLibrary;
                        }
                    }
                }
            }
        }

        private async void listViewSong_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedItem = (Song)e.ClickedItem;

            loadRemote();

            if (_remote != null)
            {
                var response = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandPlayerOpen("songid", Convert.ToInt32(clickedItem.SongId)));

                if (response != "statusError" && response != "connectionError")
                    WindowWrapper.Current().NavigationServices.FirstOrDefault().Navigate(typeof(MainPage));
            }
        }

        private async void menuFlyoutItemAlbum_Click(object sender, RoutedEventArgs e)
        {
            if (_album != null)
            {
                menuFlyoutItemAlbum.IsEnabled = true;

                loadRemote();

                if (_remote != null)
                {
                    var response = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandPlayerOpen("albumid", Convert.ToInt32(_album.AlbumId)));

                    if (response != "statusError" && response != "connectionError")
                        WindowWrapper.Current().NavigationServices.FirstOrDefault().Navigate(typeof(MainPage));
                }
            }
        }

        private async void menuFlyoutItemArtist_Click(object sender, RoutedEventArgs e)
        {
            if (_artist != null)
            {
                loadRemote();

                if (_remote != null)
                {
                    var response = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandPlayerOpen("artistid", Convert.ToInt32(_artist.ArtistId)));

                    if (response != "statusError" && response != "connectionError")
                        WindowWrapper.Current().NavigationServices.FirstOrDefault().Navigate(typeof(MainPage));
                }
            }
        }

        private void buttonStartArtist_Click(object sender, RoutedEventArgs e)
        {
            if (_artist != null)
            {
                menuFlyoutItemArtist.IsEnabled = true;
            }
            else
                menuFlyoutItemArtist.IsEnabled = false;


            if (_album != null)
            {
                menuFlyoutItemAlbum.IsEnabled = true;
            }
            else
                menuFlyoutItemAlbum.IsEnabled = false;
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
