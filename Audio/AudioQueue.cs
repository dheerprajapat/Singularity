using System.Collections.ObjectModel;
using System.IO;
using AngleSharp.Dom;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace Singularity.Audio
{
    internal class AudioQueue
    {
        public ObservableCollection<AudioItem> Songs { get; } = new();

        public static YoutubeClient Client { get; set; }

        public AudioQueue()
        {
            Client = SingletonFactory.YoutubeClient;
        }

        public void AddSong(AudioItem stream)
        {
            var item = Songs.FirstOrDefault(x=>x.Video.Id==stream.Video.Id);

            if(item != null)
                Songs.Remove(item);

            Songs.Insert(0, stream);
        }

        public static async Task<AudioItem> GetAudioItem(string url)
        {
            var videoInfo = await Client.Videos.GetAsync(url);
            return new AudioItem(Video.From(videoInfo));
        }
        public async Task AddSongAsync(string url)
        {
            var audio = await GetAudioItem(url);
            AddSong(audio);
        }
        public void AddSongEnd(AudioItem stream)
        {
            var item = Songs.FirstOrDefault(x => x.Video.Id == stream.Video.Id);

            if (item != null)
                return;
                Songs.Add(stream);
        }
        public async Task AddSongEndAsync(string url)
        {
            var audio = await GetAudioItem(url);
            AddSongEnd(audio);
        }
    }

    public record AudioItem(Video Video)
    {
        public IStreamInfo? StreamInfo { get; private set; }
        public IStreamInfo? VideoStreamInfo { get;private set; }
        public Task LoadStreamData()
        {
            if (StreamInfo != null)
                return Task.CompletedTask;

            return Task.Run(async() =>
            {
                var streams = await SingletonFactory.YoutubeClient.Videos.Streams.GetManifestAsync(Video.Id);
                StreamInfo = streams.GetAudioStreams().GetWithHighestBitrate();
                //var videStreams = streams.GetVideoOnlyStreams().OrderBy(x=>x.VideoResolution.Area).ToList();
                //int length = videStreams.Count / 2;
                //VideoStreamInfo = videStreams[length];
            });
        }
    }
}
