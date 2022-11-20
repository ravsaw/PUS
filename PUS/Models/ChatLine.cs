namespace PUS.Models
{
    public class ChatLine
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public Chat Chat { get; set; }
        public Profile User { get; set; }
    }
}
