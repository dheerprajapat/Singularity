using SonicAudioApp.Components;
using SonicAudioApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace SonicAudioApp.Services.YoutubeSearch
{
    public static class YoutubeManager
    {
        public static YoutubeClient Youtube { get; } = new YoutubeClient();
        public static async Task UpdateUrlAsync(AudioQueueItem c)
        {
            c.LastUpdateTimeStamp = DateTime.Now.Ticks;
            var streamManifest = await Youtube.Videos.Streams.GetManifestAsync(c.Id);
            //get highest audio
            var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
            c.Url = streamInfo.Url;
        }
        public static async Task<AudioQueueItem> GetVideoInfo(string id)
        {
            var video = await Youtube.Videos.GetAsync(id);
            return new AudioQueueItem
            {
                Title = video.Title,
                Singers = video.Author.Title,
                Id = video.Id,
                VideoUrl = video.Url,
                DurationString = MediaPlayerControl.ConvertTimeSpanToDuration(video.Duration.GetValueOrDefault()),
                ThumbnailUrl = video.Thumbnails.OrderByDescending(x => x.Resolution.Area).First().Url,
                Liked = LikedSongManager.LikedSongs.Count(x=>x.Id==video.Id) > 0
            };
        }
    }
}
