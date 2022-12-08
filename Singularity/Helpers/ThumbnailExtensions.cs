using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode.Common;

namespace Singularity.Helpers;
internal static class ThumbnailExtensions
{
    public static string GetBestThumbnail(this IReadOnlyList<Thumbnail> list)
    {
        var thumbnail = list.OrderByDescending(x=>x.Resolution.Area).FirstOrDefault();
        if(thumbnail == null) return string.Empty;
        var url=thumbnail.Url;
        if (url.StartsWith("//"))
            return $"https:{url}";
        return url;
    }
}
