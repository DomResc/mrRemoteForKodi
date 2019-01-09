using Template10.Services.SerializationService;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using mrRemoteForKodi_Update_1.Services.SettingsServices;

namespace mrRemoteForKodi_Update_1.Views
{
    public sealed partial class SettingsPage : Page
    {
        ISerializationService _SerializationService;
        private SettingsService _settings;
        private ApplicationTheme _startTheme;
        private bool _startSystemTheme;

        public SettingsPage()
        {
            InitializeComponent();
            _SerializationService = SerializationService.Json;
            // load the apptheme setting 
            _settings = SettingsService.Instance;
            _startTheme = _settings.AppTheme;
            _startSystemTheme = _settings.UseSystemTheme;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MyPivot.SelectedIndex = int.Parse(_SerializationService.Deserialize(e.Parameter?.ToString()).ToString());
        }

        private void toggleSwitchSystemTheme_Toggled(object sender, RoutedEventArgs e)
        {
            if (toggleSwitchSystemTheme.IsOn == true)
            {
                UseLightThemeToggleSwitch.Visibility = Visibility.Collapsed;
                if (_startSystemTheme == _settings.UseSystemTheme && _startTheme == _settings.AppTheme)
                    textBlockRebootApp.Visibility = Visibility.Collapsed;
                else
                    textBlockRebootApp.Visibility = Visibility.Visible;
            }
            else
            {
                UseLightThemeToggleSwitch.Visibility = Visibility.Visible;
                if (_startSystemTheme == _settings.UseSystemTheme && _startTheme == _settings.AppTheme)
                    textBlockRebootApp.Visibility = Visibility.Collapsed;
                else
                    textBlockRebootApp.Visibility = Visibility.Visible;
            }
        }

        private void UseLightThemeToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (_startSystemTheme == _settings.UseSystemTheme && _startTheme == _settings.AppTheme)
                textBlockRebootApp.Visibility = Visibility.Collapsed;
            else
                textBlockRebootApp.Visibility = Visibility.Visible;
        }
    }
}