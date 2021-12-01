using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicAudioApp.Models;

public record AudioQueueItem
{
    public string Url { get; set; }
    public string Title { get; set; }
    public string Singers { get; set; }
    public string ThumbnailUrl { get; set; }
    public string Id { get; set; }
    public string VideoUrl { get; set; }
}