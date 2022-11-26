using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Singularity.Core.Contracts.Services;
using Windows.Media.Core;
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
    [ObservableProperty]
    public Video? video;

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

    public string? Title => video?.Title;
    public string? Singer => video?.Author.ChannelTitle;
    async void LoadVideoInfo()
    {
        const string id = "DqgK4llE1cw";

        Video = await Youtube.GetVideoInfo(id);
        var stream = await Youtube.GetBestQualityAudio(id);

        if (AudioStream is not null)
            AudioStream.Dispose();

        AudioStream = MediaSource.CreateFromUri(new Uri(stream.Url));
    }

    internal void InitPlayer(MediaPlayerElement videoPlayer) => playerElement = videoPlayer;
}