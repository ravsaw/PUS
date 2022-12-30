using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUS.Models
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }
        public Profile Client { get; set; }

        public ICollection<ChatLine> ChatLines { get; set; }
    }
}
