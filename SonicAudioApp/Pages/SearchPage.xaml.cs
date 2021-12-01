using Microsoft.UI.Xaml.Controls;
using SonicAudioApp.Models;
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

namespace SonicAudioApp.Pages
{
    public sealed partial class SearchPage : Page
    {
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
        }

        private async void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var res=await YoutubeSearch.GetVideosAsync("Madonna - Frozen (Sickick Remix)");
            foreach(var item in res)
            {

            }
        }


        public List<AudioQueueItem> Songs
        {
            get { return (List<AudioQueueItem>)GetValue(SongsProperty); }
            set { SetValue(SongsProperty, value); }
        }

        public static readonly DependencyProperty SongsProperty =
            DependencyProperty.Register("Songs", typeof(List<AudioQueueItem>), typeof(SearchPage), new PropertyMetadata(
                new List<AudioQueueItem> { new AudioQueueItem("test url",null,null), new AudioQueueItem("test url 2", null, null) }
            ));


    }
}
