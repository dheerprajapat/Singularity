using SonicAudioApp.AudioEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


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
        }

       

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
                var totalDur =ConvertTimeSpanToDuration(AudioPlayer.TotalDuration);
                if(totalDur !=TotalDuration)
                    TotalDuration = totalDur;

                Position = ConvertTimeSpanToDuration(AudioPlayer.Position);
                PositionValue =(int) AudioPlayer.Position.TotalSeconds;
                var max=(int)AudioPlayer.TotalDuration.TotalSeconds;
                if(MaxValue!=max)
                    MaxValue = max;
            });
        }

        ~MediaPlayerControl()
        {
            AudioPlayer.PositionChanged-=AudioPlayer_PositionChanged;
            AudioPlayer.PlaybackStateChanged -= AudioPlayer_PlaybackStateChanged;
        }

        private Dictionary<UIElement, Brush> PreviousColors = new Dictionary<UIElement, Brush>();
        private void FontIcon_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var icon = sender as FontIcon;
            if (!PreviousColors.ContainsKey(icon))
                PreviousColors.Add(icon, icon.Foreground);

            if (icon.Name == "LikeButton")
                icon.Foreground = new SolidColorBrush(Colors.Green);
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
                sb.Append(time.Hours.ToString().PadLeft(2, '0')+':');
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
            AudioPlayer.Volume = e.NewValue/100.0;
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
                if (AudioPlayer.PlaybackState == Windows.Media.Playback.MediaPlaybackState.Paused)
                {
                    PlayPauseIcon.Glyph = "\uf5b0";
                }
                else if (AudioPlayer.PlaybackState == Windows.Media.Playback.MediaPlaybackState.Playing)
                {
                    PlayPauseIcon.Glyph = "\uf8ae";
                }
                else if(AudioPlayer.PlaybackState==Windows.Media.Playback.MediaPlaybackState.Buffering)
                {
                    PlayPauseIcon.Glyph = "\uebd3";
                }
            });
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (AudioPlayer.PlaybackState == Windows.Media.Playback.MediaPlaybackState.Paused)
            {
                AudioPlayer.Resume();
            }
            else if (AudioPlayer.PlaybackState == Windows.Media.Playback.MediaPlaybackState.Playing)
            {
                AudioPlayer.Stop();
            }
        }
    }
}
