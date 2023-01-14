using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Singularity.Contracts.Services;
using Singularity.Core.Contracts.Services;
using Singularity.Helpers;
using Singularity.Models;
using Singularity.Views;
using YoutubeExplode.Videos;

namespace Singularity.ViewModels;
public partial class SearchItemViewModel : ObservableRecipient, ICrossThreadOperable
{
    [ObservableProperty]
    public SearchFragmentItem? item;

    [ObservableProperty]
    public Visibility currentlyPlaying = Visibility.Collapsed;

    private DispatcherQueue dispatchQueue = DispatcherQueue.GetForCurrentThread();

    public SongStringPageInfoModel? MetaInfo;

    [ObservableProperty]
    public SymbolIcon likeUnlikeSymbol;
    [ObservableProperty]
    public string likeUnlikeText;

    public DispatcherQueue DispatcherQueue
    {
        get => dispatchQueue;
        set => dispatchQueue = value;
    }
    public INavigationService NavigationService
    {
        get;
    }
    public IUserSettingsService UserSettingsService
    {
        get;
    }

    public SearchItemViewModel(INavigationService navigationService, IUserSettingsService userSettingsService)
    {
        AudioQueue.OnCurrentPlaybackItemChanged += AudioQueue_OnCurrentPlaybackItemChanged;
        MusicControllerView.ExViewModel!.playerElement!.MediaPlayer!.CurrentStateChanged += MediaPlayer_CurrentStateChanged;
        NavigationService = navigationService;
        UserSettingsService = userSettingsService;
        UserSettingsService.CurrentSetting.OnLikePageToggledForId += CurrentSetting_OnLikePageToggledForId;
    }

    private void CurrentSetting_OnLikePageToggledForId(string id, bool added = false)
    {
        UpdateLikeContextMenu();
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
            UpdateCurrentPlayingState(MusicControllerView.ExViewModel.playerElement!.MediaPlayer!.CurrentState == Windows.Media.Playback.MediaPlayerState.Playing);

        });
    }

    private async void AudioQueue_OnCurrentPlaybackItemChanged(Windows.Media.Playback.MediaPlaybackList sender, Windows.Media.Playback.CurrentMediaPlaybackItemChangedEventArgs args)
    {
        await (this as ICrossThreadOperable).ExecuteOnUIThread(() =>
        {
            UpdateCurrentPlayingState();

        });
    }

    public void UpdateLikeContextMenu()
    {
        if (Item == null)
            return;
        bool isLiked = UserSettingsService.CurrentSetting.LikedSongs.Contains(Item.Id);
        LikeUnlikeSymbol = new(isLiked
            ? Symbol.Dislike : Symbol.Like);
        LikeUnlikeText = isLiked ? "Dislike" : "Like";
    }

    public void UpdateCurrentPlayingState(bool isPlaying = true)
    {
        UpdateLikeContextMenu();

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
