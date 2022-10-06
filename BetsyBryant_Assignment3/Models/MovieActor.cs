using System.ComponentModel.DataAnnotations.Schema;

namespace BetsyBryant_Assignment3.Models
{
    public class MovieActor
    {
        public int Id { get; set; }

        [ForeignKey("Movie")]
        public int? MovieID { get; set; }
        public Movie? Movie { get; set; }

        [ForeignKey("Actor")]
        public int? ActorID { get; set; }
        public Actor? Actor { get; set; }
    }
}
