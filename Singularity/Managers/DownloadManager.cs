using AngleSharp.Dom.Events;
using Singularity.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Singularity.Managers;

public class DownloadManager
{
    public DownloadManager(HttpClient httpClient)
    {
        HttpClient = httpClient;
        buffer = new byte[1024 * 20];
    }
    public HttpClient HttpClient { get; }
    public event EventHandler<DownloadProgressEventArgs>? DownloadProgressChanged;

    private Dictionary<string, ISong> DownloadedMetaData = new();

    private byte[] buffer;
    public async ValueTask DownloadAsync(string url, string outputPath)
    {
        var response = await HttpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        if (!response.IsSuccessStatusCode)
        {
            return;
        }
        var totalLength = response.Content.Headers.ContentLength.GetValueOrDefault();
        using var stream = await response.Content.ReadAsStreamAsync();
        using var outputStream = File.OpenWrite(outputPath);
        int bytesRead = 0;
        long totalRead = 0;
        do
        {
            bytesRead = stream.Read(buffer, 0, buffer.Length);
            totalRead += bytesRead;
            outputStream.Write(buffer, 0, bytesRead);
            DownloadProgressChanged?.Invoke(this, new DownloadProgressEventArgs()
            {
                FileName = outputPath,
                Percentage = totalRead * 100.0 / totalLength
            });
        } while (bytesRead != 0);
        outputStream.Flush();
    }

    public async ValueTask DownloadSongAsync(ISong song)
    {
        var songUrl = await song.GetAudioUrlAsync();
        var folderPath = FileSystem.Current.AppDataDirectory;
        folderPath = Path.Combine(folderPath, song.Id);
        if (Directory.Exists(folderPath))
        {
            Directory.Delete(folderPath, true);
        }
        Directory.CreateDirectory(folderPath);
        var filePath = Path.Combine(folderPath, song.Id + ".mp3");
        await DownloadAsync(songUrl, filePath);
        filePath = Path.Combine(folderPath, song.Id + ".png");
        await DownloadAsync(song.ThumbnailUrl, filePath);
        filePath = Path.Combine(folderPath, song.Id + ".json");
        File.WriteAllText(filePath, JsonSerializer.Serialize(song));
    }
}

public class DownloadProgressEventArgs
{
    public string? FileName { get; set; }
    public double Percentage { get; set; }
}
