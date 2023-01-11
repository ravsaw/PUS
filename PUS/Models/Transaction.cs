using System.ComponentModel.DataAnnotations;

namespace PUS.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public string OfferTo { get; set; }
        public string OfferBack { get; set; }
        public string Remarks { get; set; }
        public DateTime ExchangeDate { get; set; }
        public int EQI { get; set; }
        public Service Service { get; set; }

    }
}
