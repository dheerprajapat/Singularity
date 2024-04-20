using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Singularity.Audio;
using Singularity.Models;
using YoutubeExplode;

namespace Singularity
{
    internal static class SingletonFactory
    {
        public static YoutubeClient YoutubeClient { get; } = new YoutubeClient();
        public static AudioManager AudioManager { get; } = new AudioManager();
        public static UserSetting Settings =>UserSetting.Instance;
        public static HttpClient Http { get; } = new HttpClient();
    }
}
