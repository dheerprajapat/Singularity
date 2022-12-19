using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Singularity.Contracts.Services;
using Singularity.ViewModels;
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



    public void DoAction()
    {
        if(Item== null) return;

        if (Item is VideoSearchResult v)
        {
            PlayNow(v.Id);
        }
        else if (Item is PlaylistSearchResult p)
        {
            var service= App.GetService<INavigationService>();
            service.NavigateTo(typeof(PlaylistItemPageViewModel).FullName!, p.Id);
        }
        else if (Item is PlaylistVideoSearchResult pv)
        {
            PlayNow(pv.PlaylistVideo.Id);
        }

    }
    private async void PlayNow(string id)
    {
        await AudioQueue.AddSong(id, playNow: true);
        MusicControllerView.ExViewModel.Play();
    }

}
