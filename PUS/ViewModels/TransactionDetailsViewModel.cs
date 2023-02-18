using MessagePack;
using PUS.Models;
using System.ComponentModel.DataAnnotations;

namespace PUS.ViewModels
{
    public class TransactionDetailsViewModel
    {
        public string ServiceTitle { get; set; }
        public string ClientName { get; set; }
        public string OfferTo { get; set; }
        public string OfferBack { get; set; }
        public DateTime ExchangeDate { get; set; }
        public int EQI { get; set; }
        public string? Remarks { get; set; }
        public Transaction.Status Status { get; set; }
        public int ServiceId { get; set; }
        public int TransactionId { get; set; }
        public string ownerId { get; set; }
    }
}
