using Singularity.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singularity.Models
{
    public class LocalPlaylistItem : IPlaylistItem
    {
        public List<Video> Videos {  get; set; } = new List<Video>();
        public void Add(Video video)
        {
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
            if(!Videos.Contains(video)) return;
            Videos.Remove(video);
        }
    }
}
