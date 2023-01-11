using System.ComponentModel.DataAnnotations;

namespace PUS.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string NIP { get; set; }
        public string REGON { get; set; }
    }
}
