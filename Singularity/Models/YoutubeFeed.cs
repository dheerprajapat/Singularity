using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YoutubeExplode.Playlists;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Singularity.Models;

public class Genre
{
    public Genre(
        string name,
        string playlistId,
        string thumbail
    )
    {
        this.Name = name;
        this.PlaylistId = playlistId;
        Thumbnail = new BitmapImage { UriSource = new Uri(thumbail) };
    }

    public string Name
    {
        get;
    }

    public string PlaylistId
    {
        get;
    }
    public ImageSource Thumbnail
    {
        get;
    }
}