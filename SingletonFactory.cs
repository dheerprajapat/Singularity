using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Singularity.Audio;
using YoutubeExplode;

namespace Singularity
{
    internal static class SingletonFactory
    {
        public static YoutubeClient YoutubeClient { get; } = new YoutubeClient();
        public static AudioManager AudioManager { get; } = new AudioManager();
    }
}
