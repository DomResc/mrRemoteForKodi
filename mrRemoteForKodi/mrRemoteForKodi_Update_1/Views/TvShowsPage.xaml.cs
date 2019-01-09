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
    public sealed partial class TvShowsPage : Page
    {
        private Remote _remote;
        private string _savedOrder;

        public TvShowsPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        #region appBar
        private async void appBarButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            var tvShowLibraryList = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandVideoLibraryGetTVshows());

            if (tvShowLibraryList != "statusError" && tvShowLibraryList != "connectionError")
            {
                var tvShowList = GetInformationHelper.getTvShowLibrary(tvShowLibraryList);

                if (tvShowList != null)
                {
                    // clear the database
                    deleteTvShow();

                    using (var db = new TvShowContext())
                    {
                        try
                        {
                            db.TvShows.AddRange(tvShowList);
                            db.SaveChanges();
                            loadTvShow();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    }
                }
            }
        }

        private void menuFlyoutItemOrderName_Click(object sender, RoutedEventArgs e)
        {
            orderByName();
            _savedOrder = "name";
        }

        private void menuFlyoutItemOrderYear_Click(object sender, RoutedEventArgs e)
        {
            orderByYear();
            _savedOrder = "year";
        }

        private void menuFlyoutItemOrderRating_Click(object sender, RoutedEventArgs e)
        {
            orderByRating();
            _savedOrder = "rating";
        }

        private void menuFlyoutItemOrderDateAdded_Click(object sender, RoutedEventArgs e)
        {
            orderByDateAdded();
            _savedOrder = "dateAdded";
        }

        private void Flyout_Opened(object sender, object e)
        {
            textBoxSearch.Focus(FocusState.Programmatic);
        }

        private void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            using (var db = new TvShowContext())
            {
                if (db.TvShows.Count() > 0)
                {
                    var query = (from p in db.TvShows where (p.Title.Contains(textBoxSearch.Text) || p.Genre.Contains(textBoxSearch.Text)) orderby p.Title select p).ToList();

                    GridViewTvShows.ItemsSource = query;
                    relativePanelNoList.Visibility = Visibility.Collapsed;
                }
                else
                {
                    GridViewTvShows.ItemsSource = null;
                    relativePanelNoList.Visibility = Visibility.Visible;
                }
            }
        }

        private void appBarButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            textBoxSearch.Text = "";
        }
        #endregion

        private void GridViewTvShows_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedTvShow = (TvShow)e.ClickedItem;

            WindowWrapper.Current().NavigationServices.FirstOrDefault().Navigate(typeof(TvShowDetailPage), clickedTvShow);
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
                appBarButtonOrder.IsEnabled = true;

                // load tvshowlist 
                loadTvShow();
            }
            else
            {
                relativePanelNoFav.Visibility = Visibility.Visible;
                appBarButtonRefresh.IsEnabled = false;
                appBarButtonSearch.IsEnabled = false;
                appBarButtonOrder.IsEnabled = false;
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

        // load tvShow
        private void loadTvShow()
        {
            if (string.IsNullOrWhiteSpace(_savedOrder))
                orderByName();
            else if (_savedOrder == "name")
                orderByName();
            else if (_savedOrder == "year")
                orderByYear();
            else if (_savedOrder == "rating")
                orderByRating();
            else if (_savedOrder == "dateAdded")
                orderByDateAdded();
        }

        // delete all tvshows in database
        private void deleteTvShow()
        {
            using (var db = new TvShowContext())
            {
                try
                {
                    db.TvShows.RemoveRange(db.TvShows.ToList());

                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        private void orderByName()
        {
            using (var db = new TvShowContext())
            {
                if (db.TvShows.Count() > 0)
                {
                    var query = (from p in db.TvShows orderby p.Title select p).ToList();

                    GridViewTvShows.ItemsSource = query;
                    relativePanelNoList.Visibility = Visibility.Collapsed;
                }
                else
                {
                    GridViewTvShows.ItemsSource = null;
                    relativePanelNoList.Visibility = Visibility.Visible;
                }
            }
        }

        private void orderByYear()
        {
            using (var db = new TvShowContext())
            {
                if (db.TvShows.Count() > 0)
                {
                    var query = (from p in db.TvShows orderby p.Year select p).ToList();

                    GridViewTvShows.ItemsSource = query;
                    relativePanelNoList.Visibility = Visibility.Collapsed;
                }
                else
                {
                    GridViewTvShows.ItemsSource = null;
                    relativePanelNoList.Visibility = Visibility.Visible;
                }
            }
        }

        private void orderByRating()
        {
            using (var db = new TvShowContext())
            {
                if (db.TvShows.Count() > 0)
                {
                    var query = (from p in db.TvShows orderby p.Rating select p).ToList();

                    GridViewTvShows.ItemsSource = query;
                    relativePanelNoList.Visibility = Visibility.Collapsed;
                }
                else
                {
                    GridViewTvShows.ItemsSource = null;
                    relativePanelNoList.Visibility = Visibility.Visible;
                }
            }
        }

        private void orderByDateAdded()
        {
            using (var db = new TvShowContext())
            {
                if (db.TvShows.Count() > 0)
                {
                    var query = (from p in db.TvShows orderby p.DateAdded descending select p).ToList();

                    GridViewTvShows.ItemsSource = query;
                    relativePanelNoList.Visibility = Visibility.Collapsed;
                }
                else
                {
                    GridViewTvShows.ItemsSource = null;
                    relativePanelNoList.Visibility = Visibility.Visible;
                }
            }
        }
    }
}