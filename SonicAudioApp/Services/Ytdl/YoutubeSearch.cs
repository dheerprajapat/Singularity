using SonicAudioApp.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicAudioApp.Services.Ytdl;
public static class YoutubeSearch
{
    public static string GetJson(string str)
    {
        Process p = new Process();
        p.StartInfo.WorkingDirectory =NativeFilePaths.VendorPath;
        p.StartInfo.FileName = NativeFilePaths.YtdlPath;
        p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        p.StartInfo.RedirectStandardError = true;   
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.CreateNoWindow = true;
        p.Start();
        p.WaitForExit();
        var r = p.StandardError.ReadToEnd();
        return "";
    }
}