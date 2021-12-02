using Microsoft.UI.Xaml.Controls;
using SonicAudioApp.AudioEngine;
using SonicAudioApp.Components;
using SonicAudioApp.Models;
using SonicAudioApp.Services.YoutubeSearch;
using SonicAudioApp.Services.Ytdl;
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
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace SonicAudioApp.Pages
{
    public sealed partial class SearchPage : Page
    {
        public static YoutubeClient Youtube = new YoutubeClient();
        public SearchPage()
        {
            this.InitializeComponent();
        }


        public string SearchFilter
        {
            get { return (string)GetValue(SearchFilterProperty); }
            set { SetValue(SearchFilterProperty, value); }
        }

        public static readonly DependencyProperty SearchFilterProperty =
            DependencyProperty.Register("SearchFilter", typeof(string), typeof(SearchPage), new PropertyMetadata("Relevance"));

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

        private async void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (string.IsNullOrWhiteSpace(sender.Text))
                return;

            var res=await YoutubeSearch.GetVideosAsync(sender.Text);
            var newList = new List<AudioQueueItem>();

            foreach(var item in res)
            {
                newList.Add(new AudioQueueItem
                {
                    Id=item.VideoId,
                    Title=item.Title,
                    ThumbnailUrl=item.Image,
                    Singers=item.Author.Name,
                    VideoUrl=item.Url,
                    DurationString=MediaPlayerControl.ConvertTimeSpanToDuration(TimeSpan.FromSeconds(item.Seconds))
                });
            }
            Songs=newList;
        }


        public List<AudioQueueItem> Songs
        {
            get { return (List<AudioQueueItem>)GetValue(SongsProperty); }
            set { SetValue(SongsProperty, value); }
        }

        public static readonly DependencyProperty SongsProperty =
            DependencyProperty.Register("Songs", typeof(List<AudioQueueItem>), typeof(SearchPage), new PropertyMetadata(
                new List<AudioQueueItem> {  }
            ));

        private async void topResultGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            var s = sender as ListView;
            if (s.SelectedIndex < 0 || s.SelectedIndex >= Songs.Count)
                return;

            var c = Songs[s.SelectedIndex];
            var streamManifest = await Youtube.Videos.Streams.GetManifestAsync(c.Id);
            var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
            c.Url = streamInfo.Url;
            AudioQueue.AddAndPlay(c);
        }


        private async void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (string.IsNullOrWhiteSpace(sender.Text))
                return;
            sender.ItemsSource = await SearchSuggestions.SuggestionsAsync(sender.Text);

        }

        private Dictionary<Grid, bool> MouseInside = new();
        private void songItemGrid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var g = sender as Grid;
            foreach (var c in g.Children.OfType<FontIcon>())
            {
                if(c.Visibility!=Visibility.Visible)
                c.Visibility = Visibility.Visible;
            }
        }

        private void songItemGrid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            var g=sender as Grid;
            foreach(var c in g.Children.OfType<FontIcon>())
            {
                if (c.Name == "likeButton" && c.Glyph == "\uEB52")
                    continue;

                c.Visibility =Visibility.Collapsed;
            }

        }
    }
}
