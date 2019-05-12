using System;
using System.Collections.Generic;
using MySite.Model;
using MySite.Model.Requests;
using MySite.Services;

namespace MySite.Models
{
    public class ListReadViewModel
    {
        public ListReadViewModel(Read read) {
            CoverPath = read.CoverPath;
            Title = read.Title;
            Rating = read.Rating;
            ReleaseYear = read.ReleaseDate.HasValue ? read.ReleaseDate.Value.Year.ToString() : null;
        }

        public string CoverPath { get; set; }
        public string Title { get; set; }
        public Rating Rating { get; set; }
        public string ReleaseYear { get; set; }
    }

    public class SelectReadViewModel
    {
        public SelectReadViewModel(List<GoodreadsSearchResult> goodreadsSearchResults)
        {
            SearchResults = goodreadsSearchResults;
        }

        public List<GoodreadsSearchResult> SearchResults { get; set; }
    }

    public class CreateReadViewModel
    {
        public CreateReadViewModel() { }

        public CreateReadViewModel(GoodreadsBookResponse goodreadsBookResponse)
        {
            Request = new CreateReadRequest
            {
                GoodreadsId = goodreadsBookResponse.Id
            };
            Title = goodreadsBookResponse.Title;
            Description = goodreadsBookResponse.Description;
            ReleaseDate = goodreadsBookResponse.ReleaseDate;
            CoverPath = goodreadsBookResponse.CoverPath;
        }

        public CreateReadRequest Request { get; set;  }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string CoverPath { get; set; }
    }
}
