using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUS.Models
{
    public enum MessageDirection
    {
        From,
        To
    }

    [Table("ChatLines")]
    public class ChatLine
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public MessageDirection Direction { get; set; }
    }

}
