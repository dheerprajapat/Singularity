using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SonicAudioApp.Services.YoutubeSearch
{
    public static class SearchSuggestions
    {
        public static HttpClient Http=new HttpClient();
        private const string _url = "https://clients1.google.com/complete/search?client=youtube&gs_ri=youtube&ds=yt&q=";
        public static async Task<List<string>> SuggestionsAsync(string query,CancellationToken token=default)
        {
            query=Uri.EscapeDataString(query);
            query=_url + query;
            var res=await Http.GetAsync(query);
            var js= await res.Content.ReadAsStringAsync();

            var parts=js.Split('[').Where(t=>t.Split('"').Length>2).Select(t=>t.Split('"')[1]);
            
            return parts.ToList();
        }
    }
}
