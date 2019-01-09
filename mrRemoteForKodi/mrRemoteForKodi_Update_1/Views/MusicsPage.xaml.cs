using mrRemoteForKodi_Update_1.Helpers;
using mrRemoteForKodi_Update_1.Models;
using System;
using System.Diagnostics;
using System.Linq;
using Template10.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace mrRemoteForKodi_Update_1.Views
{
    public sealed partial class MusicsPage : Page
    {
        private Remote _remote;

        public MusicsPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        #region appBar
        private async void appBarButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            var artistLibraryList = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandAudioLibraryGetArtist());

            if (artistLibraryList != "statusError" && artistLibraryList != "connectionError")
            {
                var artistList = GetInformationHelper.getAudioLibraryArtist(artistLibraryList);

                if (artistList != null)
                {
                    // clear the database
                    deleteArtist();

                    using (var db = new ArtistContext())
                    {
                        try
                        {
                            db.Artists.AddRange(artistList);
                            db.SaveChanges();
                            loadMusic();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    }
                }
            }
        }

        private void Flyout_Opened(object sender, object e)
        {
            textBoxSearch.Focus(FocusState.Programmatic);
        }

        private void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            using (var db = new ArtistContext())
            {
                if (db.Artists.Count() > 0)
                {
                    var query = (from p in db.Artists where (p.Label.Contains(textBoxSearch.Text) || p.Genre.Contains(textBoxSearch.Text)) orderby p.Label select p).ToList();

                    GridViewArtists.ItemsSource = query;
                    relativePanelNoList.Visibility = Visibility.Collapsed;
                }
                else
                {
                    GridViewArtists.ItemsSource = null;
                    relativePanelNoList.Visibility = Visibility.Visible;
                }
            }
        }

        private void appBarButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            textBoxSearch.Text = "";
        }
        #endregion

        private void GridViewArtists_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedArtist = (Artist)e.ClickedItem;

            WindowWrapper.Current().NavigationServices.FirstOrDefault().Navigate(typeof(ArtistDetailPage), clickedArtist);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // load _remote
            loadRemote();

            if (_remote != null)
            {
                // hide the nofav relativepanel
                relativePanelNoFav.Visibility = Visibility.Collapsed;
                appBarButtonRefresh.IsEnabled = true;
                appBarButtonSearch.IsEnabled = true;

                // load musiclist 
                loadMusic();
            }
            else
            {
                relativePanelNoFav.Visibility = Visibility.Visible;
                appBarButtonRefresh.IsEnabled = false;
                appBarButtonSearch.IsEnabled = false;
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

        // load music
        private void loadMusic()
        {
            using (var db = new ArtistContext())
            {
                if (db.Artists.Count() > 0)
                {
                    var query = (from p in db.Artists orderby p.Label select p).ToList();

                    GridViewArtists.ItemsSource = query;
                    relativePanelNoList.Visibility = Visibility.Collapsed;
                }
                else
                {
                    GridViewArtists.ItemsSource = null;
                    relativePanelNoList.Visibility = Visibility.Visible;
                }
            }
        }

        // delete all artist in database
        private void deleteArtist()
        {
            using (var db = new ArtistContext())
            {
                try
                {
                    db.Artists.RemoveRange(db.Artists.ToList());

                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }
    }
}