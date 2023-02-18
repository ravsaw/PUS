using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUS.Models
{
    [Table("Transactions")]
    public class Transaction
    {
        public enum Status : int
        {
            Pending = 0, 
            Accept = 1, 
            Deny = 2
        }

        [Key]
        public int Id { get; set; }
        public string OfferTo { get; set; }
        public string OfferBack { get; set; }
        public string Remarks { get; set; }
        public DateTime ExchangeDate { get; set; }
        public int EQI { get; set; }
        public Status TransactionStatus { get; set; }
        public bool IsArchived { get; set; }

        public virtual Service Service { get; set; }
        public Profile Client { get; set; }

    }
}
