using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Singularity.Core.Contracts.Services;
using YoutubeExplode;
using YoutubeExplode.Playlists;
using YoutubeExplode.Search;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;
using static System.Net.WebRequestMethods;

namespace Singularity.Core.Services;
public class YoutubeExplodeService : IYoutubeService
{
    public static readonly HttpClient Http = new();

    private readonly YoutubeClient _youtubeClient = new();
    private readonly Dictionary<string, Video> videoCache = new();
    public Dictionary<string, Video> GetVideoCache() => videoCache;

    private const string search_url = "https://clients1.google.com/complete/search?client=youtube&gs_ri=youtube&ds=yt&q=";

    public ValueTask<Video> GetVideoFromCache(string id)
    {
        if (videoCache.ContainsKey(id))
            return ValueTask.FromResult(videoCache[id]);
        return GetVideoInfo(id);
    }
    public async ValueTask<Video> GetVideoInfo(string id)
    {
        var vid = await _youtubeClient.Videos.GetAsync(id);
        videoCache.TryAdd(id, vid);
        return vid;
    }

    public async ValueTask<IStreamInfo> GetBestQualityAudio(string id)
    {
        var mainfest = await _youtubeClient.Videos.Streams.GetManifestAsync(id);
        return mainfest.GetAudioOnlyStreams().GetWithHighestBitrate();

    }

    public async ValueTask<string> GetThumbnailUrl(string id)
    {
        var video = await GetVideoFromCache(id);
        return video.Thumbnails.OrderByDescending(x => x.Resolution.Area).First().Url;
    }
    public async ValueTask<ISearchResult?> GetTopSeachQuery(string query,CancellationToken token=default)
    {
        var res=_youtubeClient.Search.GetResultsAsync(query, token);
        await foreach (var r in res)
        {
            return r;// skip rest
        }
        return null;
    }
    public  IAsyncEnumerable<ISearchResult> GetSearchResult(string query, SearchType type,
        CancellationToken token = default)
    {
        if(type is SearchType.Video)
            return _youtubeClient.Search.GetVideosAsync(query, token);
        else if (type is SearchType.Playlist)
            return _youtubeClient.Search.GetPlaylistsAsync(query, token);
        else
            return _youtubeClient.Search.GetChannelsAsync(query, token);
    }
    
    public async ValueTask<List<string>> SuggestionsAsync(string query, CancellationToken token = default)
    {
        query = Uri.EscapeDataString(query);
        query = search_url + query;
        var res = await Http.GetAsync(query,token);
        var js = await res.Content.ReadAsStringAsync(token);

        var parts = js.Split('[').Where(t => t.Split('"').Length > 2).Select(t => t.Split('"')[1]);

        return parts.ToList();
    }
}
