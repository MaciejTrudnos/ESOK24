using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using esok.api.Application.Common.Enum;

namespace esok.api.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal NetPrice { get; set; }
        public int Quanity { get; set; }
        public Unit Unit { get; set; }
        public int PredecessorId { get; set; }
    }
}
