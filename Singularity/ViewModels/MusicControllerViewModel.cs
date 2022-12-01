using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Singularity.Core.Contracts.Services;
using Singularity.Helpers;
using Singularity.Models;
using Windows.ApplicationModel.Core;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Core;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace Singularity.ViewModels;

public partial class MusicCotrollerViewModel : ObservableRecipient
{
    public MediaPlayerElement? playerElement;
    public MusicCotrollerViewModel(IYoutubeService youtube)
    {
        Youtube = youtube;
        NextSongCommand = new RelayCommand(PlayNext);
        AudioQueue.OnCurrentPlaybackItemChanged += AudioQueue_OnCurrentPlaybackItemChanged;
        AudioQueue.InitAudioQueue(Youtube);

        LoadVideoInfo();
    }


    public IYoutubeService Youtube
    {
        get;
    }

    [AlsoNotifyChangeFor(nameof(Title))]
    [AlsoNotifyChangeFor(nameof(Singer))]
    [AlsoNotifyChangeFor(nameof(Thumbnail))]
    [AlsoNotifyChangeFor(nameof(MaxDurationString))]
    [AlsoNotifyChangeFor(nameof(MaxDuration))]

    [ObservableProperty]
    public Video? video;

    public string? Title => video?.Title;
    public string? Singer => video?.Author.ChannelTitle;
    
    public string MaxDurationString => video is not null ?
        MediaPlayerHelper.ConvertTimeSpanToDuration(video.Duration.GetValueOrDefault()) : "0:00:00";
    public int MaxDuration => video is not null ? (int)video.Duration.GetValueOrDefault().TotalSeconds : 0;

    [ObservableProperty]
    public string positionString = "0:00";

    [ObservableProperty]
    public int position=0;

    public ICommand NextSongCommand; 

    readonly DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
    
    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(VolumeIcon))]
    public double volume = 0.5;

    public string VolumeIcon=> volume switch
    {
        > .75 => "\ue995",
        <= .75 and > .25 => "\ue994",
        <= .25 and > .10 => "\ue993",
        _ => "\ue992",
    };

    public ImageSource? Thumbnail
    {
        get
        {
            if (video is null) return null;
            return new BitmapImage()
            {
                UriSource = new(video.Thumbnails.OrderByDescending(x => x.Resolution.Area).First().Url)
            };
        }
    }
    async void LoadVideoInfo()
    {
        const string id = "DqgK4llE1cw";

        Video = await Youtube.GetVideoInfo(id);

        await AudioQueue.AddSong(video!);
    }

    internal void InitPlayer(MediaPlayerElement videoPlayer)
    {
        playerElement = videoPlayer;
        if (videoPlayer.MediaPlayer is not null)
        {
            videoPlayer.MediaPlayer.PlaybackSession.PositionChanged += PlaybackSession_PositionChanged;
            playerElement!.MediaPlayer!.Source = AudioQueue.currentList;
        }
    }

    async void PlayNext()
    {
        await AudioQueue.AddSong("h7MYJghRWt0");
        AudioQueue.PlayNext();
        Position = 0;
        //Video=await AudioQueue.GetCurrentVideo();
    }
    private async void AudioQueue_OnCurrentPlaybackItemChanged(MediaPlaybackList sender,
        CurrentMediaPlaybackItemChangedEventArgs args)
    {
        await ExecuteOnUIThread(async() =>
        {
            Video = await AudioQueue.GetVideoFromPlaybackItem(args.NewItem);
        });
    }


    private async void PlaybackSession_PositionChanged(Windows.Media.Playback.MediaPlaybackSession sender, object args)
    {
        if (sender.PlaybackState != MediaPlaybackState.Playing)
            return;
        await ExecuteOnUIThread(() =>
        {
            PositionString = MediaPlayerHelper.ConvertTimeSpanToDuration(sender.Position);
            Position = (int)sender.Position.TotalSeconds;
        });

    }
    public void PositionChanged(int value)
    {
        if (playerElement is null || video is null || value >= MaxDuration)
            return;

        playerElement.MediaPlayer!.PlaybackSession.Position = TimeSpan.FromSeconds(value);
    }
    public void SetVolume(int val)
    {
        Volume = val / 100.0;

        if (playerElement is not null && playerElement.MediaPlayer is not null)
        {
            playerElement.MediaPlayer.Volume = volume;
        }
    }
    private async ValueTask ExecuteOnUIThread(Action action)
    {
        if (dispatcherQueue is null)
            return;

        await Task.Run(() =>
        {
            dispatcherQueue.TryEnqueue(() =>
            {
                action.Invoke();
            });
        });
    }
}