using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace Singularity.Core.Contracts.Services;
public interface IYoutubeService
{

    /// <summary>
    /// Get Youtube audio associated metadata
    /// </summary>
    /// <param name="id">id or full url of video </param>
    /// <returns></returns>
    ValueTask<Video> GetVideoInfo(string id);
    Task<IStreamInfo> GetBestQualityAudio(string id);
    Task<string> GetThumbnailUrl(string id);
    
}
