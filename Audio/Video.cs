using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using YoutubeExplode.Common;

namespace Singularity.Audio;

public class Video : IEquatable<Video>
{
    [JsonConstructor]
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
        if (other == null) return false;
        return Id == other.Id;
    }
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}