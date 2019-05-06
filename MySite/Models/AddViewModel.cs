using System;
using System.Collections.Generic;
using MySite.Model.Requests;
using MySite.Services;

namespace MySite.Models
{
    public class SelectMovieViewModel
    {
        public SelectMovieViewModel(List<TmdbMovieSearchResult> tmdbMovieSearchResults)
        {
            MovieResults = tmdbMovieSearchResults;
        }

        public List<TmdbMovieSearchResult> MovieResults { get; set; }
    }

    public class AddMovieViewModel
    {
        public AddMovieViewModel() { }

        public AddMovieViewModel(TmdbMovieResponse tmdbMovieResponse)
        {
            Request = new AddMovieRequest
            {
                TmdbId = tmdbMovieResponse.Id
            };
            Title = tmdbMovieResponse.Title;
            Overview = tmdbMovieResponse.Overview;
            ReleaseDate = tmdbMovieResponse.ReleaseDate;
            Poster = tmdbMovieResponse.Poster;
        }

        public AddMovieRequest Request { get; set;  }
        public string Title { get; set; }
        public string Overview { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Poster { get; set; }
    }
}
