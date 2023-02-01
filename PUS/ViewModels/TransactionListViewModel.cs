using PUS.Models;

namespace PUS.ViewModels
{
    public class TransactionListViewModel
    {
        public enum Side
        {
            provider, consumer
        }

        public int Position { get; set; }
        public DateTime ExchangeDate { get; set; }
        public string ServiceTitle { get; set; }
        public string ClientName { get; set; }
        public string OwnerName { get; set; }
        public int EQI { get; set; }
        public Transaction.Status TransactionStatus { get; set; }

        public Side UserSide { get; set; }

        public string ClientId { get; set; }
        public string OwnerId { get; set; }
        public int TransactionId { get; set; }
        public int ServiceId { get; set; }
        public int ChatId { get; set; }

    }
}
