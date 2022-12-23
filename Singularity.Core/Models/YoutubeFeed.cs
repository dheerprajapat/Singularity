using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YoutubeExplode.Playlists;

namespace Singularity.Core.Models;

public class Genre
{
    public Genre(
        string name,
        List<string> playlistIds
    )
    {
        this.Name = name;
        this.PlaylistIds = playlistIds;
    }

    [JsonProperty("name")]
    [JsonPropertyName("name")]
    public string Name
    {
        get;
    }

    [JsonProperty("playlistIds")]
    [JsonPropertyName("playlistIds")]
    public IReadOnlyList<string> PlaylistIds
    {
        get;
    }
}

public class YoutubeFeed
{
    public YoutubeFeed(
        List<Genre> genres,
        List<string> trendingVideosIds
    )
    {
        this.Genres = genres;
        this.TrendingVideosIds = trendingVideosIds;
    }

    [JsonProperty("genres")]
    [JsonPropertyName("genres")]
    public IReadOnlyList<Genre> Genres
    {
        get;
    }

    [JsonProperty("trendingVideosIds")]
    [JsonPropertyName("trendingVideosIds")]
    public IReadOnlyList<string> TrendingVideosIds
    {
        get;
    }
}

