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
        public async Task LoadStreamData()
        {
            if (StreamInfo != null)
                return;

            await Task.Run(async() =>
            {
                var streams = await SingletonFactory.YoutubeClient.Videos.Streams.GetManifestAsync(Video.Id);
                StreamInfo = streams.GetAudioStreams().GetWithHighestBitrate();
            });
        }
    }

    public class Video :IEquatable<Video>
    {
        public Video(string title, Author author, TimeSpan? duration, string id, IReadOnlyList<Thumbnail> thumbnails, string url)
        {
            Title = title;
            Author = author;
            Duration = duration;
            Id = id;
            Thumbnails = thumbnails;
            Url = url;
        }

        public string Title { get; }
        public Author Author { get; }
        public TimeSpan? Duration { get; }
        public string Id { get; }
        public IReadOnlyList<Thumbnail> Thumbnails { get; }
        public string Url { get; }

        public static Video From(YoutubeExplode.Videos.Video video)
        {
            return new(video.Title, video.Author, video.Duration, video.Id, video.Thumbnails, video.Url);
        }
        public static Video From(YoutubeExplode.Search.VideoSearchResult video)
        {
            return new(video.Title, video.Author, video.Duration, video.Id, video.Thumbnails, video.Url);
        }

        public bool Equals(Video? other)
        {
            if(other==null) return false;
            return Id== other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
