using PUS.Models;

namespace PUS.ViewModels
{
    public class TransactionListViewModel
    {
        /*
                            <th scope="col">#</th>
                            <th scope="col">Termin</th>
                            <th scope="col">Oferta</th>
                            <th scope="col">Kto?</th>
                            <th scope="col">EQI</th>
                            <th scope="col">Status</th>
                            <th scope="col">&nbsp;</th>
         */

        public int Position { get; set; }
        public DateTime ExchangeDate { get; set; }
        public string ServiceTitle { get; set; }
        public string UserName { get; set; }
        public int EQI { get; set; }
        public Transaction.Status TransactionStatus { get; set; }

        public string UserId { get; set; }
        public int TransactionId { get; set; }
        public int ServiceId { get; set; }
        public int ChatId { get; set; }

    }
}
