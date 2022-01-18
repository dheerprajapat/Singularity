using Newtonsoft.Json;
using SonicAudioApp.Components;
using SonicAudioApp.Models;
using SonicAudioApp.Services.YoutubeSearch;
using SonicAudioApp.Services.YoutubeSearch.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SonicAudioApp.Services.Ytdl;
public static class YoutubeSearch
{
    public static async Task<IReadOnlyList<AudioQueueItem>> SearchFor(string query,uint amount=10,CancellationToken token = default)
    {
        var lst=new List<AudioQueueItem>();

        await foreach (var result in YoutubeManager.Youtube.Search.GetVideosAsync(query))
        {
            try
            {
                 var item=new AudioQueueItem
                {
                    Title = result.Title,
                    Singers = result.Author.Title,
                    Id = result.Id,
                    VideoUrl = result.Url,
                    DurationString = MediaPlayerControl.ConvertTimeSpanToDuration(result.Duration.GetValueOrDefault()),
                    ThumbnailUrl = result.Thumbnails.OrderByDescending(x => x.Resolution.Area).First().Url,
                    Liked = LikedSongManager.LikedSongs.Count(x => x.Id == result.Id) > 0
                };
                lst.Add(item);
            }
            catch { amount++; }
            amount--;

            if (amount == 0)
                break;
        }

       return lst;
    }
}