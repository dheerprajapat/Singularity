using Microsoft.UI.Xaml.Controls;
using SonicAudioApp.AudioEngine;
using SonicAudioApp.Components;
using SonicAudioApp.Models;
using SonicAudioApp.Services.YoutubeSearch;
using SonicAudioApp.Services.Ytdl;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
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

        SortedSet<long> previousTimeStampSet = new();
        private async void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (string.IsNullOrWhiteSpace(sender.Text))
                return;

            //show loading spinner and hide everything else
            LoadingVisibilty = Visibility.Visible;
            infoPanel.Visibility = Visibility.Collapsed;
            topResultLabel.Text = "";

            var currentTimeStamp = DateTime.Now.Ticks;
            try
            {
                if (previousTimeStampSet.Count > 0 && previousTimeStampSet.Last() > currentTimeStamp)
                    return;

               Songs=SearchedItems=new( await YoutubeSearch.SearchFor(sender.Text));
              
                previousTimeStampSet.Clear();
                previousTimeStampSet.Add(currentTimeStamp);

               // AssignCollectionFromSearchResult(res);
            }
            finally
            {
                //hide loading spinner and show everything else
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
