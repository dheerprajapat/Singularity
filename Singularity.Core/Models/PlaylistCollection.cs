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
    public ObservableCollection<PlaylistCollection> Playlists { get; set; } = new();
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
    public PlaylistItem(string name, string author, ObservableCollection<string> songs, string thumbnailUrl)
    {
        Name = name;
        Author = author;
        Songs = songs;
        ThumbnailUrl = thumbnailUrl;
    }
}
