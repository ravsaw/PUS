using PUS.Models;

namespace PUS.ViewModels
{
    public class ChatViewModel
    {
        public string currentUserID { get; set; }
        public int chatID { get; set; }
        public int serviceID { get; set; }
        public string serviceTitle { get; set; }
        public List<ChatLine> chatLines { get; set; }
    }
}
