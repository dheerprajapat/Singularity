using SonicAudioApp.Models;
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
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
        }

        private void focusBtn_Click(object sender, RoutedEventArgs e)
        {
            GoToPlaylist("https://www.youtube.com/playlist?list=PLzJK4umy8SSl0kUsUzVsBQCWgJVyZdZz6");
        }
        void GoToPlaylist(string url)
        {
            var frame = FindParent<Frame>(this);
            var str=
            frame.NavigateToType(typeof(PlaylistViewerPage), new PageIntent { FromPage=this,Url=url},
                new FrameNavigationOptions { IsNavigationStackEnabled=true});
        }
        public static T FindParent<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);

            if (parent == null) return null;

            var parentT = parent as T;
            return parentT ?? FindParent<T>(parent);
        }

        private void romanticBtn_Click(object sender, RoutedEventArgs e)
        {
            GoToPlaylist("https://www.youtube.com/playlist?list=PLgzTt0k8mXzE6H9DDgiY7Pd8pKZteis48");
        }

        private void chillBtn_Click(object sender, RoutedEventArgs e)
        {
            GoToPlaylist("https://www.youtube.com/playlist?list=PLgzTt0k8mXzEpH7-dOCHqRZOsakqXmzmG");
        }
    }
}
