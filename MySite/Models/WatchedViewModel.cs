using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            SearchResults = new List<SearchResultWatchedViewModel>();
            foreach (var tmdbMovieSearchResult in tmdbMovieSearchResults)
            {
                SearchResults.Add(new SearchResultWatchedViewModel(tmdbMovieSearchResult));
            }
        }

        public SelectWatchedViewModel(List<TmdbSerieSearchResult> tmdbSerieSearchResults)
        {
            SearchResults = new List<SearchResultWatchedViewModel>();
            foreach (var tmdbSerieSearchResult in tmdbSerieSearchResults)
            {
                SearchResults.Add(new SearchResultWatchedViewModel(tmdbSerieSearchResult));
            }
        }

        public List<SearchResultWatchedViewModel> SearchResults { get; set; }
    }

    public class SearchResultWatchedViewModel
    {
        public SearchResultWatchedViewModel(TmdbMovieSearchResult tmdbMovieSearchResult)
        {
            Id = tmdbMovieSearchResult.Id;
            Title = tmdbMovieSearchResult.Title;
            ReleaseDate = tmdbMovieSearchResult.ReleaseDate;
            BackdropPath = tmdbMovieSearchResult.BackdropPath;
            WatchedType = WatchedType.Movie;
        }

        public SearchResultWatchedViewModel(TmdbSerieSearchResult tmdbSerieSearchResult)
        {
            Id = tmdbSerieSearchResult.Id;
            Title = tmdbSerieSearchResult.Title;
            ReleaseDate = tmdbSerieSearchResult.ReleaseDate;
            BackdropPath = tmdbSerieSearchResult.BackdropPath;
            WatchedType = WatchedType.Serie;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string BackdropPath { get; set; }
        public WatchedType WatchedType { get; set; }
    }

    public class CreateWatchedViewModel
    {
        public CreateWatchedViewModel() { }

        public CreateWatchedViewModel(TmdbWatchedResponse tmdbWatchedResponse, WatchedType watchedType)
        {
            Request = new CreateWatchedRequest
            {
                TmdbId = tmdbWatchedResponse.Id,
                WatchedType = watchedType
            };
            Title = tmdbWatchedResponse.Title;
            Description = tmdbWatchedResponse.Overview;
            ReleaseDate = tmdbWatchedResponse.ReleaseDate;
            PosterPath = tmdbWatchedResponse.PosterPath;
            SeasonSelectListItems = new List<SelectListItem>();

            for(var i = 1; i <= tmdbWatchedResponse.SeasonCount; i++)
            {
                SeasonSelectListItems.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = $"Season {i}"
                });
            }
        }

        public CreateWatchedViewModel(TmdbWatchedResponse tmdbWatchedResponse, Watched watched) : this(tmdbWatchedResponse, watched.WatchedType)
        {
            Request.Id = watched.Id;
            Request.Rating = watched.Rating;
            Request.SeasonNumber = watched.SeasonNumber;
        }

        public CreateWatchedRequest Request { get; set; }
        public string Description { get; set; }
        public string PosterPath { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public List<SelectListItem> SeasonSelectListItems { get; set;}
        public string Title { get; set; }
    }
}
