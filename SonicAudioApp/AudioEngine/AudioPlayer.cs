using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace SonicAudioApp.AudioEngine;

public static class AudioPlayer
{
    private static MediaPlayer Audio = new MediaPlayer();
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

    static AudioPlayer()
    {
        Audio.MediaEnded += Audio_MediaEnded;
        Audio.PlaybackSession.PositionChanged += PlaybackSession_PositionChanged;
    }



    public static void Play()
    {
        if (AudioQueue.Count == 0)
            return;

        var currentSong = AudioQueue.Current;

        Audio.Source = MediaSource.CreateFromUri(new(currentSong.Url));
        
        Audio.Play();
    }

    private static void Audio_MediaEnded(MediaPlayer sender, object args)
    {
        var next=AudioQueue.Next();
        if (next is null)
            return;

        Play();
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
    public delegate void PositionChangedHandler(MediaPlaybackSession sender, object args);
    public static event PositionChangedHandler PositionChanged;
}