using System;
using System.Collections.Generic;
using MySite.Model;
using MySite.Model.Requests;
using MySite.Services;

namespace MySite.Models
{
    public class ListWatchedViewModel
    {
        public ListWatchedViewModel(Watched watched) {
            Id = watched.Id;
            PosterPath = watched.PosterPath;
            Title = watched.Title;
            Rating = watched.Rating;
            ReleaseYear = watched.ReleaseDate.HasValue ? watched.ReleaseDate.Value.Year.ToString() : null;
        }

        public int Id { get; set; }
        public string PosterPath { get; set; }
        public string Title { get; set; }
        public Rating Rating { get; set; }
        public string ReleaseYear { get; set; }
    }

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
            Description = tmdbMovieResponse.Overview;
            ReleaseDate = tmdbMovieResponse.ReleaseDate;
            PosterPath = tmdbMovieResponse.Poster;
        }

        public CreateWatchedViewModel(Watched watched)
        {
            Request = new CreateWatchedRequest
            {
                Id = watched.Id,
                TmdbId = watched.TmdbId
            };
            Title = watched.Title;
            Description = watched.Description;
            ReleaseDate = watched.ReleaseDate;
            PosterPath = watched.PosterPath;
        }

        public CreateWatchedRequest Request { get; set;  }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string PosterPath { get; set; }
    }
}
