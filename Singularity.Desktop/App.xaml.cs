using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.ExtendedExecution.Foreground;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Singularity.Desktop
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                Window.Current.Activate();
            }

            if (_session == null)
                await PreventFromSuspending();
        }

        ExtendedExecutionForegroundSession _session;
        private async Task PreventFromSuspending()
        {
            ExtendedExecutionForegroundSession newSession = new ExtendedExecutionForegroundSession();
            newSession.Reason = ExtendedExecutionForegroundReason.Unconstrained;
            newSession.Revoked += SessionRevoked;

            ExtendedExecutionForegroundResult result = await newSession.RequestExtensionAsync();
            switch (result)
            {
                case ExtendedExecutionForegroundResult.Allowed:
                    _session = newSession;
                    break;
                default:
                case ExtendedExecutionForegroundResult.Denied:
                    newSession.Dispose();
                    break;
            }
        }

        private void SessionRevoked(object sender, ExtendedExecutionForegroundRevokedEventArgs args)
        {
            if (_session != null)
            {
                _session.Dispose();
                _session = null;
            }
        }
    }
}
