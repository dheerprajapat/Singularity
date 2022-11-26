using CommunityToolkit.Mvvm.ComponentModel;
using Singularity.Core.Contracts.Services;
using Windows.Media.Core;

namespace Singularity.ViewModels;

public partial class MusicCotrollerViewModel : ObservableRecipient
{
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
    public MediaSource audioStreamUrl;

    async void LoadVideoInfo()
    {
        var stream = await Youtube.GetBestQualityAudio("DqgK4llE1cw");
        AudioStreamUrl = MediaSource.CreateFromUri(new Uri(stream.Url));
    }

}