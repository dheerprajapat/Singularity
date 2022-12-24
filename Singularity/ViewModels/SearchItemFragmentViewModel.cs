
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Singularity.Core.Contracts.Services;
using Singularity.Helpers;
using Singularity.Models;
using YoutubeExplode.Search;

namespace Singularity.ViewModels;
public partial class SearchItemFragmentViewModel : ObservableRecipient
{
    [AlsoNotifyChangeFor(nameof(HeaderVisibility))]
    [ObservableProperty]
    public string? header;

    public Visibility HeaderVisibility => header != null ? Visibility.Visible : Visibility.Collapsed;

    [ObservableProperty]
    public IAsyncEnumerable<ISearchResult>? items;

    public int MaxItemsToDisplay = 3;
    private int originalAmount = -1;


    [AlsoNotifyChangeFor(nameof(ListItemVisible))]
    [AlsoNotifyChangeFor(nameof(LoaderVisible))]
    [ObservableProperty]
    bool isInitalLoaded = false;

    public Visibility ListItemVisible => IsInitalLoaded ? Visibility.Visible : Visibility.Collapsed;
    public Visibility LoaderVisible => !IsInitalLoaded ? Visibility.Visible : Visibility.Collapsed;


    [ObservableProperty]
    public ObservableCollection<SearchFragmentItem>? searchItems;

    public bool ShowAllItems = false;

    [ObservableProperty]
    public Visibility moreItemVisibiity = Visibility.Collapsed;

    public ICommand ExpandMoreCommand;

    public SearchItemFragmentViewModel(IYoutubeService youtube)
    {
        Youtube = youtube;
        ExpandMoreCommand = new RelayCommand(ExpandMore);
    }

    private async ValueTask CheckHasMoreItemsAsync()
    {
        if (ShowAllItems && MaxItemsToDisplay < 100)
        {
            MaxItemsToDisplay = 100;
        }
        var cloneIterator = items;
        var ct = 0;
        var hasMore = false;
        if (MaxItemsToDisplay != 0 && cloneIterator != null)
            await foreach (var item in cloneIterator)
            {
                ct++;
                if (ct <= MaxItemsToDisplay)
                    continue;
                hasMore = true;
                break;
            }
        if (hasMore)
        {
            MoreItemVisibiity = Visibility.Visible;
        }
        else
            MoreItemVisibiity = Visibility.Collapsed;
    }

    public IYoutubeService Youtube
    {
        get;
    }

    public async Task ProcessSerchItems(bool cleanList = true)
    {
        if (SearchItems == null)
            IsInitalLoaded = false;

        await CheckHasMoreItemsAsync();

        ObservableCollection<SearchFragmentItem> r;
        if (cleanList)
            SearchItems = new ObservableCollection<SearchFragmentItem>();
        SearchItems ??= new();
        r = SearchItems;
        var ct = 0;
        if (items != null)
            await foreach (var item in items)
            {
                if (r.Count >= MaxItemsToDisplay)
                    break;

                if (r.Count > ct++)
                    continue;

                r.Add(GetItemFromSearchResult(item));

            }
        IsInitalLoaded = true;
    }

    public static SearchFragmentItem GetItemFromSearchResult(ISearchResult item)
    {
        if (item is VideoSearchResult i)
        {
            return new()
            {
                ChannelName = i.Author.ChannelTitle,
                MediaType = "Video",
                Name = i.Title,
                Duration = MediaPlayerHelper.ConvertTimeSpanToDuration(i.Duration.GetValueOrDefault()),
                ThumbnailUrl = i.Thumbnails.GetBestThumbnail(),
                Item = i
            };
        }
        else if (item is ChannelSearchResult c)
        {
            return new()
            {
                MediaType = "Channel",
                Name = c.Title,
                ThumbnailUrl = c.Thumbnails.GetBestThumbnail(),
                Item = c
            };
        }
        else if (item is PlaylistSearchResult p)
        {
            return new()
            {
                MediaType = "Playlist",
                ChannelName = p.Author!.ChannelTitle,
                Name = p.Title,
                ThumbnailUrl = p.Thumbnails.GetBestThumbnail(),
                Item = p
            };
        }
        else if (item is PlaylistVideoSearchResult pv)
        {
            return new()
            {
                MediaType = "Video",
                ChannelName = pv.PlaylistVideo.Author!.ChannelTitle,
                Name = pv.Title,
                ThumbnailUrl = pv.PlaylistVideo.Thumbnails.GetBestThumbnail(),
                Item = pv,
                Duration = MediaPlayerHelper.ConvertTimeSpanToDuration(pv.PlaylistVideo.Duration.GetValueOrDefault())
            };
        }
        throw new NotImplementedException();
    }

    internal void SelectionChanged(int selectedIndex)
    {
        if (SearchItems == null || (uint)selectedIndex >= SearchItems.Count)
            return;
        SearchItems[selectedIndex].DoAction();
    }


    private async void ExpandMore()
    {
        if (MaxItemsToDisplay == -1)
            return;

        if (originalAmount == -1)
            originalAmount = MaxItemsToDisplay;

        MaxItemsToDisplay += originalAmount;

        await ProcessSerchItems(false);
    }
}
