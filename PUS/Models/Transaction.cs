using System.ComponentModel.DataAnnotations;

namespace PUS.Models
{
    public class Transaction
    {
        public enum Status
        {
            Pending, Accept, Deny
        }

        [Key]
        public int Id { get; set; }
        public string OfferTo { get; set; }
        public string OfferBack { get; set; }
        public string Remarks { get; set; }
        public DateTime ExchangeDate { get; set; }
        public int EQI { get; set; }
        public Status TransactionStatus { get; set; }

        //public Service Service { get; set; }
        public Profile Client { get; set; }

    }
}
