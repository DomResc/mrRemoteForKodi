using mrRemoteForKodi.Helpers;
using mrRemoteForKodi.Models;
using mrRemoteForKodi.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace mrRemoteForKodi.Views
{
    public sealed partial class ConfiguredRemotesPage : Page, INotifyPropertyChanged
    {
        private bool IsTextBlockEmptyVisible;

        public ConfiguredRemotesPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            var remotes = await ApplicationData.Current.LocalFolder.ReadAsync<List<Remote>>("RemotesList");
            
            if (remotes?.Any() ?? false)
                IsTextBlockEmptyVisible = false;
            else
                IsTextBlockEmptyVisible = true;
        }

        private void AppBarButtonAddARemote_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(typeof(AddARemotePage));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
