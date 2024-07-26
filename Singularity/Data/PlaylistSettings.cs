using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Singularity.Contracts;
using Singularity.Models;

namespace Singularity.Data;

internal partial class PlaylistSettings : SettingsBase<PlaylistSettings>
{
    [ObservableProperty] Dictionary<string, UserPlaylist> playlists = new Dictionary<string, UserPlaylist>();

    [Obsolete]
    public PlaylistSettings()
    {
    }

    protected override async void OnPropertyChanged(PropertyChangedEventArgs e)
    {

        base.OnPropertyChanged(e);
        await SaveSettingsInDb();
    }

    public bool IsPlaylistExist(string playlistName)
    {
        return Playlists.ContainsKey(playlistName);
    }

    public ValueTask CreateNewPlaylistAsync(string playlistName)
    {
        if (IsPlaylistExist(playlistName))
            return ValueTask.CompletedTask;

        Playlists[playlistName] = new UserPlaylist()
        {
            Id = playlistName,
            Name = playlistName,
            Singer = string.Empty,
            Description = string.Empty,
            ThumbnailUrl = string.Empty
        };

        PlaylistUpdated?.Invoke(this, EventArgs.Empty);

        return SaveSettingsInDb();
    }

    public ValueTask AddSongToPlaylistAsync(string playlistName, ISong song)
    {
        if (Playlists[playlistName].Songs.Contains(song.Id))
            return ValueTask.CompletedTask;

        Playlists[playlistName].Songs.Add(song.Id);
        PlaylistUpdated?.Invoke(this, EventArgs.Empty);
        return SaveSettingsInDb();
    }

    public ValueTask RemoveSongFromPlaylistAsync(string playlistName, ISong song)
    {
        Playlists[playlistName].Songs.Remove(song.Id);
        PlaylistUpdated?.Invoke(this, EventArgs.Empty);
        return SaveSettingsInDb();
    }
    public ValueTask RemoveSongFromPlaylistAsync(string playlistName, string song)
    {
        Playlists[playlistName].Songs.Remove(song);
        PlaylistUpdated?.Invoke(this, EventArgs.Empty);
        return SaveSettingsInDb();
    }
    public ValueTask RemovePlaylistAsync(string playlistName)
    {
        Playlists.Remove(playlistName);
        PlaylistUpdated?.Invoke(this, EventArgs.Empty);
        return SaveSettingsInDb();
    }

    public event EventHandler? PlaylistUpdated;
}