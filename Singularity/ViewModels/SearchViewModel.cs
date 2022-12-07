using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Singularity.Core.Contracts.Services;
using YoutubeExplode.Search;

namespace Singularity.ViewModels;
public partial class SearchViewModel: ObservableRecipient
{
    [ObservableProperty]
    public ObservableCollection<ISearchResult>? videos;
    private int kMax = 3;
    public SearchViewModel(IYoutubeService youtube)
    {
        Youtube = youtube;
        SearchVideo();
    }

    public IYoutubeService Youtube
    {
        get;
    }

    async void SearchVideo()
    {
        var i = 0;
        var r= new List<VideoSearchResult>();
        await foreach (VideoSearchResult v in Youtube.GetSearchResult("Die for you", SearchType.Video))
        {
            r.Add(v);
            i++;
            if (i >= kMax)
                break;
        }
        Videos = new(r);
    }
}
