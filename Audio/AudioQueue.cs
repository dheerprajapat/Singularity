using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace Singularity.Audio
{
    internal class AudioQueue
    {
        public List<IStreamInfo> Songs { get; } = new();

        public YoutubeClient Client { get; set; }

        public AudioQueue()
        {
            Client = SingletonFactory.YoutubeClient;
        }

        public void AddSong(IStreamInfo stream)
        {
            Songs.Insert(0, stream);
        }
        public async Task AddSongAsync(string url)
        {
            var streams = await Client.Videos.Streams.GetManifestAsync(url);
            var audio = streams.GetAudioStreams().GetWithHighestBitrate();
            AddSong(audio);
        }
        public void AddSongEnd(IStreamInfo stream)
        {
            Songs.Add(stream);
        }
        public async Task AddSongEndAsync(string url)
        {
            var streams = await Client.Videos.Streams.GetManifestAsync(url);
            var audio = streams.GetAudioStreams().GetWithHighestBitrate();
            AddSongEnd(audio);
        }
    }
}
