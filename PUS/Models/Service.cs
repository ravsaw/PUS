using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUS.Models
{
    [Table("Services")]  // za pomocą atrybutu nadajemy nazwę naszej tabeli
    public class Service
    {
        [Key]  // ustawiamy klucz główny tabeli
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        // będziemy widzieli, które samochody przynależą do tego komentarza
        public string Description { get; set; } = string.Empty;

        [ForeignKey("UserForeignKey")]
        public Profile User { get; set; }
    }
}
