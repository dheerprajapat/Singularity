using SonicAudioApp.Models;
using SonicAudioApp.Services.YoutubeSearch;
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


        private static List<PlaylistSerializable> GetPlaylistSeraizable() =>
            Playlist.Select(x => new PlaylistSerializable 
            { 
                Title = x.Title,
                SongIds=x.Songs.Select(x => x.Id).ToList(),
            }).ToList();


        private async static void PlaylistSongs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var content = JsonSerializer.Serialize(GetPlaylistSeraizable());
            await FileManager.WriteAllText(LikeInfoKeyPath, content);
        }

        private static  void LoadPlaylists(List<PlaylistSerializable> lst)
        {
            foreach (var item in lst)
            {

                Playlist.Add(new PlaylistInfo
                {
                    Songs = ((ObservableCollection<AudioQueueItem>)item.SongIds.Select(async x => await YoutubeManager.GetInfo(x))),
                    Title = item.Title,
                });
            }
        }

        public static async Task LoadPlaylistSettingsIfNotExists()
        {
            var content = await FileManager.ReadAllText(LikeInfoKeyPath);
            if (!string.IsNullOrWhiteSpace(content))
            {
                var pl = JsonSerializer.Deserialize<List<PlaylistSerializable>>(content);
                LoadPlaylists(pl);
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
            if (!v.Songs.Contains(item))
            {
                v.Songs.Add(item);
                PlaylistSongs_CollectionChanged(null,null);
            }
        }

        public static void RemoveSong(string playListName, AudioQueueItem d)
        {
            if (playListName == null)
                return;
            if (Playlist.Count(p => p.Title == playListName) <= 0)
                return;
            var v = Playlist.First(p => p.Title == playListName);
            if (v.Songs.Contains(d))
            {
                v.Songs.Remove(d);
                PlaylistSongs_CollectionChanged(null, null);
            }
        }

        private record PlaylistSerializable
        {
            public string Title { get; set; }
            public List<string> SongIds { get; set; }
        }
    }
}
