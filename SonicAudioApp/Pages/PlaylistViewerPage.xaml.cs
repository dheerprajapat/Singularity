using SonicAudioApp.AudioEngine;
using SonicAudioApp.Components;
using SonicAudioApp.Models;
using SonicAudioApp.Services.YoutubeSearch;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using YoutubeExplode.Common;

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


        public ObservableCollection<AudioQueueItem> Songs
        {
            get { return (ObservableCollection<AudioQueueItem>)GetValue(SongsProperty); }
            set { SetValue(SongsProperty, value); }
        }

        public static readonly DependencyProperty SongsProperty =
            DependencyProperty.Register("Songs", typeof(ObservableCollection<AudioQueueItem>), typeof(PlaylistViewerPage), new PropertyMetadata(new ObservableCollection<AudioQueueItem>()));



        public PlaylistInfo Currentplaylist
        {
            get { return (PlaylistInfo)GetValue(CurrentplaylistProperty); }
            set { SetValue(CurrentplaylistProperty, value); }
        }

        public static readonly DependencyProperty CurrentplaylistProperty =
            DependencyProperty.Register("Currentplaylist", typeof(PlaylistInfo), typeof(PlaylistViewerPage), new PropertyMetadata(new PlaylistInfo()));


        public async void LoadInfoAsync()
        {
            var info =await YoutubeManager.Youtube.Playlists.GetAsync(PlayListUrl);

            Currentplaylist=new() { Title=info.Title,Thumbnail=info.Thumbnails.GetWithHighestResolution().Url,Author=info.Author.Title};

            var songs= await YoutubeManager.Youtube.Playlists.GetVideosAsync(PlayListUrl);
            var list = new List<AudioQueueItem>();
            foreach(var song in songs)
            {
                list.Add(new()
                {
                    Id = song.Id,
                    ThumbnailUrl = song.Thumbnails.TryGetWithHighestResolution().Url,
                    VideoUrl = song.Url,
                    Title = song.Title,
                    Singers = song.Author.Title,
                    DurationString = MediaPlayerControl.ConvertTimeSpanToDuration(song.Duration.Value),
                });

                if (AudioQueue.Current != null && AudioQueue.Current.Id == list.Last().Id)
                {
                    list.Last().WaveformVisibilty = Visibility.Visible;
                }
            }
            progress.Visibility = Visibility.Collapsed;
            Songs = new(list);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            PlayListUrl = (string)e.Parameter;
            LoadInfoAsync();
        }

        // Using a DependencyProperty as the backing store for PlayListUrl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayListUrlProperty =
            DependencyProperty.Register("PlayListUrl", typeof(string), typeof(PlaylistViewerPage), new PropertyMetadata(""));

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            var contentFrame=HomePage.FindParent<Frame>(this);
            contentFrame.Navigate(typeof(HomePage));
        }
    }
}
