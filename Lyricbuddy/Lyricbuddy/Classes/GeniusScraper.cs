using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lyricbuddy.Classes
{
    class GeniusScraper
    {
        private const string clientAccessToken = "a6kljZlqI9W8eI7mCSSwi49-REBCTJqCN1SO-vn_yoyXEVOt9dbc_YKhcrGGPpGM";
        private const string serverAuthentication = "https://api.genius.com/oauth/authorize";
        private HttpClient httpClient;

        public GeniusScraper()
        {
            httpClient = new HttpClient();
        }
        public async Task<string>SearchGeniusASync(string searchParameter)
        {
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", clientAccessToken);

            var result = await httpClient.GetAsync(new Uri("https://api.genius.com/search?q=" + searchParameter), HttpCompletionOption.ResponseContentRead);
            var data = await result.Content.ReadAsStringAsync();

            return data;
        }
    }
}