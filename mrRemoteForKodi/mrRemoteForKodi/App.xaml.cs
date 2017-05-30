using mrRemoteForKodi.Helpers;
using mrRemoteForKodi.Models;
using mrRemoteForKodi.Services;
using mrRemoteForKodi.Views;

using System;
using System.Collections.Generic;
using System.Linq;

using Windows.ApplicationModel.Activation;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace mrRemoteForKodi
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private Lazy<ActivationService> _activationService;
        private ActivationService ActivationService { get { return _activationService.Value; } }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();

            //Deferred execution until used. Check https://msdn.microsoft.com/library/dd642331(v=vs.110).aspx for further info on Lazy<T> class.
            _activationService = new Lazy<ActivationService>(CreateActivationService);
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (!e.PrelaunchActivated)
            {
                await ActivationService.ActivateAsync(e);
            }
        }

        /// <summary>
        /// Invoked when the application is activated by some means other than normal launching.
        /// </summary>
        /// <param name="args">Event data for the event.</param>
        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }

        private ActivationService CreateActivationService()
        {
            StatusBarSettingsAsync();

            if (!IsConfiguredRemotesEmpty())
                return new ActivationService(this, typeof(MainPage), new ShellPage());
            else
                return new ActivationService(this, typeof(ConfiguredRemotesPage), new ShellPage());
        }

        // Check if there is remote in applicationdata
        private bool IsConfiguredRemotesEmpty()
        {
            var remotesTask = ApplicationData.Current.LocalFolder.ReadAsync<List<Remote>>("RemotesList");

            var remotes = remotesTask.Result;

            if (remotes?.Any() ?? false)
                return false;
            else
                return true;
        }

        // Disable statusbar on mobile
        private async void StatusBarSettingsAsync()
        {
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = StatusBar.GetForCurrentView();
                if (statusBar != null)
                {
                    await statusBar.HideAsync();
                }
            }
        }
    }
}
