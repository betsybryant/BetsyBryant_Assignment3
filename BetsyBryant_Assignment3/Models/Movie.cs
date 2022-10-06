using System.ComponentModel.DataAnnotations;
namespace BetsyBryant_Assignment3.Models
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string MovieTitle { get; set; }
        [Url]
        public string MovieIMDBLink { get; set; }
        public string MovieGenre { get; set; }
        public int YearOfRelease { get; set; }
        public byte[]? Poster { get; set; }
    }
}
