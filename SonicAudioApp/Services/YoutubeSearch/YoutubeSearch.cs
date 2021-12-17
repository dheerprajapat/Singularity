using Newtonsoft.Json;
using SonicAudioApp.Services.YoutubeSearch;
using SonicAudioApp.Services.YoutubeSearch.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YoutubeExplode.Search;

namespace SonicAudioApp.Services.Ytdl;
public static class YoutubeSearch
{

    public static async Task<IReadOnlyList<VideoSearchResult>> GetVideosAsync(string query, uint amount = 10, CancellationToken token = default)
    {
        
        try
        {
            var r=YoutubeManager.Youtube.Search.GetVideosAsync(query);
            int c = (int)amount;
            List<VideoSearchResult> results = new List<VideoSearchResult>();
            await foreach (var item in r)
            {
                if (c == 0)
                    break;
                results.Add(item);
                c--;
            }
            return results;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}