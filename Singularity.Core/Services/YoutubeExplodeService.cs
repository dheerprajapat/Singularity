using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Singularity.Core.Contracts.Services;
using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace Singularity.Core.Services;
public class YoutubeExplodeService:IYoutubeService
{
    private readonly YoutubeClient _youtubeClient=new();
    private readonly Dictionary<string, Video> videoCache = new();
    public ValueTask<Video> GetVideoFromCache(string id)
    {
        if (videoCache.ContainsKey(id))
            return ValueTask.FromResult(videoCache[id]);
        return GetVideoInfo(id);
    }
    public async ValueTask<Video> GetVideoInfo(string id)
    {
        var vid= await _youtubeClient.Videos.GetAsync(id);
        videoCache.TryAdd(id, vid);
        return vid;
    }
    
    public async ValueTask<IStreamInfo> GetBestQualityAudio(string id)
    {
        var mainfest=await _youtubeClient.Videos.Streams.GetManifestAsync(id);
        return mainfest.GetAudioOnlyStreams().GetWithHighestBitrate();

    }

    public async ValueTask<string> GetThumbnailUrl(string id)
    {
        var video = await GetVideoFromCache(id);
        return video.Thumbnails.OrderByDescending(x => x.Resolution.Area).First().Url;
    }

    public Dictionary<string, Video> GetVideoCache() => videoCache;
}
