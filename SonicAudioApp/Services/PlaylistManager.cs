using SonicAudioApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace SonicAudioApp.Services
{
    public static class PlaylistManager
    {
        public static ObservableCollection<PlaylistInfo> Playlist { get; } = new();
        public static void Add(string playlistName)
        {
            if (Playlist.Count(p=>p.Title==playlistName)<=0)
                Playlist.Add(new PlaylistInfo { Title=playlistName});
        }
        public static PlaylistInfo Get(string playlistName)
        {
            var v = Playlist.First(x => x.Title == playlistName);
            return v;
        }

    }
}
