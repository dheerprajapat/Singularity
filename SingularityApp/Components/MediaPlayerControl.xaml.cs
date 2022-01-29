using SonicAudioApp.AudioEngine;
using SonicAudioApp.Pages;
using SonicAudioApp.Services;
using SonicAudioApp.Services.YoutubeSearch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using YoutubeExplode.Videos.Streams;

namespace SonicAudioApp.Components
{
    public sealed partial class MediaPlayerControl : UserControl
    {
        public MediaPlayerControl()
        {
            this.InitializeComponent();
            DataContext = this;
            AudioPlayer.PositionChanged += AudioPlayer_PositionChanged;
            AudioPlayer.PlaybackStateChanged += AudioPlayer_PlaybackStateChanged;
            AudioPlayer.SourceChanged += AudioPlayer_SourceChanged;
            AudioPlayer.AudioEnded += AudioPlayer_AudioEnded;
            Window.Current.SizeChanged += Current_SizeChanged;
            //sync icons
            SyncIcons();
        }
        ~MediaPlayerControl()
        {
            AudioPlayer.PositionChanged -= AudioPlayer_PositionChanged;
            AudioPlayer.PlaybackStateChanged -= AudioPlayer_PlaybackStateChanged;
            AudioPlayer.SourceChanged -= AudioPlayer_SourceChanged;
            AudioPlayer.AudioEnded -= AudioPlayer_AudioEnded;
            Window.Current.SizeChanged -= Current_SizeChanged;

        }


        private async void AudioPlayer_AudioEnded(Windows.Media.Playback.MediaPlayer sender)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,async () =>
             {
                 await AudioPlayer.PlayNextAsync();
             });
        }

        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            if(!videoPlayer.IsFullWindow && videoPlayer.MediaPlayer.PlaybackSession.PlaybackState==Windows.Media.Playback.MediaPlaybackState.Playing)
            {
                AudioPlayer.UpdatePosition(videoPlayer.MediaPlayer.PlaybackSession.Position);
                HideVideo();
            }
        }

        void SyncIcons()
        {
            GetRepeatIcon();
            IsLiked = AudioQueue.Current != null ? AudioQueue.Current.Liked : false;
        }



        public bool IsLiked
        {
            get { return (bool)GetValue(IsLikedProperty); }
            set { SetValue(IsLikedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsLiked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLikedProperty =
            DependencyProperty.Register("IsLiked", typeof(bool), typeof(MediaPlayerControl), new PropertyMetadata(false));



        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(MediaPlayerControl), new PropertyMetadata(""));



        public ImageSource Thumbnail
        {
            get { return (ImageSource)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("Thumbnail", typeof(ImageSource), typeof(MediaPlayerControl), new PropertyMetadata(new BitmapImage()));



        public string Singers
        {
            get { return (string)GetValue(SingersProperty); }
            set { SetValue(SingersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Singers.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SingersProperty =
            DependencyProperty.Register("Singers", typeof(string), typeof(MediaPlayerControl), new PropertyMetadata(""));




        public string Position
        {
            get { return (string)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(string), typeof(MediaPlayerControl), new PropertyMetadata("00:00"));


        public string TotalDuration
        {
            get { return (string)GetValue(TotalDurationProperty); }
            set { SetValue(TotalDurationProperty, value); }
        }

        public static readonly DependencyProperty TotalDurationProperty =
            DependencyProperty.Register("TotalDuration", typeof(string), typeof(MediaPlayerControl), new PropertyMetadata("00:00"));



        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(int), typeof(MediaPlayerControl), new PropertyMetadata(0));



        public int PositionValue
        {
            get { return (int)GetValue(PositionValueProperty); }
            set { SetValue(PositionValueProperty, value); }
        }

        public static readonly DependencyProperty PositionValueProperty =
            DependencyProperty.Register("PositionValue", typeof(int), typeof(MediaPlayerControl), new PropertyMetadata(0));



        private async void AudioPlayer_PositionChanged(Windows.Media.Playback.MediaPlaybackSession sender, object args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {

                if (Visibility != Visibility.Visible)
                    return;

                var totalDur =ConvertTimeSpanToDuration(AudioPlayer.TotalDuration);

                if(totalDur !=TotalDuration)
                    TotalDuration = totalDur;

                Position = ConvertTimeSpanToDuration(AudioPlayer.Position);
                PositionValue =(int) AudioPlayer.Position.TotalSeconds;


                var max=(int)AudioPlayer.TotalDuration.TotalSeconds;
                if (MaxValue != max)
                {
                    MaxValue = max;
                    Title = AudioQueue.Current.Title;
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.UriSource = new(AudioQueue.Current.ThumbnailUrl);
                    Thumbnail = bitmapImage;

                    Singers = AudioQueue.Current.Singers;
                }
            });
        }


        private Dictionary<UIElement, Brush> PreviousColors = new Dictionary<UIElement, Brush>();
        private void FontIcon_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var icon = sender as FontIcon;
            if (!PreviousColors.ContainsKey(icon))
                PreviousColors.Add(icon, icon.Foreground);

            if (icon.Name == "LikeButton")
                icon.Foreground = new SolidColorBrush(Colors.Pink);
            else
                icon.Foreground = new SolidColorBrush(Colors.DeepSkyBlue);
        }

        private void FontIcon_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            var icon = sender as FontIcon;
            icon.Foreground = PreviousColors.ContainsKey(icon) ? PreviousColors[icon] : new SolidColorBrush(Colors.White);
        }

        public static string ConvertTimeSpanToDuration(TimeSpan time)
        {
            StringBuilder sb = new StringBuilder();
            if(time.Hours != 0)
                sb.Append(time.Hours.ToString()+':');
            sb.Append(time.Minutes.ToString().PadLeft(2,'0'));
            sb.Append(':');
            sb.Append(time.Seconds.ToString().PadLeft(2, '0'));
            return sb.ToString();
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (Math.Abs(e.NewValue - e.OldValue) <= 1.5)
                return;
            AudioPlayer.UpdatePosition(TimeSpan.FromSeconds(e.NewValue));
        }

        private void Volume_SliderValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            AudioPlayer.Volume = (int)e.NewValue;
            AudioPlayer.ISMuted = false;
            SoundIcon.Glyph = GetVolumeIcon();
        }

        private void SoundIcon_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (AudioPlayer.ISMuted)
            {
                AudioPlayer.ISMuted = false;
                SoundIcon.Glyph = GetVolumeIcon();
            }
            else
            {
                AudioPlayer.ISMuted = true;
                SoundIcon.Glyph = "\ue74f";
            }
        }

        private string GetVolumeIcon()
        {
            var val = (int)(AudioPlayer.Volume*100);
            return val switch
            {
                >75=> "\ue995",
                <=75 and >25 => "\ue994",
                <=25  and >10 => "\ue993",
                _ => "\ue992",
            };
        }

        private async void AudioPlayer_PlaybackStateChanged(Windows.Media.Playback.MediaPlaybackSession sender, object args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (AudioQueue.Count == 0)
                {
                    var grid=HomePage.FindParent<Grid>(this);
                    if (grid.RowDefinitions.Count == 2)
                        grid.RowDefinitions.RemoveAt(1);
                    Visibility= Visibility.Collapsed;
                    return;
                }

                if (AudioPlayer.PlaybackState == Windows.Media.Playback.MediaPlaybackState.Paused)
                {
                    //PlayPauseIcon.Foreground=new SolidColorBrush(Colors.IndianRed);
                    PlayPauseIcon.Glyph = "\uf5b0";
                    AudioQueue.Current.WaveformVisibilty = Visibility.Collapsed;
                }
                else if (AudioPlayer.PlaybackState == Windows.Media.Playback.MediaPlaybackState.Playing)
                {
                    //PlayPauseIcon.Foreground = new SolidColorBrush(Colors.IndianRed);
                    PlayPauseIcon.Glyph = "\uf8ae";
                    AudioQueue.Current.WaveformVisibilty = Visibility.Visible;
                }
                else if(AudioPlayer.PlaybackState==Windows.Media.Playback.MediaPlaybackState.Buffering
                    || AudioPlayer.PlaybackState==Windows.Media.Playback.MediaPlaybackState.Opening)
                {
                    //PlayPauseIcon.Foreground = new SolidColorBrush(Colors.White);
                    PlayPauseIcon.Glyph = "\uebd3";
                    AudioQueue.Current.WaveformVisibilty = Visibility.Collapsed;
                }
            });
        }

        private async void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (AudioPlayer.PlaybackState == Windows.Media.Playback.MediaPlaybackState.Paused)
            {
                await AudioPlayer.PlayAsync(false);
            }
            else if (AudioPlayer.PlaybackState == Windows.Media.Playback.MediaPlaybackState.Playing)
            {
                AudioPlayer.Stop();
            }
        }
        private async void AudioPlayer_SourceChanged(Windows.Media.Playback.MediaPlayer sender, object args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Title = AudioQueue.Current.Title;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new(AudioQueue.Current.ThumbnailUrl);
                Thumbnail = bitmapImage;
                Singers = AudioQueue.Current.Singers;
                IsLiked = AudioQueue.Current.Liked;
            });
        }

        private async void previousBtn_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
           await AudioPlayer.PlayPreviousAsync();
        }

        private async void nextPlayBtn_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            await AudioPlayer.PlayNextAsync();
        }

        private void LikeButton_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (AudioQueue.Count < 1)
                return;

                IsLiked= !IsLiked;
            if(IsLiked)
                LikedSongManager.Add(AudioQueue.Current);
            else
                LikedSongManager.Remove(AudioQueue.Current);
        }

       
        private void loopMode_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SetNextRepeatMode();
            GetRepeatIcon();
        }
        void SetNextRepeatMode()
        {
            if (AudioQueue.Repeat == LoopMode.LoopSingle)
            {
                AudioQueue.Repeat = LoopMode.NoLoop;
            }
            else if (AudioQueue.Repeat == LoopMode.NoLoop)
            {
                AudioQueue.Repeat = LoopMode.LoopAll;
            }
            else
            {
                AudioQueue.Repeat = LoopMode.LoopSingle;
            }
        }

        void GetRepeatIcon()
        {
            if (AudioQueue.Repeat == LoopMode.LoopSingle)
            {
                loopMode.Glyph = "\uE8ED";
            }
            else if (AudioQueue.Repeat == LoopMode.NoLoop)
            {
                loopMode.Glyph = "\uF5E7";
            }
            else
            {
                loopMode.Glyph = "\uE8EE";
            }
        }

        private async void ShowVideo()
        {
            if (AudioQueue.Current is null)
                return;
            if (CurrentVideoId == null || CurrentVideoId != AudioQueue.Current.Id)
            {
                if(videoPlayer.MediaPlayer.Source is not null)
                 ((MediaSource)videoPlayer.MediaPlayer.Source).Dispose();

                videoPlayer.MediaPlayer.Source = MediaSource.CreateFromUri(await GetBestVideoUrl());
            }
            videoPlayer.MediaPlayer.PlaybackSession.Position= AudioPlayer.Position;
            videoPlayer.Visibility = Visibility.Visible;

            videoPlayer.MediaPlayer.Play();
        }

        private async Task<Uri> GetBestVideoUrl()
        {
           CurrentVideoId=AudioQueue.Current.Id;
           var r= await YoutubeManager.Youtube.Videos.Streams.GetManifestAsync(AudioQueue.Current.Id);
           var g = r.GetMuxedStreams().GetWithHighestVideoQuality();
           return new(g.Url);
        }
        public static string CurrentVideoId;
        private void HideVideo()
        {
            videoPlayer.MediaPlayer.Pause();
            videoPlayer.Visibility= Visibility.Collapsed;
        }
        private void thumnailImg_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            AudioPlayer.Stop();
            videoPlayer.MediaPlayer.RealTimePlayback = true;
            videoPlayer.IsFullWindow = true;
                ShowVideo();
        }

        private void videoPlayer_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (!videoPlayer.IsFullWindow)
                return;
            if (e.Key == Windows.System.VirtualKey.Escape)
            {
                e.Handled = true;
                HideVideo();
                videoPlayer.IsFullWindow=false;
            }
        }
    }
}
