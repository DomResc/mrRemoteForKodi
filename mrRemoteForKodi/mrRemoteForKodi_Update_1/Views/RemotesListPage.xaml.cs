using mrRemoteForKodi_Update_1.Helpers;
using mrRemoteForKodi_Update_1.Models;
using System;
using System.Diagnostics;
using System.Linq;
using Template10.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace mrRemoteForKodi_Update_1.Views
{
    public sealed partial class RemotesListPage : Page
    {
        public RemotesListPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            remotesCount();
        }

        // count the remotes in the db and set the visibility of the information text, set the itemsource of the listview
        private void remotesCount()
        {
            using (var db = new RemoteContext())
            {
                if (db.Remotes.Count() == 0)
                {
                    relativePanelNoRemote.Visibility = Visibility.Visible;
                    listViewRemotes.ItemsSource = null;

                    appBarButtonFavorite.IsEnabled = false;
                    appBarButtonEdit.IsEnabled = false;
                    appBarButtonDelete.IsEnabled = false;
                    appBarButtonWol.IsEnabled = false;
                }
                else if (db.Remotes.Count() > 0)
                {
                    relativePanelNoRemote.Visibility = Visibility.Collapsed;
                    listViewRemotes.ItemsSource = db.Remotes.ToList();

                    appBarButtonFavorite.IsEnabled = true;
                    appBarButtonEdit.IsEnabled = true;
                    appBarButtonDelete.IsEnabled = true;
                    appBarButtonWol.IsEnabled = true;
                }
            }
        }

        // go to addremotepage
        private void appBarButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            WindowWrapper.Current().NavigationServices.FirstOrDefault().Navigate(typeof(AddRemotePage));
        }

        // delete the selected remote
        private async void appBarButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            var deleteFileDialog = new ContentDialog()
            {
                Title = "Delete remote permanently?",
                Content = "If you delete this remote, you won't be able to recover it. Do you want to delete it?",
                PrimaryButtonText = "Cancel",
                SecondaryButtonText = "Delete"
            };

            var result = await deleteFileDialog.ShowAsync();

            // Delete the file if the user clicked the second button. 
            // Otherwise, do nothing. 
            if (result == ContentDialogResult.Secondary)
            {
                foreach (Remote remote in listViewRemotes.SelectedItems.ToList())
                    using (var db = new RemoteContext())
                    {
                        // try to save the database
                        try
                        {
                            db.Remotes.Remove(remote);

                            db.SaveChanges();

                            // reload the loadpage function
                            remotesCount();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    }
            }
        }

        // go to editremotepage passing the selected remote as page value
        private void appBarButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            foreach (Remote remote in listViewRemotes.SelectedItems.ToList())
                WindowWrapper.Current().NavigationServices.FirstOrDefault().Navigate(typeof(EditRemotePage), remote);
        }

        // set the selected remote as favorite
        private void appBarButtonFavorite_Click(object sender, RoutedEventArgs e)
        {
            // set fav propriety of all remote to false
            setAllAsUnfav();

            // set to true the fav attribute of the selected remote
            foreach (Remote remote in listViewRemotes.SelectedItems.ToList())
                using (var db = new RemoteContext())
                {
                    remote.Fav = true;

                    try
                    {
                        db.Remotes.Update(remote);

                        db.SaveChanges();

                        // reload the loadpage function
                        remotesCount();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
        }

        private void appBarButtonWol_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (Remote remote in listViewRemotes.SelectedItems.ToList())
                    WakeOnLanHelper.WakeUp(remote.WolMac, remote.WolMask, remote.WolPort);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
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
    }
}