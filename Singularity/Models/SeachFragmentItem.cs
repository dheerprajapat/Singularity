using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Singularity.Views;
using YoutubeExplode.Search;

namespace Singularity.Models;
public class SearchFragmentItem
{
    public string? Name { get; set; }
    public string? MediaType { get; set; }
    public string? ChannelName { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? Duration
    {
        get; set;
    }
    public Visibility HideDuration =>  Item is not VideoSearchResult
    ? Visibility.Collapsed : Visibility.Visible;

    public ISearchResult? Item
    {
        get; set;
    }

    public SearchFragmentItem Self => this;

    public async ValueTask DoAction()
    {
        if(Item== null) return;

        if (Item is VideoSearchResult v)
        {
            await AudioQueue.AddSong(v.Id,playNow:true);
            MusicControllerView.ExViewModel.Play();
        }
    }

}
