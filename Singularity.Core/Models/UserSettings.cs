using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Singularity.Core.Services;

namespace Singularity.Core.Models;
public class UserSettings
{
    public MediaSettngs Media
    {
        get; set;
    } = new MediaSettngs();

    public ObservableCollection<string> LikedSongs { get; set; } = new();

    public bool IsLiked(string id) => LikedSongs.Contains(id);
    public void ToggleLiked(string id)
    {
        if (IsLiked(id))
            LikedSongs.Remove(id);
        else
        {
            LikedSongs.Add(id);
        }

        new UserSettingsService().Write(this);
    }
}

public class MediaSettngs
{
    public int Volume
    {
        get; set;
    } = 50;
    public bool RepeatEnabled
    {
        get; set;
    } = true;

    public string? LastPlayedId
    {
        get; set;
    } = null;
}