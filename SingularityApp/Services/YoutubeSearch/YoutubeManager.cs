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
        public static YoutubeClient Youtube => new YoutubeClient();
        public static async Task UpdateUrlAsync(AudioQueueItem c)
        {
            c.LastUpdateTimeStamp = DateTime.Now.Ticks;
            var streamManifest = await Youtube.Videos.Streams.GetManifestAsync(c.Id);
            //get highest audio
            var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
            c.Url = streamInfo.Url;
        }
        public static async Task<AudioQueueItem> GetInfo(string id)
        {
            var vid=await Youtube.Videos.GetAsync(id);
            return new AudioQueueItem
            {
                Url = vid.Url,
                Liked = LikedSongManager.LikedSongs.Count(x => x.Id == id) > 0,
                WaveformVisibilty = Windows.UI.Xaml.Visibility.Collapsed,
                Id = id,
                Singers = vid.Author.Title,
                Title = vid.Title,
                ThumbnailUrl = vid.Thumbnails.OrderByDescending(x => x.Resolution.Area).First().Url,
                DurationString = MediaPlayerControl.ConvertTimeSpanToDuration(vid.Duration.GetValueOrDefault())
            };
        }

    }
}
