using SonicAudioApp.AudioEngine;
using SonicAudioApp.Models;
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



        private async void topResultGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            var s = sender as ListView;
            //ignore invalid selection
            if (s.SelectedIndex < 0 || s.SelectedIndex >= Songs.Count)
                return;

            //get current song
            var c = Songs[s.SelectedIndex];

            await YoutubeManager.UpdateUrlAsync(c);
            AudioQueue.AddAndPlay(c);
        }

        private async void AudioPlayer_SourceChanged(Windows.Media.Playback.MediaPlayer sender, object args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (AudioQueue.Current == null)
                    return;

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
            var d=(AudioQueueItem)((AppBarButton)e.OriginalSource).DataContext;
            if(d is not null)
            {
                await YoutubeManager.UpdateUrlAsync(d);

                if (AudioQueue.Count==0)
                    AudioQueue.AddAndPlay(d);
                else
                    AudioQueue.Add(d);
            }
        }
    }
}
