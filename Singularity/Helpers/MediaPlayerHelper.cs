using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singularity.Helpers;
public class MediaPlayerHelper
{
    public static string ConvertTimeSpanToDuration(TimeSpan time)
    {
        StringBuilder sb = new StringBuilder();
        if (time.Hours != 0)
            sb.Append(time.Hours.ToString() + ':');
        sb.Append(time.Minutes.ToString().PadLeft(2, '0'));
        sb.Append(':');
        sb.Append(time.Seconds.ToString().PadLeft(2, '0'));
        return sb.ToString();
    }
}
