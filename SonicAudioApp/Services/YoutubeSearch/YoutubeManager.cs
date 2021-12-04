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
        public static YoutubeClient Youtube = new YoutubeClient();
        public static async Task UpdateUrlAsync(AudioQueueItem c)
        {
            var streamManifest = await Youtube.Videos.Streams.GetManifestAsync(c.Id);
            //get highest audio
            var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
            c.Url = streamInfo.Url;
        }
    }
}
