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
        public static ObservableCollection<AudioQueueItem> LikedSongs=new ObservableCollection<AudioQueueItem>();
        public const string LikeInfoKeyPath = "liked.json";
        public static ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        static LikedSongManager()
        {
            LoadLikedSettingsIfNotExists();
            LikedSongs.CollectionChanged += LikedSongs_CollectionChanged;
        }

        private static void LoadLikedSettingsIfNotExists()
        {
            var content=localSettings.Values[LikeInfoKeyPath] as string;
            if(content != null)
            {
                LikedSongs=new(JsonSerializer.Deserialize<List<AudioQueueItem>>(content));
            }
        }

        private static void LikedSongs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var content=JsonSerializer.Serialize(LikedSongs.ToList());
            localSettings.Values[LikeInfoKeyPath]= content;
        }

        public static void Add(AudioQueueItem item)
        {
            if (LikedSongs.Contains(item))
                return;
            item.Liked = true;
            LikedSongs.Add(item);
        }
        public static void Remove(AudioQueueItem item)
        {
            item.Liked = false;

            if (LikedSongs.Contains(item))
                LikedSongs.Remove(item);
        }
    }
}
