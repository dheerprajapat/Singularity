using Singularity.Audio;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
        public Dictionary<string, PlaylistItem> PlayList { get; set; } = new Dictionary<string, PlaylistItem>();

        public LoopMode LoopMode { get; set; } = LoopMode.All;

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
            catch(Exception e) {
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

        public void AddLocalPlaylist(string playlistName)
        {
            if (PlayList.ContainsKey(playlistName)) return;
            PlayList.Add(playlistName, new PlaylistItem());
            Save();
        }

        public void RemovePlaylist(string playlistName)
        {
            if (!PlayList.ContainsKey(playlistName)) return;
            PlayList.Remove(playlistName);
            Save();
        }

        public void AddSongInLocalPlaylist(string playlistName, Video video)
        {
            AddLocalPlaylist(playlistName);
            PlayList[playlistName].Add(video);
            Save();
        }

    }

    [JsonSerializable(typeof(UserSetting))]
    public partial class UserSettingContext: JsonSerializerContext
    {

    }
}
