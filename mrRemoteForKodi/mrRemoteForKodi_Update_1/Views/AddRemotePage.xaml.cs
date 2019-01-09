using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using mrRemoteForKodi_Update_1.Helpers;
using mrRemoteForKodi_Update_1.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Template10.Common;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Zeroconf;

namespace mrRemoteForKodi_Update_1.Views
{
    public sealed partial class AddRemotePage : Page
    {
        public AddRemotePage()
        {
            InitializeComponent();
        }

        // search with zeroconf for configured mediacenter
        private async void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            disableForSearchAndTest();

            try
            {
                IReadOnlyList<IZeroconfHost> results = await ZeroconfResolver.ResolveAsync("_xbmc-jsonrpc-h._tcp.local.");

                // if it's found
                if (results.Count() > 0)
                {
                    foreach (IZeroconfHost host in results)
                    {
                        foreach (IService service in host.Services.Values)
                        {
                            textBoxRemoteName.Text = host.DisplayName;
                            textBoxRemoteHost.Text = host.IPAddress;
                            textBoxRemotePort.Text = Convert.ToString(service.Port);
                        }
                    }
                    enableForSearchAndTest();
                    textBlockError.Foreground = new SolidColorBrush(Colors.Green);
                    textBlockError.Text = "Kodi/XBMC Mediacenter found, please insert Username and Password and click Test button to continue";
                }
                // if itsn't found
                else
                {
                    enableForSearchAndTest();
                    textBlockError.Foreground = new SolidColorBrush(Colors.Yellow);
                    textBlockError.Text = "Kodi/XBMC Mediacenter not found, please fill all the fields manually";
                }
            }
            catch (Exception ex)
            {
                enableForSearchAndTest();
                Debug.WriteLine("buttonSearch_Click: {0}", ex.Message);
            }
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
                disableForSearchAndTest();

                var response = await ConnectionHelper.ExecuteRequest(textBoxRemoteHost.Text, textBoxRemotePort.Text, textBoxRemoteUser.Text, passwordBoxRemotePass.Password, JsonHelper.JsonCommandPing());

                // response connection error
                if (response == "connectionError")
                {
                    enableForSearchAndTest();
                    textBlockError.Foreground = new SolidColorBrush(Colors.Red);
                    textBlockError.Text = "Cannot connect to Kodi/XBMC, checks if all the information are correct";
                }
                // response status error
                else if (response == "statusError")
                {
                    enableForSearchAndTest();
                    textBlockError.Foreground = new SolidColorBrush(Colors.Red);
                    textBlockError.Text = "Successfully connected to Kodi/XBMC but no response from it, checks if Username/Password are correct";
                }
                // response is a message
                else if (response != null)
                {
                    if (toggleSwitchFav.IsOn == true)
                    {
                        setAllAsUnfav();
                    }

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
                            db.Remotes.Add(remote);

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
                                enableForSearchAndTest();
                                textBlockError.Foreground = new SolidColorBrush(Colors.Red);
                                textBlockError.Text = "Another remote with the same name already exist, please select another name for this remote";
                            }
                        }
                        // generic exception from db.SaveChanges
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                            enableForSearchAndTest();
                            textBlockError.Foreground = new SolidColorBrush(Colors.Red);
                            textBlockError.Text = "Unable to save this remote";
                        }
                    }
                }
            }
        }

        // go to help page
        private void hyperlinkHelp_Click(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            WindowWrapper.Current().NavigationServices.FirstOrDefault().Navigate(typeof(HelpPage));
        }

        // set all remotes in the db as Fav = false
        private void setAllAsUnfav()
        {
            using (var db = new RemoteContext())
            {
                var remotes = db.Remotes
                    .Where(b => b.Fav.Equals(true))
                    .ToList();

                if (remotes.Count() != 0)
                {
                    foreach (Remote remote in remotes)
                    {
                        remote.Fav = false;

                        try
                        {
                            db.Remotes.Update(remote);
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

        #region GuiEnableDisable
        private void disableForSearchAndTest()
        {
            textBoxRemoteName.IsEnabled = false;
            textBoxRemoteHost.IsEnabled = false;
            textBoxRemotePort.IsEnabled = false;
            textBoxRemoteUser.IsEnabled = false;
            passwordBoxRemotePass.IsEnabled = false;
            textBoxRemoteWolMac.IsEnabled = false;
            textBoxRemoteWolSubnet.IsEnabled = false;
            textBoxRemoteWolPort.IsEnabled = false;
            toggleSwitchFav.IsEnabled = false;
            buttonSearch.IsEnabled = false;
            buttonTest.IsEnabled = false;
            progressRingSearchAndTest.IsActive = true;
            textBlockError.Text = "";            
        }

        private void enableForSearchAndTest()
        {
            textBoxRemoteName.IsEnabled = true;
            textBoxRemoteHost.IsEnabled = true;
            textBoxRemotePort.IsEnabled = true;
            textBoxRemoteUser.IsEnabled = true;
            passwordBoxRemotePass.IsEnabled = true;
            textBoxRemoteWolMac.IsEnabled = true;
            textBoxRemoteWolSubnet.IsEnabled = true;
            textBoxRemoteWolPort.IsEnabled = true;
            toggleSwitchFav.IsEnabled = true;
            buttonSearch.IsEnabled = true;
            buttonTest.IsEnabled = true;
            progressRingSearchAndTest.IsActive = false;
            textBlockError.Text = "";
        }
        #endregion
    }
}