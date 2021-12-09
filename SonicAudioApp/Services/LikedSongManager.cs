using SonicAudioApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicAudioApp.Services
{
    public static class LikedSongManager
    {
        public static ObservableCollection<AudioQueueItem> LikedSongs=new ObservableCollection<AudioQueueItem>();
        static LikedSongManager()
        {
            LikedSongs.CollectionChanged += LikedSongs_CollectionChanged;
        }

        private static void LikedSongs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
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
