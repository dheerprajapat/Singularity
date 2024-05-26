using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Implementation;

namespace Singularity.Audio
{
    internal class AudioPlayer
    {
        private string? src;

        public string? Src
        {
            get
            {
                return src;
            }
            set
            {
                if(value!=null)
                    MediaPlayer.Source=MediaSource.FromUri(value);
                src = value;
            }
        }

        public TimeSpan Duration
        {
            get => MediaPlayer.Duration;
        }

        public TimeSpan CurrentTime
        {
            get => MediaPlayer.Position;
            set => MediaPlayer.SeekTo(value);
        }

        public double Volume
        {
            get => MediaPlayer.Volume;
            set => MediaPlayer.Volume=value;
        }

        internal MediaElement MediaPlayer { get; }
        public AudioPlayer(string? src=null)
        {
            MediaPlayer = MainPage.MediaElement;
            MediaPlayer.StateChanged += MediaPlayer_StateChanged;
            MediaPlayer.PositionChanged += MediaPlayer_PositionChanged;
            MediaPlayer.MediaOpened += MediaPlayer_MediaOpened;

            this.src = src;
        }

        private void MediaPlayer_MediaOpened(object? sender, EventArgs e)
        {
            OnLoadedMetadata?.Invoke(this);
        }

        private void MediaPlayer_PositionChanged(object? sender, MediaPositionChangedEventArgs e)
        {
            OnTimeUpdate?.Invoke(this);
        }

        ~AudioPlayer()
        {
            MediaPlayer.StateChanged -= MediaPlayer_StateChanged;
        }

        private void MediaPlayer_StateChanged(object? sender, MediaStateChangedEventArgs e)
        {
            OnMediaStateChange?.Invoke(this,e);
        }

        public void Play()
        {
            MediaPlayer.Play();
        }

        public void Pause()
        {
            MediaPlayer.Pause();
        }



        public delegate void AudioEventHandler(object sender);
        public delegate void MediaStateChangeHandler(object sender, MediaStateChangedEventArgs e);

        public event MediaStateChangeHandler? OnMediaStateChange;
        public event AudioEventHandler? OnTimeUpdate;
        public event AudioEventHandler? OnLoadedMetadata;

    }
    public enum ReadyStates
    {
        HaveNothing = 0,
        HaveMetadata = 1,
        HaveCurrentData = 2,
        HaveFutureData = 3,
        HaveEnoughData
    }
}
