using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Singularity.Contracts.Services;
using Singularity.Helpers;
using Singularity.Models;
using Singularity.Views;
using YoutubeExplode.Videos;

namespace Singularity.ViewModels;
public partial class SearchItemViewModel:ObservableRecipient, ICrossThreadOperable
{
    [ObservableProperty]
    public SearchFragmentItem? item;

    [ObservableProperty]
    public Visibility currentlyPlaying = Visibility.Collapsed;

    private DispatcherQueue dispatchQueue=DispatcherQueue.GetForCurrentThread();

    public SongStringPageInfoModel? MetaInfo;
    public DispatcherQueue DispatcherQueue
    {
        get => dispatchQueue;
        set => dispatchQueue = value;
    }
    public INavigationService NavigationService
    {
        get;
    }

    public SearchItemViewModel(INavigationService navigationService)
    {
        AudioQueue.OnCurrentPlaybackItemChanged += AudioQueue_OnCurrentPlaybackItemChanged;
        MusicControllerView.ExViewModel!.playerElement!.MediaPlayer!.CurrentStateChanged += MediaPlayer_CurrentStateChanged;
        NavigationService = navigationService;
    }


    ~SearchItemViewModel()
    {
        AudioQueue.OnCurrentPlaybackItemChanged -= AudioQueue_OnCurrentPlaybackItemChanged;
        MusicControllerView.ExViewModel!.playerElement!.MediaPlayer!.CurrentStateChanged -= MediaPlayer_CurrentStateChanged;
    }
    private async void MediaPlayer_CurrentStateChanged(Windows.Media.Playback.MediaPlayer sender, object args)
    {
        await (this as ICrossThreadOperable).ExecuteOnUIThread(() =>
        {
            UpdateCurrentPlayingState(MusicControllerView.ExViewModel.playerElement!.MediaPlayer!.CurrentState==Windows.Media.Playback.MediaPlayerState.Playing);

        });
    }

    private async void AudioQueue_OnCurrentPlaybackItemChanged(Windows.Media.Playback.MediaPlaybackList sender, Windows.Media.Playback.CurrentMediaPlaybackItemChangedEventArgs args)
    {
        await (this as ICrossThreadOperable).ExecuteOnUIThread(() =>
        {
            UpdateCurrentPlayingState();

        });
    }
    
    public void UpdateCurrentPlayingState(bool isPlaying=true)
    {
        if (AudioQueue.CurrentPlayingItemId != null &&
           Item != null && isPlaying &&
           Item.Id == AudioQueue.CurrentPlayingItemId)
        {
            CurrentlyPlaying = Visibility.Visible;
        }
        else
            CurrentlyPlaying = Visibility.Collapsed;
    }
}
