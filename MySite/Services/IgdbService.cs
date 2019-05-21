using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MySite.Models;
using Newtonsoft.Json;

namespace MySite.Services
{
    public class IgdbService
    {
        private readonly Config _config;
        private const string BaseUrl = "https://api-v3.igdb.com/";

        public IgdbService(Config config)
        {
            _config = config;
        }

        public async Task<IList<IgdbGameResponse>> SearchAsync(string query)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("user-key", _config.IgdbApiKey);
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(BaseUrl + "games"),
                    Content = new StringContent($"search \"{HttpUtility.UrlEncode(query)}\"; fields name,summary,cover.url;", Encoding.UTF8)
                };

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var stringResult = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<IgdbGameResponse>>(stringResult, new JsonSerializerSettings
                {
                    DateFormatString = "yyyy-MM-dd"
                });
                return result;
            }
        }

        public async Task<IgdbGameResponse> GetDetails(int igdbId)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("user-key", _config.IgdbApiKey);
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(BaseUrl + "games"),
                    Content = new StringContent($"fields name,summary,cover.url; where id = {igdbId};", Encoding.UTF8)
                };

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var stringResult = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<IgdbGameResponse>>(stringResult, new JsonSerializerSettings
                {
                    DateFormatString = "yyyy-MM-dd"
                });
                return result.First();
            }
        }
    }

    public class IgdbGameResponse
    {
        public int Id { get; set; }
        public IgdbCoverResponse Cover { get; set; }
        [JsonProperty("summary")]
        public string Description { get; set; }
        [JsonProperty("name")]
        public string Title { get; set; }
    }

    public class IgdbCoverResponse
    {
        private string _url { get; set; }
        public string Url { 
            get { return _url; }
            set { _url = value.Replace("t_thumb", "t_cover_big"); }
        }
    }
}
