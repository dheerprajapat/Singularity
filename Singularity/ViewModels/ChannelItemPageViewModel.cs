using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Singularity.Core.Contracts.Services;
using Singularity.Helpers;
using Singularity.Models;
using YoutubeExplode.Channels;
using YoutubeExplode.Playlists;
using YoutubeExplode.Search;

namespace Singularity.ViewModels;
public partial class ChannelItemPageViewModel : ObservableRecipient
{
    [AlsoNotifyChangeFor(nameof(ChannelName))]
    [AlsoNotifyChangeFor(nameof(Thumbnail))]
    [ObservableProperty]
    private Channel channel;

    [ObservableProperty]
    public IAsyncEnumerable<ISearchResult>? playlistVideos;
    public string? ChannelName => Channel?.Title;
    public ImageSource? Thumbnail
    {
        get
        {
            if (Channel is null) return null;
            return new BitmapImage()
            {
                UriSource = new(Channel.Thumbnails.GetBestThumbnail())
            };
        }
    }
    public string? ChannelId
    {
        get; set;
    }
    public IYoutubeService Youtube
    {
        get;
    }

    public ChannelItemPageViewModel(IYoutubeService youtube)
    {
        Youtube = youtube;
    }
    public async void LoadChannelInfo()
    {
        Channel =await Youtube.GetChannelMetadata(ChannelId);
        PlaylistVideos =  LoadUploads();
    }
    private async IAsyncEnumerable<PlaylistVideoSearchResult> LoadUploads()
    {
        await foreach (var v in Youtube.GetChannelUploads(ChannelId))
        {
            yield return new PlaylistVideoSearchResult(v);
        }
    }
}
