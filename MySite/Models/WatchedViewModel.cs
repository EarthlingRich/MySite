using System;
using System.Collections.Generic;
using MySite.Model.Requests;
using MySite.Services;

namespace MySite.Models
{
    public class SelectWatchedViewModel
    {
        public SelectWatchedViewModel(List<TmdbMovieSearchResult> tmdbMovieSearchResults)
        {
            SearchResults = tmdbMovieSearchResults;
        }

        public List<TmdbMovieSearchResult> SearchResults { get; set; }
    }

    public class CreateWatchedViewModel
    {
        public CreateWatchedViewModel() { }

        public CreateWatchedViewModel(TmdbMovieResponse tmdbMovieResponse)
        {
            Request = new CreateWatchedRequest
            {
                TmdbId = tmdbMovieResponse.Id
            };
            Title = tmdbMovieResponse.Title;
            Overview = tmdbMovieResponse.Overview;
            ReleaseDate = tmdbMovieResponse.ReleaseDate;
            Poster = tmdbMovieResponse.Poster;
        }

        public CreateWatchedRequest Request { get; set;  }
        public string Title { get; set; }
        public string Overview { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Poster { get; set; }
    }
}
