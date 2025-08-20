using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace esok.api.Data
{
    public class Customer : Entity
    {
        [Key]
        public int Id { get; set; }
        public string NameSurname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }

        public Group Group { get; set; }

        [ForeignKey("Group")]
        public int GroupId { get; set; }
    }
}
