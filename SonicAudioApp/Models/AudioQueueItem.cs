using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicAudioApp.Models;

public record AudioQueueItem
{
    public AudioQueueItem(string url, string name, string singers, long duration)
    {
        Url = url;
        Name = name;
        Singers = singers;
        Duration = duration;
    }

    public string Url { get; }
    public string Name { get; }
    public string Singers { get; }
    public long Duration { get; }
}