using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Options;
using MySite.Models;
using Newtonsoft.Json;

namespace MySite.Services
{
    public class TmdbService
    {
        Config _config;
        const string BaseUrl = "https://api.themoviedb.org/3/";

        public TmdbService(Config config)
        {
            _config = config;
        }

        public async Task<IList<TmdbMovieSearchResult>> SearchAsync(string query)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(BaseUrl + $"search/movie?api_key={_config.TmdbApiKey}&query={HttpUtility.UrlEncode(query)}&include_adult=false");
                response.EnsureSuccessStatusCode();

                var stringResult = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TmdbMovieSearchResponse>(stringResult, new JsonSerializerSettings
                {
                    DateFormatString = "yyyy-MM-dd"
                });
                return result.Results;
            }
        }

        public async Task<TmdbMovieResponse> GetMovieDetails(int tmdbId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(BaseUrl + $"movie/{tmdbId}?api_key={_config.TmdbApiKey}");
                response.EnsureSuccessStatusCode();

                var stringResult = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TmdbMovieResponse>(stringResult, new JsonSerializerSettings
                {
                    DateFormatString = "yyyy-MM-dd"
                });
                return result;
            }
        }
    }

    public class TmdbMovieSearchResponse
    {
        public IList<TmdbMovieSearchResult> Results { get; set; }
    }

    public class TmdbMovieSearchResult
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [JsonProperty("release_date")]
        public DateTime? ReleaseDate { get; set; }
        [JsonProperty("backdrop_path")]
        public string Backdrop { get; set; }
    }

    public class TmdbMovieResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        [JsonProperty("release_date")]
        public DateTime? ReleaseDate { get; set; }
        [JsonProperty("poster_path")]
        public string Poster { get; set; }
    }
}
