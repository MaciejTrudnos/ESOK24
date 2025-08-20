using esok.api.Application.Common.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace esok.api.Data
{
    public class Product : Entity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal NetPrice { get; set; }
        public int Quanity { get; set; }
        public Unit Unit { get; set; }
        public int PredecessorId { get; set; }

        public Group Group { get; set; }

        [ForeignKey("Group")]
        public int GroupId { get; set; }
    }
}
