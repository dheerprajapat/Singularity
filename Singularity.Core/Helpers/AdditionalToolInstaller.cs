using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Singularity.Core.Helpers;
public static class AdditionalToolInstaller
{
    public const string AdditionalToolFolderName = "Tools";
    public static string DenoDirPath = Path.Join(AppDomain.CurrentDomain.BaseDirectory,AdditionalToolFolderName, DenoDirPath);
    public static string DenoFullPath = Path.Join(DenoDirPath,DenoExeName);

    public static string DenoExeName
    {
        get
        {
            if (OperatingSystem.IsWindows())
                return "deno.exe";
            else
                throw new NotImplementedException();
        }
    }

    public static Uri DenoDownloadUrl
    {
        get
        {
            if (OperatingSystem.IsWindows())
                return new Uri("https://github.com/denoland/deno/releases/download/v1.29.1/deno-x86_64-pc-windows-msvc.zip");
            else
                throw new NotImplementedException();
        }
    }
    public static bool IsAdditionToolsRequired()
    {
        if (!File.Exists(Path.Join(DenoDirPath,DenoExeName)))
            return true;
        return false;
    }
    public static async ValueTask DownloadDenoAsync()
    {
        using var responseStream = await HttpHelper.Http.GetStreamAsync(DenoDownloadUrl);
        var denoZipPath = Path.Join(
            AppDomain.CurrentDomain.BaseDirectory, "Deno.zip");

        using var fileStream = new FileStream(denoZipPath, FileMode.Create);
        responseStream.CopyTo(fileStream);
        responseStream.Close();
        fileStream.Flush();
        fileStream.Close();

        if(Directory.Exists(DenoDirPath))
            Directory.Delete(DenoDirPath, true);

        ZipFile.ExtractToDirectory(denoZipPath, DenoDirPath,true);
    }
}
