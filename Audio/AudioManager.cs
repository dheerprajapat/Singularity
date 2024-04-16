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

        public IStreamInfo? Current => Queue.Songs.FirstOrDefault();

        public async Task InitialzePlayerAsync(IJSRuntime runtime)
        {
            Audio = await AudioPlayer.CreateAsync(runtime);
        }

        public void AddSong(IStreamInfo stream)
        {
            Queue.AddSong(stream);
        }
        public Task AddSongAsync(string url)
        {
            return Queue.AddSongAsync(url);
        }
        public async ValueTask PlayAsync()
        {
            if (Current is null)
                return;

            await Audio.SetSrc(Current.Url);
            await Audio.Play();
        }

    }
}
