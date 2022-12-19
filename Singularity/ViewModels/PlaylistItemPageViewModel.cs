using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Singularity.Contracts.Services;
using Singularity.Core.Contracts.Services;
using Singularity.Models;
using Singularity.Services;
using YoutubeExplode.Search;

namespace Singularity.ViewModels;
public partial class PlaylistItemPageViewModel:ObservableRecipient
{
    public string? PlaylistId;
    [ObservableProperty]
    public IAsyncEnumerable<ISearchResult>? videosInPlaylist;
    public string? PlaylistID;
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
        VideosInPlaylist = ProcessPlaylist();
    }

}
