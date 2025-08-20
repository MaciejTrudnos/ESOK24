using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace esok.api.Data
{
    public class UserGroup : Entity
    {
        [Key]
        public int Id { get; set; }
        
        public Group Group { get; set; }
        
        [ForeignKey("Group")]
        public int GroupId { get; set; }
        
        public int EmployeeId { get; set; }
    }
}
