using Singularity.Audio;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Singularity.Models
{
    public class UserSetting
    {
        private static UserSetting? instance;

        public static UserSetting Instance 
        {
            get { 
                if(instance == null)
                {
                    Init();
                }
                return instance!;
            } 
        }

        public ObservableCollection<Video> LikedSongs { get; set; } = new();

        private static string FilePath 
        { 
            get
            {
                return Path.Combine(FileSystem.Current.AppDataDirectory, "Settings.json");
            }
        }
        private static void Init()
        {
            if(!File.Exists(FilePath))
            {
                instance = new UserSetting();
                return;
            }
            try
            {
                var json = File.ReadAllText(FilePath);
                instance = JsonSerializer.Deserialize<UserSetting>(json)!;
            }
            catch {
                instance = new UserSetting();
                return;
            }
        }

        public static void Save()
        {
            if (instance == null) return;

            var json = JsonSerializer.Serialize(instance);
            File.WriteAllText(FilePath, json);
        }

        public bool IsLiked(Video video)
        {
            return LikedSongs.Contains(video);
        }

        public void AddToLike(Video video)
        {
            if (LikedSongs.Contains(video)) return;
            LikedSongs.Add(video);
            Save();
        }

        public void RemoveFromLike(Video video)
        {
            if (LikedSongs.Contains(video))
            {
                LikedSongs.Remove(video);
                Save();
            }
        }
    }
}
