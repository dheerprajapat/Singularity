using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Singularity.Core.Contracts.Services;
using Singularity.Helpers;
using Windows.ApplicationModel.Core;
using Windows.Media.Core;
using Windows.UI.Core;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace Singularity.ViewModels;

public partial class MusicCotrollerViewModel : ObservableRecipient
{
    public MediaPlayerElement? playerElement;
    public MusicCotrollerViewModel(IYoutubeService youtube)
    {
        Youtube = youtube;
        LoadVideoInfo();
    }

    public IYoutubeService Youtube
    {
        get;
    }

    [ObservableProperty]
    private MediaSource? audioStream;

    [ObservableProperty]
    public double volume = 0.5;

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

    readonly DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
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
        var stream = await Youtube.GetBestQualityAudio(id);

        if (AudioStream is not null)
            AudioStream.Dispose();

        AudioStream = MediaSource.CreateFromUri(new Uri(stream.Url));
    }

    internal void InitPlayer(MediaPlayerElement videoPlayer)
    {
        playerElement = videoPlayer;
        if(videoPlayer.MediaPlayer is not null)
            videoPlayer.MediaPlayer.PlaybackSession.PositionChanged += PlaybackSession_PositionChanged;
    }

    private async void PlaybackSession_PositionChanged(Windows.Media.Playback.MediaPlaybackSession sender, object args)
    {

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