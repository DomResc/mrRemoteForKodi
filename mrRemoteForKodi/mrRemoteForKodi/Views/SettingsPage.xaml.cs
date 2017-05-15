using mrRemoteForKodi.Helpers;
using mrRemoteForKodi.Services;

using System.ComponentModel;
using System.Runtime.CompilerServices;

using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace mrRemoteForKodi.Views
{
    public sealed partial class SettingsPage : Page, INotifyPropertyChanged
    {
        // Add other settings as necessary. See https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/settings.md

        private bool _isLightThemeEnabled;
        public bool IsLightThemeEnabled
        {
            get { return _isLightThemeEnabled; }
            set { Set(ref _isLightThemeEnabled, value); }
        }

        private bool _isVibrateEnabled;
        public bool IsVibrateEnabled
        {
            get { return _isVibrateEnabled; }
            set { Set(ref _isVibrateEnabled, value); }
        }

        private string _appVersion;
        public string AppVersion
        {
            get { return _appVersion; }
            set { Set(ref _appVersion, value); }
        }

        private string _appName;
        public string AppName
        {
            get { return _appName; }
            set { Set(ref _appName, value); }
        }

        private string _appDeveloperName;
        public string AppDeveloperName
        {
            get { return _appDeveloperName; }
            set { Set(ref _appDeveloperName, value); }
        }

        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            IsLightThemeEnabled = ThemeSelectorService.IsLightThemeEnabled;
            IsVibrateEnabled = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(IsVibrateEnabled));
            AppVersion = GetAppVersion();
            AppName = GetAppName();
            AppDeveloperName = GetDeveloperName();
        }

        private string GetAppVersion()
        {
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        private string GetAppName()
        {
            var package = Package.Current;

            return $"{package.DisplayName}";
        }

        private string GetDeveloperName()
        {
            var package = Package.Current;

            return $"{package.PublisherDisplayName}";
        }

        private async void ThemeToggle_Toggled(object sender, RoutedEventArgs e)
        {
            //Only switch theme if value has changed (not on initialization)
            var toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn != ThemeSelectorService.IsLightThemeEnabled)
                    await ThemeSelectorService.SwitchThemeAsync();
            }
        }

        private async void VibrateToggle_Toggled(object sender, RoutedEventArgs e)
        {
            //Only switch vibrate if value has changed (not on initialization)
            var toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn)
                    await ApplicationData.Current.LocalSettings.SaveAsync(nameof(IsVibrateEnabled), true);
                else
                    await ApplicationData.Current.LocalSettings.SaveAsync(nameof(IsVibrateEnabled), false);
            }
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
