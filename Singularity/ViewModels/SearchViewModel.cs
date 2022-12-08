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
    public ObservableCollection<ISearchResult>? videos;
    [ObservableProperty]
    public ObservableCollection<ISearchResult>? playlists;
    [ObservableProperty]
    public ObservableCollection<ISearchResult>? artists;

    public const int MaxItemsToDisplay = 3;
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

        Videos = await Search<VideoSearchResult>(query, SearchType.Video);
        Playlists = await Search<PlaylistSearchResult>(query, SearchType.Playlist);
        Artists = await Search<ChannelSearchResult>(query, SearchType.Artist);
    }

    async ValueTask<ObservableCollection<ISearchResult>> Search<T>(string query,SearchType searchType, CancellationToken token = default)
        where T : ISearchResult
    {
        var i = 0;
        var r = new List<ISearchResult>();
        await foreach (T v in Youtube.GetSearchResult(query, searchType, token))
        {
            r.Add(v);
            i++;
            if (i >= MaxItemsToDisplay)
                break;
        }
        return new(r);
    }

}
