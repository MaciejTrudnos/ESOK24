using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace esok.api.Data
{
    public class Rent : Entity
    {
        [Key]
        public int Id { get; set; }
        public int NumberRentOfDay { get; set; }
        public string Comments { get; set; }

        public Group Group { get; set; }

        [ForeignKey("Group")]
        public int GroupId { get; set; }

        public int CustomerId { get; set; }
    }
}