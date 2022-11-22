namespace PUS.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public virtual Service Service { get; set; }
        public virtual ICollection<ChatLine> ChatLines { get; set; }
    }
}
