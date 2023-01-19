using Humanizer;
using PUS.Models;
using static System.Formats.Asn1.AsnWriter;

namespace PUS.ViewModels
{
    public class ChatListViewModel
    {
        public int Position { get; set; }
        public DateTime LastUpdate { get; set; }
        public string ServiceTitle { get; set; }
        public string UserName { get; set; }
        public Transaction.Status TransactionStatus { get; set; }
        public bool HaveTransaction { get; set; }

        public string UserId { get; set; }
        public int TransactionId { get; set; }
        public int ServiceId { get; set; }
        public int ChatId { get; set; }
    }
}
