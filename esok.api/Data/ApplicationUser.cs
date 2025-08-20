using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace esok.api.Data
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string NameSurname { get; set; }
        public bool Active { get; set; }

        [NotMapped]
        public int GroupId { get; set; }
    }
}
