using System;
using Template10.Common;
using Template10.Services.SettingsService;
using Windows.UI.Xaml;

namespace mrRemoteForKodi_Update_1.Services.SettingsServices
{
    public class SettingsService
    {
        public static SettingsService Instance { get; } = new SettingsService();
        ISettingsHelper _helper;

        private SettingsService()
        {
            _helper = new SettingsHelper();
        }

        public bool VibrateOnPress
        {
            get { return _helper.Read<bool>(nameof(VibrateOnPress), true); }
            set { _helper.Write(nameof(VibrateOnPress), value); }
        }

        public bool UseSystemTheme
        {
            get { return _helper.Read<bool>(nameof(UseSystemTheme), true); }
            set { _helper.Write(nameof(UseSystemTheme), value); }
        }

        public string SettingVersion
        {
            get { return _helper.Read<string>(nameof(SettingVersion), null); }
            set { _helper.Write(nameof(SettingVersion), value.ToString()); }
        }

        public ApplicationTheme AppTheme
        {
            get
            {
                var theme = ApplicationTheme.Light;
                var value = _helper.Read<string>(nameof(AppTheme), theme.ToString());
                return Enum.TryParse<ApplicationTheme>(value, out theme) ? theme : ApplicationTheme.Dark;
            }
            set
            {
                _helper.Write(nameof(AppTheme), value.ToString());
                // set theme at reboot
                //(Window.Current.Content as FrameworkElement).RequestedTheme = value.ToElementTheme();
                //Views.Shell.HamburgerMenu.RefreshStyles(value);
            }
        }

        public TimeSpan CacheMaxDuration
        {
            get { return _helper.Read<TimeSpan>(nameof(CacheMaxDuration), TimeSpan.FromDays(2)); }
            set
            {
                _helper.Write(nameof(CacheMaxDuration), value);
                BootStrapper.Current.CacheMaxDuration = value;
            }
        }
    }
}