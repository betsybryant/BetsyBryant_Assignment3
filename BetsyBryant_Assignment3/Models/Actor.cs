using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetsyBryant_Assignment3.Models
{
    public class Actor
    {
        public int ActorId { get; set; }
        public string ActorName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        [Url]
        public string ActorIMDBLink { get; set; }
        public byte[]? ActorPhoto { get; set; }

        [ForeignKey("Movie")]
        public int? MovieID { get; set; }
        public Movie? Movie { get; set; }
    }
}
