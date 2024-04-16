using System.Collections.ObjectModel;
using System.IO;
using AngleSharp.Dom;
using YoutubeExplode;
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
            var index = Songs.IndexOf(stream);

            if(index >= 0)
                Songs.RemoveAt(index);

            Songs.Insert(0, stream);
        }

        public static async Task<AudioItem> GetAudioItem(string url)
        {
            var videoInfo = await Client.Videos.GetAsync(url);
            return new AudioItem(videoInfo);
        }
        public async Task AddSongAsync(string url)
        {
            var audio = await GetAudioItem(url);
            AddSong(audio);
        }
        public void AddSongEnd(AudioItem stream)
        {
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
        public async Task LoadStreamData()
        {
            if (StreamInfo != null)
                return;

            var streams = await SingletonFactory.YoutubeClient.Videos.Streams.GetManifestAsync(Video.Id);
            StreamInfo = streams.GetAudioStreams().GetWithHighestBitrate();
        }
    }
}
