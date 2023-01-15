using MessagePack;

namespace PUS.ViewModels
{
    public class TransactionCreateViewModel
    {
        public int ServiceId { get; set; }
        public string ServiceTitle { get; set; }
        public string OfferIn { get; set; }
        public string OfferOut { get; set; }
        public DateTime ExchangeDate { get; set; }
        public int EQI { get; set; }
        public string Remarks { get; set; }
    }
}
