using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicAudioApp.Native;
public static class NativeFilePaths
{
    private readonly static string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
    private readonly static string NativePath = Path.Combine(BaseDirectory,"Native");
    private  readonly static string VendorPath = Path.Combine(NativePath, "Vendor");
    public readonly static string YouTubeSearchDir = Path.Combine(VendorPath, "YouTubeSearch");

    public readonly static string YouTubeSearchExePath = Path.Combine(YouTubeSearchDir, "ytsearch.exe");

}