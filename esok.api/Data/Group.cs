using System.ComponentModel.DataAnnotations;

namespace esok.api.Data
{
    public class Group : Entity
    {
        [Key]
        public int Id { get; set; }
        public int ManagerId { get; set; }
    }
}
