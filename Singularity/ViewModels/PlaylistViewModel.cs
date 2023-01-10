using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Singularity.Core.Contracts.Services;
using Singularity.Core.Models;

namespace Singularity.ViewModels;
public partial class PlaylistViewModel: ObservableRecipient
{
    [ObservableProperty]
    public ObservableCollection<PlaylistItem> playlists;
    public PlaylistViewModel(IUserSettingsService userSettingsService)
    {
        UserSettingsService = userSettingsService;
        Playlists = userSettingsService.CurrentSetting.PlaylistCollection.Playlists;
    }

    public IUserSettingsService UserSettingsService
    {
        get;
    }

    internal void CreateNewPlaylist(string text)
    {
        if(string.IsNullOrEmpty(text) || Playlists.Any(x=>x.Name==text))
        {
            return;
        }

        Playlists.Add(new PlaylistItem(text, string.Empty, new(), "../Assets/playlist_logo.jpg"));
    }
}
