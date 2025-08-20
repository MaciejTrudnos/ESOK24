using esok.api.Application.Common.Enum;
using esok.api.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace esok.api.Data
{
    public class RentProduct : Entity
    {
        [Key]
        public int Id { get; set; }
        public int Quanity { get; set; }

        public Product Product { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public Rent Rent { get; set; }

        [ForeignKey("Rent")]
        public int RentId { get; set; }
    }
}