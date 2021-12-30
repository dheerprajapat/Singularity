using SonicAudioApp.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicAudioApp.Services.Ytdl;
public static class YoutubeHelperExtensions
{
    public static ProcessStartInfo CreateYtdlProcessInfo(string args)
    {
        ProcessStartInfo p = new ProcessStartInfo();
        p.Arguments = args;
        p.FileName =NativeFilePaths.YouTubeSearchExePath;
        p.WorkingDirectory = NativeFilePaths.YouTubeSearchDir;
        p.WindowStyle = ProcessWindowStyle.Hidden;
        p.RedirectStandardError = true;
        p.RedirectStandardOutput = true;
        p.CreateNoWindow = true;
        return p;
    }
}
