using SonicAudioApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class PlaylistCollectionPage : Page
    {




        public ObservableCollection<PlaylistInfo> PlaylistInfos
        {
            get { return (ObservableCollection<PlaylistInfo>)GetValue(PlaylistInfosProperty); }
            set { SetValue(PlaylistInfosProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlaylistInfos.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaylistInfosProperty =
            DependencyProperty.Register("PlaylistInfos", typeof(ObservableCollection<PlaylistInfo>), typeof(PlaylistCollectionPage), new PropertyMetadata(new ObservableCollection<PlaylistInfo>()));




        public PlaylistCollectionPage()
        {
            this.InitializeComponent();
        }

        
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void newBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
