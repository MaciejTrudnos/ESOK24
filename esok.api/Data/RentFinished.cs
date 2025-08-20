using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace esok.api.Data
{
    public class RentFinished : Entity
    {
        [Key]
        public int Id { get; set; }
     
        public Group Group { get; set; }

        [ForeignKey("Group")]
        public int GroupId { get; set; }

        public Rent Rent { get; set; }

        [ForeignKey("Rent")]
        public int RentId { get; set; }

        public int NumberRentOfDay { get; set; }

        public TimeSpan ElapsedTime { get; set; }

        public decimal NetPrice { get; set; }
    }
}
