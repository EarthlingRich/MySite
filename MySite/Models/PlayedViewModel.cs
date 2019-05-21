using System;
using System.Collections.Generic;
using MySite.Model;
using MySite.Model.Requests;
using MySite.Services;

namespace MySite.Models
{
    public class ListPlayedViewModel
    {
        public ListPlayedViewModel(Played played)
        {
            Id = played.Id;
            CoverPath = played.CoverPath;
            Title = played.Title;
            Rating = played.Rating;
        }

        public int Id { get; set; }
        public string CoverPath { get; set; }
        public string Title { get; set; }
        public Rating Rating { get; set; }
    }

    public class SelectPlayedViewModel
    {
        public SelectPlayedViewModel(List<IgdbGameResponse> igdbGameResponses)
        {
            SearchResults = igdbGameResponses;
        }

        public List<IgdbGameResponse> SearchResults { get; set; }
    }

    public class CreatePlayedViewModel
    {
        public CreatePlayedViewModel() { }

        public CreatePlayedViewModel(IgdbGameResponse igdbGameResponse)
        {
            Request = new CreatePlayedRequest
            {
                IgdbId = igdbGameResponse.Id,
            };
            Title = igdbGameResponse.Title;
            Description = igdbGameResponse.Description;
            PosterPath = igdbGameResponse.Cover.Url;
        }

        public CreatePlayedViewModel(Played played, IgdbGameResponse igdbGameResponse) : this(igdbGameResponse)
        {
            Request = new CreatePlayedRequest
            {
                Id = played.Id,
                IgdbId = played.IdgbId,
                Rating = played.Rating
            };
        }

        public CreatePlayedRequest Request { get; set; }
        public string Description { get; set; }
        public string PosterPath { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Title { get; set; }
    }
}
