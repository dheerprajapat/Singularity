using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using YoutubeExplode.Videos.Streams;
using YoutubeExplode;
using CommunityToolkit.Maui.Core.Primitives;
using Singularity.Models;

namespace Singularity.Audio
{
    internal class AudioManager:BindableObject
    {
        public AudioQueue Queue { get; } = new AudioQueue();

        public AudioPlayer Audio { get; private set; }

        public AudioItem? Current => Queue.Songs.FirstOrDefault();

        public bool IsPlaying => Audio.MediaPlayer.CurrentState == CommunityToolkit.Maui.Core.Primitives.MediaElementState.Playing;

        public void Init()
        {
            if (Audio == null)
            {
                Audio = new();
                Audio.MediaPlayer.MediaEnded += MediaPlayer_MediaEnded; ;
            }
        }

        private void MediaPlayer_MediaEnded(object? sender, EventArgs e)
        {
            this.Dispatcher.Dispatch(async () =>
            {
                if (UserSetting.Instance.LoopMode == LoopMode.None)
                    return;

                else if (UserSetting.Instance.LoopMode == LoopMode.Same)
                {
                    Audio.CurrentTime = TimeSpan.Zero;
                    Audio.Play();
                }
                else if (UserSetting.Instance.LoopMode == LoopMode.All)
                {
                    await PlayNextAsync();
                }
            });
            

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
        public async Task PlayNextAsync()
        {
            if(Queue.Songs.Count <= 0)
            {
                return;
            }

            var first = Queue.Songs.First();
            Queue.Songs.Remove(first);
            Queue.AddSongEnd(first);
            Audio.CurrentTime=TimeSpan.Zero;
            await PlayAsync();
        }
        public async Task PlayPreviousAsync()
        {
            if (Queue.Songs.Count <= 0)
            {
                return;
            }

            var time = Audio.CurrentTime;

            if(time.TotalSeconds>5)
            {
                Audio.CurrentTime=TimeSpan.Zero;
                return;
            }

            var last = Queue.Songs.Last();
            Queue.Songs.Remove(last);
            Queue.AddSong(last);

            Audio.CurrentTime = TimeSpan.Zero;
            await PlayAsync();
        }

        

        public async Task PlayAsync()
        {
            if (Current is null)
                return;
            await Current.LoadStreamData();
             Audio.Src=Current.StreamInfo!.Url;
            Audio.Play();
        }
    }

    public enum LoopMode
    {
        All,
        None,
        Same
    }
}
