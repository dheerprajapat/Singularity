using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Singularity.Helpers;
using Singularity.Models;
using YoutubeExplode.Videos;

namespace Singularity.ViewModels;
public partial class SearchItemViewModel:ObservableRecipient, ICrossThreadOperable
{
    [ObservableProperty]
    public SearchFragmentItem? item;

    [ObservableProperty]
    public Visibility currentlyPlaying = Visibility.Collapsed;

    private DispatcherQueue dispatchQueue=DispatcherQueue.GetForCurrentThread();
    public DispatcherQueue DispatcherQueue
    {
        get => dispatchQueue;
        set => dispatchQueue = value;
    }

    public SearchItemViewModel()
    {
        AudioQueue.OnCurrentPlaybackItemChanged += AudioQueue_OnCurrentPlaybackItemChanged;
    }
    ~SearchItemViewModel()
    {
        AudioQueue.OnCurrentPlaybackItemChanged -= AudioQueue_OnCurrentPlaybackItemChanged;
    }
    private async void AudioQueue_OnCurrentPlaybackItemChanged(Windows.Media.Playback.MediaPlaybackList sender, Windows.Media.Playback.CurrentMediaPlaybackItemChangedEventArgs args)
    {
        await (this as ICrossThreadOperable).ExecuteOnUIThread(() =>
        {
            UpdateCurrentPlayingState();

        });

    }
    public void UpdateCurrentPlayingState()
    {
        if (AudioQueue.CurrentPlayingItemId != null &&
           Item != null &&
           Item.Id == AudioQueue.CurrentPlayingItemId)
        {
            CurrentlyPlaying = Visibility.Visible;
        }
        else
            CurrentlyPlaying = Visibility.Collapsed;
    }
}
