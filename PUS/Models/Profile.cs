using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUS.Models
{
    [Table("Profiles")]
    public class Profile: IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public int GoodLevel { get; set; } = 0;
        public int BadLevel { get; set; } = 0;
        public int NeutralLevel { get
            {
                return 100 - GoodLevel - BadLevel;
            } 
        }

        public int EQI { get; set; } = 0;

        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }
}