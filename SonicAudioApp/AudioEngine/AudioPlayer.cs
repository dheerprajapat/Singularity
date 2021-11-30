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
    private static MediaPlayer Audio=new MediaPlayer();

    public static void Play()
    {
        if (AudioQueue.Count == 0)
            return;

        var currentSong = AudioQueue.Current;

        Audio.Source = MediaSource.CreateFromUri(new(currentSong.Url));

        Audio.Play();
    }
}