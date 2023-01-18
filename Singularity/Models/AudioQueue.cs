using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Singularity.Contracts.Services;
using Singularity.Core.Contracts.Services;
using Singularity.Helpers;
using Singularity.ViewModels;
using Singularity.Views;
using Windows.Devices.Spi;
using Windows.Foundation;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage.Streams;
using YoutubeExplode.Exceptions;
using YoutubeExplode.Videos;

namespace Singularity.Models;
internal static class AudioQueue
{
    internal static readonly MediaPlaybackList currentList = new();
    internal static readonly ObservableCollection<string> currentVideoIds = new();
    private static readonly Dictionary<string, string> IdFromTitleChannelNameMap = new();
    
    public static string? CurrentPlayingItemId
    {
        get; private set;
    }
#nullable disable
    internal static IYoutubeService Youtube
    {
        get; private set;
    }
#nullable restore
    static AudioQueue()
    {
        currentList.CurrentItemChanged += CurrentPlaybackItemChanged;
    }

    private static async void CurrentPlaybackItemChanged(MediaPlaybackList sender,
        CurrentMediaPlaybackItemChangedEventArgs args)
    {
        if (args.NewItem == null)
            CurrentPlayingItemId = null;
        else
            CurrentPlayingItemId = args.NewItem.GetDisplayProperties().MusicProperties.Genres[0];

        if (CurrentPlayingItemId != null)
        {
            var index = currentVideoIds.IndexOf(CurrentPlayingItemId);
            currentVideoIds.RemoveAt(index);
            currentVideoIds.Insert(0,CurrentPlayingItemId);

            //set discord presence
            App.GetService<DiscordPresenceService>()
                .SetSongInfo(await Youtube.GetVideoFromCache(CurrentPlayingItemId));
        }

        OnCurrentPlaybackItemChanged?.Invoke(sender, args);
    }



    public delegate void CurrentItemChangedHandler(MediaPlaybackList sender,
        CurrentMediaPlaybackItemChangedEventArgs args);

    public static event CurrentItemChangedHandler? OnCurrentPlaybackItemChanged;

    public static void InitAudioQueue(IYoutubeService youtube)
    {
        Youtube = youtube;
    }
    public static async ValueTask AddSong(string id, bool playNow = false)
    {
        var vid = await Youtube.GetVideoInfo(id);
        await AddSong(vid, playNow);
    }
    public static async ValueTask AddSong(Video video, bool playNow = false)
    {
        if (currentVideoIds.Contains(video.Id))
        {
            if (playNow)
                MoveToSong(video.Id);
            return;
        }
        MediaSource source;
        try
        {
            var stream = await Youtube.GetBestQualityAudio(video.Id);
            source = MediaSource.CreateFromUri(
                new Uri(stream.Url));
        }
        catch (VideoUnplayableException)
        {
            source = MediaSource.CreateFromUri(new Uri(await Youtube.GetLiveStreamUrl(video.Id)));
        }
        var playbackItem = new MediaPlaybackItem(source);
        var props = playbackItem.GetDisplayProperties();
        props.Type = Windows.Media.MediaPlaybackType.Music;
        props.Thumbnail = RandomAccessStreamReference
            .CreateFromUri(new Uri(video.Thumbnails.GetBestThumbnail()));
        props.MusicProperties.Title = video.Title;
        props.MusicProperties.Artist = video.Author.ChannelTitle;
        props.MusicProperties.Genres.Add(video.Id); ///storing id n genres
        playbackItem.ApplyDisplayProperties(props);


        currentList.Items.Add(playbackItem);
        IdFromTitleChannelNameMap.TryAdd(GetIdTitleKey(props), video.Id);
        currentVideoIds.Add(video.Id);

        if (playNow)
            MoveToSong(playbackItem);
    }
    public static void MoveToSong(MediaPlaybackItem item)
    {
        var id= item.GetDisplayProperties().MusicProperties.Genres[0]; ;
        //save last played song
        App.GetService<IUserSettingsService>().CurrentSetting
            .Media.LastPlayedId=id;

        var index = currentVideoIds.IndexOf(id);

        if(index>0)
            currentVideoIds.Move(0, index);

        OnCurrentVideoIdChanged?.Invoke(id);

        currentList.MoveTo((uint)currentList.Items.IndexOf(item));
        MusicControllerView.ExViewModel.Position = 0;
        MusicControllerView.ExViewModel.playerElement!.MediaPlayer!.Play();
    }
    public static void MoveToSong(string id)
    {
        var item = currentList.Items
            .FirstOrDefault(x => x.GetDisplayProperties()
            .MusicProperties.Genres[0] == id);
        if (item != null)
            MoveToSong(item);
    }
    private static string GetIdTitleKey(MediaItemDisplayProperties props)
    {
        return props.MusicProperties.Title + ":(:):" + props.MusicProperties.Artist;
    }
    public static async ValueTask<Video?> GetVideoFromPlaybackItem(MediaPlaybackItem media)
    {
        if (media is null)
            return null;
        var key = GetIdTitleKey(media.GetDisplayProperties());
        if (!IdFromTitleChannelNameMap.ContainsKey(key))
            return null;
        var id = IdFromTitleChannelNameMap[key];
        return await Youtube.GetVideoFromCache(id);
    }
    public static void PlayNext()
    {
        currentList.MoveNext();
    }
    public static void PlayPrevious()
    {
        currentList.MovePrevious();
    }
    public static bool ToggleShuffle()
    {
        return currentList.ShuffleEnabled = !currentList.ShuffleEnabled;
    }

    internal static void ToggleRepeat()
    {
        currentList.AutoRepeatEnabled = !currentList.AutoRepeatEnabled;
    }
    public static bool AutoRepeatEnabled => currentList == null ? false: currentList.AutoRepeatEnabled;

    public delegate void OnCurrentItemChangedHandler(string? id);
    public static event OnCurrentItemChangedHandler? OnCurrentVideoIdChanged;
}
