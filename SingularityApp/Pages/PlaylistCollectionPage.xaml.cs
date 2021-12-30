using SonicAudioApp.Models;
using SonicAudioApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
            Init();
        }
        ~PlaylistCollectionPage()
        {
            PlaylistInfos.CollectionChanged += PlaylistInfos_CollectionChanged;
        }

        void Init()
        {
            PlaylistInfos = PlaylistManager.Playlist;
            if (PlaylistInfos.Count == 0)
                playlistTextGrid.Visibility = Visibility.Visible;
            else
                playlistTextGrid.Visibility = Visibility.Collapsed;
            PlaylistInfos.CollectionChanged += PlaylistInfos_CollectionChanged;
        }

        private void PlaylistInfos_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(PlaylistInfos.Count==0)
                playlistTextGrid.Visibility= Visibility.Visible;
            else
                playlistTextGrid.Visibility= Visibility.Collapsed;
        }

        static TextBox playlisTxtBox;
        private void newBtn_Click(object sender, RoutedEventArgs e)
        {
            ShowNewDialog();
        }
        public static async void ShowNewDialog(AudioQueueItem audioQueueItem=null)
        {
            var cd = new ContentDialog()
            {
                Title = "Add New Playlist",
                Content = "Ok",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
            };

            playlisTxtBox = new TextBox()
            {
                PlaceholderText = "Playlist Name",
            };
            playlisTxtBox.TextChanged += (sender, e) =>
            {
                cd.PrimaryButtonText = !string.IsNullOrWhiteSpace(playlisTxtBox.Text) ? "Create" : "";
            };
            cd.Content = playlisTxtBox;
            cd.PrimaryButtonClick += (_, _) => CreateNew(audioQueueItem);
            await cd.ShowAsync();
        }
        
        private static void CreateNew(AudioQueueItem item = null)
        {
            if (item == null)
                PlaylistManager.Add(playlisTxtBox.Text);
            else
                PlaylistManager.AddSong(playlisTxtBox.Text, item);

        }

        private void playlistBtn_Click(object sender, RoutedEventArgs e)
        {
            var v=PlaylistManager.Get((string)(sender as Button).Tag);
            if (v == null)
                return;
            var frame = HomePage.FindParent<Frame>(this);
            var str =
            frame.NavigateToType(typeof(PlaylistViewerPage), new PageIntent { FromPage = this, Type = PlayListType.CustomPlaylist,Data=v.Title },
                new FrameNavigationOptions { IsNavigationStackEnabled = true });
        }
    }
}
