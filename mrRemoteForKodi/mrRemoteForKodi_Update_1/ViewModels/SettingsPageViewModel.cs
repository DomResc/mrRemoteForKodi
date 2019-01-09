using mrRemoteForKodi_Update_1.Services.SettingsServices;
using System;
using Template10.Mvvm;
using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace mrRemoteForKodi_Update_1.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        public SettingsPartViewModel SettingsPartViewModel { get; } = new SettingsPartViewModel();
        public AboutPartViewModel AboutPartViewModel { get; } = new AboutPartViewModel();
    }

    public class SettingsPartViewModel : ViewModelBase
    {
        SettingsService _settings;

        public SettingsPartViewModel()
        {
            _settings = SettingsService.Instance;
        }

        public bool VibrateOnPress
        {
            get { return _settings.VibrateOnPress.Equals(true); }
            set { _settings.VibrateOnPress = value ? true : false; base.RaisePropertyChanged(); }
        }

        public bool UseSystemTheme
        {
            get { return _settings.UseSystemTheme.Equals(true); }
            set { _settings.UseSystemTheme = value ? true : false; base.RaisePropertyChanged(); }
        }

        public bool UseLightThemeButton
        {
            get { return _settings.AppTheme.Equals(ApplicationTheme.Light); }
            set { _settings.AppTheme = value ? ApplicationTheme.Light : ApplicationTheme.Dark; base.RaisePropertyChanged(); }
        }
    }

    public class AboutPartViewModel : ViewModelBase
    {
        public Uri Logo => Package.Current.Logo;

        public string DisplayName => Package.Current.DisplayName;

        public string Publisher => Package.Current.PublisherDisplayName;

        public string Version
        {
            get
            {
                var v = Package.Current.Id.Version;
                return $"{v.Major}.{v.Minor}.{v.Build}.{v.Revision}";
            }
        }

        public Uri Website => new Uri("http://domenicorescigno.wordpress.com");
        public Uri Email => new Uri("mailto:domenico.rescigno@gmail.com");
        public Uri Twitter => new Uri("http://twitter.com/domresc");
        public Uri RateMe => new Uri("ms-windows-store://review/?ProductId=9nblggh4qvkx");
    }
}