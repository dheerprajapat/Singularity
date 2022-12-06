
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Singularity.Core.Contracts.Services;
using Singularity.Models;
using YoutubeExplode.Search;

namespace Singularity.ViewModels;
public partial class SearchItemFragmentViewModel : ObservableRecipient
{
    [ObservableProperty]
    public ObservableCollection<SearchFragmentItem> items;
    public static int MaxCount=5;
    public SearchItemFragmentViewModel(IYoutubeService youtube)
    {
        Youtube = youtube;
        LoadTempItems();
    }

    public IYoutubeService Youtube
    {
        get;
    }

    async void LoadTempItems()
    {

        var items =  Youtube.GetSearchResult("die for you",SearchType.Video);
        var res = new List<SearchFragmentItem>();
        int k = 0;
        await foreach (VideoSearchResult i in items)
        {
            res.Add(new()
            {
            ChannelName=i.Author.ChannelTitle,
            MediaType="Video",
            Name=i.Title,
            Duration=i.Duration.ToString(),
            ThumbnailUrl=i.Thumbnails.OrderByDescending(x=>x.Resolution.Area).FirstOrDefault().Url
            });
            k++;
            if (k >= MaxCount)
                break;
        }
        Items = new ObservableCollection<SearchFragmentItem>(res);
    }

}
