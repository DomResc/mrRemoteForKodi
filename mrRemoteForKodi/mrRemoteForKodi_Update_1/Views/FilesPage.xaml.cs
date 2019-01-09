using mrRemoteForKodi_Update_1.Helpers;
using mrRemoteForKodi_Update_1.Models;
using System.Linq;
using Template10.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace mrRemoteForKodi_Update_1.Views
{
    public sealed partial class FilesPage : Page
    {
        private Remote _remote;

        public FilesPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // load _remote
            loadRemote();

            if (_remote != null)
            {
                // hide the nofav relativepanel
                relativePanelNoFav.Visibility = Visibility.Collapsed;

                // load musiclist 
                loadFile();
            }
            else
            {
                relativePanelNoFav.Visibility = Visibility.Visible;
            }
        }

        private async void loadFile()
        {
            if (_remote != null)
            {
                var responseVideo = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandFilesGetSources("video"));
                var responseMusic = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandFilesGetSources("music"));
                var responsePictures = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandFilesGetSources("pictures"));

                if (responseVideo != "statusError" && responseVideo != "connectionError")
                {
                    var videoLibrary = GetInformationHelper.getDirectoryFile(responseVideo);

                    if (videoLibrary != null)
                    {
                        relativePanelNoVideo.Visibility = Visibility.Collapsed;
                        listViewVideo.ItemsSource = videoLibrary;
                    }
                    else
                        relativePanelNoVideo.Visibility = Visibility.Visible;
                }

                if (responseMusic != "statusError" && responseMusic != "connectionError")
                {
                    var musicLibrary = GetInformationHelper.getDirectoryFile(responseMusic);

                    if (musicLibrary != null)
                    {
                        relativePanelNoMusic.Visibility = Visibility.Collapsed;
                        listViewMusic.ItemsSource = musicLibrary;
                    }
                    else
                        relativePanelNoMusic.Visibility = Visibility.Visible;
                }

                if (responsePictures != "statusError" && responsePictures != "connectionError")
                {
                    var picturesLibrary = GetInformationHelper.getDirectoryFile(responsePictures);

                    if (picturesLibrary != null)
                    {
                        relativePanelNoPictures.Visibility = Visibility.Collapsed;
                        listViewPictures.ItemsSource = picturesLibrary;
                    }
                    else
                        relativePanelNoPictures.Visibility = Visibility.Visible;
                }
            }
        }

        private async void listViewVideo_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedFile = (FileDirectory)e.ClickedItem;

            if (clickedFile != null)
            {
                loadRemote();

                if (_remote != null)
                {
                    var response = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandFilesGetDirectory(clickedFile.Directory));

                    if (response != "statusError" && response != "connectionError")
                    {
                        var fileLibrary = GetInformationHelper.getFile(response);

                        if (fileLibrary != null)
                            WindowWrapper.Current().NavigationServices.FirstOrDefault().Navigate(typeof(FileDetailPage), fileLibrary);
                    }
                }
            }
        }

        private async void listViewMusic_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedFile = (FileDirectory)e.ClickedItem;

            if (clickedFile != null)
            {
                loadRemote();

                if (_remote != null)
                {
                    var response = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandFilesGetDirectory(clickedFile.Directory));

                    if (response != "statusError" && response != "connectionError")
                    {
                        var fileLibrary = GetInformationHelper.getFile(response);

                        if (fileLibrary != null)
                            WindowWrapper.Current().NavigationServices.FirstOrDefault().Navigate(typeof(FileDetailPage), fileLibrary);
                    }
                }
            }
        }

        private async void listViewPictures_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedFile = (FileDirectory)e.ClickedItem;

            if (clickedFile != null)
            {
                loadRemote();

                if (_remote != null)
                {
                    var response = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandFilesGetDirectory(clickedFile.Directory));

                    if (response != "statusError" && response != "connectionError")
                    {
                        var fileLibrary = GetInformationHelper.getFile(response);

                        if (fileLibrary != null)
                            WindowWrapper.Current().NavigationServices.FirstOrDefault().Navigate(typeof(FileDetailPage), fileLibrary);
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