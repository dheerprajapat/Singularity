using SonicAudioApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SonicAudioApp.Services
{
    public static class LikedSongManager
    {
        public static ObservableCollection<AudioQueueItem> LikedSongs=new ObservableCollection<AudioQueueItem>();
        public const string LikeInfoFilePath = "liked.json";
        static LikedSongManager()
        {
            CreateNewFileIfNotExists();
            LikedSongs=JsonSerializer.Deserialize<ObservableCollection<AudioQueueItem>>(File.ReadAllText(LikeInfoFilePath));
            LikedSongs.CollectionChanged += LikedSongs_CollectionChanged;
        }

        private static void CreateNewFileIfNotExists()
        {
            if (File.Exists(LikeInfoFilePath))
                return;
            File.Create(LikeInfoFilePath);
        }

        private static void LikedSongs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            File.WriteAllText(LikeInfoFilePath,JsonSerializer.Serialize(LikedSongs));
        }

        public static void Add(AudioQueueItem item)
        {
            if (LikedSongs.Contains(item))
                return;
            LikedSongs.Add(item);
        }
        public static void Remove(AudioQueueItem item)
        {
            if (LikedSongs.Contains(item))
                LikedSongs.Remove(item);
        }
    }
}
