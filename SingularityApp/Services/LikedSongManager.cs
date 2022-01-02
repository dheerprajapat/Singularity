using SonicAudioApp.Models;
using SonicAudioApp.Services.YoutubeSearch;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace SonicAudioApp.Services
{
    public static class LikedSongManager
    {
        public static ObservableCollection<AudioQueueItem> LikedSongs { get; set; } =new ObservableCollection<AudioQueueItem>();
        public static readonly string LikeInfoKeyPath = "liked.json";
        private static List<string> SerializableSongs()=>LikedSongs.Select(s => s.Id).ToList();

        private async static void LikedSongs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
             var content=JsonSerializer.Serialize(SerializableSongs());
             await FileManager.WriteAllText(LikeInfoKeyPath, content);
        }

        public static async Task LoadLikedSettingsIfNotExists()
        {
            var content = await FileManager.ReadAllText(LikeInfoKeyPath);
            if (!string.IsNullOrWhiteSpace(content))
            {
                var serialized = JsonSerializer.Deserialize<List<string>>(content);
                await DesrializeSong(serialized);
            }
            foreach(var v in LikedSongs)
                v.WaveformVisibilty = Windows.UI.Xaml.Visibility.Collapsed;
            LikedSongs.CollectionChanged += LikedSongs_CollectionChanged;

        }
        public static async Task DesrializeSong(List<string> serializedSongs)
        {

            foreach(var s in serializedSongs)
            {
                LikedSongs.Add(await YoutubeManager.GetInfo(s));
            }
        }

        public static void Add(AudioQueueItem item)
        {
            if (LikedSongs.Contains(item))
                return;
            item.Liked = true;
            LikedSongs.Insert(0,item);
        }
        public static void Remove(AudioQueueItem item)
        {
            item.Liked = false;

            if (LikedSongs.Contains(item))
                LikedSongs.Remove(item);
        }

    }
}
