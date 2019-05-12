using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using MySite.Models;
using Newtonsoft.Json;

namespace MySite.Services
{
    public class GoodreadsService
    {
        Config _config;
        const string BaseUrl = "https://www.goodreads.com/";

        public GoodreadsService(Config config)
        {
            _config = config;
        }

        public async Task<IList<GoodreadsSearchResult>> SearchAsync(string query)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(BaseUrl + $"search/index.xml?key={_config.GoodreadsApiKey}&q={HttpUtility.UrlEncode(query)}");
                response.EnsureSuccessStatusCode();

                var stringResult = await response.Content.ReadAsStringAsync();

                var xml = new XmlDocument();
                xml.LoadXml(stringResult);

                var results = new List<GoodreadsSearchResult>();

                var workNodes = xml.SelectNodes("GoodreadsResponse/search/results/work");

                foreach(XmlNode workNode in workNodes)
                {
                    var bookNode = workNode.SelectSingleNode("best_book");

                    var coverPath = bookNode.SelectSingleNode("image_url").InnerText;
                    if (coverPath.Contains("/nophoto/"))
                    {
                        coverPath = "";
                    }
                    else
                    {
                        var coverPathBuilder = new StringBuilder(coverPath);
                        coverPathBuilder[coverPath.LastIndexOf("/", StringComparison.Ordinal) - 1] = 'l';
                        coverPath = coverPathBuilder.ToString();
                    }

                    results.Add(new GoodreadsSearchResult
                    {
                        Id = int.Parse(bookNode.SelectSingleNode("id").InnerText),
                        Title = bookNode.SelectSingleNode("title").InnerText,
                        CoverPath = coverPath
                    });
                }

                return results;
            }
        }

        public async Task<GoodreadsBookResponse> GetBookDetails(int goodreadsId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(BaseUrl + $"/book/show/{goodreadsId}.xml?key={_config.GoodreadsApiKey}");
                response.EnsureSuccessStatusCode();

                var stringResult = await response.Content.ReadAsStringAsync();

                var xml = new XmlDocument();
                xml.LoadXml(stringResult);

                var bookNode = xml.SelectSingleNode("GoodreadsResponse/book");

                var coverPath = bookNode.SelectSingleNode("image_url").InnerText;
                if (coverPath.Contains("/nophoto/"))
                {
                    coverPath = "";
                }
                else
                {
                    var coverPathBuilder = new StringBuilder(coverPath);
                    coverPathBuilder[coverPath.LastIndexOf("/", StringComparison.Ordinal) - 1] = 'l';
                    coverPath = coverPathBuilder.ToString();
                }

                var result = new GoodreadsBookResponse
                {
                    Id = int.Parse(bookNode.SelectSingleNode("id").InnerText),
                    Description = Regex.Replace(bookNode.SelectSingleNode("description").InnerText, @"<[\w\s\/]*>", ""),
                    Title = bookNode.SelectSingleNode("title").InnerText,
                    CoverPath = coverPath
                };

                return result;
            }
        }
    }

    [XmlRoot(ElementName = "Search")]
    public class GoodreadsSearchResult
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string CoverPath { get; set; }
    }

    public class GoodreadsBookResponse
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string CoverPath { get; set; }
    }
}
