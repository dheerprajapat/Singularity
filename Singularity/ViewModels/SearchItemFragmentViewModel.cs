
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Singularity.Core.Contracts.Services;
using Singularity.Helpers;
using Singularity.Models;
using YoutubeExplode.Search;

namespace Singularity.ViewModels;
public partial class SearchItemFragmentViewModel : ObservableRecipient
{
    [ObservableProperty]
    public string? header;

    [ObservableProperty]
    public IAsyncEnumerable<ISearchResult>? items;

    public int MaxItemsToDisplay = 3;


    [ObservableProperty]
    public ObservableCollection<SearchFragmentItem>? searchItems;

    [ObservableProperty]
    public Visibility moreItemVisibiity=Visibility.Visible;

    public SearchItemFragmentViewModel(IYoutubeService youtube)
    {
        Youtube = youtube;
        //LoadTempItems();
    }

    private async ValueTask CheckHasMoreItemsAsync()
    {
        var cloneIterator = items;
        var ct = 0;
        var hasMore = false;
        if(MaxItemsToDisplay!=0 && cloneIterator != null) 
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

    public async Task ProcessSerchItems()
    {
        await CheckHasMoreItemsAsync();

        var r = new ObservableCollection<SearchFragmentItem>();

        if (items != null)
            await foreach (var item in items)
            {
                if (MaxItemsToDisplay!=-1 && r.Count >= MaxItemsToDisplay)
                    break;

                if (item is VideoSearchResult i)
                {
                    r.Add(new()
                    {
                        ChannelName = i.Author.ChannelTitle,
                        MediaType = "Video",
                        Name = i.Title,
                        Duration = MediaPlayerHelper.ConvertTimeSpanToDuration(i.Duration.GetValueOrDefault()),
                        ThumbnailUrl = i.Thumbnails.GetBestThumbnail(),
                        Item = i
                    });
                }
                else if (item is ChannelSearchResult c)
                {
                    r.Add(new()
                    {
                        MediaType = "Channel",
                        Name = c.Title,
                        ThumbnailUrl = c.Thumbnails.GetBestThumbnail(),
                        Item = c
                    });
                }
                else if (item is PlaylistSearchResult p)
                {
                    r.Add(new()
                    {
                        MediaType = "Playlist",
                        ChannelName = p.Author!.ChannelTitle,
                        Name = p.Title,
                        ThumbnailUrl = p.Thumbnails.GetBestThumbnail(),
                        Item = p
                    });
                }
            }
        SearchItems = new ObservableCollection<SearchFragmentItem>(r);
    }

    internal async void SelectionChanged(int selectedIndex)
    {
        if(SearchItems==null || (uint)selectedIndex >= SearchItems.Count)
            return;
        await SearchItems[selectedIndex].DoAction();
    }
}
