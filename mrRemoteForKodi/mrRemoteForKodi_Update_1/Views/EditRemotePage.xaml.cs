using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using mrRemoteForKodi_Update_1.Helpers;
using mrRemoteForKodi_Update_1.Models;
using System;
using System.Diagnostics;
using System.Linq;
using Template10.Common;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace mrRemoteForKodi_Update_1.Views
{
    public sealed partial class EditRemotePage : Page
    {
        public EditRemotePage()
        {
            InitializeComponent();
        }

        // read all the information from the page navigation
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var param = e.Parameter?.ToString();
            var service = Template10.Services.SerializationService.SerializationService.Json;
            var value = service.Deserialize<Remote>(param);

            textBoxRemoteName.Text = value.Name;
            textBoxRemoteHost.Text = value.Host;
            textBoxRemotePort.Text = value.Port;

            if (string.IsNullOrWhiteSpace(value.User) != true)
                textBoxRemoteUser.Text = value.User;
            if (string.IsNullOrWhiteSpace(value.Pass) != true)
                passwordBoxRemotePass.Password = value.Pass;

            if (string.IsNullOrWhiteSpace(value.WolMac) != true)
                textBoxRemoteWolMac.Text = value.WolMac;
            if (string.IsNullOrWhiteSpace(value.WolMask) != true)
                textBoxRemoteWolSubnet.Text = value.WolMask;
            if (string.IsNullOrWhiteSpace(value.WolPort) != true)
                textBoxRemoteWolPort.Text = value.WolPort;

            toggleSwitchFav.IsOn = value.Fav;
        }

        // validate the textbox, test the connection with the server and save the remote in the database
        private async void buttonTest_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxRemoteName.Text) || string.IsNullOrWhiteSpace(textBoxRemoteHost.Text) || string.IsNullOrWhiteSpace(textBoxRemotePort.Text))
            {
                textBlockError.Foreground = new SolidColorBrush(Colors.Red);
                textBlockError.Text = "Please fill all the required fields";
            }
            else
            {
                disableForTest();

                var response = await ConnectionHelper.ExecuteRequest(textBoxRemoteHost.Text, textBoxRemotePort.Text, textBoxRemoteUser.Text, passwordBoxRemotePass.Password, JsonHelper.JsonCommandPing());

                // response connection error
                if (response == "connectionError")
                {
                    enableForTest();
                    textBlockError.Foreground = new SolidColorBrush(Colors.Red);
                    textBlockError.Text = "Cannot connect to Kodi/XBMC, checks if all the information are correct";
                }
                // response status error
                else if (response == "statusError")
                {
                    enableForTest();
                    textBlockError.Foreground = new SolidColorBrush(Colors.Red);
                    textBlockError.Text = "Successfully connected to Kodi/XBMC but no response from it, checks if Username/Password are correct";
                }
                // response is a message
                else if (response != null)
                {
                    using (var db = new RemoteContext())
                    {
                        var remote = new Remote
                        {
                            Name = textBoxRemoteName.Text,
                            Host = textBoxRemoteHost.Text,
                            Port = textBoxRemotePort.Text,
                            User = textBoxRemoteUser.Text,
                            Pass = passwordBoxRemotePass.Password,
                            WolMac = textBoxRemoteWolMac.Text,
                            WolMask = textBoxRemoteWolSubnet.Text,
                            WolPort = textBoxRemoteWolPort.Text,
                            Fav = toggleSwitchFav.IsOn
                        };

                        try
                        {
                            // add remote to Remotes db
                            db.Remotes.Update(remote);

                            // save the remote
                            db.SaveChanges();

                            // if no exception was throw return to remoteslistpage
                            WindowWrapper.Current().NavigationServices.FirstOrDefault().Navigate(typeof(RemotesListPage));
                        }
                        // catch if same name
                        catch (DbUpdateException ex)
                        {
                            var sex = ex.GetBaseException() as SqliteException;
                            if (sex.SqliteErrorCode == 19)
                            {
                                enableForTest();
                                textBlockError.Foreground = new SolidColorBrush(Colors.Red);
                                textBlockError.Text = "Another remote with the same name already exist, please select another name for this remote";
                            }
                        }
                        // generic exception from db.SaveChanges
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                            enableForTest();
                            textBlockError.Foreground = new SolidColorBrush(Colors.Red);
                            textBlockError.Text = "Unable to save this remote";
                        }
                    }
                }
            }
        }

        #region GuiEnableDisable
        private void disableForTest()
        {
            textBoxRemoteHost.IsEnabled = false;
            textBoxRemotePort.IsEnabled = false;
            textBoxRemoteUser.IsEnabled = false;
            passwordBoxRemotePass.IsEnabled = false;
            textBoxRemoteWolMac.IsEnabled = false;
            textBoxRemoteWolSubnet.IsEnabled = false;
            textBoxRemoteWolPort.IsEnabled = false;
            buttonTest.IsEnabled = false;
            progressRingSearchAndTest.IsActive = true;
            textBlockError.Text = "";
        }

        private void enableForTest()
        {
            textBoxRemoteHost.IsEnabled = true;
            textBoxRemotePort.IsEnabled = true;
            textBoxRemoteUser.IsEnabled = true;
            passwordBoxRemotePass.IsEnabled = true;
            textBoxRemoteWolMac.IsEnabled = true;
            textBoxRemoteWolSubnet.IsEnabled = true;
            textBoxRemoteWolPort.IsEnabled = true;
            buttonTest.IsEnabled = true;
            progressRingSearchAndTest.IsActive = false;
            textBlockError.Text = "";
        }
        #endregion
    }
}