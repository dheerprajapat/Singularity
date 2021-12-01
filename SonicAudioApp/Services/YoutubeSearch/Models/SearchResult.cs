using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicAudioApp.Services.YoutubeSearch.Models;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
public class Duration
{
    [JsonConstructor]
    public Duration(
        [JsonProperty("seconds")] int seconds,
        [JsonProperty("timestamp")] string timestamp
    )
    {
        this.Seconds = seconds;
        this.Timestamp = timestamp;
    }

    [JsonProperty("seconds")]
    public int Seconds { get; }

    [JsonProperty("timestamp")]
    public string Timestamp { get; }
}

public class Author
{
    [JsonConstructor]
    public Author(
        [JsonProperty("name")] string name,
        [JsonProperty("url")] string url
    )
    {
        this.Name = name;
        this.Url = url;
    }

    [JsonProperty("name")]
    public string Name { get; }

    [JsonProperty("url")]
    public string Url { get; }
}

public class SearchResult
{
    [JsonConstructor]
    public SearchResult(
        [JsonProperty("type")] string type,
        [JsonProperty("videoId")] string videoId,
        [JsonProperty("url")] string url,
        [JsonProperty("title")] string title,
        [JsonProperty("description")] string description,
        [JsonProperty("image")] string image,
        [JsonProperty("thumbnail")] string thumbnail,
        [JsonProperty("seconds")] int seconds,
        [JsonProperty("timestamp")] string timestamp,
        [JsonProperty("duration")] Duration duration,
        [JsonProperty("ago")] string ago,
        [JsonProperty("views")] int views,
        [JsonProperty("author")] Author author
    )
    {
        this.Type = type;
        this.VideoId = videoId;
        this.Url = url;
        this.Title = title;
        this.Description = description;
        this.Image = image;
        this.Thumbnail = thumbnail;
        this.Seconds = seconds;
        this.Timestamp = timestamp;
        this.Duration = duration;
        this.Ago = ago;
        this.Views = views;
        this.Author = author;
    }

    [JsonProperty("type")]
    public string Type { get; }

    [JsonProperty("videoId")]
    public string VideoId { get; }

    [JsonProperty("url")]
    public string Url { get; }

    [JsonProperty("title")]
    public string Title { get; }

    [JsonProperty("description")]
    public string Description { get; }

    [JsonProperty("image")]
    public string Image { get; }

    [JsonProperty("thumbnail")]
    public string Thumbnail { get; }

    [JsonProperty("seconds")]
    public int Seconds { get; }

    [JsonProperty("timestamp")]
    public string Timestamp { get; }

    [JsonProperty("duration")]
    public Duration Duration { get; }

    [JsonProperty("ago")]
    public string Ago { get; }

    [JsonProperty("views")]
    public int Views { get; }

    [JsonProperty("author")]
    public Author Author { get; }
}

