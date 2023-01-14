using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Singularity.Core.Contracts.Services;
using Singularity.Core.Services;
using YoutubeExplode.Playlists;

namespace Singularity.Core.Models;
public class PlaylistCollection
{
    public ObservableCollection<PlaylistItem> Playlists { get; set; } = new();
    public void AddSong(string playlistName, string song)
    {
        if (song == null || playlistName==null) return;
        var playlist = Playlists.FirstOrDefault(x => x.Name == playlistName);
        if (playlist == null)
            return;

        playlist.Add(song);
    }
    public void RemoveSong(string playlistName, string song)
    {
        if (song == null || playlistName == null) return;
        var playlist = Playlists.FirstOrDefault(x => x.Name == playlistName);
        if (playlist == null) return;
        playlist.Remove(song);
        SongRemoved?.Invoke(song);
    }
    public delegate void SongRemovedHandler(string id);
    public event SongRemovedHandler? SongRemoved;
}

#nullable enable
public class PlaylistItem
{

    public string Name
    {
        get; set;
    }
    public string Author
    {
        get; set;
    }
    public ObservableCollection<string> Songs
    {
        get; set;
    } = new ObservableCollection<string>();

    public string ThumbnailUrl
    {
        get; set;
    }
    public PlaylistItem(string name, string author, ObservableCollection<string> songs, string thumbnailUrl = "../Assets/playlist_logo.jpg")
    {
        Name = name;
        Author = author;
        Songs = songs;
        ThumbnailUrl = thumbnailUrl;
    }
    public void Add(string song)
    {
        if (Songs.Contains(song)) return;
        Songs.Add(song);
    }

    internal void Remove(string song)
    {
        if (Songs.Contains(song))
        {
            Songs.Remove(song);
        }
    }
}
