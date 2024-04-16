using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using YoutubeExplode.Videos.Streams;
using YoutubeExplode;

namespace Singularity.Audio
{
    internal class AudioManager
    {
        public AudioQueue Queue { get; } = new AudioQueue();

        public AudioPlayer Audio { get; private set; }

        public AudioItem? Current => Queue.Songs.FirstOrDefault();

        public bool IsPlaying { get; private set; } = false;

        public async Task InitialzePlayerAsync(IJSRuntime runtime)
        {
            Audio = await AudioPlayer.CreateAsync(runtime);
            Audio.OnPlaying += Audio_OnPlaying;
            Audio.OnEnded += Audio_OnEnded;
        }
        ~AudioManager()
        {
            Audio.OnPlaying -= Audio_OnPlaying;
            Audio.OnEnded -= Audio_OnEnded;
        }

        private void Audio_OnEnded(object sender)
        {
            IsPlaying = false;
            OnAudioPlayPlaused?.Invoke(this);
        }

        private void Audio_OnPlaying(object sender)
        {
            IsPlaying = true;
            OnAudioPlayPlaused?.Invoke(this);
        }

        public void AddSong(AudioItem audio)
        {
            Queue.AddSong(audio);
        }
        public Task AddSongAsync(string url)
        {
            return Queue.AddSongAsync(url);
        }
        public void AddSongEnd(AudioItem audio)
        {
            Queue.AddSongEnd(audio);
        }
        public Task AddSongEndAsync(string url)
        {
            return Queue.AddSongEndAsync(url);
        }
        public async Task AddAndPlayAsync(AudioItem item)
        {
            AddSong(item);
            await PlayAsync();
        }

        public async ValueTask PlayAsync()
        {
            if (Current is null)
                return;
            await Current.LoadStreamData();
            await Audio.SetSrc(Current.StreamInfo!.Url);
            await Audio.Play();
        }

        public delegate void PlayPlausedHandler(object sender);
        public event PlayPlausedHandler? OnAudioPlayPlaused;
    }
}
