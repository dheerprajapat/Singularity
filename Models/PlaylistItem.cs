using Singularity.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Singularity.Models
{
    //[JsonDerivedType(typeof(LocalPlaylistItem), typeDiscriminator: "LocalPlaylistItem")]
    public class PlaylistItem
    {
        public List<Video> Videos { get; set; } = new List<Video>();
        public PlaylistType PlaylistType { get; set; } = PlaylistType.Local;
        public string PlaylistId { get; set; } = string.Empty;

        public PlaylistItem()
        {
        }
        
        /// <summary>
        /// Create a youtube playlist
        /// </summary>
        /// <param name="playListId"></param>
        public PlaylistItem(string playListId)
        {
            PlaylistId = playListId;
            PlaylistType = PlaylistType.Youtube;
        }
        public void Add(Video video)
        {
            if (PlaylistType == PlaylistType.Youtube)
                return;

            if (Videos.Contains(video)) return;
            Videos.Add(video);
        }

        public bool Contains(Video video)
        {
            return Videos.Contains(video);
        }

        public IEnumerable<Video> GetAll()
        {
            return Videos;
        }

        public void Remove(Video video)
        {
            if (PlaylistType != PlaylistType.Local)
                return;
            if (!Videos.Contains(video)) return;
                Videos.Remove(video);
        }
    }

    public enum PlaylistType
    {
        Local,
        Youtube
    }
}
