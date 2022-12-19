using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Singularity.Contracts.Services;
using Singularity.ViewModels;
using Singularity.Views;
using YoutubeExplode.Search;

namespace Singularity.Models;
public partial class SearchFragmentItem:ObservableRecipient
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
    ? Visibility.Collapsed : 
        Duration!="00:00"?
        Visibility.Visible:
        Visibility.Collapsed;

    [AlsoNotifyChangeFor(nameof(HideDuration))]
    [ObservableProperty]
    private ISearchResult? item;
    
    public SearchFragmentItem Self => this;

    public Visibility IsLive => Item is VideoSearchResult && (Item as VideoSearchResult)!.Duration is null?
        Visibility.Visible:Visibility.Collapsed;


    public void DoAction()
    {
        if(Item== null) return;

        if (Item is VideoSearchResult v)
        {
            PlayNow(v.Id);
        }
        else if (Item is PlaylistSearchResult p)
        {
            var service = App.GetService<INavigationService>();
            service.NavigateTo(typeof(PlaylistItemPageViewModel).FullName!, p.Id);
        }
        else if (Item is PlaylistVideoSearchResult pv)
        {
            PlayNow(pv.PlaylistVideo.Id);
        }
        else if (Item is ChannelSearchResult c)
        {
            var service = App.GetService<INavigationService>();
            service.NavigateTo(typeof(ChannelItemPageViewModel).FullName!, c.Id);
        }

    }
    private async void PlayNow(string id)
    {
        await AudioQueue.AddSong(id, playNow: true);
        MusicControllerView.ExViewModel.Play();
    }

}
