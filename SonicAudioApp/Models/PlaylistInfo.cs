using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicAudioApp.Models
{
    public record PlaylistInfo
    {
        public string Title { get; set; }
        public string Thumbnail { get; set; } = @"../Assets/playlist_logo.jpg";
        public string Author { get; set; }
        public ObservableCollection<AudioQueueItem> Songs { get; set; } = new();
    }
}
