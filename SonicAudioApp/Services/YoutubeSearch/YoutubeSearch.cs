using Newtonsoft.Json;
using SonicAudioApp.Native;
using SonicAudioApp.Services.YoutubeSearch.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SonicAudioApp.Services.Ytdl;
public static class YoutubeSearch
{
    public static async Task<string> GetJsonAsync(string query, uint amount=1, CancellationToken token = default)
    {
        try
        {
            return await Task.Run(() =>
            {
                using Process p = new Process();
                var args = @$" ""{query}"" {amount}";
                p.StartInfo = YoutubeHelperExtensions.CreateYtdlProcessInfo(args);
                p.Start();
                token.Register(() => p.Kill());
                var stdout = p.StandardOutput.ReadToEnd();
                var stderr=p.StandardError.ReadToEnd();
                p.WaitForExit();

                return stdout;
            });
        }
        catch
        {
            throw new Exception("Failed to search for given query");
        }
    }
    public static async Task<IReadOnlyList<SearchResult>> GetVideosAsync(string query, uint amount = 2, CancellationToken token = default)
    {
        
        try
        {
            var resp=await GetJsonAsync(query, amount, token);
            return JsonConvert.DeserializeObject<List<SearchResult>>(resp);
        }
        catch
        {
            throw new Exception("Can't parse the search results");
        }
    }
}