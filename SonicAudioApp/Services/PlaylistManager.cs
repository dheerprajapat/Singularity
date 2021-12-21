using SonicAudioApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace SonicAudioApp.Services
{
    public static class PlaylistManager
    {
        public static ObservableCollection<PlaylistInfo> Playlist { get; private set; } = new();
        public static readonly string LikeInfoKeyPath = "playlists.json";

        private async static void PlaylistSongs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var content = JsonSerializer.Serialize(Playlist.ToList());
            await FileManager.WriteAllText(LikeInfoKeyPath, content);
        }

        public static async Task LoadPlaylistSettingsIfNotExists()
        {
            var content = await FileManager.ReadAllText(LikeInfoKeyPath);
            if (!string.IsNullOrWhiteSpace(content))
            {
                Playlist = new(JsonSerializer.Deserialize<List<PlaylistInfo>>(content));
            }

            Playlist.CollectionChanged += PlaylistSongs_CollectionChanged;

        }

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
        public static void AddSong(string playlistName,AudioQueueItem item)
        {
            if (Playlist.Count(p => p.Title == playlistName) <= 0)
                Playlist.Add(new PlaylistInfo { Title = playlistName });
            var v = Playlist.First(p => p.Title == playlistName);
            if(!v.Songs.Contains(item))
                v.Songs.Add(item);
        }

    }
}
