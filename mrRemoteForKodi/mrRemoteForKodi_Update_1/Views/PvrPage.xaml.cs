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
    public sealed partial class PvrPage : Page
    {
        private Remote _remote;

        public PvrPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // load _remote
            loadRemote();

            if (_remote != null)
            {
                // hide the nofav relativepanel
                relativePanelNoFav.Visibility = Visibility.Collapsed;

                // load tvshowlist 
                loadPvrChannel();
            }
            else
            {
                relativePanelNoFav.Visibility = Visibility.Visible;
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

        // load PvrChannel
        private async void loadPvrChannel()
        {
            if (_remote != null)
            {
                var responseTv = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandPvrGetChannels("alltv"));
                var responseRadio = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandPvrGetChannels("allradio"));

                if (responseTv != "statusError" && responseTv != "connectionError")
                {
                    var channelLibrary = GetInformationHelper.getPvrChannel(responseTv);

                    if (channelLibrary != null)
                    {
                        relativePanelNoChannelList.Visibility = Visibility.Collapsed;
                        listViewPvrTv.ItemsSource = channelLibrary;
                    }
                    else
                        relativePanelNoChannelList.Visibility = Visibility.Visible;
                }

                if (responseRadio != "statusError" && responseRadio != "connectionError")
                {
                    var channelLibrary = GetInformationHelper.getPvrChannel(responseRadio);

                    if (channelLibrary != null)
                    {
                        relativePanelNoRadioList.Visibility = Visibility.Collapsed;

                        listViewPvrRadio.ItemsSource = channelLibrary;
                    }
                    else
                        relativePanelNoRadioList.Visibility = Visibility.Visible;
                }
            }
        }

        private async void listViewPvrTv_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedEpisode = (PvrChannel)e.ClickedItem;

            loadRemote();

            if (_remote != null)
            {
                var response = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandPlayerOpen("channelid", Convert.ToInt32(clickedEpisode.ChannelId)));

                if (response != "statusError" && response != "connectionError")
                    WindowWrapper.Current().NavigationServices.FirstOrDefault().Navigate(typeof(MainPage));
            }
        }

        private async void listViewPvrRadio_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedEpisode = (PvrChannel)e.ClickedItem;

            loadRemote();

            if (_remote != null)
            {
                var response = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandPlayerOpen("channelid", Convert.ToInt32(clickedEpisode.ChannelId)));

                if (response != "statusError" && response != "connectionError")
                    WindowWrapper.Current().NavigationServices.FirstOrDefault().Navigate(typeof(MainPage));
            }
        }
    }
}