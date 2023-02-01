using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUS.Models
{

    [Table("Addresses")]
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string FullAdress { get
            {
                return PostCode + " " + City + ", " + Address1 + " " + Address2;
            }
        }
    }
}
