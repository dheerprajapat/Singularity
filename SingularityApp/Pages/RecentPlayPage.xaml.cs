using SonicAudioApp.AudioEngine;
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
    public sealed partial class RecentPlayPage : Page
    {
        public RecentPlayPage()
        {
            this.InitializeComponent();
            Songs = AudioQueue.Queue;
            HideInfoPanel();
            Songs.CollectionChanged += Songs_CollectionChanged;
        }
        
        private async void Songs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                HideInfoPanel();
            });
        }

        void HideInfoPanel()
        {
            if (AudioQueue.Count > 0)
                loaderPanel.Visibility = Visibility.Collapsed;
            else
                loaderPanel.Visibility = Visibility.Visible;
        }

        ~RecentPlayPage()
        {
            Songs.CollectionChanged -= Songs_CollectionChanged;
        }

        public ObservableCollection<AudioQueueItem> Songs
        {
            get { return (ObservableCollection<AudioQueueItem>)GetValue(SongsProperty); }
            set { SetValue(SongsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SongsProperty =
            DependencyProperty.Register("Songs", typeof(int), typeof(RecentPlayPage), new PropertyMetadata(new ObservableCollection<AudioQueueItem>()));


    }
}
