using Humanizer;
using static System.Formats.Asn1.AsnWriter;

namespace PUS.ViewModels
{
    public class ChatListViewModel
    {
        public enum Status
        {
            Pending, Accept, Deny
        }

        public int Position { get; set; }
        public DateTime LastUpdate { get; set; }
        public string ServiceTitle { get; set; }
        public string User { get; set; }
        public Status TransactionStatus { get; set; }
        public bool HaveTransaction { get; set; }

        public string UserId { get; set; }
        public int TransactionId { get; set; }
        public int ServiceId { get; set; }
        public int ChatId { get; set; }
    }
}
