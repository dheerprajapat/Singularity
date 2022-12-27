using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
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
    [ObservableProperty]
    public IAsyncEnumerable<ISearchResult>? topResultItem;

    [ObservableProperty]
    public static ObservableCollection<string>? trendingVideoIds;

    [ObservableProperty]
    public ObservableCollection<string>? suggestions;

    public static string? CurrentQuery;

    [ObservableProperty]
    public Visibility showTrending = Visibility.Visible;
    [ObservableProperty]
    public Visibility showSearch = Visibility.Collapsed;

    private CancellationTokenSource sourceToken = new();

    private bool isFetchingResult = false;

    public SearchViewModel(IYoutubeService youtube)
    {
        Youtube = youtube;
        if (CurrentQuery != null)
        {
            SearchQuery();
        }
        else
            LoadTrendngVideos();
    }

    public IYoutubeService Youtube
    {
        get;
    }


    IAsyncEnumerable<ISearchResult> Search<T>(string query,SearchType searchType, CancellationToken token = default)
        where T : ISearchResult
    {
       return Youtube.GetSearchResult(query, searchType, token);
    }

    void SearchQuery()
    {
        TopResultItem = ProcessTopResult();
        Videos = Search<VideoSearchResult>(CurrentQuery, SearchType.Video, sourceToken.Token);
        Playlists = Search<PlaylistSearchResult>(CurrentQuery, SearchType.Playlist, sourceToken.Token);
        Artists = Search<ChannelSearchResult>(CurrentQuery, SearchType.Artist, sourceToken.Token);
    }
    async IAsyncEnumerable<ISearchResult>? ProcessTopResult()
    {
         yield return await Youtube.GetTopSeachQuery(CurrentQuery, sourceToken.Token);
    }
    async void LoadTrendngVideos()
    {
        var ct = 0;
        var vids=new ObservableCollection<string>();
        await foreach (var id in Youtube.GetTrendingMusicVideos(sourceToken.Token))
        {
            if (ct >= 10)
                break;

            vids.Add(id);

            ct++;
        }
        TrendingVideoIds = vids;
    }

    internal async Task FetchSearchResults(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            ShowTrending = Visibility.Visible;
            ShowSearch = Visibility.Collapsed;
            return;
        }
            ShowTrending = Visibility.Collapsed;
            ShowSearch = Visibility.Visible;

        if (CurrentQuery == text)
            return;

        CurrentQuery = text;
        if (isFetchingResult)
        {
            sourceToken.Cancel();
            sourceToken= new CancellationTokenSource();
        }
        isFetchingResult = true;
        var r=await Youtube.SuggestionsAsync(CurrentQuery,sourceToken.Token);
        Suggestions = new(r);
        SearchQuery();
        isFetchingResult = false;
    }
}
