using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Media;
using Singularity.Contracts.Services;
using Singularity.Core.Contracts.Services;
using Singularity.Models;
using Singularity.Services;
using YoutubeExplode.Playlists;
using YoutubeExplode.Search;
using YoutubeExplode.Videos;
using Singularity.Helpers;

namespace Singularity.ViewModels;
public partial class PlaylistItemPageViewModel:ObservableRecipient
{
    public string? PlaylistId;
    [ObservableProperty]
    public IAsyncEnumerable<ISearchResult>? videosInPlaylist;

    [AlsoNotifyChangeFor(nameof(Title))]
    [AlsoNotifyChangeFor(nameof(Author))]
    [AlsoNotifyChangeFor(nameof(Thumbnail))]

    [ObservableProperty]
    public Playlist playlist;

    public string? Title =>Playlist?.Title;
    public string? Author => Playlist?.Author?.ChannelTitle;
    public ImageSource? Thumbnail
    {
        get
        {
            if (Playlist is null) return null;
            return new BitmapImage()
            {
                UriSource = new(Playlist.Thumbnails.GetBestThumbnail())
            };
        }
    }

    public IYoutubeService YoutubeService
    {
        get;
    }

    public PlaylistItemPageViewModel(IYoutubeService youtubeService)
    {
        YoutubeService = youtubeService;
    }


    async IAsyncEnumerable<PlaylistVideoSearchResult> ProcessPlaylist()
    {
        await foreach (var playlistVideo in YoutubeService.GetPlaylistVideos(PlaylistId))
        {
            yield return new PlaylistVideoSearchResult(playlistVideo); 
        }
    }

    public void LoadPlaylist()
    {
        LoadMetaData();
        VideosInPlaylist = ProcessPlaylist();
    }
    private async void LoadMetaData()
    {
        Playlist = await YoutubeService.GetPlaylistMetadata(PlaylistId);
    }

}
