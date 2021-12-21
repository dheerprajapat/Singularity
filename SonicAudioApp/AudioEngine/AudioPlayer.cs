using SonicAudioApp.Services.YoutubeSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace SonicAudioApp.AudioEngine;

public static class AudioPlayer
{
    private static MediaPlayer Audio = new MediaPlayer() { RealTimePlayback=true};
    public static TimeSpan TotalDuration => Audio.PlaybackSession is not null ?
        Audio.PlaybackSession.NaturalDuration : TimeSpan.Zero;
    public static TimeSpan Position
        => Audio.PlaybackSession is not null ? Audio.PlaybackSession.Position : TimeSpan.Zero;

    public static double Volume
    {
        get => Audio.Volume;
        set => Audio.Volume = value;
    }

    public static bool ISMuted
    {
        get => Audio.IsMuted;
        set=> Audio.IsMuted = value;
    }

    public static MediaPlaybackState PlaybackState => Audio.PlaybackSession.PlaybackState;

    static AudioPlayer()
    {
        Audio.MediaEnded += Audio_MediaEnded;
        Audio.PlaybackSession.PositionChanged += PlaybackSession_PositionChanged;
        Audio.PlaybackSession.PlaybackStateChanged += PlaybackSession_PlaybackStateChanged;
        Audio.SourceChanged += Audio_SourceChanged;
    }

    public static async Task PlayAsync(bool begin=true)
    {
        if (AudioQueue.Count == 0)
            return;

        var currentSong = AudioQueue.Current;
        if (currentSong.RenewRequired) 
            await YoutubeManager.UpdateUrlAsync(currentSong);
        
        var currentSource = MediaSource.CreateFromUri(new(currentSong.Url));

        if (begin)
        {
            Audio.Source = currentSource;
            UpdatePosition(TimeSpan.Zero);
        }
        
        Audio.Play();
    }
    public static void Stop()
    {
        Audio.Pause();
    }

    public static async Task PlayNextAsync()
    {
        var next = AudioQueue.Next();
        if (next is null)
            return;
        await PlayAsync();
    }
    public static async Task PlayPreviousAsync()
    {
        var p = AudioQueue.Previous();
        if (p is null)
            return;
        await PlayAsync();
    }


    private static async void Audio_MediaEnded(MediaPlayer sender, object args)
    {
        await PlayNextAsync();
    }
    public static void UpdatePosition(TimeSpan time)
    {
        if (Audio.PlaybackSession is not null)
            Audio.PlaybackSession.Position = time;
    }
    private static void PlaybackSession_PositionChanged(MediaPlaybackSession sender, object args)
    {
        PositionChanged?.Invoke(sender, args);
    }
    private static void PlaybackSession_PlaybackStateChanged(MediaPlaybackSession sender, object args)
    {
        PlaybackStateChanged?.Invoke(sender, args);
    }
    private static void Audio_SourceChanged(MediaPlayer sender, object args)
    {
        SourceChanged?.Invoke(sender, args);
    }

    public delegate void PlaybackStateHandler(MediaPlaybackSession sender, object args);
    public static event PlaybackStateHandler PositionChanged;
    public static event PlaybackStateHandler PlaybackStateChanged;
    public delegate void SourceChangedHandler(MediaPlayer sender, object args);
    public static event SourceChangedHandler SourceChanged;

}