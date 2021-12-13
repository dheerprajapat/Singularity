using SonicAudioApp.Models;
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


        private async static void LikedSongs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
             var content=JsonSerializer.Serialize(LikedSongs.ToList());
             await FileManager.WriteAllText(LikeInfoKeyPath, content);
        }

        public static async Task LoadLikedSettingsIfNotExists()
        {
            var content = await FileManager.ReadAllText(LikeInfoKeyPath);
            if(!string.IsNullOrWhiteSpace(content))
            {
                LikedSongs=new(JsonSerializer.Deserialize<List<AudioQueueItem>>(content));
            }

            foreach(var v in LikedSongs)
            {
                v.WaveformVisibilty = Windows.UI.Xaml.Visibility.Collapsed;
            }
            LikedSongs.CollectionChanged += LikedSongs_CollectionChanged;

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
