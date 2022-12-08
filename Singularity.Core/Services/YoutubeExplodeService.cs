using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Singularity.Core.Contracts.Services;
using YoutubeExplode;
using YoutubeExplode.Playlists;
using YoutubeExplode.Search;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace Singularity.Core.Services;
public class YoutubeExplodeService : IYoutubeService
{
    private readonly YoutubeClient _youtubeClient = new();
    private readonly Dictionary<string, Video> videoCache = new();
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
    public Dictionary<string, Video> GetVideoCache() => videoCache;

    
}
