using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Singularity.Core.Services;

namespace Singularity.Core.Models;
public partial class UserSettings: ObservableRecipient
{
    [JsonIgnore]
    readonly UserSettingsService userSettingsService = new();
    [ObservableProperty]
    public ObservableCollection<string> likedSongs=new();
    public PlaylistCollection PlaylistCollection { get; set; } = new();
    public MediaSettngs Media
    {
        get; set;
    } = new MediaSettngs();

    public bool IsLiked(string id) => LikedSongs.Contains(id);
    public void ToggleLiked(string id)
    {
        if (id == null)
            return;

        if (IsLiked(id))
        {
            LikedSongs.Remove(id);
            OnLikePageToggledForId?.Invoke(id, false);
        }
        else
        {
            LikedSongs.Insert(0, id);
            OnLikePageToggledForId?.Invoke(id, true);
        }

    }
    public delegate void LikePageToggledForIdHandler(string id,bool added=false);
    public event LikePageToggledForIdHandler? OnLikePageToggledForId;


    public void Save()
    {
        userSettingsService.Write(this);
    }
}
