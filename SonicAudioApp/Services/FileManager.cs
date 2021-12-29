using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace SonicAudioApp.Services;
public static class FileManager
{
    private const string DirName = "Singularity";
    private static readonly string DocPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}";
    private static SemaphoreSlim FileWriteSema=new SemaphoreSlim(1,1);
    public static async Task<string> ReadAllText(string fileName)
    {
        var folder = await StorageFolder.GetFolderFromPathAsync(DocPath);
        folder = await folder.CreateFolderAsync(DirName, CreationCollisionOption.OpenIfExists);
        var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
        using var stream = await file.OpenAsync(FileAccessMode.Read);
        using var reader = new StreamReader(stream.AsStream());
        return reader.ReadToEnd();
    }
    public static async Task WriteAllText(string fileName, string content)
    {
        await FileWriteSema.WaitAsync();

        var folder = await StorageFolder.GetFolderFromPathAsync(DocPath);
        folder = await folder.CreateFolderAsync(DirName, CreationCollisionOption.OpenIfExists);
        var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
        using var stream = await file.OpenAsync(FileAccessMode.ReadWrite);
        using var reader = new StreamWriter(stream.AsStream());

        reader.Write(content);
        FileWriteSema.Release();
    }
}