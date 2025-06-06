﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Singularity.Contracts;
using Singularity.Managers;
using Singularity.Models;

namespace Singularity.Data;

public partial class UserSettings : SettingsBase<UserSettings>
{

    [ObservableProperty]
    LoopMode loopMode = LoopMode.All;

    [ObservableProperty]
    ObservableCollection<string> likedSongs = new ObservableCollection<string>();

    [Obsolete]
    public UserSettings()
    {

    }


    protected override async void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        await SaveSettingsInDb();
    }

    public async ValueTask AddToLikeAsync(ISong song)
    {
        if (IsLiked(song)) return;

        LikedSongs.Insert(0,song.Id);
        await SaveSettingsInDb();
    }
    public async ValueTask RemoveFromLikeAsync(ISong song)
    {
        if (!IsLiked(song)) return;

        LikedSongs.Remove(song.Id);
        await SaveSettingsInDb();
    }

    public bool IsLiked(ISong song)
    {
        return LikedSongs.Contains(song.Id);
    }

    public async IAsyncEnumerable<ISong> GetLikedSongAsync()
    {
        var musicHub = SystemManager.GetService<IMusicHub>();
        if(musicHub==null)
            yield break;

        foreach (var songId in LikedSongs.ToArray())
        {
           var song = await musicHub.GetSongMetaDataAsync(songId);
            if (song == null) continue;
            yield return song;
        }
    }
    public void PrefetchLikedSongs()
    {
        var musicHub = SystemManager.GetService<IMusicHub>();

        if (musicHub == null)
            return;

        //prefetch liked songs
        Parallel.ForEach(LikedSongs, song =>
        {
            _ = musicHub.GetSongMetaDataAsync(song);
        });
    }
}
