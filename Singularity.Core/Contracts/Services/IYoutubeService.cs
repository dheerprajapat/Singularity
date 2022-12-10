using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode.Search;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;
using static Singularity.Core.Services.YoutubeExplodeService;

namespace Singularity.Core.Contracts.Services;
public interface IYoutubeService
{

    /// <summary>
    /// Get Youtube audio associated metadata
    /// </summary>
    /// <param name="id">id or full url of video </param>
    /// <returns></returns>
    ValueTask<Video> GetVideoInfo(string id);
    ValueTask<IStreamInfo> GetBestQualityAudio(string id);
    ValueTask<string> GetThumbnailUrl(string id);
    ValueTask<Video> GetVideoFromCache(string id);
    IAsyncEnumerable<ISearchResult> GetSearchResult(string query, SearchType type,
        CancellationToken token = default);
    ValueTask<List<string>> SuggestionsAsync(string query, CancellationToken token = default);
    ValueTask<ISearchResult?> GetTopSeachQuery(string query, CancellationToken token = default);

}
public enum SearchType
{
    Video,
    Playlist,
    Artist
}
