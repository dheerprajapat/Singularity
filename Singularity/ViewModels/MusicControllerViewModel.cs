using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Singularity.Core.Contracts.Services;
using Singularity.Helpers;
using Singularity.Models;
using Windows.ApplicationModel.Core;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Core;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace Singularity.ViewModels;

public partial class MusicCotrollerViewModel : ObservableRecipient
{
    public MediaPlayerElement? playerElement;
    public MusicCotrollerViewModel(IYoutubeService youtube, IUserSettingsService userSettingsService)
    {
        Youtube = youtube;
        UserSettingsService = userSettingsService;
        NextSongCommand = new RelayCommand(PlayNext);
        PreviousSongCommand = new RelayCommand(PlayPrevious);
        ShuffleCommand = new RelayCommand(ToggleShuffle);
        PlayCommand = new RelayCommand(Play);
        ToggleRepeatCommand = new RelayCommand(ToggleRepeat);

        AudioQueue.OnCurrentPlaybackItemChanged += AudioQueue_OnCurrentPlaybackItemChanged;
        AudioQueue.InitAudioQueue(Youtube);
        AudioQueue.ToggleRepeat();
    }


    public IYoutubeService Youtube
    {
        get;
    }
    public IUserSettingsService UserSettingsService
    {
        get;
    }

    [AlsoNotifyChangeFor(nameof(LikedIcon))]
    [AlsoNotifyChangeFor(nameof(Title))]
    [AlsoNotifyChangeFor(nameof(Singer))]
    [AlsoNotifyChangeFor(nameof(Thumbnail))]
    [AlsoNotifyChangeFor(nameof(MaxDurationString))]
    [AlsoNotifyChangeFor(nameof(MaxDuration))]

    [ObservableProperty]
    public Video? video;

    public string? Title => video?.Title;
    public string? Singer => video?.Author.ChannelTitle;
    
    public string MaxDurationString => video is not null ?
        MediaPlayerHelper.ConvertTimeSpanToDuration(video.Duration.GetValueOrDefault()) : "00:00";
    public int MaxDuration => video is not null ? (int)video.Duration.GetValueOrDefault().TotalSeconds : 0;

    [ObservableProperty]
    public string positionString = "0:00";

    [ObservableProperty]
    public static int position=0;

    public ICommand NextSongCommand;
    public ICommand PreviousSongCommand;
    public ICommand ShuffleCommand;
    public ICommand PlayCommand;
    public ICommand ToggleRepeatCommand;


    [ObservableProperty]
    public string repeatModeIcon= "\uE8EE";

    readonly DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
    
    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(VolumeIcon))]
    public double volume = 0.5;

    public string LikedIcon
    {
        get
        {
            if(video!= null)
                return UserSettingsService.CurrentSetting.LikedSongs.Contains(Video.Id)? "\uEB52": "\uEB51";
            return "\uEB51";
        }
    }
    public string VolumeIcon=> volume switch
    {
        > .75 => "\ue995",
        <= .75 and > .25 => "\ue994",
        <= .25 and > .10 => "\ue993",
        _ => "\ue992",
    };

    [ObservableProperty]
    public string? playPauseIcon;
    public ImageSource? Thumbnail
    {
        get
        {
            if (video is null) return null;
            return new BitmapImage()
            {
                UriSource = new(video.Thumbnails.GetBestThumbnail())
            };
        }
    }

    internal void InitPlayer(MediaPlayerElement videoPlayer)
    {
        playerElement = videoPlayer;
        if (videoPlayer.MediaPlayer is not null)
        {
            videoPlayer.MediaPlayer.PlaybackSession.PositionChanged += PlaybackSession_PositionChanged;
            videoPlayer.MediaPlayer.CurrentStateChanged += MediaPlayer_CurrentStateChanged;
            playerElement!.MediaPlayer!.Source = AudioQueue.currentList;
        }
    }

    private async void MediaPlayer_CurrentStateChanged(MediaPlayer sender, object args)
    {
        await ExecuteOnUIThread(() =>
        {
            PlayPauseIcon = GetPlayPauseIcon();
        });
    }
    public string GetPlayPauseIcon()
    {
        if (playerElement is null)
            return "\uf5b0";
        switch (playerElement.MediaPlayer!.CurrentState)
        {
            case MediaPlayerState.Playing:
                return "\uf8ae";
            case MediaPlayerState.Stopped:
            case MediaPlayerState.Paused:
                return "\uf5b0";
            default:
                return "\uebd3";
        }
    }

    public void Play()
    {
        if(playerElement is null) return;

        if (playerElement.MediaPlayer!.CurrentState == MediaPlayerState.Playing)
        {
            playerElement.MediaPlayer.Pause();
        }
        else if(playerElement.MediaPlayer.CurrentState == MediaPlayerState.Paused 
            || playerElement.MediaPlayer.CurrentState == MediaPlayerState.Stopped) 
            playerElement.MediaPlayer?.Play();
    }
    void PlayPrevious()
    {
        AudioQueue.PlayPrevious();
        Position = 0;
    }
    void PlayNext()
    {
        AudioQueue.PlayNext();
        Position = 0;
    }
    void ToggleShuffle()
    {
        AudioQueue.ToggleShuffle();
    }

    void ToggleRepeat()
    {
        AudioQueue.ToggleRepeat();
        RepeatModeIcon = AudioQueue.AutoRepeatEnabled ? "\uE8EE" : "\uF5E7";
    }
    private async void AudioQueue_OnCurrentPlaybackItemChanged(MediaPlaybackList sender,
        CurrentMediaPlaybackItemChangedEventArgs args)
    {
        await ExecuteOnUIThread(async() =>
        {
            Video = await AudioQueue.GetVideoFromPlaybackItem(args.NewItem);
        });
    }


    private async void PlaybackSession_PositionChanged(Windows.Media.Playback.MediaPlaybackSession sender, object args)
    {
        if (sender.PlaybackState != MediaPlaybackState.Playing)
            return;
        await ExecuteOnUIThread(() =>
        {
            PositionString = MediaPlayerHelper.ConvertTimeSpanToDuration(sender.Position);
            Position = (int)sender.Position.TotalSeconds;
        });

    }
    public void PositionChanged(int value)
    {
        if (playerElement is null || video is null || value >= MaxDuration)
            return;

        playerElement.MediaPlayer!.PlaybackSession.Position = TimeSpan.FromSeconds(value);
    }
    public void SetVolume(int val)
    {
        Volume = val / 100.0;

        UserSettingsService.CurrentSetting.Media.Volume = val;

        if (playerElement is not null && playerElement.MediaPlayer is not null)
        {
            playerElement.MediaPlayer.Volume = volume;
        }
    }
    private async ValueTask ExecuteOnUIThread(Action action)
    {
        if (dispatcherQueue is null)
            return;

        await Task.Run(() =>
        {
            dispatcherQueue.TryEnqueue(() =>
            {
                action.Invoke();
            });
        });
    }

    internal void ToggleLiked()
    {
        if (Video != null)
        {
            UserSettingsService.CurrentSetting.ToggleLiked(Video.Id);
            this.OnPropertyChanged(nameof(LikedIcon));
        }
    }
}