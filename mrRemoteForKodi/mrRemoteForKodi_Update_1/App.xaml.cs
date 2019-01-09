using Microsoft.EntityFrameworkCore;
using mrRemoteForKodi_Update_1.Models;
using mrRemoteForKodi_Update_1.Services.SettingsServices;
using System;
using System.Threading.Tasks;
using Template10.Controls;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace mrRemoteForKodi_Update_1
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki

    [Bindable]
    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();
            SplashFactory = (e) => new Views.Splash(e);

            #region App settings

            var settings = SettingsService.Instance;

            // set app theme only if the UseSystemTheme toggle in settings is off
            if (settings.UseSystemTheme == false)
                RequestedTheme = settings.AppTheme;

            CacheMaxDuration = settings.CacheMaxDuration;

            #endregion
        }

        public override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            if (Window.Current.Content as ModalDialog == null)
            {
                // create a new frame 
                var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);

                // create modal root
                Window.Current.Content = new ModalDialog
                {
                    DisableBackButtonWhenModal = true,
                    Content = new Views.Shell(nav),
                    ModalContent = new Views.Busy(),
                };
            }

            // set window min size
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(320, 500));

            // hide statusbar if mobile
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = StatusBar.GetForCurrentView();
                if (statusBar != null)
                {
                    await statusBar.HideAsync();
                }
            }
            await Task.CompletedTask;
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            using (var db = new MovieContext())
            {
                db.Database.Migrate();
            }

            using (var db = new TvShowContext())
            {
                db.Database.Migrate();
            }

            using (var db = new ArtistContext())
            {
                db.Database.Migrate();
            }

            using (var db = new RemoteContext())
            {
                db.Database.Migrate();

                // if database is empty go to remotelistpage
                if (await db.Remotes.CountAsync() == 0)
                    NavigationService.Navigate(typeof(Views.RemotesListPage));
                // else go to mainpage
                else
                    NavigationService.Navigate(typeof(Views.MainPage));
            }

            var settings = SettingsService.Instance;
            var settingVersion = settings.SettingVersion;
            var packageVersion = Package.Current.Id.Version;

            if (settingVersion != $"{packageVersion.Major}.{packageVersion.Minor}.{packageVersion.Build}.{packageVersion.Revision}")
            {
                var updateDialog = new ContentDialog()
                {
                    Title = $"{packageVersion.Major}.{packageVersion.Minor}.{packageVersion.Build}.{packageVersion.Revision}",
                    Content = "Thanks for all the donations, requests and bugs reported" + Environment.NewLine + Environment.NewLine +
                              "I've been really busy at this time so unfortunately, I had little time for developing mrRemote. However, a few bugs have been fixed and some requested feature has been added." + Environment.NewLine + Environment.NewLine +
                              "Go to Settings->Changelog for a complete list of the changes.",
                    PrimaryButtonText = "Continue"
                };

                await updateDialog.ShowAsync();

                settings.SettingVersion = $"{packageVersion.Major}.{packageVersion.Minor}.{packageVersion.Build}.{packageVersion.Revision}";
            }

            await Task.CompletedTask;
        }
    }
}