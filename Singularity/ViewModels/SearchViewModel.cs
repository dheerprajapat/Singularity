using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Singularity.Core.Contracts.Services;
using YoutubeExplode.Search;
using YoutubeExplode.Videos;

namespace Singularity.ViewModels;
public partial class SearchViewModel: ObservableRecipient
{
    [ObservableProperty]
    public IAsyncEnumerable<ISearchResult>? videos;
    [ObservableProperty]
    public IAsyncEnumerable<ISearchResult>? playlists;
    [ObservableProperty]
    public IAsyncEnumerable<ISearchResult>? artists;

    public SearchViewModel(IYoutubeService youtube)
    {
        Youtube = youtube;
        string query = "on my way song";
        SearchQuery(query);

    }

    public IYoutubeService Youtube
    {
        get;
    }
    async void SearchQuery(string query)
    {

        Videos =  Search<VideoSearchResult>(query, SearchType.Video);
        Playlists =  Search<PlaylistSearchResult>(query, SearchType.Playlist);
        Artists =  Search<ChannelSearchResult>(query, SearchType.Artist);
    }

    IAsyncEnumerable<ISearchResult> Search<T>(string query,SearchType searchType, CancellationToken token = default)
        where T : ISearchResult
    {
       return Youtube.GetSearchResult(query, searchType, token);
    }

}
