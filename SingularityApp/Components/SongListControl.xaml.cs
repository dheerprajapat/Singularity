using SonicAudioApp.AudioEngine;
using SonicAudioApp.Models;
using SonicAudioApp.Pages;
using SonicAudioApp.Services;
using SonicAudioApp.Services.YoutubeSearch;
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
using YoutubeExplode.Videos.Streams;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SonicAudioApp.Components
{
    public sealed partial class SongListControl : UserControl
    {
        public SongListControl()
        {
            this.InitializeComponent();
            AudioPlayer.SourceChanged += AudioPlayer_SourceChanged;
        }
        ~SongListControl()
        {
            AudioPlayer.SourceChanged -= AudioPlayer_SourceChanged;
        }
        int previousWaveIndex = -1;

        public ObservableCollection<AudioQueueItem> Songs
        {
            get { return (ObservableCollection<AudioQueueItem>)GetValue(SongsProperty); }
            set { SetValue(SongsProperty, value); }
        }

        public static readonly DependencyProperty SongsProperty =
            DependencyProperty.Register("Songs", typeof(ObservableCollection<AudioQueueItem>), typeof(SongListControl), new PropertyMetadata(new ObservableCollection<AudioQueueItem>()));



        public Visibility RemoveBtnVisibilty
        {
            get { return (Visibility)GetValue(RemoveBtnVisibiltyProperty); }
            set { SetValue(RemoveBtnVisibiltyProperty, value); }
        }

        public static readonly DependencyProperty RemoveBtnVisibiltyProperty =
            DependencyProperty.Register("RemoveBtnVisibilty", typeof(Visibility), typeof(SongListControl), new PropertyMetadata(Visibility.Collapsed));




        public ListType SongListType
        {
            get { return (ListType)GetValue(SongListTypeProperty); }
            set { SetValue(SongListTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SongListType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SongListTypeProperty =
            DependencyProperty.Register("SongListType", typeof(ListType), typeof(SongListControl), new PropertyMetadata(ListType.Other));


        private async void topResultGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            var s = sender as ListView;
            //ignore invalid selection
            if (s.SelectedIndex < 0 || s.SelectedIndex >= Songs.Count)
                return;

            //get current song
            var c = Songs[s.SelectedIndex];

            await AudioQueue.AddAndPlayAsync(c);
        }

        private async void AudioPlayer_SourceChanged(Windows.Media.Playback.MediaPlayer sender, object args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (AudioQueue.Current == null)
                    return;

                foreach (var s in Songs)
                {
                    s.WaveformVisibilty = Visibility.Collapsed;
                }

                if (previousWaveIndex != -1 && Songs.Count > previousWaveIndex)
                {
                    Songs[previousWaveIndex].WaveformVisibilty = Visibility.Collapsed;
                }
                var ind = Songs.IndexOf(AudioQueue.Current);
                if (ind != -1)
                {
                    Songs[ind].WaveformVisibilty = Visibility.Visible;
                }
                previousWaveIndex = ind;
            });

        }

        private async void addToQBtn_Click(object sender, RoutedEventArgs e)
        {
            var d=GetAudioItem(e.OriginalSource);
            if(d is not null)
            {
                if (AudioQueue.Count==0)
                    await AudioQueue.AddAndPlayAsync(d);
                else
                    AudioQueue.Add(d);
            }
        }
        private AudioQueueItem GetAudioItem(object e)
        {
            return (AudioQueueItem)((AppBarButton)e).DataContext;
        }

        private void likedislikeBtn_Click(object sender, RoutedEventArgs e)
        {
            var d = GetAudioItem(e.OriginalSource);
            if(d.Liked)
            {
                LikedSongManager.Remove(d);
            }
            else
            {
                LikedSongManager.Add(d);
            }
        }

        
        private void playlistBtn_Loading(FrameworkElement sender, object args)
        {
            var btn = sender as AppBarButton;
            var m = new MenuFlyout();
            btn.Flyout = m;
            var song=GetAudioItem(sender);
            var createNew = new MenuFlyoutItem { Text = "Create New" };
            m.Items.Add(createNew);
            createNew.Click += (_, _) => PlaylistCollectionPage.ShowNewDialog(song);
            foreach (var item in PlaylistManager.Playlist)
            {
                var ttm = new MenuFlyoutItem() { Text = item.Title };
                ttm.Click += (_,_)=> ClickedItem(song,item.Title);
                m.Items.Add(ttm);
            }
            
        }

        private void ClickedItem(AudioQueueItem song,string playListName)
        {
            if(!string.IsNullOrWhiteSpace(playListName))
            {
                PlaylistManager.AddSong(playListName,song);
            }
        }

        private void removeBtn_Loading(FrameworkElement sender, object args)
        {
            var btn= sender as AppBarButton;
            btn.Visibility = RemoveBtnVisibilty;
        }

        public enum ListType
        {
            Other,
            Recent,
            Playlist
        }

        private void removeBtn_Click(object sender, RoutedEventArgs e)
        {
            var d = GetAudioItem(e.OriginalSource);

            if (SongListType==ListType.Recent)
            {
                AudioQueue.Remove(d);
            }
        }
    }
}
