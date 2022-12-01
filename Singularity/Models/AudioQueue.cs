using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Singularity.Core.Contracts.Services;
using Windows.Devices.Spi;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage.Streams;
using YoutubeExplode.Videos;

namespace Singularity.Models;
internal static class AudioQueue
{
    internal static readonly MediaPlaybackList currentList=new();
    private static readonly HashSet<string> currentVideoIds=new();
#nullable disable
    internal static  IYoutubeService Youtube { get; private set; }
#nullable restore
    public static void InitAudioQueue(IYoutubeService youtube)
    {
        Youtube = youtube;
    }
    public static async ValueTask AddSong(string id)
    {
        var vid = await Youtube.GetVideoInfo(id);
        await AddSong(vid);
    }
    public static async ValueTask AddSong(Video video)
    {
        if (currentVideoIds.Contains(video.Id))
        {
            return;
        }
        var stream = await Youtube.GetBestQualityAudio(video.Id);
        var source= MediaSource.CreateFromUri(
            new Uri(stream.Url));
        var playbackItem = new MediaPlaybackItem(source);
        var props = playbackItem.GetDisplayProperties();
        props.Type = Windows.Media.MediaPlaybackType.Music;
        props.Thumbnail = RandomAccessStreamReference
            .CreateFromUri(new Uri(await Youtube.GetThumbnailUrl(video.Id)));
        props.MusicProperties.Title = video.Title;
        props.MusicProperties.Artist = video.Author.ChannelTitle;
        playbackItem.ApplyDisplayProperties(props);
        currentList.Items.Add(playbackItem);

        currentVideoIds.Add(video.Id);
    }
    public static void PlayNext()
    {
        currentList.MoveNext();
    }
}
