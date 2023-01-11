using System.ComponentModel.DataAnnotations;

namespace PUS.Models
{
    public enum MessageDirection
    {
        From,
        To
    }

    public class ChatLine
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public MessageDirection Direction { get; set; }
    }

}
