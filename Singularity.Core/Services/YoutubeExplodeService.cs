using System;
using System.Collections.Generic;
using System.Linq;
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

    public ValueTask<Video> GetVideoInfo(string id)
    {
        return _youtubeClient.Videos.GetAsync(id);
    }
    public async Task<IStreamInfo> GetBestQualityAudio(string id)
    {
        var mainfest=await _youtubeClient.Videos.Streams.GetManifestAsync(id);
        return mainfest.GetAudioOnlyStreams().GetWithHighestBitrate();

    }
}
