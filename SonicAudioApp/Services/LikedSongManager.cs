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
        private static readonly string DirPath = $@"{DocPath}\Singularity";
        private static readonly string DocPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}";
        public static readonly string LikeInfoKeyPath = "liked.json";
        public static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        static LikedSongManager()
        {
            LoadLikedSettingsIfNotExists();
            LikedSongs.CollectionChanged += LikedSongs_CollectionChanged;
        }

        private static async void LoadLikedSettingsIfNotExists()
        {
            var folder = await StorageFolder.GetFolderFromPathAsync(DocPath);
            folder=await folder.CreateFolderAsync("Singularity", CreationCollisionOption.OpenIfExists);
            var file = await folder.CreateFileAsync(LikeInfoKeyPath, CreationCollisionOption.OpenIfExists);
            using var stream= await file.OpenAsync(FileAccessMode.Read);
            using var reader = new StreamReader(stream.AsStream());
            var content = reader.ReadToEnd();
            if(!string.IsNullOrWhiteSpace(content))
            {
                LikedSongs=new(JsonSerializer.Deserialize<List<AudioQueueItem>>(content));
            }

            foreach(var v in LikedSongs)
            {
                v.WaveformVisibilty = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        private static async void LikedSongs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var content=JsonSerializer.Serialize(LikedSongs.ToList());
            var folder = await StorageFolder.GetFolderFromPathAsync(DocPath);
            folder = await folder.CreateFolderAsync("Singularity", CreationCollisionOption.OpenIfExists);
            var file = await folder.CreateFileAsync(LikeInfoKeyPath, CreationCollisionOption.OpenIfExists);
            using var stream = await file.OpenAsync(FileAccessMode.ReadWrite);
            using var reader = new StreamWriter(stream.AsStream());
            reader.Write(content);
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
