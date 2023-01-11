using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Singularity.Core.Services;

namespace Singularity.Core.Models;
public class UserSettings
{
    [JsonIgnore]
    readonly UserSettingsService userSettingsService = new();

    public ObservableCollection<string> LikedSongs { get; set; } = new();
    public PlaylistCollection PlaylistCollection { get; set; } = new();
    public MediaSettngs Media
    {
        get; set;
    } = new MediaSettngs();

    public bool IsLiked(string id) => LikedSongs.Contains(id);
    public void ToggleLiked(string id)
    {
        if (IsLiked(id))
            LikedSongs.Remove(id);
        else
        {
            LikedSongs.Insert(0,id);
        }

    }

    public void Save()
    {
        userSettingsService.Write(this);
    }
}
