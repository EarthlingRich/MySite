using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySite.Services;

namespace MySite.Models
{
    public class AddViewModel
    {
        public string SearchQuery { get; set; }
    }

    public class AddMovieViewModel
    {
        public AddMovieViewModel(List<TmdbMovieSearchResult> tmdbMovieSearchResults)
        {
            MovieResults = tmdbMovieSearchResults;
        }

        public List<TmdbMovieSearchResult> MovieResults { get; set; }
    }
}
