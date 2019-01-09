using mrRemoteForKodi_Update_1.Helpers;
using mrRemoteForKodi_Update_1.Models;
using mrRemoteForKodi_Update_1.Services.SettingsServices;
using System;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.Foundation.Metadata;
using Windows.Phone.Devices.Notification;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace mrRemoteForKodi_Update_1.Views
{
    public sealed partial class MainPage : Page
    {
        private SettingsService _settings;
        private bool _vibrateOnPress;
        private ThreadPoolTimer _timer;
        private Remote _remote;
        private int _playerId;
        private bool _isSystemChange;
        private string _imageArtSource;
        private string _imageSourceThumb;


        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;

            _settings = SettingsService.Instance;
        }

        // at the page load start the timer and load all the avaible information
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // load the vibrateonpress settings
            _vibrateOnPress = _settings.VibrateOnPress;

            // check if there is a fav remote and so on
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
                        // load the _remote
                        _remote = remote;

                        // set the pageheader with the remote name
                        if (_remote.Name.Length > 12)
                        {
                            var remoteName = _remote.Name.Remove(12);
                            remoteName = remoteName + "...";
                            pageHeader.Text = remoteName.ToUpper();
                        }
                        else
                        {
                            pageHeader.Text = _remote.Name.ToUpper();
                        }

                        // hide the nofav relativepanel
                        relativePanelNoFav.Visibility = Visibility.Collapsed;

                        // start the timer to check connection and player status every seconds
                        _timer = ThreadPoolTimer.CreatePeriodicTimer(async (t) =>
                        {
                            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            {
                                // ping the mediacenter to show if there is connection
                                var responsePing = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandPing());

                                // check if connection works
                                if (responsePing != "statusError" && responsePing != "connectionError")
                                {
                                    // hide no connection and show control remotepanel
                                    relativePanelNoConnection.Visibility = Visibility.Collapsed;
                                    relativePanelAllControl.Visibility = Visibility.Visible;

                                    #region  ApplicationProperties
                                    var applicationProperties = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandApplicationGetProperties("volume"));

                                    if (applicationProperties != "statusError" && applicationProperties != "connectionError")
                                    {
                                        var volumeSource = GetInformationHelper.getApplicationPropertiesVolume(applicationProperties);

                                        if (volumeSource != null)
                                        {
                                            sliderVolumeBar.Value = Convert.ToDouble(volumeSource);
                                        }
                                        else
                                        {
                                            sliderVolumeBar.Value = 0;
                                        }
                                    }
                                    #endregion

                                    // request player id
                                    var responsePlayer = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandMethod("Player.GetActivePlayers"));

                                    // check if player is started
                                    if (responsePlayer != "statusError" && responsePlayer != "connectionError")
                                    {
                                        // load the _playerId
                                        _playerId = GetInformationHelper.getActivePlayersId(responsePlayer);

                                        // player run
                                        if (_playerId != -1)
                                        {
                                            // hide noplaying and show art and info relative panel
                                            relativePanelItemNoPlaying.Visibility = Visibility.Collapsed;
                                            relativePanelPlayerArtInfo.Visibility = Visibility.Visible;

                                            #region PlayerItem
                                            var playerItem = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandPlayerGetItem(_playerId));

                                            if (playerItem != "statusError" && playerItem != "connectionError")
                                            {
                                                var imageBrushArtSource = GetInformationHelper.getPlayerItemFanart(playerItem);
                                                var imageBrushThumbSource = GetInformationHelper.getPlayerItemThumb(playerItem);
                                                var titleSource = GetInformationHelper.getPlayerItemTitle(playerItem);
                                                var yearSource = GetInformationHelper.getPlayerItemYear(playerItem);
                                                var ratingSource = GetInformationHelper.getPlayerItemRating(playerItem);
                                                var genreSource = GetInformationHelper.getPlayerItemGenre(playerItem);
                                                var tagLineSource = GetInformationHelper.getPlayerItemTagline(playerItem);
                                                var showTitleSource = GetInformationHelper.getPlayerItemShowTitle(playerItem);
                                                var seasonSource = GetInformationHelper.getPlayerItemSeason(playerItem);
                                                var episodeSource = GetInformationHelper.getPlayerItemEpisode(playerItem);
                                                var albumArtistSource = GetInformationHelper.getPlayerItemAlbumArtist(playerItem);
                                                var albumSource = GetInformationHelper.getPlayerItemAlbum(playerItem);
                                                var trackSource = GetInformationHelper.getPlayerItemTrack(playerItem);
                                                var channelSource = GetInformationHelper.getPlayerItemChannel(playerItem);
                                                var channelNumberSource = GetInformationHelper.getPlayerItemChannelNumber(playerItem);
                                                var directorSource = GetInformationHelper.getPlayerItemDirector(playerItem);
                                                var writerSource = GetInformationHelper.getPlayerItemWriter(playerItem);
                                                var plotSource = GetInformationHelper.getPlayerItemPlot(playerItem);

                                                // set art
                                                if (imageBrushArtSource != null && imageBrushArtSource.StartsWith("image://"))
                                                {
                                                    try
                                                    {
                                                        if (_imageArtSource != imageBrushArtSource)
                                                        {
                                                            _imageArtSource = imageBrushArtSource;

                                                            var bitImage = new BitmapImage();

                                                            bitImage.StreamToBitmapConverter(imageBrushArtSource);

                                                            imageBrushArt.ImageSource = bitImage;
                                                        }
                                                    }
                                                    catch
                                                    {
                                                        imageBrushArt.ImageSource = null;
                                                    }
                                                }
                                                else
                                                {
                                                    imageBrushArt.ImageSource = null;
                                                }

                                                // set thumb
                                                if (imageBrushThumbSource != null && imageBrushThumbSource.StartsWith("image://"))
                                                {
                                                    try
                                                    {
                                                        if (_imageSourceThumb != imageBrushThumbSource)
                                                        {
                                                            _imageSourceThumb = imageBrushThumbSource;

                                                            var bitImage = new BitmapImage();

                                                            bitImage.StreamToBitmapConverter(imageBrushThumbSource);

                                                            imageThumb.Source = bitImage;
                                                        }
                                                    }
                                                    catch
                                                    {
                                                        imageThumb.Source = new BitmapImage(new Uri("ms-appx:///Assets/Image_Placeholder.png"));
                                                    }
                                                }
                                                else
                                                {
                                                    imageThumb.Source = new BitmapImage(new Uri("ms-appx:///Assets/Image_Placeholder.png"));
                                                }

                                                // set title
                                                if (titleSource != null)
                                                {
                                                    textBlockTitle.Text = titleSource;
                                                }
                                                else
                                                {
                                                    textBlockTitle.Text = "";
                                                }

                                                // set year | rating
                                                if (yearSource != null && ratingSource != null)
                                                {
                                                    if (ratingSource.Length > 3)
                                                    {
                                                        textBlockYearRating.Text = yearSource + "    |    " + ratingSource.Remove(3);
                                                    }
                                                    else
                                                    {
                                                        textBlockYearRating.Text = yearSource + "    |    " + ratingSource;
                                                    }
                                                }
                                                else if (yearSource != null)
                                                {
                                                    textBlockYearRating.Text = yearSource;
                                                }
                                                else if (ratingSource != null)
                                                {
                                                    if (ratingSource.Length > 3)
                                                    {
                                                        textBlockYearRating.Text = ratingSource.Remove(3);
                                                    }
                                                    else
                                                    {
                                                        textBlockYearRating.Text = ratingSource;
                                                    }
                                                }
                                                else
                                                {
                                                    textBlockYearRating.Text = "";
                                                }

                                                // set the generic
                                                if (tagLineSource != null)
                                                {
                                                    textBlockGeneric.Text = tagLineSource;
                                                }
                                                else if (showTitleSource != null && seasonSource != null && episodeSource != null)
                                                {
                                                    textBlockGeneric.Text = showTitleSource + "  " + "Season: " + seasonSource + "  " + "Episonde: " + episodeSource;
                                                }
                                                else if (albumArtistSource != null && albumSource != null && trackSource != null)
                                                {
                                                    textBlockGeneric.Text = albumArtistSource + "  " + "Album: " + albumSource + "  " + "Track: " + trackSource;
                                                }
                                                else if (channelSource != null && channelNumberSource != null)
                                                {
                                                    textBlockGeneric.Text = channelSource + "  " + "Channel: " + channelNumberSource;
                                                }
                                                else
                                                {
                                                    textBlockGeneric.Text = "";
                                                }

                                                // set genre
                                                if (genreSource != null)
                                                {
                                                    textBlockGenre.Text = genreSource;
                                                }
                                                else
                                                {
                                                    textBlockGenre.Text = "";
                                                }

                                                // set director
                                                if (directorSource != null)
                                                {
                                                    textBlockDirector.Text = "Director: " + directorSource;
                                                }
                                                else
                                                {
                                                    textBlockDirector.Text = "";
                                                }

                                                // set writer
                                                if (writerSource != null)
                                                {
                                                    textBlockWriter.Text = "Writer: " + writerSource;
                                                }
                                                else
                                                {
                                                    textBlockWriter.Text = "";
                                                }

                                                // set plot
                                                if (plotSource != null)
                                                {
                                                    textBlockPlot.Text = plotSource;
                                                }
                                                else
                                                {
                                                    textBlockPlot.Text = "";
                                                }
                                            }
                                            #endregion

                                            #region PlayerProperties
                                            var playerProperties = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandPlayerGetProperties(_playerId));

                                            if (playerProperties != "statusError" && playerProperties != "connectionError")
                                            {
                                                var canSeekSource = GetInformationHelper.getPlayerPropertiesCanSeek(playerProperties);
                                                var percentageSource = GetInformationHelper.getPlayerPropertiesPercentage(playerProperties);
                                                var timeSource = GetInformationHelper.getPlayerPropertiesTime(playerProperties);
                                                var totalTimeSource = GetInformationHelper.getPlayerPropertiesTotalTime(playerProperties);
                                                var speedSource = GetInformationHelper.getPlayerPropertiesSpeed(playerProperties);
                                                var subtitleEnabledSource = GetInformationHelper.getPlayerPropertiesSubtitleEnabled(playerProperties);
                                                var canRepeatSource = GetInformationHelper.getPlayerPropertiesCanRepeat(playerProperties);
                                                var canShuffleSource = GetInformationHelper.getPlayerPropertiesCanShuffle(playerProperties);

                                                // enable or disable the seekbar
                                                if (canSeekSource != null)
                                                {
                                                    if (canSeekSource == "True")
                                                        sliderSeekBar.IsEnabled = true;
                                                    else if (canSeekSource == "False")
                                                        sliderSeekBar.IsEnabled = false;
                                                }
                                                else
                                                {
                                                    sliderSeekBar.IsEnabled = true;
                                                }

                                                // set the seekbar
                                                if (percentageSource != null)
                                                {
                                                    _isSystemChange = true;
                                                    sliderSeekBar.Value = Convert.ToDouble(percentageSource);
                                                    _isSystemChange = false;
                                                }
                                                else
                                                {
                                                    _isSystemChange = true;
                                                    sliderSeekBar.Value = 0;
                                                    _isSystemChange = false;
                                                }

                                                // set the time
                                                if (timeSource != null)
                                                {
                                                    textBlockTime.Text = timeSource;
                                                }
                                                else
                                                {
                                                    textBlockTime.Text = "00:00:00";
                                                }

                                                // set the time
                                                if (totalTimeSource != null)
                                                {
                                                    textBlockTotalTime.Text = totalTimeSource;
                                                }
                                                else
                                                {
                                                    textBlockTotalTime.Text = "00:00:00";
                                                }

                                                // set the play/pause icon
                                                if (speedSource != null)
                                                {
                                                    if (speedSource == "0")
                                                        buttonPlayPause.Content = "\uE102";
                                                    else if (speedSource != "0")
                                                        buttonPlayPause.Content = "\uE103";
                                                }
                                                else
                                                {
                                                    buttonPlayPause.Content = "\uE102";
                                                }

                                                // enable/disable buttonSubtitleShowHide
                                                if (subtitleEnabledSource != null)
                                                {
                                                    if (subtitleEnabledSource == "True")
                                                    {
                                                        _isSystemChange = true;
                                                        toggleSwitchSubtitle.IsOn = true;
                                                        toggleSwitchSubtitle.IsEnabled = true;
                                                        _isSystemChange = false;
                                                        listViewSubtitles.Visibility = Visibility.Visible;
                                                    }

                                                    else if (subtitleEnabledSource == "False")
                                                    {
                                                        _isSystemChange = true;
                                                        toggleSwitchSubtitle.IsOn = false;
                                                        toggleSwitchSubtitle.IsEnabled = true;
                                                        _isSystemChange = false;
                                                        listViewSubtitles.Visibility = Visibility.Collapsed;
                                                    }
                                                }
                                                else
                                                {
                                                    _isSystemChange = true;
                                                    toggleSwitchSubtitle.IsOn = false;
                                                    toggleSwitchSubtitle.IsEnabled = false;
                                                    _isSystemChange = false;
                                                }

                                                if (canRepeatSource != null)
                                                {
                                                    if (canRepeatSource == "True")
                                                    {
                                                        MenuFlyoutItemRepeatAll.IsEnabled = true;
                                                        MenuFlyoutItemRepeatOne.IsEnabled = true;
                                                        MenuFlyoutItemRepeatOff.IsEnabled = true;
                                                    }
                                                    else if (canRepeatSource == "False")
                                                    {
                                                        MenuFlyoutItemRepeatAll.IsEnabled = false;
                                                        MenuFlyoutItemRepeatOne.IsEnabled = false;
                                                        MenuFlyoutItemRepeatOff.IsEnabled = false;
                                                    }
                                                }
                                                else
                                                {
                                                    MenuFlyoutItemRepeatAll.IsEnabled = false;
                                                    MenuFlyoutItemRepeatOne.IsEnabled = false;
                                                    MenuFlyoutItemRepeatOff.IsEnabled = false;
                                                }

                                                if (canShuffleSource != null)
                                                {
                                                    if (canShuffleSource == "True")
                                                    {
                                                        MenuFlyoutItemShuffleOff.IsEnabled = true;
                                                        MenuFlyoutItemShuffleOn.IsEnabled = true;
                                                    }
                                                    else if (canShuffleSource == "False")
                                                    {
                                                        MenuFlyoutItemShuffleOff.IsEnabled = false;
                                                        MenuFlyoutItemShuffleOn.IsEnabled = false;
                                                    }
                                                }
                                                else
                                                {
                                                    MenuFlyoutItemShuffleOff.IsEnabled = false;
                                                    MenuFlyoutItemShuffleOn.IsEnabled = false;
                                                }
                                            }
                                            #endregion
                                        }
                                        // player don't run
                                        else
                                        {
                                            relativePanelItemNoPlaying.Visibility = Visibility.Visible;
                                            relativePanelPlayerArtInfo.Visibility = Visibility.Collapsed;

                                            imageBrushArt.ImageSource = null;
                                            imageThumb.Source = null;
                                            textBlockTitle.Text = "";
                                            textBlockYearRating.Text = "";
                                            textBlockGenre.Text = "";
                                            textBlockGeneric.Text = "";
                                            textBlockDirector.Text = "";
                                            textBlockWriter.Text = "";
                                            textBlockPlot.Text = "";

                                            sliderSeekBar.IsEnabled = true;
                                            sliderSeekBar.Value = 0;
                                            textBlockTime.Text = "00:00:00";
                                            textBlockTotalTime.Text = "00:00:00";
                                            buttonPlayPause.Content = "\uE102";
                                            listViewSubtitles.ItemsSource = null;
                                            toggleSwitchSubtitle.IsOn = false;
                                            toggleSwitchSubtitle.IsEnabled = false;
                                            MenuFlyoutItemRepeatAll.IsEnabled = false;
                                            MenuFlyoutItemRepeatOne.IsEnabled = false;
                                            MenuFlyoutItemRepeatOff.IsEnabled = false;
                                            MenuFlyoutItemShuffleOff.IsEnabled = false;
                                            MenuFlyoutItemShuffleOn.IsEnabled = false;
                                        }
                                    }
                                }
                                // connection don't works
                                else
                                {
                                    relativePanelNoConnection.Visibility = Visibility.Visible;
                                    relativePanelAllControl.Visibility = Visibility.Collapsed;
                                    relativePanelItemNoPlaying.Visibility = Visibility.Collapsed;
                                    relativePanelPlayerArtInfo.Visibility = Visibility.Collapsed;

                                    imageBrushArt.ImageSource = null;
                                    imageThumb.Source = null;
                                    textBlockTitle.Text = "";
                                    textBlockYearRating.Text = "";
                                    textBlockGenre.Text = "";
                                    textBlockGeneric.Text = "";
                                    textBlockDirector.Text = "";
                                    textBlockWriter.Text = "";
                                    textBlockPlot.Text = "";

                                    sliderSeekBar.IsEnabled = true;
                                    sliderSeekBar.Value = 0;
                                    sliderVolumeBar.Value = 0;
                                    textBlockTime.Text = "00:00:00";
                                    textBlockTotalTime.Text = "00:00:00";
                                    buttonPlayPause.Content = "\uE102";
                                    listViewSubtitles.ItemsSource = null;
                                    toggleSwitchSubtitle.IsOn = false;
                                    toggleSwitchSubtitle.IsEnabled = false;
                                    MenuFlyoutItemRepeatAll.IsEnabled = false;
                                    MenuFlyoutItemRepeatOne.IsEnabled = false;
                                    MenuFlyoutItemRepeatOff.IsEnabled = false;
                                    MenuFlyoutItemShuffleOff.IsEnabled = false;
                                    MenuFlyoutItemShuffleOn.IsEnabled = false;
                                }
                            });
                        }, TimeSpan.FromSeconds(1));
                    }
                }
                // no fav remotes
                else
                {
                    pageHeader.Text = "SET A REMOTE AS YOUR FAVORITE";
                    relativePanelNoFav.Visibility = Visibility.Visible;
                    relativePanelNoConnection.Visibility = Visibility.Collapsed;
                    relativePanelItemNoPlaying.Visibility = Visibility.Collapsed;
                    relativePanelAllControl.Visibility = Visibility.Collapsed;
                    relativePanelPlayerArtInfo.Visibility = Visibility.Collapsed;

                    imageBrushArt.ImageSource = null;
                    imageThumb.Source = null;
                    textBlockTitle.Text = "";
                    textBlockYearRating.Text = "";
                    textBlockGenre.Text = "";
                    textBlockGeneric.Text = "";
                    textBlockDirector.Text = "";
                    textBlockWriter.Text = "";
                    textBlockPlot.Text = "";

                    sliderSeekBar.IsEnabled = true;
                    sliderSeekBar.Value = 0;
                    sliderVolumeBar.Value = 0;
                    textBlockTime.Text = "00:00:00";
                    textBlockTotalTime.Text = "00:00:00";
                    buttonPlayPause.Content = "\uE102";
                    listViewSubtitles.ItemsSource = null;
                    toggleSwitchSubtitle.IsOn = false;
                    toggleSwitchSubtitle.IsEnabled = false;
                    MenuFlyoutItemRepeatAll.IsEnabled = false;
                    MenuFlyoutItemRepeatOne.IsEnabled = false;
                    MenuFlyoutItemRepeatOff.IsEnabled = false;
                    MenuFlyoutItemShuffleOff.IsEnabled = false;
                    MenuFlyoutItemShuffleOn.IsEnabled = false;
                }
            }
        }

        // cancel the timer if it's started
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_timer != null)
            {
                _timer.Cancel();
            }
        }

        #region appbar
        // set the visibility of control
        private void appBarToggleButtonControl_Click(object sender, RoutedEventArgs e)
        {
            if (appBarToggleButtonControl.IsChecked == true)
            {
                relativePanelControlNavigation.Visibility = Visibility.Collapsed;
                relativePanelPlayerControl.Visibility = Visibility.Visible;
            }
            else
            {
                relativePanelControlNavigation.Visibility = Visibility.Visible;
                relativePanelPlayerControl.Visibility = Visibility.Collapsed;
            }
        }

        private void appBarButtonToggleFullscreen_Click(object sender, RoutedEventArgs e)
        {
            executeCommand(JsonHelper.JsonCommandAction("togglefullscreen"));
        }

        private void appBarButtonSelectProfile_Click(object sender, RoutedEventArgs e)
        {
            executeCommand(JsonHelper.JsonCommandWindow("loginscreen"));
        }

        private void menuFlyoutItemScanAudioLibrary_Click(object sender, RoutedEventArgs e)
        {
            executeCommand(JsonHelper.JsonCommandMethod("AudioLibrary.Scan"));
        }

        private void menuFlyoutItemScanVideoLibrary_Click(object sender, RoutedEventArgs e)
        {
            executeCommand(JsonHelper.JsonCommandMethod("VideoLibrary.Scan"));
        }

        private void menuFlyoutItemCleanAudioLibrary_Click(object sender, RoutedEventArgs e)
        {
            executeCommand(JsonHelper.JsonCommandMethod("AudioLibrary.Clean"));
        }

        private void menuFlyoutItemCleanVideoLibrary_Click(object sender, RoutedEventArgs e)
        {
            executeCommand(JsonHelper.JsonCommandMethod("VideoLibrary.Clean"));
        }

        private void menuFlyoutItemExit_Click(object sender, RoutedEventArgs e)
        {
            executeCommand(JsonHelper.JsonCommandMethod("Application.Quit"));
        }

        private void menuFlyoutItemHibernate_Click(object sender, RoutedEventArgs e)
        {
            executeCommand(JsonHelper.JsonCommandMethod("System.Hibernate"));
        }

        private void menuFlyoutItemSuspend_Click(object sender, RoutedEventArgs e)
        {
            executeCommand(JsonHelper.JsonCommandMethod("System.Suspend"));
        }

        private void menuFlyoutItemShutdown_Click(object sender, RoutedEventArgs e)
        {
            executeCommand(JsonHelper.JsonCommandMethod("System.Shutdown"));
        }

        private void menuFlyoutItemReboot_Click(object sender, RoutedEventArgs e)
        {
            executeCommand(JsonHelper.JsonCommandMethod("System.Reboot"));
        }
        #endregion

        #region relativePanelPlayerControl
        private void buttonPlayPause_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandAction("playpause"));
        }

        private void buttonPrevious_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandAction("skipprevious"));
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandAction("skipnext"));
        }

        private void buttonStepBack_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandAction("stepback"));
        }

        private void buttonStepForward_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandAction("stepforward"));
        }

        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandAction("stop"));
        }

        private void sliderSeekBar_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (_playerId != -1 && _isSystemChange == false)
            {
                executeCommand(JsonHelper.JsonCommandPlayerSetPropertiesSeek(_playerId, Convert.ToInt32(sliderSeekBar.Value)));
            }
        }

        private async void buttonSubtitles_Click(object sender, RoutedEventArgs e)
        {
            if (_playerId != -1)
            {
                var playerProperties = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandPlayerGetProperties(_playerId));

                if (playerProperties != "statusError" && playerProperties != "connectionError")
                {
                    var subtitlesSource = GetInformationHelper.getPlayerPropertiesSubtitles(playerProperties);

                    if (subtitlesSource != null)
                    {
                        listViewSubtitles.ItemsSource = subtitlesSource;
                    }
                    else
                    {
                        listViewSubtitles.ItemsSource = null;
                    }
                }
            }
        }

        private void listViewSubtitles_ItemClick(object sender, ItemClickEventArgs e)
        {
            vibrateDevices();

            if (_playerId != -1)
            {
                var subtitleClicked = (Subtitle)e.ClickedItem;
                executeCommand(JsonHelper.JsonCommandPlayerSetPropertiesSubtitle(_playerId, subtitleClicked.Index));
            }
        }

        private async void buttonAudioTrack_Click(object sender, RoutedEventArgs e)
        {
            if (_playerId != -1)
            {
                var playerProperties = await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, JsonHelper.JsonCommandPlayerGetProperties(_playerId));

                if (playerProperties != "statusError" && playerProperties != "connectionError")
                {
                    var audiostreamsSource = GetInformationHelper.getPlayerPropertiesAudiostreams(playerProperties);

                    if (audiostreamsSource != null)
                    {
                        listViewAudiostreams.ItemsSource = audiostreamsSource;
                    }
                    else
                    {
                        listViewAudiostreams.ItemsSource = null;
                    }
                }
            }
        }

        private void listViewAudiostreams_ItemClick(object sender, ItemClickEventArgs e)
        {
            vibrateDevices();

            if (_playerId != -1)
            {
                var audiostreamsClicked = (AudioStream)e.ClickedItem;
                executeCommand(JsonHelper.JsonCommandPlayerSetPropertiesAudioStreams(_playerId, audiostreamsClicked.Index));
            }
        }

        private void toggleSwitchSubtitle_Toggled(object sender, RoutedEventArgs e)
        {
            vibrateDevices();

            if (_isSystemChange == false)
            {
                executeCommand(JsonHelper.JsonCommandAction("showsubtitles"));
            }
        }

        private void buttonMute_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandAction("mute"));
        }

        private void sliderVolumeBar_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var volumeValue = Convert.ToInt32(sliderVolumeBar.Value);

            executeCommand(JsonHelper.JsonCommandApplicationSetPropertiesVolume(volumeValue));

            if (volumeValue == 0)
            {
                buttonVolume.Content = "\uE198";
            }
            else if (volumeValue > 0 && volumeValue <= 35)
            {
                buttonVolume.Content = "\uE993";
            }
            else if (volumeValue > 35 && volumeValue <= 75)
            {
                buttonVolume.Content = "\uE994";
            }
            else if (volumeValue > 75 && volumeValue <= 100)
            {
                buttonVolume.Content = "\uE995";
            }
        }

        private void MenuFlyoutItemRepeatAll_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();

            if (_playerId != -1)
            {
                executeCommand(JsonHelper.JsonCommandPlayerSetPropertiesRepeat(_playerId, "all"));
            }
        }

        private void MenuFlyoutItemRepeatOne_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();

            if (_playerId != -1)
            {
                executeCommand(JsonHelper.JsonCommandPlayerSetPropertiesRepeat(_playerId, "one"));
            }
        }

        private void MenuFlyoutItemRepeatOff_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();

            if (_playerId != -1)
            {
                executeCommand(JsonHelper.JsonCommandPlayerSetPropertiesRepeat(_playerId, "off"));
            }
        }

        private void MenuFlyoutItemShuffleOn_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();

            if (_playerId != -1)
            {
                executeCommand(JsonHelper.JsonCommandPlayerSetPropertiesShuffle(_playerId, true));
            }
        }

        private void MenuFlyoutItemShuffleOff_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();

            if (_playerId != -1)
            {
                executeCommand(JsonHelper.JsonCommandPlayerSetPropertiesShuffle(_playerId, false));
            }
        }
        #endregion

        #region relativePanelControlNavigation
        private void buttonEnter_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandAction("select"));
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandAction("back"));
        }

        private void buttonMenu_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandAction("contextmenu"));
        }

        private void menuFlyoutItemOsd_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandAction("osd"));
        }

        private void menuFlyoutItemInfo_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandAction("info"));
        }

        private void menuFlyoutItemCodec_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandAction("codecinfo"));
        }

        private void buttonSendText_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandInputSendText(textBoxSendText.Text));
            textBoxSendText.Text = "";
        }

        private void buttonUp_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandAction("up"));
        }

        private void buttonDown_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandAction("down"));
        }

        private void buttonLeft_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandAction("left"));
        }

        private void buttonRight_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandAction("right"));
        }

        private void menuFlyoutItemHome_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandWindow("home"));
        }

        private void menuFlyoutItemPlayer_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandAction("fullscreen"));
        }

        private void menuFlyoutItemMovies_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandWindowParameters("videolibrary", "movietitles"));
        }

        private void menuFlyoutItemTvShows_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandWindowParameters("videolibrary", "tvshowtitles"));
        }

        private void menuFlyoutItemMusicVideos_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandWindowParameters("videolibrary", "musicvideotitles"));
        }

        private void menuFlyoutItemArtists_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandWindowParameters("musiclibrary", "artists"));
        }

        private void menuFlyoutItemAlbums_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandWindowParameters("musiclibrary", "albums"));
        }

        private void menuFlyoutItemGeneres_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandWindowParameters("musiclibrary", "genres"));
        }

        private void menuFlyoutItemPlaylists_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandWindowParameters("musiclibrary", "playlists"));
        }

        private void buttonPictures_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandWindow("pictures"));
        }

        private void menuFlyoutItemVideoAddons_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandWindowParameters("videolibrary", "addons://sources/video/"));
        }

        private void menuFlyoutItemMusicAddons_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandWindowParameters("musiclibrary", "addons://sources/audio/"));
        }

        private void menuFlyoutItemProgramAddons_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandWindowParameters("programs", "addons://sources/executable/"));
        }

        private void menuFlyoutItemPvrTvChannels_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandWindow("tvchannels"));
        }

        private void menuFlyoutItemPvrTvRecordings_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandWindow("tvrecordings"));
        }

        private void menuFlyoutItemPvrTvGuide_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandWindow("tvguide"));
        }

        private void menuFlyoutItemPvrRadioChannels_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandWindow("radiochannels"));
        }

        private void menuFlyoutItemPvrRadioRecordings_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandWindow("radiorecordings"));
        }

        private void menuFlyoutItemPvrRadioGuide_Click(object sender, RoutedEventArgs e)
        {
            vibrateDevices();
            executeCommand(JsonHelper.JsonCommandWindow("radioguide"));
        }
        #endregion

        // vibrate the device if setting is enabled and it's a phone
        private void vibrateDevices()
        {
            if (ApiInformation.IsApiContractPresent("Windows.Phone.PhoneContract", 1, 0))
            {
                if (_vibrateOnPress == true)
                {
                    VibrationDevice.GetDefault()?.Vibrate(TimeSpan.FromSeconds(0.025));
                }
            }
        }

        // execute the given (string)jsoncommand
        private async void executeCommand(string command)
        {
            if (_remote != null)
            {
                try
                {
                    await ConnectionHelper.ExecuteRequest(_remote.Host, _remote.Port, _remote.User, _remote.Pass, command);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }
    }
}