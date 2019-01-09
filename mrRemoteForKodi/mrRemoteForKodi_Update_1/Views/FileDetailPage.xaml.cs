using mrRemoteForKodi_Update_1.Helpers;
using mrRemoteForKodi_Update_1.Models;
using System.Collections.Generic;
using System.Linq;
using Template10.Common;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace mrRemoteForKodi_Update_1.Views
{
    public sealed partial class FileDetailPage : Page
    {
        private List<FileMedia> _listFileMedia;
        private Remote _remote;

        public FileDetailPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var param = e.Parameter?.ToString();
            var service = Template10.Services.SerializationService.SerializationService.Json;
            _listFileMedia = service.Deserialize<List<FileMedia>>(param);

            if (_listFileMedia != null)
            {
                listViewFile.ItemsSource = _listFileMedia;
            }
        }

        private async void listViewFile_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedFile = (FileMedia)e.ClickedItem;

            if (clickedFile.Filetype == "file")
            {
                loadRemote();

                if (_remote != null)
                {
                    var response = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandPlayerOpenFile(clickedFile.Directory));

                    if (response != "statusError" && response != "connectionError")
                        WindowWrapper.Current().NavigationServices.FirstOrDefault().Navigate(typeof(MainPage));
                }
            }
            else if (clickedFile.Filetype == "directory")
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