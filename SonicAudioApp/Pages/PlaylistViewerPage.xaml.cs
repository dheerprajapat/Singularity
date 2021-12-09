using SonicAudioApp.AudioEngine;
using SonicAudioApp.Components;
using SonicAudioApp.Models;
using SonicAudioApp.Services.YoutubeSearch;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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

            progress.Visibility = Visibility.Collapsed;
            playlistHeaderGrid.Visibility = Visibility.Visible;

            Currentplaylist = new() { Title = info.Title, Thumbnail = info.Thumbnails.GetWithHighestResolution().Url, Author = info.Author.Title };

            var songs = await YoutubeManager.Youtube.Playlists.GetVideosAsync(PlayListUrl);
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
            Songs = new(list);
        }

        PageIntent PageIntent;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            PageIntent= (PageIntent)e.Parameter;
            PlayListUrl = PageIntent.Url;
            if (PageIntent.Type == PlayListType.Youtube)
                LoadInfoAsync();
            else if (PageIntent.Type == PlayListType.Liked)
                ProcessLikedSongs();
        }

        // Using a DependencyProperty as the backing store for PlayListUrl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayListUrlProperty =
            DependencyProperty.Register("PlayListUrl", typeof(string), typeof(PlaylistViewerPage), new PropertyMetadata(""));


        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame contentFrame=null;
            var type=PageIntent.FromPage.GetType();
            if (PageIntent.FromPage is HomePage)
            {
                contentFrame= HomePage.FindParent<Frame>(this);
                contentFrame.Navigate(typeof(HomePage));
            }
        }

        async void ProcessLikedSongs()
        {
            var users=await User.FindAllAsync();
            var current = users.Where(p => p.AuthenticationStatus == UserAuthenticationStatus.LocallyAuthenticated &&
                            p.Type == UserType.LocalUser).FirstOrDefault();

            // user may have username
            var data = await current.GetPropertyAsync(KnownUserProperties.DisplayName);
            string displayName = (string)data;

            Currentplaylist = new PlaylistInfo
            {
                Author = displayName,
                Title="Liked",
                Thumbnail= "../Assets/heart.jpg"
            };
            progress.Visibility = Visibility.Collapsed;
            playlistHeaderGrid.Visibility = Visibility.Visible;
        }

        private async void playAllBtn_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < Songs.Count; i++)
            {
                AudioQueueItem s = Songs[i];
                if (i == 0)
                    await AudioQueue.AddAndPlayAsync(s);
                else
                    AudioQueue.InsertAt(s,i);
            }
        }
    }
}
