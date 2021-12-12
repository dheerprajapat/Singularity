using SonicAudioApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicAudioApp.Services
{
    public static class PlaylistManager
    {
        public static Dictionary<string, ObservableCollection<AudioQueueItem>> Playlist { get; } = new();
        public static void Add(string playlistName,AudioQueueItem item)
        {
            if (!Playlist.ContainsKey(playlistName))
                Playlist.Add(playlistName, new ObservableCollection<AudioQueueItem>());
            if(item is not null)
                Playlist[playlistName].Add(item);
        }
        public static void Remove(string playlistName, AudioQueueItem item)
        {
            if (!Playlist.ContainsKey(playlistName))
                return;
            if (!Playlist[playlistName].Contains(item))
                return;
            Playlist[playlistName].Remove(item);
        }
    }
}
