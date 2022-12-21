using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singularity.Core.Helpers;
public static class DenoServerLauncher
{
    public static string SourceFileDir => Path.Join(
            AppDomain.CurrentDomain.BaseDirectory,
            "DenoHost");
    public static string IndexFilePath=>
        Path.Join(SourceFileDir,"index.ts");

    public static FluentProcess? DenoProcess;

    public static Uri GetUri(string route)
    {
        return new Uri($"http://localhost:9867/{route}");
    }

    public static async ValueTask StartServer()
    {
        if (await PingSuccessAsnc())
            return;

        DenoProcess = FluentProcess.Create(AdditionalToolInstaller.DenoFullPath)
            .WithWorkingDirectory(SourceFileDir)
            .WithArguments("run -A index.ts").Run();

        while (!await PingSuccessAsnc())
        {
            await Task.Delay(2000);
        }
    }
    public static async ValueTask<bool> PingSuccessAsnc()
    {
        try
        {
            var res = await HttpHelper.Http.GetAsync(GetUri("ping"));
            return res.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
