using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singularity.Core.Helpers;
public static class HttpHelper
{
    public static readonly HttpClient Http = new();
    public static async ValueTask<string> GetResponseStringAsync(string url)
    {
        try
        {
            var reqMsg = new HttpRequestMessage(HttpMethod.Get, url);
            reqMsg.Headers.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36");
            var res=await Http.SendAsync(reqMsg);
            return await res.Content.ReadAsStringAsync();
        }
        catch
        {
            return null;
        }
    }
}
