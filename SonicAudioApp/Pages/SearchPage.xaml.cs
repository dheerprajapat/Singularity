using Microsoft.UI.Xaml.Controls;
using SonicAudioApp.AudioEngine;
using SonicAudioApp.Components;
using SonicAudioApp.Models;
using SonicAudioApp.Services.YoutubeSearch;
using SonicAudioApp.Services.Ytdl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace SonicAudioApp.Pages
{
    public sealed partial class SearchPage : Page
    {
        public SearchPage()
        {
            this.InitializeComponent();
            CheckedPreserveMode();
            AudioPlayer.SourceChanged += AudioPlayer_SourceChanged;
        }
        ~SearchPage()
        {
            AudioPlayer.SourceChanged -= AudioPlayer_SourceChanged;
        }
        int previousWaveIndex = -1;
        private async void AudioPlayer_SourceChanged(Windows.Media.Playback.MediaPlayer sender, object args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (AudioQueue.Current == null)
                    return;

                if(previousWaveIndex!=-1 && Songs.Count>previousWaveIndex)
                {
                    Songs[previousWaveIndex].WaveformVisibilty = Visibility.Collapsed;
                }
                var ind=Songs.IndexOf(AudioQueue.Current);
                if(ind!=-1)
                {
                    Songs[ind].WaveformVisibilty=Visibility.Visible;
                }
                previousWaveIndex = ind;
            });

        }

        void CheckedPreserveMode()
        {
            if (SearchedItems.Count == 0)
                return;

            topResultLabel.Visibility = Visibility.Visible;
            LoadingVisibilty = Visibility.Collapsed;
            infoPanel.Visibility = Visibility.Collapsed;
            Songs = SearchedItems;
        }


        public Visibility LoadingVisibilty
        {
            get { return (Visibility)GetValue(LoadingVisibiltyProperty); }
            set { SetValue(LoadingVisibiltyProperty, value); }
        }

        public static readonly DependencyProperty LoadingVisibiltyProperty =
            DependencyProperty.Register("LoadingVisibilty", typeof(Visibility), typeof(SearchPage), new PropertyMetadata(Visibility.Collapsed));


        public string SearchFilter
        {
            get { return (string)GetValue(SearchFilterProperty); }
            set { SetValue(SearchFilterProperty, value); }
        }

        public static readonly DependencyProperty SearchFilterProperty =
            DependencyProperty.Register("SearchFilter", typeof(string), typeof(SearchPage), new PropertyMetadata("Relevance"));

        //this is cache of searched songs for next load
        public static ObservableCollection<AudioQueueItem> SearchedItems = new();

        public enum FilterModes
        {
            Relevance,
            Views,
            UploadDate,
            Rating
        }

        private void RadioMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var radio= (RadioMenuFlyoutItem)sender;
            if(radio.IsChecked)
            {
                var text=(radio.Text.Replace(" ", ""));
                if(Enum.TryParse(text, out FilterModes mode))
                {
                    SearchFilter = radio.Text;
                }
            }
        }

        private void SearchBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            SearchBox_QuerySubmitted(sender, null);
        }
        private SortedSet<long> RecentResultDict = new();

        private async void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (string.IsNullOrWhiteSpace(sender.Text))
                return;

            LoadingVisibilty = Visibility.Visible;
            infoPanel.Visibility = Visibility.Collapsed;
            topResultLabel.Text = "";
            var ts = DateTime.Now.Ticks;
            try
            {

                var res = await YoutubeSearch.GetVideosAsync(sender.Text);
                if (RecentResultDict.Count > 0 && RecentResultDict.Last() > ts)
                    return;
                if(RecentResultDict.Count > 0)
                {
                    RecentResultDict.Clear();
                    RecentResultDict.Add(ts);
                }
                var newList = new List<AudioQueueItem>();

                foreach (var item in res)
                {
                    newList.Add(new AudioQueueItem
                    {
                        Id = item.VideoId,
                        Title = item.Title,
                        ThumbnailUrl = item.Image,
                        Singers = item.Author.Name,
                        VideoUrl = item.Url,
                        DurationString = MediaPlayerControl.ConvertTimeSpanToDuration(TimeSpan.FromSeconds(item.Seconds.Value))
                    });
                }
                SearchedItems = Songs = new(newList);
            }
            finally
            {
                LoadingVisibilty = Visibility.Collapsed;
                topResultLabel.Text = "Top Results";
            }
        }

        public ObservableCollection<AudioQueueItem> Songs
        {
            get { return (ObservableCollection<AudioQueueItem>)GetValue(SongsProperty); }
            set { SetValue(SongsProperty, value); }
        }

        public static readonly DependencyProperty SongsProperty =
            DependencyProperty.Register("Songs", typeof(ObservableCollection<AudioQueueItem>), typeof(SearchPage), new PropertyMetadata(
                new ObservableCollection<AudioQueueItem> {  }
            ));


        private async void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (string.IsNullOrWhiteSpace(sender.Text))
                return;
            sender.ItemsSource = await SearchSuggestions.SuggestionsAsync(sender.Text);

        }

    }
}
