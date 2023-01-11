using System.ComponentModel.DataAnnotations;

namespace PUS.Models
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }
        public Profile User { get; set; }
        public virtual ICollection<ChatLine> ChatLines { get; set; }
    }
}
