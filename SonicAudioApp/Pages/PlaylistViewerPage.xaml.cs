using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SonicAudioApp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class PlaylistViewerPage : Page
    {
        public PlaylistViewerPage()
        {
            this.InitializeComponent();
        }
        public string PlayListUrl
        {
            get { return (string)GetValue(PlayListUrlProperty); }
            set { SetValue(PlayListUrlProperty, value); }
        }
        public async void LoadInfoAsync()
        {

        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            PlayListUrl = (string)e.Parameter;
        }

        // Using a DependencyProperty as the backing store for PlayListUrl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayListUrlProperty =
            DependencyProperty.Register("PlayListUrl", typeof(string), typeof(PlaylistViewerPage), new PropertyMetadata(""));

    }
}
