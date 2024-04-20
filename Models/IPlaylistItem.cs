using Singularity.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singularity.Models
{
    public interface IPlaylistItem
    {
        void Add(Video video);
        void Remove(Video video);    
        bool Contains(Video video);
        IEnumerable<Video> GetAll();
    }
}
