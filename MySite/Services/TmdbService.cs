using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Options;
using MySite.Model;
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

        public async Task<IList<TmdbMovieSearchResult>> SearchMovie(string query)
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

        public async Task<IList<TmdbSerieSearchResult>> SearchSerie(string query)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(BaseUrl + $"search/tv?api_key={_config.TmdbApiKey}&query={HttpUtility.UrlEncode(query)}&include_adult=false");
                response.EnsureSuccessStatusCode();

                var stringResult = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TmdbSerieSearchResponse>(stringResult, new JsonSerializerSettings
                {
                    DateFormatString = "yyyy-MM-dd"
                });
                return result.Results;
            }
        }

        public async Task<TmdbWatchedResponse> GetDetails(int tmdbId, WatchedType watchedType)
        {
            if (watchedType == WatchedType.Movie)
            {
                var tmdbMovieResponse = await GetMovieDetails(tmdbId);
                return new TmdbWatchedResponse
                {
                    Id = tmdbMovieResponse.Id,
                    Title = tmdbMovieResponse.Title,
                    Overview = tmdbMovieResponse.Overview,
                    ReleaseDate = tmdbMovieResponse.ReleaseDate,
                    PosterPath = tmdbMovieResponse.PosterPath
                };
            }

            var tmdbSerieResponse = await GetSerieDetails(tmdbId);
            return new TmdbWatchedResponse
            {
                Id = tmdbSerieResponse.Id,
                Title = tmdbSerieResponse.Title,
                Overview = tmdbSerieResponse.Overview,
                ReleaseDate = tmdbSerieResponse.ReleaseDate,
                PosterPath = tmdbSerieResponse.PosterPath
            };
        }

        private async Task<TmdbMovieResponse> GetMovieDetails(int tmdbId)
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

        private async Task<TmdbSerieResponse> GetSerieDetails(int tmdbId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(BaseUrl + $"tv/{tmdbId}?api_key={_config.TmdbApiKey}");
                response.EnsureSuccessStatusCode();

                var stringResult = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TmdbSerieResponse>(stringResult, new JsonSerializerSettings
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
        public string BackdropPath { get; set; }
    }

    public class TmdbMovieResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        [JsonProperty("release_date")]
        public DateTime? ReleaseDate { get; set; }
        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }
    }

    public class TmdbSerieSearchResponse
    {
        public IList<TmdbSerieSearchResult> Results { get; set; }
    }

    public class TmdbSerieSearchResult
    {
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Title { get; set; }
        [JsonProperty("first_air_date")]
        public DateTime? ReleaseDate { get; set; }
        [JsonProperty("backdrop_path")]
        public string BackdropPath { get; set; }
    }

    public class TmdbSerieResponse
    {
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Title { get; set; }
        public string Overview { get; set; }
        [JsonProperty("first_air_date")]
        public DateTime? ReleaseDate { get; set; }
        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }
    }

    public class TmdbWatchedResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string PosterPath { get; set; }
    }
}
